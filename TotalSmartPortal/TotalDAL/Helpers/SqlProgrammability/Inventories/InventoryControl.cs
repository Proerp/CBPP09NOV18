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

            queryString = " @AspUserID nvarchar(128), @SummaryOnly bit, @LabOptionID int, @FilterOptionID int, @ExpiryDay int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalAspUserID nvarchar(128),      @LocalSummaryOnly bit,                      @LocalLabOptionID int,                      @LocalFilterOptionID int,                           @LocalExpiryDay int" + "\r\n";
            queryString = queryString + "       SET         @LocalAspUserID = @AspUserID        SET @LocalSummaryOnly = @SummaryOnly        SET @LocalLabOptionID = @LabOptionID        SET @LocalFilterOptionID = @FilterOptionID          SET @LocalExpiryDay = @ExpiryDay " + "\r\n";

            queryString = queryString + "       DECLARE     @InventoryControls TABLE (CommodityID int NOT NULL, BinLocationID int NULL, EntryDate datetime NOT NULL, Reference nvarchar(10) NULL, Code nvarchar(60) NULL, SealCode nvarchar(60) NULL, BatchCode nvarchar(60) NULL, LabCode nvarchar(60) NULL, Barcode nvarchar(60) NULL, ProductionDate datetime NULL, ExpiryDate datetime NULL, Approved bit NOT NULL, BisQuantity decimal(18, 2) NOT NULL, BisQuantityIssued decimal(18, 2) NOT NULL, BisQuantityRemains decimal(18, 2) NOT NULL, QuantityAvailableArrivals decimal(18, 2) NOT NULL, QuantityAvailableLocation1 decimal(18, 2) NOT NULL, QuantityAvailableLocation2 decimal(18, 2) NOT NULL) " + "\r\n";


            queryString = queryString + "       IF  (@LocalFilterOptionID = 0) " + "\r\n";
            queryString = queryString + "           " + this.GetInventoryControlIndexSQL(0) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               IF  (@LocalFilterOptionID = 10) " + "\r\n";
            queryString = queryString + "                   " + this.GetInventoryControlIndexSQL(10) + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n"; //20
            queryString = queryString + "                   " + this.GetInventoryControlIndexSQL(20) + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            queryString = queryString + "       IF  (@LocalSummaryOnly = 1) " + "\r\n";
            queryString = queryString + "           " + this.GetInventoryControlIndexSQL(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n"; //20
            queryString = queryString + "           " + this.GetInventoryControlIndexSQL(false) + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetInventoryControls", queryString);
        }

        private string GetInventoryControlIndexSQL(bool summaryOnly)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, CommodityCategories.Name AS CategoryName, Commodities.SalesUnit, Commodities.LeadTime, " + (summaryOnly ? "NULL" : "BinLocations.Code") + " AS BinLocationCode, " + "\r\n";
            queryString = queryString + "                   InventoryControls.EntryDate, InventoryControls.Code, InventoryControls.SealCode, InventoryControls.BatchCode, InventoryControls.LabCode, InventoryControls.Barcode, InventoryControls.ProductionDate, InventoryControls.ExpiryDate, InventoryControls.Approved, " + "\r\n";
            queryString = queryString + "                   InventoryControls.BisQuantity, InventoryControls.BisQuantityIssued, InventoryControls.BisQuantityRemains, InventoryControls.QuantityAvailableArrivals, InventoryControls.QuantityAvailableLocation1, InventoryControls.QuantityAvailableLocation2  " + "\r\n";

            queryString = queryString + "       FROM        " + "\r\n";
            if (summaryOnly)
                queryString = queryString + "              (SELECT CommodityID, NULL AS BinLocationID, NULL AS EntryDate, NULL AS Code, NULL AS SealCode, NULL AS BatchCode, NULL AS LabCode, NULL AS Barcode, NULL AS ProductionDate, NULL AS ExpiryDate, NULL AS Approved, SUM(BisQuantity) AS BisQuantity, SUM(BisQuantityIssued) AS BisQuantityIssued, SUM(BisQuantityRemains) AS BisQuantityRemains, SUM(QuantityAvailableArrivals) AS QuantityAvailableArrivals, SUM(QuantityAvailableLocation1) AS QuantityAvailableLocation1, SUM(QuantityAvailableLocation2) AS QuantityAvailableLocation2 FROM @InventoryControls GROUP BY CommodityID) InventoryControls " + "\r\n";
            else
                queryString = queryString + "               @InventoryControls InventoryControls " + "\r\n";

            queryString = queryString + "                   INNER JOIN Commodities ON InventoryControls.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CommodityCategories ON Commodities.CommodityCategoryID = CommodityCategories.CommodityCategoryID " + "\r\n";
            if (!summaryOnly)
                queryString = queryString + "               LEFT JOIN  BinLocations ON InventoryControls.BinLocationID = BinLocations.BinLocationID " + "\r\n";

            queryString = queryString + "       ORDER BY    CommodityCategories.Name, Commodities.Code, InventoryControls.ExpiryDate " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetInventoryControlIndexSQL(int filterOptionID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@LocalLabOptionID = 1) " + "\r\n";
            queryString = queryString + "           " + this.GetInventoryControlIndexSQL(filterOptionID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetInventoryControlIndexSQL(filterOptionID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetInventoryControlIndexSQL(int filterOptionID, bool labOptionID)
        {

            //filterOptionID: 0: NORMAL
            //filterOptionID: 10: WITH BisQuantityRemains
            //filterOptionID: 20: WITH ExpiryDate

            string queryString = "";

            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       INSERT INTO @InventoryControls (CommodityID, BinLocationID, EntryDate, Code, SealCode, BatchCode, LabCode, Barcode, ProductionDate, ExpiryDate, Approved, BisQuantity, BisQuantityIssued, BisQuantityRemains, QuantityAvailableArrivals, QuantityAvailableLocation1, QuantityAvailableLocation2) " + "\r\n";
            queryString = queryString + "       SELECT      CommodityID, NULL AS BinLocationID, EntryDate, NULL AS Code, NULL AS SealCode, NULL AS BatchCode, NULL AS LabCode, NULL AS Barcode, NULL AS ProductionDate, NULL AS ExpiryDate, Approved, Quantity AS BisQuantity, QuantityIssued AS BisQuantityIssued, Quantity - QuantityIssued AS BisQuantityRemains, 0 AS QuantityAvailableArrivals, 0 AS QuantityAvailableLocation1, 0 AS QuantityAvailableLocation2 FROM BlendingInstructionDetails WHERE ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 " + "\r\n";

            queryString = queryString + "       INSERT INTO @InventoryControls (CommodityID, BinLocationID, EntryDate, Code, SealCode, BatchCode, LabCode, Barcode, ProductionDate, ExpiryDate, Approved, BisQuantity, BisQuantityIssued, BisQuantityRemains, QuantityAvailableArrivals, QuantityAvailableLocation1, QuantityAvailableLocation2) " + "\r\n";
            queryString = queryString + "       SELECT      CommodityID, NULL AS BinLocationID, EntryDate, Code, SealCode, BatchCode, LabCode, Barcode, ProductionDate, ExpiryDate, Approved, 0 AS BisQuantity, 0 AS BisQuantityIssued, 0 AS BisQuantityRemains, Quantity - QuantityReceipted AS QuantityAvailableArrivals, 0 AS QuantityAvailableLocation1, 0 AS QuantityAvailableLocation2 FROM GoodsArrivalPackages WHERE ROUND(Quantity - QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") > 0 " + (filterOptionID == 10 ? " AND CommodityID IN (SELECT DISTINCT CommodityID FROM @InventoryControls)" : "") + (filterOptionID == 20 ? " AND DATEDIFF(DAY, GETDATE(), ExpiryDate) <= @LocalExpiryDay" : "") + (labOptionID ? " AND LabID IN (SELECT LabID FROM Labs WHERE Approved = 1 AND InActive = 0)" : "") + "\r\n";

            queryString = queryString + "       INSERT INTO @InventoryControls (CommodityID, BinLocationID, EntryDate, Code, SealCode, BatchCode, LabCode, Barcode, ProductionDate, ExpiryDate, Approved, BisQuantity, BisQuantityIssued, BisQuantityRemains, QuantityAvailableArrivals, QuantityAvailableLocation1, QuantityAvailableLocation2) " + "\r\n";
            queryString = queryString + "       SELECT      GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.BinLocationID, GoodsReceiptDetails.EntryDate, GoodsReceiptDetails.Code, GoodsReceiptDetails.SealCode, GoodsReceiptDetails.BatchCode, GoodsReceiptDetails.LabCode, GoodsReceiptDetails.Barcode, GoodsReceiptDetails.ProductionDate, GoodsReceiptDetails.ExpiryDate, GoodsReceiptDetails.Approved, 0 AS BisQuantity, 0 AS BisQuantityIssued, 0 AS BisQuantityRemains, 0 AS QuantityAvailableArrivals, CASE WHEN Warehouses.LocationID = 1 THEN GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued ELSE 0 END AS QuantityAvailableLocation1, CASE WHEN Warehouses.LocationID = 2 THEN GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued ELSE 0 END AS QuantityAvailableLocation2 FROM GoodsReceiptDetails INNER JOIN Warehouses ON ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 " + (filterOptionID == 10 ? " AND CommodityID IN (SELECT DISTINCT CommodityID FROM @InventoryControls)" : "") + (filterOptionID == 20 ? " AND DATEDIFF(DAY, GETDATE(), ExpiryDate) <= @LocalExpiryDay" : "") + (labOptionID ? " AND GoodsReceiptDetails.LabID IN (SELECT LabID FROM Labs WHERE Approved = 1 AND InActive = 0)" : "") + " AND GoodsReceiptDetails.WarehouseID = Warehouses.WarehouseID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            return queryString;
        }
    }
}