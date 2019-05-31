using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Productions
{
    public class WorkOrder
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public WorkOrder(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetWorkOrderIndexes();

            this.GetWorkOrderViewDetails();

            this.GetWorkOrderPendingFirmOrders();

            this.WorkOrderSaveRelative();
            this.WorkOrderPostSaveValidate();

            this.WorkOrderApproved();
            this.WorkOrderEditable();

            //DO NOT ALLOW WorkOrderToggleApproved DEPENDENTLY. BECAUSE: WE NEED TO RUN CHECK BOM WorkOrderSaveRelative
            //IMPORTANT: 'CHECK BOM' AT: WorkOrderSaveRelative: MUST RUN BEFORE CALL SAVE UPDATE TO FirmOrderMaterials ==> IN ORDER TO GET SQL_FirmOrderRemains CORRECTLY
            this.WorkOrderToggleApproved();

            this.WorkOrderInitReference();

            //this.WorkOrderSheet();
        }


        private void GetWorkOrderIndexes()
        {
            string queryString;

            queryString = " @NMVNTaskID int, @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      WorkOrders.WorkOrderID, CAST(WorkOrders.EntryDate AS DATE) AS EntryDate, WorkOrders.Reference, Locations.Code AS LocationCode, Customers.Name AS CustomerName, ISNULL(FirmOrders.Description, '') + ' ' + ISNULL(WorkOrders.Description, '') AS Description, WorkOrders.QuantityMaterialEstimated, WorkOrders.TotalQuantity, WorkOrders.Approved, " + "\r\n";
            queryString = queryString + "                   FirmOrders.Reference AS FirmOrdersReference, FirmOrders.Code AS FirmOrdersCode, FirmOrders.EntryDate AS FirmOrderEntryDate, FirmOrders.Specs AS FirmOrderSpecs, FirmOrders.Specification AS FirmOrderSpecification, Boms.Code AS BomCode " + "\r\n";
            queryString = queryString + "       FROM        WorkOrders " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON WorkOrders.NMVNTaskID = @NMVNTaskID AND WorkOrders.EntryDate >= @FromDate AND WorkOrders.EntryDate <= @ToDate AND WorkOrders.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = @NMVNTaskID AND AccessControls.AccessLevel > 0) AND Locations.LocationID = WorkOrders.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN FirmOrders ON WorkOrders.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON WorkOrders.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Boms ON FirmOrders.BomID = Boms.BomID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetWorkOrderIndexes", queryString);
        }


        private void GetWorkOrderViewDetails()
        {
            string queryString;

            queryString = " @WorkOrderID Int, @FirmOrderID Int, @WarehouseID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      WorkOrderDetails.WorkOrderDetailID, @WorkOrderID AS WorkOrderID, FirmOrderMaterials.FirmOrderMaterialID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   FirmOrderMaterials.BomID, FirmOrderMaterials.BomDetailID, BomDetails.LayerCode, BomDetails.BlockUnit, BomDetails.BlockQuantity, BomDetails.LayerQuantity, " + "\r\n";
            queryString = queryString + "                   FirmOrderMaterials.Quantity AS FirmOrderMaterialQuantity, FirmOrderMaterials.QuantityIssued AS FirmOrderMaterialQuantityIssued, GoodsReceiptDetails.QuantityAvailables, ISNULL(WorkOrderDetails.Quantity, 0) AS Quantity, WorkOrderDetails.Remarks " + "\r\n";

            queryString = queryString + "       FROM        FirmOrderMaterials " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON FirmOrderMaterials.FirmOrderID = @FirmOrderID AND FirmOrderMaterials.Approved = 1 AND FirmOrderMaterials.MaterialID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN BomDetails ON FirmOrderMaterials.BomDetailID = BomDetails.BomDetailID " + "\r\n";

            queryString = queryString + "                   LEFT JOIN WorkOrderDetails ON WorkOrderDetails.WorkOrderID = @WorkOrderID AND FirmOrderMaterials.FirmOrderMaterialID = WorkOrderDetails.FirmOrderMaterialID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN (SELECT CommodityID, ROUND(SUM(Quantity - QuantityIssued), " + (int)GlobalEnums.rndQuantity + ") AS QuantityAvailables FROM GoodsReceiptDetails WHERE WarehouseID = @WarehouseID AND CommodityID IN (SELECT MaterialID FROM FirmOrderMaterials WHERE FirmOrderID = @FirmOrderID) AND Approved = 1 AND ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 GROUP BY CommodityID) GoodsReceiptDetails ON FirmOrderMaterials.MaterialID = GoodsReceiptDetails.CommodityID " + "\r\n";

            queryString = queryString + "       WHERE       NOT WorkOrderDetails.WorkOrderDetailID IS NULL OR (FirmOrderMaterials.InActive = 0 AND FirmOrderMaterials.InActivePartial = 0) " + "\r\n";

            queryString = queryString + "       ORDER BY    BomDetails.LayerCode, BomDetails.BomDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetWorkOrderViewDetails", queryString);
        }


        private void GetWorkOrderPendingFirmOrders()
        {
            string queryString = " @LocationID int, @NMVNTaskID int, @FirmOrderID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          ProductionOrderDetails.ProductionOrderDetailID, ProductionOrderDetails.ProductionOrderID, ProductionOrderDetails.PlannedOrderID, ProductionOrderDetails.FirmOrderID, FirmOrders.Code AS FirmOrderCode, FirmOrders.Reference AS FirmOrderReference, FirmOrders.EntryDate AS FirmOrderEntryDate, FirmOrders.Specs AS FirmOrderSpecs, FirmOrders.Specification AS FirmOrderSpecification, FirmOrders.BomID, FirmOrders.TotalQuantity, FirmOrderRemains.TotalQuantityRemains, FirmOrders.QuantityMaterialEstimated AS FirmOrderQuantityMaterialEstimated, FirmOrders.QuantityMaterialEstimatedIssued AS FirmOrderQuantityMaterialEstimatedIssued, ROUND(FirmOrders.QuantityMaterialEstimated - FirmOrders.QuantityMaterialEstimatedIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityMaterialEstimatedRemains, " + "\r\n";
            queryString = queryString + "                       ProductionOrderDetails.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, Warehouses.Name AS WarehouseName " + "\r\n";

            queryString = queryString + "       FROM           (SELECT FirmOrderID, ROUND(SUM(Quantity - (QuantitySemifinished - QuantityShortage - QuantityFailure + QuantityExcess)), " + (int)GlobalEnums.rndQuantity + ") AS TotalQuantityRemains FROM FirmOrderDetails WHERE NMVNTaskID = @NMVNTaskID - 1000 AND Approved = 1 AND InActive = 0 AND InActivePartial = 0 AND (@FirmOrderID IS NULL OR FirmOrderID = @FirmOrderID) AND ROUND(Quantity - (QuantitySemifinished - QuantityShortage - QuantityFailure + QuantityExcess), " + (int)GlobalEnums.rndQuantity + ") > 0 GROUP BY FirmOrderID) AS FirmOrderRemains " + "\r\n";
            queryString = queryString + "                       INNER JOIN ProductionOrderDetails ON ProductionOrderDetails.Approved = 1 AND ProductionOrderDetails.InActive = 0 AND ProductionOrderDetails.InActivePartial = 0 AND FirmOrderRemains.FirmOrderID = ProductionOrderDetails.FirmOrderID " + "\r\n";//LocationID = @LocationID AND 
            queryString = queryString + "                       INNER JOIN FirmOrders ON (@NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.ProductWorkOrder + " OR ROUND(FirmOrders.QuantityMaterialEstimated - FirmOrders.QuantityMaterialEstimatedIssued, " + (int)GlobalEnums.rndQuantity + ") >0) AND FirmOrderRemains.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";

            queryString = queryString + "                       INNER JOIN Customers ON ProductionOrderDetails.CustomerID = Customers.CustomerID " + "\r\n";

            queryString = queryString + "                       INNER JOIN Warehouses ON Warehouses.WarehouseID = IIF(@NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.ProductWorkOrder + ", 6, 1) " + "\r\n";


            this.totalSmartPortalEntities.CreateStoredProcedure("GetWorkOrderPendingFirmOrders", queryString);
        }


        private void WorkOrderSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       BEGIN  " + "\r\n";

            queryString = queryString + "           DECLARE @msg NVARCHAR(300) = '' ";

            queryString = queryString + "           IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   IF (NOT EXISTS(SELECT ProductionOrderDetailID FROM ProductionOrderDetails WHERE ProductionOrderDetailID = (SELECT TOP 1 ProductionOrderDetailID FROM WorkOrders WHERE WorkOrderID = @EntityID) AND ProductionOrderDetails.Approved = 1 AND ProductionOrderDetails.InActive = 0 AND ProductionOrderDetails.InActivePartial = 0)) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       SET         @msg = N'Lệnh sản xuất đã hủy, chưa duyệt hoặc đã xóa.' ; " + "\r\n";
            queryString = queryString + "                       THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";
            queryString = queryString + "               END " + "\r\n";


            queryString = queryString + "           UPDATE          FirmOrders " + "\r\n";
            queryString = queryString + "           SET             FirmOrders.QuantityMaterialEstimatedIssued = ROUND(FirmOrders.QuantityMaterialEstimatedIssued + WorkOrders.QuantityMaterialEstimated * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "           FROM            FirmOrders  " + "\r\n";
            queryString = queryString + "                           INNER JOIN WorkOrders ON WorkOrders.WorkOrderID = @EntityID AND FirmOrders.FirmOrderID = WorkOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "           IF @@ROWCOUNT <> 1 " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   SET         @msg = N'Kế hoạch sản xuất đã hủy, chưa duyệt hoặc đã xóa.' ; " + "\r\n";
            queryString = queryString + "                   THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";

            queryString = queryString + "           UPDATE          FirmOrderMaterials " + "\r\n";
            queryString = queryString + "           SET             FirmOrderMaterials.QuantityIssued = ROUND(FirmOrderMaterials.QuantityIssued + WorkOrderDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "           FROM            FirmOrderMaterials " + "\r\n";
            queryString = queryString + "                           INNER JOIN WorkOrderDetails ON ((FirmOrderMaterials.Approved = 1 AND FirmOrderMaterials.InActive = 0 AND FirmOrderMaterials.InActivePartial = 0) OR @SaveRelativeOption = -1) AND WorkOrderDetails.WorkOrderID = @EntityID AND FirmOrderMaterials.FirmOrderMaterialID = WorkOrderDetails.FirmOrderMaterialID " + "\r\n";

            queryString = queryString + "           IF @@ROWCOUNT <> (SELECT COUNT(*) FROM WorkOrderDetails WHERE WorkOrderID = @EntityID) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   SET         @msg = N'Kế hoạch sản xuất chi tiết đã hủy, chưa duyệt hoặc đã xóa.' ; " + "\r\n";
            queryString = queryString + "                   THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";

            queryString = queryString + "       END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("WorkOrderSaveRelative", queryString);
        }

        private void WorkOrderPostSaveValidate()
        {
            string[] queryArray = new string[3];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Yêu cầu NVL: ' + CAST(ProductionOrders.EntryDate AS nvarchar) FROM WorkOrders INNER JOIN ProductionOrders ON WorkOrders.WorkOrderID = @EntityID AND WorkOrders.ProductionOrderID = ProductionOrders.ProductionOrderID AND WorkOrders.EntryDate < ProductionOrders.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Số lượng yêu cầu vượt quá số định mức nguyên vật liệu: ' + CAST(ROUND(QuantityMaterialEstimated - QuantityMaterialEstimatedIssued, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM FirmOrders WHERE (NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.PlannedItem + " AND ROUND(QuantityMaterialEstimated - QuantityMaterialEstimatedIssued, " + (int)GlobalEnums.rndQuantity + ") < 0) ";
            queryArray[2] = " SELECT TOP 1 @FoundEntity = N'Số lượng yêu cầu vượt quá số định mức nguyên vật liệu: ' + CAST(ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM FirmOrderMaterials WHERE (NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.PlannedItem + " AND ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("WorkOrderPostSaveValidate", queryArray);
        }




        private void WorkOrderApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = WorkOrderID FROM WorkOrders WHERE WorkOrderID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("WorkOrderApproved", queryArray);
        }


        private void WorkOrderEditable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = WorkOrderID FROM MaterialIssues WHERE WorkOrderID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("WorkOrderEditable", queryArray);
        }

        private void WorkOrderToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      WorkOrders  SET Approved = @Approved, ApprovedDate = GetDate() WHERE WorkOrderID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE      WorkOrderDetails  SET Approved = @Approved WHERE WorkOrderID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, N'hủy', '')  + N' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("WorkOrderToggleApproved", queryString);
        }


        private void WorkOrderInitReference()
        {
            SimpleInitReference simpleInitReference = new WorkOrderInitReference("WorkOrders", "WorkOrderID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.WorkOrder));
            this.totalSmartPortalEntities.CreateTrigger("WorkOrderInitReference", simpleInitReference.CreateQuery());
        }

        private void WorkOrderSheet()
        {
            string queryString = " @WorkOrderID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @LocalWorkOrderID int    SET @LocalWorkOrderID = @WorkOrderID" + "\r\n";

            queryString = queryString + "       SELECT          WorkOrders.WorkOrderID, WorkOrders.EntryDate AS WorkOrderEntryDate, WorkOrders.Reference, Workshifts.Code AS WorkshiftCode, ProductionLines.Code AS ProductionLineCode, " + "\r\n";
            queryString = queryString + "                       FirmOrders.EntryDate AS FirmOrderEntryDate, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, FirmOrders.Specs, FirmOrders.Specification, FirmOrders.TotalQuantity AS FirmOrderTotalQuantity, Customers.Name AS CustomerName, " + "\r\n";
            queryString = queryString + "                       Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, WorkOrderDetails.BatchEntryDate, WorkOrderDetails.Quantity, ISNULL(FirmOrders.Description, '') + ' ' + ISNULL(WorkOrders.Description, '') AS Description " + "\r\n";

            queryString = queryString + "       FROM            WorkOrders " + "\r\n";
            queryString = queryString + "                       INNER JOIN WorkOrderDetails ON WorkOrders.WorkOrderID = @LocalWorkOrderID AND WorkOrders.WorkOrderID = WorkOrderDetails.WorkOrderID " + "\r\n";
            queryString = queryString + "                       INNER JOIN FirmOrders ON WorkOrders.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON WorkOrders.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Workshifts ON WorkOrders.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                       INNER JOIN ProductionLines ON WorkOrders.ProductionLineID = ProductionLines.ProductionLineID" + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities ON WorkOrderDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "       ORDER BY        WorkOrderDetails.WorkOrderDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("WorkOrderSheet", queryString);
        }

    }
}


