using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Inventories
{
    public class TransferOrder
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public TransferOrder(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetTransferOrderIndexes();

            this.GetTransferOrderViewDetails();

            this.GetTransferOrderAvailableWarehouses();
            this.GetTransferOrderPendingBlendingInstructions();

            this.TransferOrderApproved();
            this.TransferOrderEditable();
            this.TransferOrderVoidable();

            this.TransferOrderToggleApproved();
            this.TransferOrderToggleVoid();
            this.TransferOrderToggleVoidDetail();

            this.TransferOrderInitReference();
        }


        private void GetTransferOrderIndexes()
        {
            string queryString;

            queryString = " @NMVNTaskID int, @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      TransferOrders.TransferOrderID, CAST(TransferOrders.EntryDate AS DATE) AS EntryDate, TransferOrders.Reference, Locations.Code AS LocationCode, TransferOrders.TransferOrderTypeID, TransferOrderTypes.Name AS TransferOrderTypeName, TransferOrders.TransferOrderJobs, TransferOrders.Caption, ISNULL(VoidTypes.Name, CASE TransferOrders.InActivePartial WHEN 1 THEN N'Hủy một phần đh' ELSE N'' END) AS VoidTypeName, TransferOrders.Description, TransferOrders.TotalQuantity, TransferOrders.Approved, TransferOrders.InActive, TransferOrders.InActivePartial " + "\r\n";
            queryString = queryString + "       FROM        TransferOrders " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON TransferOrders.NMVNTaskID = @NMVNTaskID AND TransferOrders.EntryDate >= @FromDate AND TransferOrders.EntryDate <= @ToDate AND TransferOrders.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = @NMVNTaskID AND AccessControls.AccessLevel > 0) AND Locations.LocationID = TransferOrders.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN TransferOrderTypes ON TransferOrders.TransferOrderTypeID = TransferOrderTypes.TransferOrderTypeID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN VoidTypes ON TransferOrders.VoidTypeID = VoidTypes.VoidTypeID" + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetTransferOrderIndexes", queryString);
        }


        #region X


        private void GetTransferOrderViewDetails()
        {
            string queryString;

            queryString = " @TransferOrderID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @TransferOrderDetails TABLE (TransferOrderDetailID int NOT NULL, TransferOrderID int NOT NULL, CommodityID int NOT NULL, Quantity decimal(18, 2) NOT NULL, Remarks nvarchar(100) NULL, VoidTypeID int, InActivePartial bit) " + "\r\n";
            queryString = queryString + "       INSERT INTO @TransferOrderDetails (TransferOrderDetailID, TransferOrderID, CommodityID, Quantity, Remarks, VoidTypeID, InActivePartial) SELECT TransferOrderDetailID, TransferOrderID, CommodityID, Quantity, Remarks, VoidTypeID, InActivePartial FROM TransferOrderDetails WHERE TransferOrderID = @TransferOrderID " + "\r\n";

            queryString = queryString + "       SELECT      TransferOrderDetails.TransferOrderDetailID, TransferOrderDetails.TransferOrderID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   ISNULL(CommoditiesAvailables.QuantityAvailables, 0) AS QuantityAvailables, TransferOrderDetails.Quantity, TransferOrderDetails.Remarks, " + "\r\n";
            queryString = queryString + "                   VoidTypes.VoidTypeID, VoidTypes.Code AS VoidTypeCode, VoidTypes.Name AS VoidTypeName, VoidTypes.VoidClassID, TransferOrderDetails.InActivePartial " + "\r\n";
            queryString = queryString + "       FROM        @TransferOrderDetails TransferOrderDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON TransferOrderDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN (SELECT CommodityID, SUM(Quantity - QuantityIssued) AS QuantityAvailables FROM GoodsReceiptDetails WHERE ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 AND WarehouseID = (SELECT TOP 1 WarehouseID FROM @TransferOrderDetails) AND CommodityID IN (SELECT DISTINCT CommodityID FROM @TransferOrderDetails) GROUP BY CommodityID) CommoditiesAvailables ON TransferOrderDetails.CommodityID = CommoditiesAvailables.CommodityID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN VoidTypes ON TransferOrderDetails.VoidTypeID = VoidTypes.VoidTypeID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetTransferOrderViewDetails", queryString);
        }


        #region Pending

        private void GetTransferOrderAvailableWarehouses()
        {
            string queryString = " @LocationID int, @NMVNTaskID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, Warehouses.Name AS WarehouseName, Warehouses.LocationID AS LocationIssuedID, WarehouseReceipts.WarehouseID AS WarehouseReceiptID, WarehouseReceipts.Code AS WarehouseReceiptCode, WarehouseReceipts.Name AS WarehouseReceiptName, WarehouseReceipts.LocationID AS LocationReceiptID " + "\r\n";
            queryString = queryString + "       FROM            Warehouses  " + "\r\n";
            queryString = queryString + "                       INNER JOIN Warehouses AS WarehouseReceipts ON Warehouses.WarehouseID <> WarehouseReceipts.WarehouseID " + "\r\n";
            queryString = queryString + "       WHERE           Warehouses.WarehouseID IN (1) AND WarehouseReceipts.WarehouseID IN (6) " + "\r\n";

            queryString = queryString + "       ORDER BY        WarehouseID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetTransferOrderAvailableWarehouses", queryString);
        }

        private void GetTransferOrderPendingBlendingInstructions()
        {
            string queryString;

            queryString = " @LocationID Int, @TransferOrderID Int, @CommodityIDs varchar(3999) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @BlendingInstructionDetails TABLE (CommodityID int NOT NULL, QuantityRemains decimal(18, 2) NOT NULL, QuantityTransferOrders decimal(18, 2) NOT NULL, QuantityAvailableL2 decimal(18, 2) NOT NULL) " + "\r\n";


            queryString = queryString + "       INSERT INTO     @BlendingInstructionDetails (CommodityID, QuantityRemains, QuantityTransferOrders, QuantityAvailableL2) " + "\r\n";
            queryString = queryString + "       SELECT          CommodityID, ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, 0 AS QuantityTransferOrders, 0 AS QuantityAvailableL2 FROM BlendingInstructionDetails WHERE InActive = 0 AND InActivePartial = 0 AND ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 " + "\r\n"; //Approved = 1 AND 

            queryString = queryString + "       INSERT INTO     @BlendingInstructionDetails (CommodityID, QuantityRemains, QuantityTransferOrders, QuantityAvailableL2) " + "\r\n";
            queryString = queryString + "       SELECT          CommodityID, 0 AS QuantityRemains, ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityTransferOrders, 0 AS QuantityAvailableL2 FROM TransferOrderDetails WHERE CommodityID IN (SELECT CommodityID FROM @BlendingInstructionDetails) AND LocationIssuedID = 1 AND LocationReceiptID = 2 AND TransferOrderID <> @TransferOrderID AND InActive = 0 AND InActivePartial = 0 AND ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 " + "\r\n"; //Approved = 1 AND 

            queryString = queryString + "       INSERT INTO     @BlendingInstructionDetails (CommodityID, QuantityRemains, QuantityTransferOrders, QuantityAvailableL2) " + "\r\n";
            queryString = queryString + "       SELECT          GoodsReceiptDetails.CommodityID, 0 AS QuantityRemains, 0 AS QuantityTransferOrders, ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains FROM GoodsReceiptDetails INNER JOIN Warehouses ON GoodsReceiptDetails.WarehouseID = Warehouses.WarehouseID WHERE GoodsReceiptDetails.CommodityID IN (SELECT CommodityID FROM @BlendingInstructionDetails) AND Warehouses.LocationID = 2 AND ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 " + "\r\n";


            queryString = queryString + "       SELECT          BlendingInstructionDetails.CommodityID, MIN(Commodities.Code) AS CommodityCode, MIN(Commodities.Name) AS CommodityName, MIN(Commodities.OfficialCode) AS OfficialCode, MIN(Commodities.CommodityTypeID) AS CommodityTypeID, SUM(BlendingInstructionDetails.QuantityRemains) AS QuantityRemains, SUM(BlendingInstructionDetails.QuantityTransferOrders) AS QuantityTransferOrders, SUM(BlendingInstructionDetails.QuantityAvailableL2) AS QuantityAvailableL2, CAST(0 AS bit) AS IsSelected " + "\r\n";
            queryString = queryString + "       FROM            @BlendingInstructionDetails BlendingInstructionDetails " + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities ON BlendingInstructionDetails.CommodityID NOT IN (SELECT Id FROM dbo.SplitToIntList (@CommodityIDs)) AND BlendingInstructionDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "       GROUP BY        BlendingInstructionDetails.CommodityID " + "\r\n";
            queryString = queryString + "       HAVING          ROUND(SUM(BlendingInstructionDetails.QuantityRemains) - SUM(BlendingInstructionDetails.QuantityTransferOrders) - SUM(BlendingInstructionDetails.QuantityAvailableL2), " + (int)GlobalEnums.rndQuantity + ") > 0 " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetTransferOrderPendingBlendingInstructions", queryString);
        }
        #endregion Pending

        private void TransferOrderApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = TransferOrderID FROM TransferOrders WHERE TransferOrderID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("TransferOrderApproved", queryArray);
        }


        private void TransferOrderEditable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = TransferOrderID FROM WarehouseTransfers WHERE TransferOrderID = @EntityID ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = TransferOrderID FROM WarehouseTransferDetails WHERE TransferOrderID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("TransferOrderEditable", queryArray);
        }

        private void TransferOrderVoidable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = TransferOrderID FROM TransferOrders WHERE TransferOrderID = @EntityID AND Approved = 0"; //Must approve in order to allow void

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("TransferOrderVoidable", queryArray);
        }

        private void TransferOrderToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      TransferOrders  SET Approved = @Approved, ApprovedDate = GetDate() WHERE TransferOrderID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          TransferOrderDetails  SET Approved = @Approved WHERE TransferOrderID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, N'hủy', '')  + N' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("TransferOrderToggleApproved", queryString);
        }


        private void TransferOrderToggleVoid()
        {
            string queryString = " @EntityID int, @InActive bit, @VoidTypeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      TransferOrders  SET InActive = @InActive, InActiveDate = GetDate(), VoidTypeID = IIF(@InActive = 1, @VoidTypeID, NULL) WHERE TransferOrderID = @EntityID AND InActive = ~@InActive" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          TransferOrderDetails     SET InActive = @InActive WHERE TransferOrderID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@InActive = 0, 'phục hồi lệnh', '')  + ' hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            this.totalSmartPortalEntities.CreateStoredProcedure("TransferOrderToggleVoid", queryString);
        }

        private void TransferOrderToggleVoidDetail()
        {
            string queryString = " @EntityID int, @EntityDetailID int, @InActivePartial bit, @VoidTypeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      TransferOrderDetails     SET InActivePartial = @InActivePartial, InActivePartialDate = GetDate(), VoidTypeID = IIF(@InActivePartial = 1, @VoidTypeID, NULL) WHERE TransferOrderID = @EntityID AND TransferOrderDetailID = @EntityDetailID AND InActivePartial = ~@InActivePartial ; " + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE         @MaxInActivePartial bit     SET @MaxInActivePartial = (SELECT MAX(CAST(InActivePartial AS int)) FROM TransferOrderDetails WHERE TransferOrderID = @EntityID) ;" + "\r\n";
            queryString = queryString + "               UPDATE          TransferOrders  SET InActivePartial = @MaxInActivePartial WHERE TransferOrderID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@InActivePartial = 0, 'phục hồi lệnh', '')  + ' hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            this.totalSmartPortalEntities.CreateStoredProcedure("TransferOrderToggleVoidDetail", queryString);
        }

        private void TransferOrderInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("TransferOrders", "TransferOrderID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.TransferOrder));
            this.totalSmartPortalEntities.CreateTrigger("TransferOrderInitReference", simpleInitReference.CreateQuery());
        }
        #endregion
    }
}
