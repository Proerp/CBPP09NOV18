using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Inventories
{
    public class WarehouseAdjustment
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public WarehouseAdjustment(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetWarehouseAdjustmentIndexes();


            this.GetWarehouseAdjustmentViewDetails();

            this.WarehouseAdjustmentSaveRelative();
            this.WarehouseAdjustmentPostSaveValidate();

            this.WarehouseAdjustmentApproved();
            this.WarehouseAdjustmentEditable();

            this.WarehouseAdjustmentToggleApproved();

            this.WarehouseAdjustmentInitReference();

            this.WarehouseAdjustmentSheet();
        }


        private void GetWarehouseAdjustmentIndexes()
        {
            string queryString;

            queryString = " @NMVNTaskID int, @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      WarehouseAdjustments.WarehouseAdjustmentID, CAST(WarehouseAdjustments.EntryDate AS DATE) AS EntryDate, WarehouseAdjustments.Reference, Locations.Code AS LocationCode, Customers.Name AS CustomerName, WarehouseAdjustments.WarehouseAdjustmentTypeID, WarehouseAdjustmentTypes.Name AS WarehouseAdjustmentTypeName, WarehouseAdjustments.AdjustmentJobs, WarehouseAdjustments.Description, WarehouseAdjustments.TotalQuantity, WarehouseAdjustments.Approved " + "\r\n";
            queryString = queryString + "       FROM        WarehouseAdjustments " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON WarehouseAdjustments.NMVNTaskID = @NMVNTaskID AND WarehouseAdjustments.EntryDate >= @FromDate AND WarehouseAdjustments.EntryDate <= @ToDate AND WarehouseAdjustments.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = @NMVNTaskID AND AccessControls.AccessLevel > 0) AND Locations.LocationID = WarehouseAdjustments.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers Customers ON WarehouseAdjustments.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN WarehouseAdjustmentTypes ON WarehouseAdjustments.WarehouseAdjustmentTypeID = WarehouseAdjustmentTypes.WarehouseAdjustmentTypeID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetWarehouseAdjustmentIndexes", queryString);
        }


        #region X


        private void GetWarehouseAdjustmentViewDetails()
        {
            string queryString;

            queryString = " @WarehouseAdjustmentID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      WarehouseAdjustmentDetails.WarehouseAdjustmentDetailID, WarehouseAdjustmentDetails.WarehouseAdjustmentID, WarehouseAdjustmentDetails.GoodsReceiptID, WarehouseAdjustmentDetails.GoodsReceiptDetailID, GoodsReceiptDetails.Reference AS GoodsReceiptReference, GoodsReceiptDetails.EntryDate AS GoodsReceiptEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, WarehouseAdjustmentDetails.BatchID, WarehouseAdjustmentDetails.BatchEntryDate, " + "\r\n";
            queryString = queryString + "                   ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued + (-WarehouseAdjustmentDetails.Quantity), " + (int)GlobalEnums.rndQuantity + ") AS QuantityAvailables, WarehouseAdjustmentDetails.Quantity, WarehouseAdjustmentDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        WarehouseAdjustmentDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON WarehouseAdjustmentDetails.WarehouseAdjustmentID = @WarehouseAdjustmentID AND WarehouseAdjustmentDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN GoodsReceiptDetails ON WarehouseAdjustmentDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetWarehouseAdjustmentViewDetails", queryString);
        }



        private void WarehouseAdjustmentSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            //queryString = queryString + "   IF (SELECT HasDeliveryAdvice FROM WarehouseAdjustments WHERE WarehouseAdjustmentID = @EntityID) = 1 " + "\r\n";
            queryString = queryString + "       BEGIN " + "\r\n";

            queryString = queryString + "           IF (@SaveRelativeOption = 1) ";
            queryString = queryString + "               BEGIN ";
            queryString = queryString + "                   UPDATE          WarehouseAdjustmentDetails " + "\r\n";
            queryString = queryString + "                   SET             WarehouseAdjustmentDetails.Reference = WarehouseAdjustments.Reference " + "\r\n";
            queryString = queryString + "                   FROM            WarehouseAdjustments INNER JOIN WarehouseAdjustmentDetails ON WarehouseAdjustments.WarehouseAdjustmentID = @EntityID AND WarehouseAdjustments.WarehouseAdjustmentID = WarehouseAdjustmentDetails.WarehouseAdjustmentID " + "\r\n";
            queryString = queryString + "               END ";

            queryString = queryString + "           UPDATE          GoodsReceiptDetails" + "\r\n";
            queryString = queryString + "           SET             GoodsReceiptDetails.QuantityIssued = ROUND(GoodsReceiptDetails.QuantityIssued + WarehouseAdjustmentDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "           FROM            (SELECT GoodsReceiptDetailID, SUM(-Quantity) AS Quantity FROM WarehouseAdjustmentDetails WHERE WarehouseAdjustmentID = @EntityID AND Quantity < 0 GROUP BY GoodsReceiptDetailID) WarehouseAdjustmentDetails " + "\r\n";
            queryString = queryString + "                           INNER JOIN GoodsReceiptDetails ON WarehouseAdjustmentDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID AND GoodsReceiptDetails.Approved = 1 " + "\r\n";

            queryString = queryString + "           IF @@ROWCOUNT <> (SELECT COUNT(*) FROM (SELECT GoodsReceiptDetailID FROM WarehouseAdjustmentDetails WHERE WarehouseAdjustmentID = @EntityID AND Quantity < 0 GROUP BY GoodsReceiptDetailID) DISTINCTWarehouseAdjustmentDetails) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   DECLARE     @msg NVARCHAR(300) = N'Phiếu giao hàng đã hủy, hoặc chưa duyệt' ; " + "\r\n";
            queryString = queryString + "                   THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("WarehouseAdjustmentSaveRelative", queryString);
        }

        private void WarehouseAdjustmentPostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày điều chỉnh kho: ' + CAST(GoodsReceipts.EntryDate AS nvarchar) FROM WarehouseAdjustmentDetails INNER JOIN GoodsReceipts ON WarehouseAdjustmentDetails.WarehouseAdjustmentID = @EntityID AND WarehouseAdjustmentDetails.GoodsReceiptID = GoodsReceipts.GoodsReceiptID AND WarehouseAdjustmentDetails.EntryDate < GoodsReceipts.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Số lượng điều chỉnh giảm vượt quá số lượng tồn kho: ' + CAST(ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM GoodsReceiptDetails WHERE (ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("WarehouseAdjustmentPostSaveValidate", queryArray);
        }




        private void WarehouseAdjustmentApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = WarehouseAdjustmentID FROM WarehouseAdjustments WHERE WarehouseAdjustmentID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("WarehouseAdjustmentApproved", queryArray);
        }


        private void WarehouseAdjustmentEditable()
        {
            string[] queryArray = new string[3]; //IMPORTANT: THESE QUERIES ARE COPIED FROM GoodsReceiptEditable

            string queryString = "       DECLARE @GoodsReceiptID int " + "\r\n";
            queryString = queryString + "       IF ((SELECT HasPositiveLine FROM WarehouseAdjustments WHERE WarehouseAdjustmentID = @EntityID) = 0) BEGIN SELECT @FoundEntity AS FoundEntity    RETURN 0 END " + "\r\n";

            queryString = queryString + "       SELECT TOP 1 @GoodsReceiptID = GoodsReceiptID FROM GoodsReceipts WHERE WarehouseAdjustmentID = @EntityID " + "\r\n";
            queryString = queryString + "       IF (@GoodsReceiptID IS NULL) BEGIN SELECT @FoundEntity AS FoundEntity    RETURN 0 END " + "\r\n";

            queryArray[0] = " SELECT TOP 1 @FoundEntity = GoodsReceiptID FROM MaterialIssueDetails WHERE GoodsReceiptID = @GoodsReceiptID ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = GoodsReceiptID FROM WarehouseTransferDetails WHERE GoodsReceiptID = @GoodsReceiptID ";
            queryArray[2] = " SELECT TOP 1 @FoundEntity = GoodsReceiptID FROM WarehouseAdjustmentDetails WHERE GoodsReceiptID = @GoodsReceiptID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("WarehouseAdjustmentEditable", queryArray, queryString);
        }

        private void WarehouseAdjustmentToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      WarehouseAdjustments  SET Approved = @Approved, ApprovedDate = GetDate() WHERE WarehouseAdjustmentID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          WarehouseAdjustmentDetails  SET Approved = @Approved WHERE WarehouseAdjustmentID = @EntityID ; " + "\r\n";

            queryString = queryString + "               UPDATE          GoodsReceipts  SET Approved = @Approved, ApprovedDate = GetDate() WHERE WarehouseAdjustmentID = @EntityID " + "\r\n";
            queryString = queryString + "               UPDATE          GoodsReceiptDetails  SET Approved = @Approved WHERE WarehouseAdjustmentID = @EntityID ; " + "\r\n";

            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, N'hủy', '')  + N' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("WarehouseAdjustmentToggleApproved", queryString);
        }

        private void WarehouseAdjustmentInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("WarehouseAdjustments", "WarehouseAdjustmentID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.WarehouseAdjustment));
            this.totalSmartPortalEntities.CreateTrigger("WarehouseAdjustmentInitReference", simpleInitReference.CreateQuery());
        }

        private void WarehouseAdjustmentSheet()
        {
            string queryString = " @WarehouseAdjustmentID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @LocalWarehouseAdjustmentID int    SET @LocalWarehouseAdjustmentID = @WarehouseAdjustmentID" + "\r\n";

            queryString = queryString + "       SELECT          NMVNTaskName = IIF(WarehouseAdjustments.NMVNTaskID IN (" + ((int)GlobalEnums.NmvnTaskID.OtherMaterialReceipt).ToString() + "," + ((int)GlobalEnums.NmvnTaskID.OtherMaterialIssue).ToString() + "," + ((int)GlobalEnums.NmvnTaskID.MaterialAdjustment).ToString() + "), N'NVL', IIF( WarehouseAdjustments.NMVNTaskID IN (" + ((int)GlobalEnums.NmvnTaskID.OtherItemReceipt).ToString() + "," + ((int)GlobalEnums.NmvnTaskID.OtherItemIssue).ToString() + "," + ((int)GlobalEnums.NmvnTaskID.ItemAdjustment).ToString() + "), N'Màng', IIF( WarehouseAdjustments.NMVNTaskID IN (" + ((int)GlobalEnums.NmvnTaskID.OtherProductReceipt).ToString() + "," + ((int)GlobalEnums.NmvnTaskID.OtherProductIssue).ToString() + "," + ((int)GlobalEnums.NmvnTaskID.ProductAdjustment).ToString() + "), N'Thành phẩm', ''))), " + "\r\n";
            queryString = queryString + "                       NMVNTaskTypeName = IIF(WarehouseAdjustments.NMVNTaskID IN (" + ((int)GlobalEnums.NmvnTaskID.OtherMaterialReceipt).ToString() + "," + ((int)GlobalEnums.NmvnTaskID.OtherItemReceipt).ToString() + "," + ((int)GlobalEnums.NmvnTaskID.OtherProductReceipt).ToString() + "), N'Nhập', IIF( WarehouseAdjustments.NMVNTaskID IN (" + ((int)GlobalEnums.NmvnTaskID.OtherMaterialIssue).ToString() + "," + ((int)GlobalEnums.NmvnTaskID.OtherItemIssue).ToString() + "," + ((int)GlobalEnums.NmvnTaskID.OtherProductIssue).ToString() + "), N'Xuất', IIF( WarehouseAdjustments.NMVNTaskID IN (" + ((int)GlobalEnums.NmvnTaskID.MaterialAdjustment).ToString() + "," + ((int)GlobalEnums.NmvnTaskID.ItemAdjustment).ToString() + "," + ((int)GlobalEnums.NmvnTaskID.ProductAdjustment).ToString() + "), N'Điều chỉnh', ''))), " + "\r\n";
            queryString = queryString + "                       WarehouseAdjustments.WarehouseAdjustmentID, WarehouseAdjustments.EntryDate, WarehouseAdjustments.Reference, WarehouseAdjustments.NMVNTaskID, WarehouseAdjustments.WarehouseID, WarehouseAdjustments.WarehouseReceiptID, WarehouseAdjustments.WarehouseAdjustmentTypeID, WarehouseAdjustments.LocationID, Customers.Name AS CustomerName, WarehouseAdjustments.AdjustmentJobs, WarehouseAdjustments.Description, " + "\r\n";
            queryString = queryString + "                       WarehouseAdjustmentDetails.BatchEntryDate, WarehouseAdjustmentDetails.CommodityID, WarehouseAdjustmentDetails.CommodityTypeID,  WarehouseAdjustmentDetails.QuantityReceipted, WarehouseAdjustmentDetails.Remarks, " + "\r\n";
            queryString = queryString + "                       Quantity = IIF(WarehouseAdjustments.NMVNTaskID IN (" + ((int)GlobalEnums.NmvnTaskID.OtherMaterialIssue).ToString() + "," + ((int)GlobalEnums.NmvnTaskID.OtherItemIssue).ToString() + "," + ((int)GlobalEnums.NmvnTaskID.OtherProductIssue).ToString() + "), - WarehouseAdjustmentDetails.Quantity, WarehouseAdjustmentDetails.Quantity), " + "\r\n";
            queryString = queryString + "                       Commodities.Code, Commodities.CodePartA, Commodities.CodePartB, Commodities.CodePartC, Commodities.CodePartD, Commodities.CodePartE, Commodities.CodePartF, Commodities.Name AS CommodityName, Commodities.CommodityLineID, Commodities.SalesUnit " + "\r\n";

            queryString = queryString + "       FROM            WarehouseAdjustments " + "\r\n";
            queryString = queryString + "                       INNER JOIN WarehouseAdjustmentDetails ON WarehouseAdjustments.WarehouseAdjustmentID = @LocalWarehouseAdjustmentID AND WarehouseAdjustments.WarehouseAdjustmentID = WarehouseAdjustmentDetails.WarehouseAdjustmentID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON WarehouseAdjustments.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities ON WarehouseAdjustmentDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "       ORDER BY        WarehouseAdjustmentDetails.CommodityTypeID " + "\r\n";


            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("WarehouseAdjustmentSheet", queryString);

        }

        #endregion
    }
}
