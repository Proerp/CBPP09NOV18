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
            this.GetBarcodeBasics();
            this.GetBarcodeJournals();
        }


        private void GetBarcodeBasics()
        {
            string queryString;
            
            queryString = " @SearchText nvarchar(50) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";
            queryString = queryString + "       SELECT      BarcodeID, Code " + "\r\n";
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

            queryString = queryString + "       IF LEN(@Barcode) > 5 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";

            queryString = queryString + "               SELECT          'Purchases/MaterialArrivals' AS ControllerName, GoodsArrivals.GoodsArrivalID AS EntryID, GoodsArrivals.EntryDate, GoodsArrivals.Reference, GoodsArrivalPackages.Barcode, GoodsArrivalPackages.Quantity, NULL AS BinLocationCode, NULL AS BlendingInstructionCode, NULL AS ShortName " + "\r\n";
            queryString = queryString + "               FROM            GoodsArrivalPackages " + "\r\n";
            queryString = queryString + "                               INNER JOIN GoodsArrivals ON GoodsArrivalPackages.Barcode = @Barcode AND GoodsArrivalPackages.GoodsArrivalID = GoodsArrivals.GoodsArrivalID " + "\r\n";

            queryString = queryString + "               UNION ALL " + "\r\n";

            queryString = queryString + "               SELECT          'Inventories/' + IIF(GoodsReceiptDetails.WarehouseTransferID IS NULL, 'MaterialReceipts', 'MaterialTransfers') AS ControllerName, ISNULL(GoodsReceiptDetails.WarehouseTransferID, GoodsReceiptDetails.GoodsReceiptID) AS EntryID, GoodsReceiptDetails.EntryDate, ISNULL(WarehouseTransfers.Reference, GoodsReceiptDetails.Reference) AS Reference, GoodsReceiptDetails.Barcode, GoodsReceiptDetails.Quantity, BinLocations.Code AS BinLocationCode, NULL AS BlendingInstructionCode, NULL AS ShortName " + "\r\n";
            queryString = queryString + "               FROM            GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                               INNER JOIN BinLocations ON GoodsReceiptDetails.Barcode = @Barcode AND GoodsReceiptDetails.BinLocationID = BinLocations.BinLocationID " + "\r\n";
            queryString = queryString + "                               LEFT JOIN WarehouseTransfers ON GoodsReceiptDetails.WarehouseTransferID = WarehouseTransfers.WarehouseTransferID " + "\r\n";

            queryString = queryString + "               UNION ALL " + "\r\n";

            queryString = queryString + "               SELECT          'Inventories/OtherMaterialIssues' AS ControllerName, WarehouseAdjustmentDetails.WarehouseAdjustmentID AS EntryID, WarehouseAdjustmentDetails.EntryDate, WarehouseAdjustmentDetails.Reference, GoodsReceiptDetails.Barcode, -WarehouseAdjustmentDetails.Quantity, BinLocations.Code AS BinLocationCode, NULL AS BlendingInstructionCode, NULL AS ShortName " + "\r\n";
            queryString = queryString + "               FROM            WarehouseAdjustmentDetails " + "\r\n";
            queryString = queryString + "                               INNER JOIN GoodsReceiptDetails ON GoodsReceiptDetails.Barcode = @Barcode AND WarehouseAdjustmentDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";
            queryString = queryString + "                               INNER JOIN BinLocations ON GoodsReceiptDetails.BinLocationID = BinLocations.BinLocationID " + "\r\n";

            queryString = queryString + "               UNION ALL " + "\r\n";

            queryString = queryString + "               SELECT          'Inventories/PackageIssues' AS ControllerName, PackageIssueDetails.PackageIssueID AS EntryID, PackageIssueDetails.EntryDate, PackageIssueDetails.Reference, PackageIssueDetails.Barcode, PackageIssueDetails.Quantity, NULL AS BinLocationCode, BlendingInstructions.Code AS BlendingInstructionCode, Commodities.Code AS ShortName " + "\r\n";
            queryString = queryString + "               FROM            PackageIssueDetails " + "\r\n";
            queryString = queryString + "                               INNER JOIN BlendingInstructions ON PackageIssueDetails.Barcode = @Barcode AND PackageIssueDetails.BlendingInstructionID = BlendingInstructions.BlendingInstructionID " + "\r\n";
            queryString = queryString + "                               INNER JOIN Commodities ON BlendingInstructions.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetBarcodeJournals", queryString);
        }
    }
}
