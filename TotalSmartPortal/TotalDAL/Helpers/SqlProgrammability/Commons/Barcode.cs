using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Commons
{
    public class Barcode
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public Barcode(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetBarcode();
            this.GetBarcodeBasics();
            this.GetBarcodeJournals();
        }


        private void GetBarcode()
        {
            string queryString;

            queryString = " @BarcodeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";
            queryString = queryString + "       SELECT      BarcodeID, Code, GoodsArrivalID, GoodsArrivalDetailID, GoodsArrivalPackageID " + "\r\n";
            queryString = queryString + "       FROM        Barcodes " + "\r\n";
            queryString = queryString + "       WHERE       BarcodeID = @BarcodeID " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetBarcode", queryString);
        }

        private void GetBarcodeBasics()
        {
            string queryString;
            
            queryString = " @SearchText nvarchar(50) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";
            queryString = queryString + "       SELECT      BarcodeID, Code, GoodsArrivalID, GoodsArrivalDetailID, GoodsArrivalPackageID " + "\r\n";
            queryString = queryString + "       FROM        Barcodes " + "\r\n";
            queryString = queryString + "       WHERE       LEN(@SearchText) >= 6 AND Code LIKE '%' + @SearchText + '%' " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetBarcodeBasics", queryString);
        }

        private void GetBarcodeJournals()
        {
            string queryString;

            queryString = " @Barcode varchar(50) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE @BarcodeJournals TABLE (EntryID int NOT NULL, EntryDate datetime NOT NULL, Reference nvarchar(10) NOT NULL, ControllerName varchar(88) NULL, Barcode nvarchar(60) NULL, Quantity decimal(18, 2) NULL, QuantityRemains decimal(18, 2) NULL, TareWeight decimal(18, 2) NULL, Approved bit, LabApproved bit, LabHold bit, LabInActive bit, BinLocationCode nvarchar(50) NULL, BlendingInstructionCode nvarchar(50) NULL, ShortName nvarchar(50) NULL, PackageIssueDetailID int NULL, PackageIssueImage1ID int NULL, PackageIssueImage2ID int NULL, UserFirstName nvarchar(60) NULL, Description nvarchar(100) NULL) " + "\r\n";

            queryString = queryString + "       IF LEN(@Barcode) > 5 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";

            queryString = queryString + "               INSERT INTO     @BarcodeJournals (EntryID, EntryDate, Reference, ControllerName, Barcode, Quantity, QuantityRemains, TareWeight, Approved, LabApproved, LabHold, LabInActive, BinLocationCode, BlendingInstructionCode, ShortName, PackageIssueDetailID, PackageIssueImage1ID, PackageIssueImage2ID, UserFirstName, Description) " + "\r\n";
            queryString = queryString + "               SELECT          GoodsArrivals.GoodsArrivalID AS EntryID, GoodsArrivals.EntryDate, GoodsArrivals.Reference, 'Purchases/MaterialArrivals' AS ControllerName, GoodsArrivalPackages.Barcode, GoodsArrivalPackages.Quantity, NULL AS QuantityRemains, GoodsArrivalPackages.TareWeight, GoodsArrivalPackages.Approved, CAST(1 AS bit) AS LabApproved, CAST(0 AS bit) AS LabHold, CAST(0 AS bit) AS LabInActive, NULL AS BinLocationCode, NULL AS BlendingInstructionCode, NULL AS ShortName, NULL AS PackageIssueDetailID, NULL AS PackageIssueImage1ID, NULL AS PackageIssueImage2ID, Users.LastName AS UserFirstName, N'Nhập hàng' AS Description " + "\r\n";
            queryString = queryString + "               FROM            GoodsArrivalPackages " + "\r\n";
            queryString = queryString + "                               INNER JOIN GoodsArrivals ON GoodsArrivalPackages.Barcode = @Barcode AND GoodsArrivalPackages.GoodsArrivalID = GoodsArrivals.GoodsArrivalID " + "\r\n";
            queryString = queryString + "                               INNER JOIN Users ON GoodsArrivals.UserID = Users.UserID " + "\r\n";

            queryString = queryString + "               INSERT INTO     @BarcodeJournals (EntryID, EntryDate, Reference, ControllerName, Barcode, Quantity, QuantityRemains, TareWeight, Approved, LabApproved, LabHold, LabInActive, BinLocationCode, BlendingInstructionCode, ShortName, PackageIssueDetailID, PackageIssueImage1ID, PackageIssueImage2ID, UserFirstName, Description) " + "\r\n";
            queryString = queryString + "               SELECT          ISNULL(GoodsReceiptDetails.WarehouseTransferID, GoodsReceiptDetails.GoodsReceiptID) AS EntryID, GoodsReceiptDetails.EntryDate, ISNULL(WarehouseTransfers.Reference, GoodsReceiptDetails.Reference) AS Reference, 'Inventories/' + IIF(GoodsReceiptDetails.WarehouseTransferID IS NULL, 'MaterialReceipts', 'MaterialTransfers') AS ControllerName, GoodsReceiptDetails.Barcode, GoodsReceiptDetails.Quantity, GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued AS QuantityRemains, GoodsReceiptDetails.TareWeight, GoodsReceiptDetails.Approved, Labs.Approved AS LabApproved, Labs.Hold AS LabHold, Labs.InActive AS LabInActive, BinLocations.Code AS BinLocationCode, NULL AS BlendingInstructionCode, NULL AS ShortName, NULL AS PackageIssueDetailID, NULL AS PackageIssueImage1ID, NULL AS PackageIssueImage2ID, Users.FirstName AS UserFirstName, IIF(GoodsReceiptDetails.WarehouseTransferID IS NULL, N'Nhập kho', N'VCNB') AS Description " + "\r\n";
            queryString = queryString + "               FROM            GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                               INNER JOIN BinLocations ON GoodsReceiptDetails.Barcode = @Barcode AND GoodsReceiptDetails.BinLocationID = BinLocations.BinLocationID " + "\r\n";
            queryString = queryString + "                               INNER JOIN Labs ON GoodsReceiptDetails.LabID = Labs.LabID " + "\r\n";
            queryString = queryString + "                               INNER JOIN GoodsReceipts ON GoodsReceiptDetails.GoodsReceiptID = GoodsReceipts.GoodsReceiptID " + "\r\n";
            queryString = queryString + "                               INNER JOIN Users ON GoodsReceipts.UserID = Users.UserID " + "\r\n";
            queryString = queryString + "                               LEFT JOIN WarehouseTransfers ON GoodsReceiptDetails.WarehouseTransferID = WarehouseTransfers.WarehouseTransferID " + "\r\n";

            queryString = queryString + "               INSERT INTO     @BarcodeJournals (EntryID, EntryDate, Reference, ControllerName, Barcode, Quantity, QuantityRemains, TareWeight, Approved, LabApproved, LabHold, LabInActive, BinLocationCode, BlendingInstructionCode, ShortName, PackageIssueDetailID, PackageIssueImage1ID, PackageIssueImage2ID, UserFirstName, Description) " + "\r\n";
            queryString = queryString + "               SELECT          WarehouseAdjustmentDetails.WarehouseAdjustmentID AS EntryID, WarehouseAdjustmentDetails.EntryDate, WarehouseAdjustmentDetails.Reference, 'Inventories/OtherMaterialIssues' AS ControllerName, GoodsReceiptDetails.Barcode, -WarehouseAdjustmentDetails.Quantity, NULL AS QuantityRemains, NULL AS TareWeight, WarehouseAdjustmentDetails.Approved, CAST(1 AS bit) AS LabApproved, CAST(0 AS bit) AS LabHold, CAST(0 AS bit) AS LabInActive, BinLocations.Code AS BinLocationCode, NULL AS BlendingInstructionCode, NULL AS ShortName, NULL AS PackageIssueDetailID, NULL AS PackageIssueImage1ID, NULL AS PackageIssueImage2ID, Users.LastName AS UserFirstName, N'Xuất khác' AS Description " + "\r\n";
            queryString = queryString + "               FROM            WarehouseAdjustmentDetails " + "\r\n";
            queryString = queryString + "                               INNER JOIN GoodsReceiptDetails ON GoodsReceiptDetails.Barcode = @Barcode AND WarehouseAdjustmentDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";
            queryString = queryString + "                               INNER JOIN BinLocations ON GoodsReceiptDetails.BinLocationID = BinLocations.BinLocationID " + "\r\n";
            queryString = queryString + "                               INNER JOIN WarehouseAdjustments ON WarehouseAdjustmentDetails.WarehouseAdjustmentID = WarehouseAdjustments.WarehouseAdjustmentID " + "\r\n";
            queryString = queryString + "                               INNER JOIN Users ON WarehouseAdjustments.UserID = Users.UserID " + "\r\n";

            queryString = queryString + "               INSERT INTO     @BarcodeJournals (EntryID, EntryDate, Reference, ControllerName, Barcode, Quantity, QuantityRemains, TareWeight, Approved, LabApproved, LabHold, LabInActive, BinLocationCode, BlendingInstructionCode, ShortName, PackageIssueDetailID, PackageIssueImage1ID, PackageIssueImage2ID, UserFirstName, Description) " + "\r\n";
            queryString = queryString + "               SELECT          PackageIssueDetails.PackageIssueID AS EntryID, PackageIssueDetails.EntryDate, PackageIssueDetails.Reference, 'Inventories/PackageIssues' AS ControllerName, PackageIssueDetails.Barcode, PackageIssueDetails.Quantity, NULL AS QuantityRemains, NULL AS TareWeight, PackageIssueDetails.Approved, CAST(1 AS bit) AS LabApproved, CAST(0 AS bit) AS LabHold, CAST(0 AS bit) AS LabInActive, NULL AS BinLocationCode, BlendingInstructions.Code AS BlendingInstructionCode, Commodities.Code AS ShortName, PackageIssueDetails.PackageIssueDetailID, PackageIssueDetails.PackageIssueImage1ID, PackageIssueDetails.PackageIssueImage2ID, Users.FirstName AS UserFirstName, N'Đổ bồn' AS Description " + "\r\n";
            queryString = queryString + "               FROM            PackageIssueDetails " + "\r\n";
            queryString = queryString + "                               INNER JOIN BlendingInstructions ON PackageIssueDetails.Barcode = @Barcode AND PackageIssueDetails.BlendingInstructionID = BlendingInstructions.BlendingInstructionID " + "\r\n";
            queryString = queryString + "                               INNER JOIN Commodities ON BlendingInstructions.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                               INNER JOIN PackageIssues ON PackageIssueDetails.PackageIssueID = PackageIssues.PackageIssueID " + "\r\n";
            queryString = queryString + "                               INNER JOIN Users ON PackageIssues.UserID = Users.UserID " + "\r\n";

            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       SELECT EntryID, EntryDate, Reference, ControllerName, Barcode, IIF(Quantity = 0, NULL, Quantity) AS Quantity, IIF(ROUND(QuantityRemains, " + (int)GlobalEnums.rndQuantity + ") = 0, NULL, QuantityRemains) AS QuantityRemains, TareWeight, Approved, LabApproved, LabHold, LabInActive, BinLocationCode, BlendingInstructionCode, ShortName, PackageIssueDetailID, PackageIssueImage1ID, PackageIssueImage2ID, UserFirstName, Description FROM @BarcodeJournals " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetBarcodeJournals", queryString);
        }
    }
}
