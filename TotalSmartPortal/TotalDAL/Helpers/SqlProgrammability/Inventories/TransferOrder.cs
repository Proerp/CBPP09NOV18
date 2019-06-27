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
            this.GetTransferOrderPendingWorkOrders();
            this.GetTransferOrderPendingBlendingInstructions();
            this.CheckTransferOrderPendingBlendingInstructions();

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

            queryString = " @NMVNTaskID int, @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime, @LabOptionID int, @FilterOptionID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalAspUserID nvarchar(128), @LocalFromDate DateTime, @LocalToDate DateTime, @LocalLabOptionID int, @LocalFilterOptionID int " + "\r\n";
            queryString = queryString + "       SET         @LocalAspUserID = @AspUserID       SET @LocalFromDate = @FromDate      SET @LocalToDate = @ToDate          SET @LocalLabOptionID = @LabOptionID            SET @LocalFilterOptionID = @FilterOptionID" + "\r\n";

            queryString = queryString + "       DECLARE     @TransferOrderIndexes TABLE (TransferOrderID int NOT NULL, EntryDate datetime NOT NULL, Reference nvarchar(10) NOT NULL, WarehouseCode nvarchar(10) NULL, WarehouseReceiptCode nvarchar(10) NULL, Description nvarchar(100) NULL, TransferOrderJobs nvarchar(100) NULL, " + "\r\n";
            queryString = queryString + "                                                TransferOrderDetailID int NULL, CommodityCode nvarchar(50) NULL, CommodityName nvarchar(200) NULL, Weight decimal(18, 3) NOT NULL, Approved bit NOT NULL, InActive bit NOT NULL, InActivePartial bit NOT NULL, VoidTypeName nvarchar(50) NULL, " + "\r\n";
            queryString = queryString + "                                                Quantity decimal(18, 2) NULL, QuantityIssued decimal(18, 2) NULL, QuantityRemains decimal(18, 2) NULL, QuantityAvailables decimal(18, 2) NULL) " + "\r\n";

            queryString = queryString + "       IF  (@LocalFilterOptionID = 0) " + "\r\n";
            queryString = queryString + "           " + this.GetTransferOrderIndexSQL(0) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               IF  (@LocalFilterOptionID = 10) " + "\r\n";
            queryString = queryString + "                   " + this.GetTransferOrderIndexSQL(10) + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       IF  (@LocalFilterOptionID = 11) " + "\r\n";
            queryString = queryString + "                           " + this.GetTransferOrderIndexSQL(11) + "\r\n";
            queryString = queryString + "                       ELSE " + "\r\n";
            queryString = queryString + "                           BEGIN " + "\r\n";
            queryString = queryString + "                               IF  (@LocalFilterOptionID = 12) " + "\r\n";
            queryString = queryString + "                                   " + this.GetTransferOrderIndexSQL(12) + "\r\n";
            queryString = queryString + "                               ELSE " + "\r\n";
            queryString = queryString + "                                   " + this.GetTransferOrderIndexSQL(20) + "\r\n";
            queryString = queryString + "                           END " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            queryString = queryString + "       SELECT      TransferOrderID, EntryDate, Reference, WarehouseCode, WarehouseReceiptCode, Description, TransferOrderJobs, " + "\r\n";
            queryString = queryString + "                   TransferOrderDetailID, CommodityCode, CommodityName, IIF(Weight = 0, NULL, Weight) AS Weight, Approved, InActive, InActivePartial, VoidTypeName, " + "\r\n";
            queryString = queryString + "                   Quantity, IIF(QuantityIssued = 0, NULL, QuantityIssued) AS QuantityIssued, IIF(QuantityRemains = 0, NULL, QuantityRemains) AS QuantityRemains, IIF(QuantityRemains = 0, NULL, IIF(Weight = 0, QuantityRemains, QuantityRemains / Weight)) AS Packages, IIF(QuantityAvailables = 0, NULL, QuantityAvailables) AS QuantityAvailables " + "\r\n";

            queryString = queryString + "       FROM        @TransferOrderIndexes " + "\r\n";
            queryString = queryString + "       ORDER BY    EntryDate DESC, Reference DESC, CommodityCode " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetTransferOrderIndexes", queryString);
        }

        private string GetTransferOrderIndexSQL(int filterOptionID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@LocalLabOptionID = 1) " + "\r\n";
            queryString = queryString + "           " + this.GetTransferOrderIndexSQL(filterOptionID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetTransferOrderIndexSQL(filterOptionID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetTransferOrderIndexSQL(int filterOptionID, bool labOptionID)
        {

            //filterOptionID: 0: NORMAL  TransferOrderDetails LEFT JOIN TransferOrderDetails: FROM TO
            //filterOptionID: 10: PENDING TransferOrderDetails LEFT JOIN TransferOrderDetails WITH: NOT InActive AND (TransferOrderDetails IS NULL (NOT APPROVED YET) OR TransferOrderDetails.QuantityPending > 0))
            //filterOptionID: 11: 10 AND NOT MATERIAL
            //filterOptionID: 12: 10 AND WITH MATERIAL
            //filterOptionID: 20: FINISH  TransferOrderDetails INNER JOIN TransferOrderDetails: FROM TO

            string queryString = "";

            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       INSERT INTO @TransferOrderIndexes (TransferOrderID, EntryDate, Reference, WarehouseCode, WarehouseReceiptCode, Description, TransferOrderJobs, " + "\r\n";
            queryString = queryString + "                                                TransferOrderDetailID, CommodityCode, CommodityName, Weight, Approved, InActive, InActivePartial, VoidTypeName, " + "\r\n";
            queryString = queryString + "                                                Quantity, QuantityIssued, QuantityRemains, QuantityAvailables) " + "\r\n";

            queryString = queryString + "       SELECT      TransferOrders.TransferOrderID, CAST(" + "TransferOrders.EntryDate" + " AS DATE) AS EntryDate, TransferOrders.Reference, Warehouses.Code AS WarehouseCode, WarehouseReceipts.Code AS WarehouseReceiptCode, TransferOrderDetails.Remarks, TransferOrders.TransferOrderJobs, " + "\r\n";
            queryString = queryString + "                   TransferOrderDetails.TransferOrderDetailID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, ISNULL(Commodities.Weight, 0) AS Weight, TransferOrders.Approved, TransferOrders.InActive, TransferOrderDetails.InActivePartial, ISNULL(VoidTypes.Name, VoidTypeDetails.Name) AS VoidTypeName, " + "\r\n";
            queryString = queryString + "                   TransferOrderDetails.Quantity, TransferOrderDetails.QuantityIssued, ROUND(TransferOrderDetails.Quantity - TransferOrderDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, GoodsReceiptAvailables.QuantityAvailables " + "\r\n";

            queryString = queryString + "       FROM        TransferOrders " + "\r\n";
            queryString = queryString + "                   INNER JOIN  Warehouses ON " + (filterOptionID == 0 || filterOptionID == 20 ? "TransferOrders.EntryDate" + " >= @LocalFromDate AND " + "TransferOrders.EntryDate" + " <= @LocalToDate AND" : "") + " TransferOrders.NMVNTaskID = @NMVNTaskID AND TransferOrders.OrganizationalUnitID IN (SELECT DISTINCT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @LocalAspUserID AND AccessControls.NMVNTaskID = @NMVNTaskID AND AccessControls.AccessLevel > 0) AND TransferOrders.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                   INNER JOIN  TransferOrderDetails ON " + this.SQLPendingVsFinished(filterOptionID) + (filterOptionID == 11 || filterOptionID == 12 ? " TransferOrders.TransferOrderID " + (filterOptionID == 11 ? "NOT IN" : "IN") + "(SELECT TransferOrderID FROM TransferOrderDetails WHERE QuantityIssued <> 0)" + " AND " : "") + " TransferOrders.TransferOrderID = TransferOrderDetails.TransferOrderID " + "\r\n";
            queryString = queryString + "                   INNER JOIN  Warehouses WarehouseReceipts ON TransferOrders.WarehouseReceiptID = WarehouseReceipts.WarehouseID " + "\r\n";
            queryString = queryString + "                   INNER JOIN  Commodities ON TransferOrderDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            
            queryString = queryString + "                   LEFT JOIN  (SELECT WarehouseID, CommodityID, ROUND(SUM(Quantity - QuantityIssued), " + (int)GlobalEnums.rndQuantity + ") AS QuantityAvailables FROM GoodsReceiptDetails WHERE ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 " + (labOptionID ? " AND LabID IN (SELECT LabID FROM Labs WHERE Approved = 1 AND InActive = 0)" : "") + " GROUP BY WarehouseID, CommodityID) AS GoodsReceiptAvailables ON TransferOrderDetails.WarehouseID = GoodsReceiptAvailables.WarehouseID AND TransferOrderDetails.CommodityID = GoodsReceiptAvailables.CommodityID " + "\r\n";

            queryString = queryString + "                   LEFT JOIN   VoidTypes ON TransferOrders.VoidTypeID = VoidTypes.VoidTypeID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN   VoidTypes VoidTypeDetails ON TransferOrderDetails.VoidTypeID = VoidTypeDetails.VoidTypeID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            return queryString;
        }

        private string SQLPendingVsFinished(int filterOptionID)
        {
            bool pendingVsFinished = filterOptionID == 10 || filterOptionID == 11 || filterOptionID == 12;
            return filterOptionID == 0 ? "" : ("(TransferOrderDetails.InActive " + (pendingVsFinished ? "=" : "<>") + " 0 " + (pendingVsFinished ? "AND" : "OR") + " TransferOrderDetails.InActivePartial " + (pendingVsFinished ? "=" : "<>") + " 0 " + (pendingVsFinished ? "AND" : "OR") + " ROUND(TransferOrderDetails.Quantity - TransferOrderDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") " + (pendingVsFinished ? ">" : "<=") + " 0) AND ");
        }

        #region X


        private void GetTransferOrderViewDetails()
        {
            string queryString;

            queryString = " @TransferOrderID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @TransferOrderDetails TABLE (TransferOrderDetailID int NOT NULL, TransferOrderID int NOT NULL, WarehouseID int NOT NULL, WarehouseReceiptID int NOT NULL, CommodityID int NOT NULL, Quantity decimal(18, 2) NOT NULL, QuantityIssued decimal(18, 2) NOT NULL, Remarks nvarchar(100) NULL, VoidTypeID int, InActivePartial bit) " + "\r\n";
            queryString = queryString + "       INSERT INTO @TransferOrderDetails (TransferOrderDetailID, TransferOrderID, WarehouseID, WarehouseReceiptID, CommodityID, Quantity, QuantityIssued, Remarks, VoidTypeID, InActivePartial) SELECT TransferOrderDetailID, TransferOrderID, WarehouseID, WarehouseReceiptID, CommodityID, Quantity, QuantityIssued, Remarks, VoidTypeID, InActivePartial FROM TransferOrderDetails WHERE TransferOrderID = @TransferOrderID " + "\r\n";

            queryString = queryString + "       SELECT      TransferOrderDetails.TransferOrderDetailID, TransferOrderDetails.TransferOrderID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Commodities.Weight, " + "\r\n";
            queryString = queryString + "                   ISNULL(CommoditiesAvailables.QuantityAvailables, 0) AS QuantityAvailables, ISNULL(CommoditiesAvailableReceipts.QuantityAvailableReceipts, 0) AS QuantityAvailableReceipts, TransferOrderDetails.Quantity, TransferOrderDetails.QuantityIssued, TransferOrderDetails.Remarks, " + "\r\n";
            queryString = queryString + "                   VoidTypes.VoidTypeID, VoidTypes.Code AS VoidTypeCode, VoidTypes.Name AS VoidTypeName, VoidTypes.VoidClassID, TransferOrderDetails.InActivePartial " + "\r\n";
            queryString = queryString + "       FROM        @TransferOrderDetails TransferOrderDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON TransferOrderDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN (SELECT CommodityID, SUM(Quantity - QuantityIssued) AS QuantityAvailables FROM GoodsReceiptDetails WHERE ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 AND WarehouseID = (SELECT TOP 1 WarehouseID FROM @TransferOrderDetails) AND CommodityID IN (SELECT DISTINCT CommodityID FROM @TransferOrderDetails) GROUP BY CommodityID) CommoditiesAvailables ON TransferOrderDetails.CommodityID = CommoditiesAvailables.CommodityID " + "\r\n";

            queryString = queryString + "                   LEFT JOIN (SELECT CommodityID, SUM(Quantity - QuantityIssued) AS QuantityAvailableReceipts FROM GoodsReceiptDetails WHERE ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 AND WarehouseID = (SELECT TOP 1 WarehouseReceiptID FROM @TransferOrderDetails) AND CommodityID IN (SELECT DISTINCT CommodityID FROM @TransferOrderDetails) GROUP BY CommodityID) CommoditiesAvailableReceipts ON TransferOrderDetails.CommodityID = CommoditiesAvailableReceipts.CommodityID " + "\r\n";

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
            queryString = queryString + "       WHERE           Warehouses.WarehouseID IN (" + (GlobalEnums.CBPP ? "1, 6" : "2") + ") AND WarehouseReceipts.WarehouseID IN (" + (GlobalEnums.CBPP ? "1, 6" : "6") + ") " + "\r\n";

            queryString = queryString + "       ORDER BY        WarehouseID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetTransferOrderAvailableWarehouses", queryString);
        }

        private void CheckTransferOrderPendingBlendingInstructions()
        {
            string queryString;

            queryString = " @WarehouseReceiptID Int, @CommodityID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE @BlendingInstructionDetails TABLE (CommodityID int NOT NULL, Quantity decimal(18, 2) NOT NULL) " + "\r\n";
            queryString = queryString + "       INSERT INTO     @BlendingInstructionDetails (CommodityID, Quantity) " + "\r\n";
            queryString = queryString + "       SELECT          CommodityID,  ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS Quantity FROM BlendingInstructionDetails WHERE Approved = 1 AND InActive = 0 AND InActivePartial = 0 AND (ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") >= 1 OR (ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 AND QuantityIssued = 0)) " + "\r\n";
            queryString = queryString + "       INSERT INTO     @BlendingInstructionDetails (CommodityID, Quantity) " + "\r\n";
            queryString = queryString + "       SELECT          CommodityID, -ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS Quantity FROM GoodsReceiptDetails WHERE WarehouseID = @WarehouseReceiptID AND CommodityID IN (SELECT CommodityID FROM @BlendingInstructionDetails) AND ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 " + "\r\n";

            queryString = queryString + "       SELECT TOP 1 CommodityID FROM @BlendingInstructionDetails BlendingInstructionDetails WHERE CommodityID = @CommodityID GROUP BY CommodityID HAVING ROUND(SUM(Quantity), " + (int)GlobalEnums.rndQuantity + ") > 0 ";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("CheckTransferOrderPendingBlendingInstructions", queryString);
        }

        private void GetTransferOrderPendingBlendingInstructions()
        {
            string queryString;

            queryString = " @LocationID Int, @TransferOrderID Int, @WarehouseID Int, @WarehouseReceiptID Int, @CommodityIDs varchar(3999) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @BlendingInstructionDetails TABLE (CommodityID int NOT NULL, CustomerName nvarchar(100) NULL, BlendingInstructionCode nvarchar(50) NULL, BlendingInstructionSpecs nvarchar(800) NULL, BinLocationCode nvarchar(800) NULL, QuantityRemains decimal(18, 2) NOT NULL, QuantityTransferOrders decimal(18, 2) NOT NULL, QuantityAvailables decimal(18, 2) NOT NULL, QuantityAvailableReceipts decimal(18, 2) NOT NULL) " + "\r\n";


            queryString = queryString + "       INSERT INTO     @BlendingInstructionDetails (CommodityID, CustomerName, BlendingInstructionCode, BlendingInstructionSpecs, BinLocationCode, QuantityRemains, QuantityTransferOrders, QuantityAvailables, QuantityAvailableReceipts) " + "\r\n";
            queryString = queryString + "       SELECT          BlendingInstructionDetails.CommodityID, NULL, BlendingInstructions.Code, Commodities.Code, NULL, ROUND(BlendingInstructionDetails.Quantity - BlendingInstructionDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, 0 AS QuantityTransferOrders, 0 AS QuantityAvailables, 0 AS QuantityAvailableReceipts FROM BlendingInstructionDetails INNER JOIN BlendingInstructions ON BlendingInstructionDetails.BlendingInstructionID = BlendingInstructions.BlendingInstructionID INNER JOIN Commodities ON BlendingInstructions.CommodityID = Commodities.CommodityID WHERE BlendingInstructionDetails.InActive = 0 AND BlendingInstructionDetails.InActivePartial = 0 AND (ROUND(BlendingInstructionDetails.Quantity - BlendingInstructionDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") >= 1 OR (ROUND(BlendingInstructionDetails.Quantity - BlendingInstructionDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 AND BlendingInstructionDetails.QuantityIssued = 0)) AND BlendingInstructionDetails.CommodityID IN (SELECT DISTINCT CommodityID FROM GoodsArrivalDetails) " + "\r\n"; //Approved = 1 AND 

            queryString = queryString + "       INSERT INTO     @BlendingInstructionDetails (CommodityID, CustomerName, BlendingInstructionCode, BlendingInstructionSpecs, BinLocationCode, QuantityRemains, QuantityTransferOrders, QuantityAvailables, QuantityAvailableReceipts) " + "\r\n";
            queryString = queryString + "       SELECT          CommodityID, NULL, NULL, NULL, NULL, 0 AS QuantityRemains, ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityTransferOrders, 0 AS QuantityAvailables, 0 AS QuantityAvailableReceipts FROM TransferOrderDetails WHERE CommodityID IN (SELECT CommodityID FROM @BlendingInstructionDetails) AND WarehouseID = @WarehouseID AND WarehouseReceiptID = @WarehouseReceiptID AND TransferOrderID <> @TransferOrderID AND InActive = 0 AND InActivePartial = 0 AND ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 " + "\r\n"; //Approved = 1 AND 

            queryString = queryString + "       INSERT INTO     @BlendingInstructionDetails (CommodityID, CustomerName, BlendingInstructionCode, BlendingInstructionSpecs, BinLocationCode, QuantityRemains, QuantityTransferOrders, QuantityAvailables, QuantityAvailableReceipts) " + "\r\n";
            queryString = queryString + "       SELECT          GoodsReceiptDetails.CommodityID, NULL, NULL, NULL, BinLocations.Code AS BinLocationCode, 0 AS QuantityRemains, 0 AS QuantityTransferOrders, ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityAvailables, 0 AS QuantityAvailableReceipts FROM GoodsReceiptDetails INNER JOIN BinLocations ON GoodsReceiptDetails.BinLocationID = BinLocations.BinLocationID WHERE GoodsReceiptDetails.WarehouseID = @WarehouseID AND GoodsReceiptDetails.CommodityID IN (SELECT CommodityID FROM @BlendingInstructionDetails) AND ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 " + "\r\n";

            queryString = queryString + "       INSERT INTO     @BlendingInstructionDetails (CommodityID, CustomerName, BlendingInstructionCode, BlendingInstructionSpecs, BinLocationCode, QuantityRemains, QuantityTransferOrders, QuantityAvailables, QuantityAvailableReceipts) " + "\r\n";
            queryString = queryString + "       SELECT          GoodsReceiptDetails.CommodityID, NULL, NULL, NULL, NULL, 0 AS QuantityRemains, 0 AS QuantityTransferOrders, 0 AS QuantityAvailables, ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityAvailableReceipts FROM GoodsReceiptDetails WHERE GoodsReceiptDetails.WarehouseID = @WarehouseReceiptID AND GoodsReceiptDetails.CommodityID IN (SELECT CommodityID FROM @BlendingInstructionDetails) AND ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 " + "\r\n";


            queryString = queryString + "       SELECT          BlendingInstructionDetails.CommodityID, MIN(Commodities.Code) AS CommodityCode, MIN(Commodities.Name) AS CommodityName, MIN(Commodities.OfficialCode) AS OfficialCode, MIN(Commodities.CommodityTypeID) AS CommodityTypeID, MIN(Commodities.Weight) AS Weight, " + "\r\n";

            queryString = queryString + "                       STUFF ((SELECT DISTINCT ', ' + STUFFCustomerName.CustomerName    FROM    @BlendingInstructionDetails STUFFCustomerName     WHERE   STUFFCustomerName.CommodityID = BlendingInstructionDetails.CommodityID    AND NOT STUFFCustomerName.CustomerName IS NULL      FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)'),1,1,'') AS CustomerNames, " + "\r\n";
            queryString = queryString + "                       STUFF ((SELECT ', ' + STUFFFirmOrderCode.BlendingInstructionCode + ' [' + CAST(CAST(ROUND(STUFFFirmOrderCode.QuantityRemains, 0) AS INT) AS VARCHAR) + ']' FROM    @BlendingInstructionDetails STUFFFirmOrderCode    WHERE   STUFFFirmOrderCode.CommodityID = BlendingInstructionDetails.CommodityID   AND NOT STUFFFirmOrderCode.BlendingInstructionCode IS NULL    FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)'),1,1,'') AS Codes, " + "\r\n";
            queryString = queryString + "                       STUFF ((SELECT DISTINCT ', ' + STUFFSpecs.BlendingInstructionSpecs                  FROM    @BlendingInstructionDetails STUFFSpecs            WHERE   STUFFSpecs.CommodityID = BlendingInstructionDetails.CommodityID           AND NOT STUFFSpecs.BlendingInstructionSpecs IS NULL                    FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)'),1,1,'') AS Specs, " + "\r\n";
            queryString = queryString + "                       STUFF ((SELECT DISTINCT ', ' + STUFFBinLocations.BinLocationCode                    FROM    @BlendingInstructionDetails STUFFBinLocations     WHERE   STUFFBinLocations.CommodityID = BlendingInstructionDetails.CommodityID    AND NOT STUFFBinLocations.BinLocationCode IS NULL                      FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)'),1,1,'') AS BinLocationCodes, " + "\r\n";

            queryString = queryString + "                       ROUND(SUM(BlendingInstructionDetails.QuantityRemains), " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, ROUND(SUM(BlendingInstructionDetails.QuantityTransferOrders), " + (int)GlobalEnums.rndQuantity + ") AS QuantityTransferOrders, ROUND(SUM(BlendingInstructionDetails.QuantityAvailables), " + (int)GlobalEnums.rndQuantity + ") AS QuantityAvailables, ROUND(SUM(BlendingInstructionDetails.QuantityAvailableReceipts), " + (int)GlobalEnums.rndQuantity + ") AS QuantityAvailableReceipts, CAST(0 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM            @BlendingInstructionDetails BlendingInstructionDetails " + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities ON BlendingInstructionDetails.CommodityID NOT IN (SELECT Id FROM dbo.SplitToIntList (@CommodityIDs)) AND BlendingInstructionDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "       GROUP BY        BlendingInstructionDetails.CommodityID " + "\r\n";
            queryString = queryString + "       HAVING          ROUND(SUM(BlendingInstructionDetails.QuantityRemains) - SUM(BlendingInstructionDetails.QuantityTransferOrders) - SUM(BlendingInstructionDetails.QuantityAvailableReceipts), " + (int)GlobalEnums.rndQuantity + ") > 0 " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetTransferOrderPendingBlendingInstructions", queryString);
        }

        private void GetTransferOrderPendingWorkOrders()
        {
            string queryString;

            queryString = " @LocationID Int, @TransferOrderID Int, @WarehouseID Int, @WarehouseReceiptID Int, @CommodityIDs varchar(3999) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @WorkOrderDetails TABLE (CommodityID int NOT NULL, CustomerName nvarchar(100) NULL, FirmOrderCode nvarchar(50) NULL, FirmOrderSpecs nvarchar(800) NULL, QuantityRemains decimal(18, 2) NOT NULL, QuantityTransferOrders decimal(18, 2) NOT NULL, QuantityAvailables decimal(18, 2) NOT NULL, QuantityAvailableReceipts decimal(18, 2) NOT NULL) " + "\r\n";


            queryString = queryString + "       INSERT INTO     @WorkOrderDetails (CommodityID, CustomerName, FirmOrderCode, FirmOrderSpecs, QuantityRemains, QuantityTransferOrders, QuantityAvailables, QuantityAvailableReceipts) " + "\r\n";
            queryString = queryString + "       SELECT          WorkOrderDetails.CommodityID, Customers.Name, FirmOrders.Code, FirmOrders.Specs, ROUND(WorkOrderDetails.Quantity - WorkOrderDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, 0 AS QuantityTransferOrders, 0 AS QuantityAvailables, 0 AS QuantityAvailableReceipts FROM WorkOrderDetails INNER JOIN FirmOrders ON WorkOrderDetails.FirmOrderID = FirmOrders.FirmOrderID INNER JOIN Customers ON FirmOrders.CustomerID = Customers.CustomerID WHERE WorkOrderDetails.NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.ProductWorkOrder + " AND WorkOrderDetails.Approved = 1 AND WorkOrderDetails.InActive = 0 AND WorkOrderDetails.InActivePartial = 0 AND ROUND(WorkOrderDetails.Quantity - WorkOrderDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 AND WorkOrderDetails.FirmOrderID IN (           SELECT DISTINCT FirmOrderID FROM FirmOrderDetails WHERE NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.PlannedProduct + " AND Approved = 1 AND InActive = 0 AND InActivePartial = 0 AND ROUND(Quantity - (QuantitySemifinished - QuantityShortage - QuantityFailure + QuantityExcess), " + (int)GlobalEnums.rndQuantity + ") > 0           ) " + "\r\n";

            queryString = queryString + "       INSERT INTO     @WorkOrderDetails (CommodityID, CustomerName, FirmOrderCode, FirmOrderSpecs, QuantityRemains, QuantityTransferOrders, QuantityAvailables, QuantityAvailableReceipts) " + "\r\n";
            queryString = queryString + "       SELECT          CommodityID, NULL, NULL, NULL, 0 AS QuantityRemains, ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityTransferOrders, 0 AS QuantityAvailables, 0 AS QuantityAvailableReceipts FROM TransferOrderDetails WHERE CommodityID IN (SELECT CommodityID FROM @WorkOrderDetails) AND WarehouseID = @WarehouseID AND WarehouseReceiptID = @WarehouseReceiptID AND TransferOrderID <> @TransferOrderID AND InActive = 0 AND InActivePartial = 0 AND ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 " + "\r\n"; //Approved = 1 AND 

            queryString = queryString + "       INSERT INTO     @WorkOrderDetails (CommodityID, CustomerName, FirmOrderCode, FirmOrderSpecs, QuantityRemains, QuantityTransferOrders, QuantityAvailables, QuantityAvailableReceipts) " + "\r\n";
            queryString = queryString + "       SELECT          GoodsReceiptDetails.CommodityID, NULL, NULL, NULL, 0 AS QuantityRemains, 0 AS QuantityTransferOrders, ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityAvailables, 0 AS QuantityAvailableReceipts FROM GoodsReceiptDetails WHERE GoodsReceiptDetails.WarehouseID = @WarehouseID AND GoodsReceiptDetails.CommodityID IN (SELECT CommodityID FROM @WorkOrderDetails) AND ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 " + "\r\n";

            queryString = queryString + "       INSERT INTO     @WorkOrderDetails (CommodityID, CustomerName, FirmOrderCode, FirmOrderSpecs, QuantityRemains, QuantityTransferOrders, QuantityAvailables, QuantityAvailableReceipts) " + "\r\n";
            queryString = queryString + "       SELECT          GoodsReceiptDetails.CommodityID, NULL, NULL, NULL, 0 AS QuantityRemains, 0 AS QuantityTransferOrders, 0 AS QuantityAvailables, ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityAvailableReceipts FROM GoodsReceiptDetails WHERE GoodsReceiptDetails.WarehouseID = @WarehouseReceiptID AND GoodsReceiptDetails.CommodityID IN (SELECT CommodityID FROM @WorkOrderDetails) AND ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 " + "\r\n";


            queryString = queryString + "       SELECT          WorkOrderDetails.CommodityID, MIN(Commodities.Code) AS CommodityCode, MIN(Commodities.Name) AS CommodityName, MIN(Commodities.OfficialCode) AS OfficialCode, MIN(Commodities.CommodityTypeID) AS CommodityTypeID, MIN(Commodities.Weight) AS Weight, " + "\r\n";

            queryString = queryString + "                       STUFF ((SELECT DISTINCT ', ' + STUFFCustomerName.CustomerName    FROM    @WorkOrderDetails STUFFCustomerName     WHERE   STUFFCustomerName.CommodityID = WorkOrderDetails.CommodityID    AND NOT STUFFCustomerName.CustomerName IS NULL      FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)'),1,1,'') AS CustomerNames, " + "\r\n";
            queryString = queryString + "                       STUFF ((SELECT ', ' + STUFFFirmOrderCode.FirmOrderCode + ' [' + CAST(CAST(ROUND(STUFFFirmOrderCode.QuantityRemains, 0) AS INT) AS VARCHAR) + ']' FROM    @WorkOrderDetails STUFFFirmOrderCode    WHERE   STUFFFirmOrderCode.CommodityID = WorkOrderDetails.CommodityID   AND NOT STUFFFirmOrderCode.FirmOrderCode IS NULL    FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)'),1,1,'') AS Codes, " + "\r\n";
            queryString = queryString + "                       STUFF ((SELECT DISTINCT ', ' + STUFFSpecs.FirmOrderSpecs                  FROM    @WorkOrderDetails STUFFSpecs            WHERE   STUFFSpecs.CommodityID = WorkOrderDetails.CommodityID           AND NOT STUFFSpecs.FirmOrderSpecs IS NULL                    FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)'),1,1,'') AS Specs, " + "\r\n";

            queryString = queryString + "                       ROUND(SUM(WorkOrderDetails.QuantityRemains), " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, ROUND(SUM(WorkOrderDetails.QuantityTransferOrders), " + (int)GlobalEnums.rndQuantity + ") AS QuantityTransferOrders, ROUND(SUM(WorkOrderDetails.QuantityAvailables), " + (int)GlobalEnums.rndQuantity + ") AS QuantityAvailables, ROUND(SUM(WorkOrderDetails.QuantityAvailableReceipts), " + (int)GlobalEnums.rndQuantity + ") AS QuantityAvailableReceipts, CAST(0 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM            @WorkOrderDetails WorkOrderDetails " + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities ON WorkOrderDetails.CommodityID NOT IN (SELECT Id FROM dbo.SplitToIntList (@CommodityIDs)) AND WorkOrderDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "       GROUP BY        WorkOrderDetails.CommodityID " + "\r\n";
            queryString = queryString + "       HAVING          ROUND(SUM(WorkOrderDetails.QuantityRemains) - SUM(WorkOrderDetails.QuantityTransferOrders) - SUM(WorkOrderDetails.QuantityAvailableReceipts), " + (int)GlobalEnums.rndQuantity + ") > 0 " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetTransferOrderPendingWorkOrders", queryString);
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
            SimpleInitReference simpleInitReference = new TransferOrderInitReference("TransferOrders", "TransferOrderID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.TransferOrder));
            this.totalSmartPortalEntities.CreateTrigger("TransferOrderInitReference", simpleInitReference.CreateQuery());
        }
        #endregion
    }
}
