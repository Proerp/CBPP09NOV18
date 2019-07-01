using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Inventories
{
    public class InventoryControl
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public InventoryControl(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetInventoryControls();
        }


        private void GetInventoryControls()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @LocationID int, @SummaryOptionID int, @LabOptionID int, @FilterOptionID int, @PendingOptionID int, @ShelfLife int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalAspUserID nvarchar(128),      @LocalLocationID int,                         @LocalSummaryOptionID int,                          @LocalLabOptionID int,                      @LocalFilterOptionID int,                           @LocalPendingOptionID int,                              @LocalShelfLife int" + "\r\n";
            queryString = queryString + "       SET         @LocalAspUserID = @AspUserID        SET @LocalLocationID = @LocationID         SET @LocalSummaryOptionID = @SummaryOptionID        SET @LocalLabOptionID = @LabOptionID        SET @LocalFilterOptionID = @FilterOptionID          SET @LocalPendingOptionID = @PendingOptionID            SET @LocalShelfLife = @ShelfLife " + "\r\n";

            queryString = queryString + "       DECLARE     @InventoryControls TABLE (PurchaseOrderID int NULL, GoodsArrivalID int NULL, GoodsReceiptID int NULL, TransferOrderID int NULL, WarehouseTransferID int NULL, WarehouseAdjustmentID int NULL, BlendingInstructionID int NULL, CommodityID int NOT NULL, BinLocationID int NULL, EntryDate datetime NULL, Reference nvarchar(10) NULL, Code nvarchar(60) NULL, SealCode nvarchar(60) NULL, BatchCode nvarchar(60) NULL, LabID int NULL, LabCode nvarchar(60) NULL, Barcode nvarchar(60) NULL, ProductionDate datetime NULL, ExpiryDate datetime NULL, Approved bit NOT NULL, LabApproved bit NOT NULL, LabHold bit NOT NULL, LabInActive bit NOT NULL, BisQuantity decimal(18, 2) NOT NULL, BisQuantityIssued decimal(18, 2) NOT NULL, BisQuantityRemains decimal(18, 2) NOT NULL, QuantityAvailableArrivals decimal(18, 2) NOT NULL, QuantityAvailableLocation1 decimal(18, 2) NOT NULL, QuantityAvailableLocation2 decimal(18, 2) NOT NULL, QuantityPurchaseOrders decimal(18, 2) NOT NULL, QuantityTransferOrders decimal(18, 2) NOT NULL) " + "\r\n";

            //CHÚ Ý: PendingOptionID: KHI PendingOptionID = 0: GetInventoryControlSQL SẼ KHÔNG BAO GỒM PurchaseOrderID VÀ TransferOrderID. TUY NHIÊN: BlendingInstructionID VẪN INSERT BÌNH THƯỜNG: NHẰM MỤC ĐÍCH FILTER CHO FilterOptionID: LẤY TỒN KHO CHỈ BAO GỒM TỒN BIS
            //SAU ĐÓ: ĐẾN LƯỢT: GetInventoryControlFinalSQL: XÉT PendingOptionID MỘT LẦN NỮA, ĐỂ LOẠI TRỪ: BlendingInstructionID

            queryString = queryString + "       IF  (@LocalLocationID = 1) " + "\r\n";
            queryString = queryString + "           " + this.GetInventoryControlSQL(1) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               IF  (@LocalLocationID = 2) " + "\r\n";
            queryString = queryString + "                   " + this.GetInventoryControlSQL(2) + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n"; //20
            queryString = queryString + "                   " + this.GetInventoryControlSQL(0) + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            queryString = queryString + "       IF  (@LocalSummaryOptionID = 0) " + "\r\n";
            queryString = queryString + "           " + this.GetInventoryControlFinalSQL(0) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               IF  (@LocalSummaryOptionID = 10) " + "\r\n";
            queryString = queryString + "                   " + this.GetInventoryControlFinalSQL(10) + "\r\n"; ;
            queryString = queryString + "               ELSE " + "\r\n"; //20
            queryString = queryString + "                   " + this.GetInventoryControlFinalSQL(20) + "\r\n";
            queryString = queryString + "           END " + "\r\n";



            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetInventoryControls", queryString);
        }

        private string GetInventoryControlFinalSQL(int summaryOptionID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@LocalPendingOptionID = 1) " + "\r\n";
            queryString = queryString + "           " + this.GetInventoryControlFinalSQL(summaryOptionID, 1) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetInventoryControlFinalSQL(summaryOptionID, 0) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetInventoryControlFinalSQL(int summaryOptionID, int pendingOptionID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      InventoryControls.PurchaseOrderID, InventoryControls.GoodsArrivalID, InventoryControls.GoodsReceiptID, InventoryControls.TransferOrderID, InventoryControls.WarehouseTransferID, InventoryControls.WarehouseAdjustmentID, InventoryControls.BlendingInstructionID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, CommodityCategories.Name AS CategoryName, Commodities.SalesUnit, IIF(Commodities.LeadTime = 0, CAST(NULL AS Int), Commodities.LeadTime) AS LeadTime, " + (summaryOptionID == 0 ? "BinLocations.Code" : "NULL") + " AS BinLocationCode, " + (summaryOptionID == 0 ? "Barcodes.BarcodeID" : "NULL") + " AS BarcodeID, " + "\r\n";
            queryString = queryString + "                   InventoryControls.EntryDate, InventoryControls.Code, InventoryControls.SealCode, InventoryControls.BatchCode, InventoryControls.LabID, InventoryControls.LabCode, InventoryControls.Barcode, InventoryControls.ProductionDate, InventoryControls.ExpiryDate, InventoryControls.Approved, InventoryControls.LabApproved, InventoryControls.LabHold, InventoryControls.LabInActive, " + "\r\n";
            queryString = queryString + "                   IIF(InventoryControls.BisQuantity = 0, NULL, InventoryControls.BisQuantity) AS BisQuantity, IIF(InventoryControls.BisQuantityIssued = 0, NULL, InventoryControls.BisQuantityIssued) AS BisQuantityIssued, IIF(InventoryControls.BisQuantityRemains = 0, NULL, InventoryControls.BisQuantityRemains) AS BisQuantityRemains, IIF(InventoryControls.QuantityAvailableArrivals = 0, NULL, InventoryControls.QuantityAvailableArrivals) AS QuantityAvailableArrivals, IIF(InventoryControls.QuantityAvailableLocation1 = 0, NULL, InventoryControls.QuantityAvailableLocation1) AS QuantityAvailableLocation1, IIF(InventoryControls.QuantityAvailableLocation2 = 0, NULL, InventoryControls.QuantityAvailableLocation2) AS QuantityAvailableLocation2, IIF(InventoryControls.QuantityPurchaseOrders = 0, NULL, InventoryControls.QuantityPurchaseOrders) AS QuantityPurchaseOrders, IIF(InventoryControls.QuantityTransferOrders = 0, NULL, InventoryControls.QuantityTransferOrders) AS QuantityTransferOrders " + "\r\n";

            queryString = queryString + "       FROM        " + "\r\n";
            if (summaryOptionID == 0)
                queryString = queryString + "               @InventoryControls InventoryControls " + "\r\n";
            else
                if (summaryOptionID == 10)
                    queryString = queryString + "              (SELECT NULL AS PurchaseOrderID, NULL AS GoodsArrivalID, NULL AS GoodsReceiptID, NULL AS TransferOrderID, NULL AS WarehouseTransferID, NULL AS WarehouseAdjustmentID, NULL AS BlendingInstructionID, CommodityID, CAST(NULL AS int) AS BinLocationID, CAST(NULL AS Datetime) AS EntryDate, CAST(NULL AS nvarchar) AS Code, CAST(NULL AS nvarchar) AS SealCode, CAST(NULL AS nvarchar) AS BatchCode, CAST(NULL AS int) AS LabID, CAST(NULL AS nvarchar) AS LabCode, CAST(NULL AS nvarchar) AS Barcode, MIN(ProductionDate) AS ProductionDate, MIN(ExpiryDate) AS ExpiryDate, CAST(1 AS Bit) AS Approved, CAST(1 AS Bit) AS LabApproved, CAST(0 AS Bit) AS LabHold, CAST(0 AS Bit) AS LabInActive, SUM(BisQuantity) AS BisQuantity, SUM(BisQuantityIssued) AS BisQuantityIssued, SUM(BisQuantityRemains) AS BisQuantityRemains, SUM(QuantityAvailableArrivals) AS QuantityAvailableArrivals, SUM(QuantityAvailableLocation1) AS QuantityAvailableLocation1, SUM(QuantityAvailableLocation2) AS QuantityAvailableLocation2, SUM(QuantityPurchaseOrders) AS QuantityPurchaseOrders, SUM(QuantityTransferOrders) AS QuantityTransferOrders FROM @InventoryControls " + (pendingOptionID == 0 ? "WHERE BlendingInstructionID IS NULL" : "") + " GROUP BY CommodityID) InventoryControls " + "\r\n";
                else //20
                    queryString = queryString + "              (SELECT NULL AS PurchaseOrderID, NULL AS GoodsArrivalID, NULL AS GoodsReceiptID, NULL AS TransferOrderID, NULL AS WarehouseTransferID, NULL AS WarehouseAdjustmentID, NULL AS BlendingInstructionID, CommodityID, CAST(NULL AS int) AS BinLocationID, CAST(NULL AS Datetime) AS EntryDate, CAST(NULL AS nvarchar) AS Code, CAST(NULL AS nvarchar) AS SealCode, BatchCode, LabID, LabCode, CAST(NULL AS nvarchar) AS Barcode, MIN(ProductionDate) AS ProductionDate, MIN(ExpiryDate) AS ExpiryDate, CAST(1 AS Bit) AS Approved, CAST(MAX(IIF(LabApproved = 1, 1, 0)) AS Bit) AS LabApproved, CAST(MAX(IIF(LabHold = 1, 1, 0)) AS Bit) AS LabHold, CAST(MAX(IIF(LabInActive = 1, 1, 0)) AS Bit) AS LabInActive, SUM(BisQuantity) AS BisQuantity, SUM(BisQuantityIssued) AS BisQuantityIssued, SUM(BisQuantityRemains) AS BisQuantityRemains, SUM(QuantityAvailableArrivals) AS QuantityAvailableArrivals, SUM(QuantityAvailableLocation1) AS QuantityAvailableLocation1, SUM(QuantityAvailableLocation2) AS QuantityAvailableLocation2, SUM(QuantityPurchaseOrders) AS QuantityPurchaseOrders, SUM(QuantityTransferOrders) AS QuantityTransferOrders FROM @InventoryControls " + (pendingOptionID == 0 ? "WHERE BlendingInstructionID IS NULL" : "") + " GROUP BY CommodityID, BatchCode, LabID, LabCode) InventoryControls " + "\r\n";

            queryString = queryString + "                   INNER JOIN Commodities ON " + (pendingOptionID == 0 ? "InventoryControls.BlendingInstructionID IS NULL AND " : "") + "InventoryControls.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CommodityCategories ON Commodities.CommodityCategoryID = CommodityCategories.CommodityCategoryID " + "\r\n";
            if (summaryOptionID == 0)
            {
                queryString = queryString + "               LEFT JOIN  BinLocations ON InventoryControls.BinLocationID = BinLocations.BinLocationID " + "\r\n";
                queryString = queryString + "               LEFT JOIN  Barcodes ON InventoryControls.Barcode = Barcodes.Code " + "\r\n";
            }

            queryString = queryString + "       ORDER BY    CommodityCategories.Name, Commodities.Code, InventoryControls.ExpiryDate " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetInventoryControlSQL(int locationID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@LocalFilterOptionID = 0) " + "\r\n";
            queryString = queryString + "           " + this.GetInventoryControlSQL(locationID, 0) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               IF  (@LocalFilterOptionID = 10) " + "\r\n";
            queryString = queryString + "                   " + this.GetInventoryControlSQL(locationID, 10) + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n"; //20
            queryString = queryString + "                   " + this.GetInventoryControlSQL(locationID, 20) + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetInventoryControlSQL(int locationID, int filterOptionID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@LocalLabOptionID = 1) " + "\r\n";
            queryString = queryString + "           " + this.GetInventoryControlSQL(locationID, filterOptionID, 1) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               IF  (@LocalLabOptionID = 0) " + "\r\n";
            queryString = queryString + "                   " + this.GetInventoryControlSQL(locationID, filterOptionID, 0) + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n";
            queryString = queryString + "                   " + this.GetInventoryControlSQL(locationID, filterOptionID, 99) + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetInventoryControlSQL(int locationID, int filterOptionID, int labOptionID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@LocalPendingOptionID = 1) " + "\r\n";
            queryString = queryString + "           " + this.GetInventoryControlSQL(locationID, filterOptionID, labOptionID, 1) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetInventoryControlSQL(locationID, filterOptionID, labOptionID, 0) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetInventoryControlSQL(int locationID, int filterOptionID, int labOptionID, int pendingOptionID)
        {
            //filterOptionID: 0: NORMAL
            //filterOptionID: 10: WITH BisQuantityRemains
            //filterOptionID: 20: WITH ExpiryDate

            string queryString = "";

            queryString = queryString + "    BEGIN " + "\r\n";

            if (filterOptionID != 20) queryString = queryString + GetInventoryControlSQL() + "\r\n";

            if (locationID == 0 || locationID == 1)
            {
                queryString = queryString + "   INSERT INTO @InventoryControls (PurchaseOrderID, GoodsArrivalID, GoodsReceiptID, TransferOrderID, WarehouseTransferID, WarehouseAdjustmentID, BlendingInstructionID, CommodityID, BinLocationID, EntryDate, Code, SealCode, BatchCode, LabID, LabCode, Barcode, ProductionDate, ExpiryDate, Approved, LabApproved, LabHold, LabInActive, BisQuantity, BisQuantityIssued, BisQuantityRemains, QuantityAvailableArrivals, QuantityAvailableLocation1, QuantityAvailableLocation2, QuantityPurchaseOrders, QuantityTransferOrders) " + "\r\n";
                queryString = queryString + "   SELECT      NULL AS PurchaseOrderID, GoodsArrivalPackages.GoodsArrivalID, NULL AS GoodsReceiptID, NULL AS TransferOrderID, NULL AS WarehouseTransferID, NULL AS WarehouseAdjustmentID, NULL AS BlendingInstructionID, GoodsArrivalPackages.CommodityID, NULL AS BinLocationID, GoodsArrivalPackages.EntryDate, GoodsArrivalPackages.Code, GoodsArrivalPackages.SealCode, GoodsArrivalPackages.BatchCode, GoodsArrivalPackages.LabID, GoodsArrivalPackages.LabCode, GoodsArrivalPackages.Barcode, GoodsArrivalPackages.ProductionDate, GoodsArrivalPackages.ExpiryDate, GoodsArrivalPackages.Approved, Labs.Approved AS LabApproved, Labs.Hold AS LabHold, Labs.InActive AS LabInActive, 0 AS BisQuantity, 0 AS BisQuantityIssued, 0 AS BisQuantityRemains, GoodsArrivalPackages.Quantity - GoodsArrivalPackages.QuantityReceipted AS QuantityAvailableArrivals, 0 AS QuantityAvailableLocation1, 0 AS QuantityAvailableLocation2, 0 AS QuantityPurchaseOrders, 0 AS QuantityTransferOrders FROM GoodsArrivalPackages INNER JOIN Labs ON GoodsArrivalPackages.LabID = Labs.LabID " + (labOptionID == 1 ? " AND Labs.Approved = 1 AND Labs.InActive = 0" : (labOptionID == 0 ? " AND (Labs.Approved = 0 OR Labs.InActive = 1)" : "")) + " AND ROUND(GoodsArrivalPackages.Quantity - GoodsArrivalPackages.QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") > 0 " + (filterOptionID == 10 ? " AND GoodsArrivalPackages.CommodityID IN (SELECT DISTINCT CommodityID FROM @InventoryControls)" : "") + (filterOptionID == 20 ? " AND DATEDIFF(DAY, GETDATE(), GoodsArrivalPackages.ExpiryDate) <= @LocalShelfLife" : "") + "\r\n";
            }
            queryString = queryString + "       INSERT INTO @InventoryControls (PurchaseOrderID, GoodsArrivalID, GoodsReceiptID, TransferOrderID, WarehouseTransferID, WarehouseAdjustmentID, BlendingInstructionID, CommodityID, BinLocationID, EntryDate, Code, SealCode, BatchCode, LabID, LabCode, Barcode, ProductionDate, ExpiryDate, Approved, LabApproved, LabHold, LabInActive, BisQuantity, BisQuantityIssued, BisQuantityRemains, QuantityAvailableArrivals, QuantityAvailableLocation1, QuantityAvailableLocation2, QuantityPurchaseOrders, QuantityTransferOrders) " + "\r\n";
            queryString = queryString + "       SELECT      NULL AS PurchaseOrderID, NULL AS GoodsArrivalID, IIF(WarehouseTransferID IS NULL AND WarehouseAdjustmentID IS NULL, GoodsReceiptID, NULL) AS GoodsReceiptID, NULL AS TransferOrderID, WarehouseTransferID, WarehouseAdjustmentID, NULL AS BlendingInstructionID, GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.BinLocationID, GoodsReceiptDetails.EntryDate, GoodsReceiptDetails.Code, GoodsReceiptDetails.SealCode, GoodsReceiptDetails.BatchCode, GoodsReceiptDetails.LabID, GoodsReceiptDetails.LabCode, GoodsReceiptDetails.Barcode, GoodsReceiptDetails.ProductionDate, GoodsReceiptDetails.ExpiryDate, GoodsReceiptDetails.Approved, Labs.Approved AS LabApproved, Labs.Hold AS LabHold, Labs.InActive AS LabInActive, 0 AS BisQuantity, 0 AS BisQuantityIssued, 0 AS BisQuantityRemains, 0 AS QuantityAvailableArrivals, CASE WHEN Warehouses.LocationID = 1 THEN GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued ELSE 0 END AS QuantityAvailableLocation1, CASE WHEN Warehouses.LocationID = 2 THEN GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued ELSE 0 END AS QuantityAvailableLocation2, 0 AS QuantityPurchaseOrders, 0 AS QuantityTransferOrders FROM GoodsReceiptDetails INNER JOIN Labs ON GoodsReceiptDetails.LabID = Labs.LabID " + (labOptionID == 1 ? " AND Labs.Approved = 1 AND Labs.InActive = 0" : (labOptionID == 0 ? " AND (Labs.Approved = 0 OR Labs.InActive = 1)" : "")) + " INNER JOIN Warehouses ON " + (locationID != 0 ? " Warehouses.LocationID = @LocalLocationID AND " : "") + " ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 " + (filterOptionID == 10 ? " AND GoodsReceiptDetails.CommodityID IN (SELECT DISTINCT CommodityID FROM @InventoryControls)" : "") + (filterOptionID == 20 ? " AND DATEDIFF(DAY, GETDATE(), GoodsReceiptDetails.ExpiryDate) <= @LocalShelfLife" : "") + " AND GoodsReceiptDetails.WarehouseID = Warehouses.WarehouseID " + "\r\n";



            if (pendingOptionID == 1)
            {
                if (locationID == 0 || locationID == 1)
                {
                    queryString = queryString + "   INSERT INTO @InventoryControls (PurchaseOrderID, GoodsArrivalID, GoodsReceiptID, TransferOrderID, WarehouseTransferID, WarehouseAdjustmentID, BlendingInstructionID, CommodityID, BinLocationID, EntryDate, Code, SealCode, BatchCode, LabID, LabCode, Barcode, ProductionDate, ExpiryDate, Approved, LabApproved, LabHold, LabInActive, BisQuantity, BisQuantityIssued, BisQuantityRemains, QuantityAvailableArrivals, QuantityAvailableLocation1, QuantityAvailableLocation2, QuantityPurchaseOrders, QuantityTransferOrders) " + "\r\n";
                    queryString = queryString + "   SELECT      PurchaseOrderID, NULL AS GoodsArrivalID, NULL AS GoodsReceiptID, NULL AS TransferOrderID, NULL AS WarehouseTransferID, NULL AS WarehouseAdjustmentID, NULL AS BlendingInstructionID, CommodityID, NULL AS BinLocationID, EntryDate, Reference AS Code, NULL AS SealCode, NULL AS BatchCode, NULL AS LabID, NULL AS LabCode, 'PO: ' + Reference AS Barcode, NULL AS ProductionDate, NULL AS ExpiryDate, Approved, CAST(1 AS Bit) AS LabApproved, CAST(0 AS Bit) AS LabHold, CAST(0 AS Bit) AS LabInActive, 0 AS BisQuantity, 0 AS BisQuantityIssued, 0 AS BisQuantityRemains, 0 AS QuantityAvailableArrivals, 0 AS QuantityAvailableLocation1, 0 AS QuantityAvailableLocation2, Quantity - QuantityArrived AS QuantityPurchaseOrders, 0 AS QuantityTransferOrders FROM PurchaseOrderDetails WHERE ROUND(Quantity - QuantityArrived, " + (int)GlobalEnums.rndQuantity + ") > 0 AND InActive = 0 AND InActivePartial = 0 " + (filterOptionID == 10 || filterOptionID == 20 ? " AND CommodityID IN (SELECT DISTINCT CommodityID FROM @InventoryControls)" : "") + "\r\n";
                }
                queryString = queryString + "       INSERT INTO @InventoryControls (PurchaseOrderID, GoodsArrivalID, GoodsReceiptID, TransferOrderID, WarehouseTransferID, WarehouseAdjustmentID, BlendingInstructionID, CommodityID, BinLocationID, EntryDate, Code, SealCode, BatchCode, LabID, LabCode, Barcode, ProductionDate, ExpiryDate, Approved, LabApproved, LabHold, LabInActive, BisQuantity, BisQuantityIssued, BisQuantityRemains, QuantityAvailableArrivals, QuantityAvailableLocation1, QuantityAvailableLocation2, QuantityPurchaseOrders, QuantityTransferOrders) " + "\r\n";
                queryString = queryString + "       SELECT      NULL AS PurchaseOrderID, NULL AS GoodsArrivalID, NULL AS GoodsReceiptID, TransferOrderID, NULL AS WarehouseTransferID, NULL AS WarehouseAdjustmentID, NULL AS BlendingInstructionID, CommodityID, NULL AS BinLocationID, EntryDate, Reference AS Code, NULL AS SealCode, NULL AS BatchCode, NULL AS LabID, NULL AS LabCode, 'TO: ' + Reference AS Barcode, NULL AS ProductionDate, NULL AS ExpiryDate, Approved, CAST(1 AS Bit) AS LabApproved, CAST(0 AS Bit) AS LabHold, CAST(0 AS Bit) AS LabInActive, 0 AS BisQuantity, 0 AS BisQuantityIssued, 0 AS BisQuantityRemains, 0 AS QuantityAvailableArrivals, 0 AS QuantityAvailableLocation1, 0 AS QuantityAvailableLocation2, 0 AS QuantityPurchaseOrders, Quantity - QuantityIssued AS QuantityTransferOrders FROM TransferOrderDetails WHERE ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 AND InActive = 0 AND InActivePartial = 0 " + (filterOptionID == 10 || filterOptionID == 20 ? " AND CommodityID IN (SELECT DISTINCT CommodityID FROM @InventoryControls)" : "") + "\r\n";
            }


            if (filterOptionID == 20) queryString = queryString + GetInventoryControlSQL() + (filterOptionID == 20 ? " AND CommodityID IN (SELECT DISTINCT CommodityID FROM @InventoryControls)" : "") + "\r\n"; //JUST GET BlendingInstructionDetails WHERE CommodityID IN @InventoryControls (IT MEANS THAT: FIRST: FILTER CommodityID BY filterOptionID THEN: USING @InventoryControls AS FILTER)

            queryString = queryString + "    END " + "\r\n";

            return queryString;
        }

        private string GetInventoryControlSQL()
        {
            string queryString = "";

            queryString = queryString + "       INSERT INTO @InventoryControls (PurchaseOrderID, GoodsArrivalID, GoodsReceiptID, TransferOrderID, WarehouseTransferID, WarehouseAdjustmentID, BlendingInstructionID, CommodityID, BinLocationID, EntryDate, Code, SealCode, BatchCode, LabID, LabCode, Barcode, ProductionDate, ExpiryDate, Approved, LabApproved, LabHold, LabInActive, BisQuantity, BisQuantityIssued, BisQuantityRemains, QuantityAvailableArrivals, QuantityAvailableLocation1, QuantityAvailableLocation2, QuantityPurchaseOrders, QuantityTransferOrders) " + "\r\n";
            queryString = queryString + "       SELECT      NULL AS PurchaseOrderID, NULL AS GoodsArrivalID, NULL AS GoodsReceiptID, NULL AS TransferOrderID, NULL AS WarehouseTransferID, NULL AS WarehouseAdjustmentID, BlendingInstructionID, CommodityID, NULL AS BinLocationID, EntryDate, NULL AS Code, NULL AS SealCode, NULL AS BatchCode, NULL AS LabID, NULL AS LabCode, 'BIS: ' + Code AS Barcode, NULL AS ProductionDate, NULL AS ExpiryDate, Approved, CAST(1 AS Bit) AS LabApproved, CAST(0 AS Bit) AS LabHold, CAST(0 AS Bit) AS LabInActive, Quantity AS BisQuantity, QuantityIssued AS BisQuantityIssued, Quantity - QuantityIssued AS BisQuantityRemains, 0 AS QuantityAvailableArrivals, 0 AS QuantityAvailableLocation1, 0 AS QuantityAvailableLocation2, 0 AS QuantityPurchaseOrders, 0 AS QuantityTransferOrders FROM BlendingInstructionDetails WHERE InActive = 0 AND InActivePartial = 0 AND ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 " + "\r\n";

            return queryString;
        }
    }
}