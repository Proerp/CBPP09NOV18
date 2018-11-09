using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Productions
{
    public class SemifinishedProduct
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public SemifinishedProduct(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetSemifinishedProductIndexes();

            this.GetSemifinishedProductPendingMaterialIssueDetails();
            this.GetSemifinishedProductPendingMaterialQuantityRemains();

            this.GetSemifinishedProductViewDetails();

            this.SemifinishedProductSaveRelative();
            this.SemifinishedProductPostSaveValidate();

            this.SemifinishedProductApproved();
            this.SemifinishedProductEditable();

            this.SemifinishedProductToggleApproved();

            this.SemifinishedProductInitReference();

            this.SemifinishedProductSheet();
        }


        private void GetSemifinishedProductIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      SemifinishedProducts.SemifinishedProductID, CAST(SemifinishedProducts.EntryDate AS DATE) AS EntryDate, SemifinishedProducts.Reference, Locations.Code AS LocationCode, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, ProductionLines.Code AS ProductionLineCode, Workshifts.EntryDate AS WorkshiftEntryDate, Workshifts.Code AS WorkshiftCode, FirmOrders.Reference AS FirmOrdersReference, FirmOrders.Code AS FirmOrdersCode, FirmOrders.Specification, SemifinishedProducts.StartSequenceNo, SemifinishedProducts.StopSequenceNo, SemifinishedProducts.Description, SemifinishedProducts.TotalQuantity, SemifinishedProducts.Approved " + "\r\n";
            queryString = queryString + "       FROM        SemifinishedProducts " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON SemifinishedProducts.EntryDate >= @FromDate AND SemifinishedProducts.EntryDate <= @ToDate AND SemifinishedProducts.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.SemifinishedProduct + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = SemifinishedProducts.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN FirmOrders ON SemifinishedProducts.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON SemifinishedProducts.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON SemifinishedProducts.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN ProductionLines ON SemifinishedProducts.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetSemifinishedProductIndexes", queryString);
        }

        #region X


        private void GetSemifinishedProductViewDetails()
        {
            string queryString;

            queryString = " @SemifinishedProductID Int, @FirmOrderID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT          ISNULL(SemifinishedProductDetails.SemifinishedProductDetailID, 0) AS SemifinishedProductDetailID, ISNULL(SemifinishedProductDetails.SemifinishedProductID, 0) AS SemifinishedProductID, FirmOrderDetails.FirmOrderDetailID, FirmOrderDetails.PlannedOrderID, FirmOrderDetails.PlannedOrderDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, FirmOrderDetails.PiecePerPack, " + "\r\n";
            queryString = queryString + "                       FirmOrderDetails.MoldQuantity, ROUND(FirmOrderDetails.Quantity - (FirmOrderDetails.QuantitySemifinished - FirmOrderDetails.QuantityShortage - FirmOrderDetails.QuantityFailure + FirmOrderDetails.QuantityExcess) + ISNULL(SemifinishedProductDetails.Quantity, 0), " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, ISNULL(SemifinishedProductDetails.Quantity, 0) AS Quantity, SemifinishedProductDetails.Remarks " + "\r\n";
            
            queryString = queryString + "       FROM            FirmOrderDetails " + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities ON FirmOrderDetails.FirmOrderID = @FirmOrderID AND FirmOrderDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                       LEFT JOIN SemifinishedProductDetails ON SemifinishedProductDetails.SemifinishedProductID = @SemifinishedProductID AND FirmOrderDetails.FirmOrderDetailID = SemifinishedProductDetails.FirmOrderDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetSemifinishedProductViewDetails", queryString);
        }

        private void GetSemifinishedProductPendingMaterialIssueDetails()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          MaterialIssueDetails.MaterialIssueDetailID, MaterialIssueDetails.MaterialIssueID, MaterialIssueDetails.PlannedOrderID, MaterialIssueDetails.FirmOrderID, FirmOrders.EntryDate AS FirmOrderEntryDate, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, FirmOrders.Specification AS FirmOrderSpecification, MaterialIssueDetails.GoodsReceiptID, GoodsReceipts.Reference AS GoodsReceiptReference, GoodsReceipts.Code AS GoodsReceiptCode, GoodsReceipts.EntryDate AS GoodsReceiptEntryDate, MaterialIssueDetails.GoodsReceiptDetailID, " + "\r\n";
            queryString = queryString + "                       MaterialIssueDetails.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, MaterialIssueDetails.WorkshiftID AS MaterialIssueDetailWorkshiftID, Workshifts.EntryDate AS MaterialIssueDetailWorkshiftEntryDate, Workshifts.Code AS MaterialIssueDetailWorkshiftCode, MaterialIssueDetails.ProductionLineID, ProductionLines.Code AS ProductionLineCode, MaterialIssueDetails.CrucialWorkerID, CrucialWorkers.Code AS CrucialWorkerCode, CrucialWorkers.Name AS CrucialWorkerName, Materials.Code AS MaterialCode, Materials.Name AS MaterialName, MaterialIssueDetails.Quantity AS MaterialQuantity, ROUND(MaterialIssueDetails.Quantity - MaterialIssueDetails.QuantitySemifinished - MaterialIssueDetails.QuantityFailure - MaterialIssueDetails.QuantityReceipted - MaterialIssueDetails.QuantityLoss, " + (int)GlobalEnums.rndQuantity + ") AS MaterialQuantityRemains " + "\r\n";

            queryString = queryString + "       FROM            MaterialIssueDetails  " + "\r\n";
            queryString = queryString + "                       INNER JOIN FirmOrders ON MaterialIssueDetails.LocationID = @LocationID AND MaterialIssueDetails.Approved = 1 AND FirmOrders.InActive = 0 AND ROUND(MaterialIssueDetails.Quantity - MaterialIssueDetails.QuantitySemifinished - MaterialIssueDetails.QuantityFailure - MaterialIssueDetails.QuantityReceipted - MaterialIssueDetails.QuantityLoss, " + (int)GlobalEnums.rndQuantity + ") > 0 AND MaterialIssueDetails.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "                       INNER JOIN GoodsReceipts ON MaterialIssueDetails.GoodsReceiptID = GoodsReceipts.GoodsReceiptID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON MaterialIssueDetails.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Workshifts ON MaterialIssueDetails.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                       INNER JOIN ProductionLines ON MaterialIssueDetails.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Employees CrucialWorkers ON MaterialIssueDetails.CrucialWorkerID = CrucialWorkers.EmployeeID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities Materials ON MaterialIssueDetails.CommodityID = Materials.CommodityID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetSemifinishedProductPendingMaterialIssueDetails", queryString);
        }

        private void GetSemifinishedProductPendingMaterialQuantityRemains()
        {
            string queryString = " @MaterialIssueDetailID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          ROUND(Quantity - QuantitySemifinished - QuantityFailure - QuantityReceipted - QuantityLoss, " + (int)GlobalEnums.rndQuantity + ") AS MaterialQuantityRemains " + "\r\n";
            queryString = queryString + "       FROM            MaterialIssueDetails WHERE MaterialIssueDetailID = @MaterialIssueDetailID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetSemifinishedProductPendingMaterialQuantityRemains", queryString);
        }

        private void SemifinishedProductSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN  " + "\r\n";
            
            queryString = queryString + "       DECLARE @msg NVARCHAR(300) ";


            #region UPDATE WorkshiftID
            queryString = queryString + "       DECLARE         @EntryDate Datetime, @ShiftID int, @WorkshiftID int " + "\r\n";
            queryString = queryString + "       SELECT          @EntryDate = CONVERT(date, EntryDate), @ShiftID = ShiftID FROM SemifinishedProducts WHERE SemifinishedProductID = @EntityID " + "\r\n";
            queryString = queryString + "       SET             @WorkshiftID = (SELECT TOP 1 WorkshiftID FROM Workshifts WHERE EntryDate = @EntryDate AND ShiftID = @ShiftID) " + "\r\n";

            queryString = queryString + "       IF             (@WorkshiftID IS NULL) " + "\r\n";
            queryString = queryString + "           BEGIN ";
            queryString = queryString + "               INSERT INTO     Workshifts(EntryDate, ShiftID, Code, Name, WorkingHours, Remarks) SELECT @EntryDate, ShiftID, Code, Name, WorkingHours, Remarks FROM Shifts WHERE ShiftID = @ShiftID " + "\r\n";
            queryString = queryString + "               SELECT          @WorkshiftID = SCOPE_IDENTITY(); " + "\r\n";
            queryString = queryString + "           END ";

            queryString = queryString + "       UPDATE          SemifinishedProducts        SET WorkshiftID = @WorkshiftID WHERE SemifinishedProductID = @EntityID " + "\r\n";
            queryString = queryString + "       UPDATE          SemifinishedProductDetails  SET WorkshiftID = @WorkshiftID WHERE SemifinishedProductID = @EntityID " + "\r\n";
            #endregion UPDATE WorkshiftID


            queryString = queryString + "       DECLARE         @SemifinishedProductDetails TABLE (FirmOrderDetailID int NOT NULL PRIMARY KEY, PlannedOrderDetailID int NOT NULL, Quantity decimal(18, 2) NOT NULL)" + "\r\n";
            queryString = queryString + "       INSERT INTO     @SemifinishedProductDetails (FirmOrderDetailID, PlannedOrderDetailID, Quantity) SELECT FirmOrderDetailID, PlannedOrderDetailID, SUM(Quantity) AS Quantity FROM SemifinishedProductDetails WHERE SemifinishedProductID = @EntityID GROUP BY FirmOrderDetailID, PlannedOrderDetailID " + "\r\n";

            queryString = queryString + "       UPDATE          FirmOrderDetails " + "\r\n";
            queryString = queryString + "       SET             FirmOrderDetails.QuantitySemifinished = ROUND(FirmOrderDetails.QuantitySemifinished + SemifinishedProductDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "       FROM            FirmOrderDetails " + "\r\n";
            queryString = queryString + "                       INNER JOIN @SemifinishedProductDetails SemifinishedProductDetails ON ((FirmOrderDetails.Approved = 1 AND FirmOrderDetails.InActive = 0 AND FirmOrderDetails.InActivePartial = 0) OR @SaveRelativeOption = -1) AND FirmOrderDetails.FirmOrderDetailID = SemifinishedProductDetails.FirmOrderDetailID " + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT <> (SELECT COUNT(*) FROM @SemifinishedProductDetails) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               SET         @msg = N'Kế hoạch sản xuất đã hủy, chưa duyệt hoặc đã xóa.' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       UPDATE          PlannedOrderDetails " + "\r\n";
            queryString = queryString + "       SET             PlannedOrderDetails.QuantitySemifinished = ROUND(PlannedOrderDetails.QuantitySemifinished + SemifinishedProductDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "       FROM            PlannedOrderDetails " + "\r\n";
            queryString = queryString + "                       INNER JOIN @SemifinishedProductDetails SemifinishedProductDetails ON ((PlannedOrderDetails.Approved = 1 AND PlannedOrderDetails.InActive = 0 AND PlannedOrderDetails.InActivePartial = 0) OR @SaveRelativeOption = -1) AND PlannedOrderDetails.PlannedOrderDetailID = SemifinishedProductDetails.PlannedOrderDetailID " + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT <> (SELECT COUNT(*) FROM @SemifinishedProductDetails) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               SET         @msg = N'Kế hoạch sản xuất đã hủy, chưa duyệt hoặc đã xóa.' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       UPDATE          MaterialIssueDetails " + "\r\n";
            queryString = queryString + "       SET             MaterialIssueDetails.QuantitySemifinished = ROUND(MaterialIssueDetails.QuantitySemifinished + SemifinishedProducts.FoilWeights * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + "), MaterialIssueDetails.QuantityFailure = ROUND(MaterialIssueDetails.QuantityFailure + SemifinishedProducts.FailureWeights * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "       FROM            MaterialIssueDetails " + "\r\n";
            queryString = queryString + "                       INNER JOIN SemifinishedProducts ON SemifinishedProducts.SemifinishedProductID = @EntityID AND MaterialIssueDetails.MaterialIssueDetailID = SemifinishedProducts.MaterialIssueDetailID " + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT <> 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               SET         @msg = N'Phiếu xuất NVL đã hủy, chưa duyệt hoặc đã xóa.' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";


            this.totalSmartPortalEntities.CreateStoredProcedure("SemifinishedProductSaveRelative", queryString);
        }

        private void SemifinishedProductPostSaveValidate()
        {
            string[] queryArray = new string[3];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày xuất nguyên liệu: ' + CAST(MaterialIssues.EntryDate AS nvarchar) FROM SemifinishedProductDetails INNER JOIN MaterialIssues ON SemifinishedProductDetails.SemifinishedProductID = @EntityID AND SemifinishedProductDetails.MaterialIssueID = MaterialIssues.MaterialIssueID AND SemifinishedProductDetails.EntryDate < MaterialIssues.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Số lượng xuất vượt quá số lượng đặt hàng: ' + CAST(ROUND(Quantity - (QuantitySemifinished - QuantityShortage - QuantityFailure + QuantityExcess), " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM FirmOrderDetails WHERE 1 = 0 AND (ROUND(Quantity - (QuantitySemifinished - QuantityShortage - QuantityFailure + QuantityExcess), " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            queryArray[2] = " SELECT TOP 1 @FoundEntity = N'Số lượng NVL sử dụng vượt quá số lượng đã xuất kho cho sản xuất: ' + CAST(ROUND(Quantity - QuantitySemifinished - QuantityFailure - QuantityReceipted - QuantityLoss, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM MaterialIssueDetails WHERE (ROUND(Quantity - QuantitySemifinished - QuantityFailure - QuantityReceipted - QuantityLoss, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("SemifinishedProductPostSaveValidate", queryArray);
        }




        private void SemifinishedProductApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = SemifinishedProductID FROM SemifinishedProducts WHERE SemifinishedProductID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("SemifinishedProductApproved", queryArray);
        }


        private void SemifinishedProductEditable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = SemifinishedProductID FROM SemifinishedProducts WHERE SemifinishedProductID = @EntityID AND NOT SemifinishedHandoverID IS NULL ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = SemifinishedProductID FROM SemifinishedHandoverDetails WHERE SemifinishedProductID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("SemifinishedProductEditable", queryArray);
        }

        private void SemifinishedProductToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      SemifinishedProducts  SET Approved = @Approved, ApprovedDate = GetDate() WHERE SemifinishedProductID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          SemifinishedProductDetails  SET Approved = @Approved WHERE SemifinishedProductID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, N'hủy', '')  + N' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("SemifinishedProductToggleApproved", queryString);
        }

        private void SemifinishedProductInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("SemifinishedProducts", "SemifinishedProductID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.SemifinishedProduct));
            this.totalSmartPortalEntities.CreateTrigger("SemifinishedProductInitReference", simpleInitReference.CreateQuery());
        }


        #endregion


        private void SemifinishedProductSheet()
        {
            string queryString = " @SemifinishedProductID int " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @LocalSemifinishedProductID int    SET @LocalSemifinishedProductID = @SemifinishedProductID" + "\r\n";

            queryString = queryString + "       SELECT          SemifinishedProducts.SemifinishedProductID, SemifinishedProducts.Reference, SemifinishedProducts.EntryDate, Workshifts.Code AS WorkshiftCode, CrucialWorkers.Name AS CrucialWorkerName, ProductionLines.Code AS ProductionLineCode, " + "\r\n";
            queryString = queryString + "                       FirmOrders.EntryDate AS FirmOrderEntryDate, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, Customers.Name AS CustomerName, Materials.Code AS MaterialCode, MaterialIssueDetails.BatchEntryDate, MaterialIssueDetails.Quantity AS MaterialQuantity, " + "\r\n";
            queryString = queryString + "                       SemifinishedProducts.StartSequenceNo, SemifinishedProducts.StopSequenceNo, SemifinishedProducts.FoilCounts, SemifinishedProducts.FoilUnitCounts, SemifinishedProducts.FoilUnitWeights, SemifinishedProducts.FoilWeights, SemifinishedProducts.FailureWeights, Commodities.Code, Commodities.Name, SemifinishedProductDetails.Quantity, SemifinishedProductDetails.MoldQuantity, SemifinishedProductDetails.PiecePerPack, ISNULL(FirmOrders.Description, '') + ISNULL(' [ĐHCK: ' + SemifinishedProducts.Description + ']', '') AS Description " + "\r\n";

            queryString = queryString + "       FROM            SemifinishedProducts " + "\r\n";
            queryString = queryString + "                       INNER JOIN SemifinishedProductDetails ON SemifinishedProducts.SemifinishedProductID = @LocalSemifinishedProductID AND SemifinishedProducts.SemifinishedProductID = SemifinishedProductDetails.SemifinishedProductID " + "\r\n";
            queryString = queryString + "                       INNER JOIN FirmOrders ON SemifinishedProducts.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON SemifinishedProducts.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN MaterialIssueDetails ON SemifinishedProducts.MaterialIssueDetailID = MaterialIssueDetails.MaterialIssueDetailID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities AS Materials ON MaterialIssueDetails.CommodityID = Materials.CommodityID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Workshifts ON SemifinishedProducts.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Employees AS CrucialWorkers ON SemifinishedProducts.CrucialWorkerID = CrucialWorkers.EmployeeID " + "\r\n";
            queryString = queryString + "                       INNER JOIN ProductionLines ON SemifinishedProducts.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities ON SemifinishedProductDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "       ORDER BY        SemifinishedProductDetails.SemifinishedProductDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";


            this.totalSmartPortalEntities.CreateStoredProcedure("SemifinishedProductSheet", queryString);
        }

    }
}
