using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Productions
{
    public class FinishedProduct
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public FinishedProduct(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetFinishedProductIndexes();

            this.GetFinishedProductPendingFirmOrders();

            this.GetFinishedProductViewDetails();

            this.FinishedProductSaveRelative();
            this.FinishedProductPostSaveValidate();

            this.FinishedProductApproved();
            this.FinishedProductEditable();

            this.FinishedProductToggleApproved();

            this.FinishedProductInitReference();
        }


        private void GetFinishedProductIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      FinishedProducts.FinishedProductID, CAST(FinishedProducts.EntryDate AS DATE) AS EntryDate, FinishedProducts.Reference, Locations.Code AS LocationCode, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, FirmOrders.Specs AS FirmOrderSpecs, FinishedProducts.Description, FinishedProducts.TotalQuantity + FinishedProducts.TotalQuantityExcess AS TotalQuantity, FinishedProducts.TotalQuantityFailure, FinishedProducts.TotalQuantityExcess, FinishedProducts.TotalQuantityShortage, FinishedProducts.TotalSwarfs, FinishedProducts.Approved " + "\r\n";
            queryString = queryString + "       FROM        FinishedProducts " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON FinishedProducts.EntryDate >= @FromDate AND FinishedProducts.EntryDate <= @ToDate AND FinishedProducts.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.FinishedProduct + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = FinishedProducts.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN FirmOrders ON FinishedProducts.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON FinishedProducts.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetFinishedProductIndexes", queryString);
        }

        #region X


        private void GetFinishedProductViewDetails()
        {
            string queryString;

            queryString = " @FinishedProductID Int, @LocationID Int, @FirmOrderID Int, @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@FinishedProductID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BUILDSQLNew() + "\r\n";
            queryString = queryString + "                   ORDER BY CommodityCode, SemifinishedProductEntryDate, SemifinishedProductDetails.SemifinishedProductID, SemifinishedProductDetails.SemifinishedProductDetailID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BUILDSQLEdit() + "\r\n";
            queryString = queryString + "                       ORDER BY CommodityCode, SemifinishedProductEntryDate, SemifinishedProductDetails.SemifinishedProductID, SemifinishedProductDetails.SemifinishedProductDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BUILDSQLNew() + " AND SemifinishedProductDetails.SemifinishedProductDetailID NOT IN (SELECT SemifinishedProductDetailID FROM FinishedProductDetails WHERE FinishedProductID = @FinishedProductID) " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       " + this.BUILDSQLEdit() + "\r\n";
            queryString = queryString + "                       ORDER BY CommodityCode, SemifinishedProductEntryDate, SemifinishedProductDetails.SemifinishedProductID, SemifinishedProductDetails.SemifinishedProductDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetFinishedProductViewDetails", queryString);
        }

        private string BUILDSQLNew()
        {
            string queryString = "";

            queryString = queryString + "       SELECT      0 AS FinishedProductDetailID, 0 AS FinishedProductID, SemifinishedProductDetails.FirmOrderID, SemifinishedProductDetails.FirmOrderDetailID, SemifinishedProductDetails.PlannedOrderID, SemifinishedProductDetails.PlannedOrderDetailID, SemifinishedProductDetails.SemifinishedProductID, SemifinishedProductDetails.SemifinishedProductDetailID, SemifinishedProductDetails.SemifinishedHandoverID, SemifinishedProductDetails.EntryDate AS SemifinishedProductEntryDate, SemifinishedProducts.Reference AS SemifinishedProductReference, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, SemifinishedProductDetails.PiecePerPack, N'' AS Remarks, " + "\r\n";
            queryString = queryString + "                   SemifinishedProductDetails.WorkshiftID, Workshifts.EntryDate AS WorkshiftEntryDate, Workshifts.Code AS WorkshiftCode, GoodsReceiptDetails.Reference AS GoodsReceiptReference, GoodsReceiptDetails.Code AS GoodsReceiptCode, GoodsReceiptDetails.BatchEntryDate, ROUND(SemifinishedProductDetails.Quantity - SemifinishedProductDetails.QuantityFinished, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, 0.0 AS Quantity, 0.0 AS QuantityFailure, 0.0 AS QuantityExcess, 0.0 AS QuantityShortage, 0.0 AS Swarfs " + "\r\n";

            queryString = queryString + "       FROM        SemifinishedProductDetails " + "\r\n"; //AND SemifinishedProductDetails.LocationID = @LocationID 
            queryString = queryString + "                   INNER JOIN Commodities ON SemifinishedProductDetails.FirmOrderID = @FirmOrderID AND SemifinishedProductDetails.Approved = 1 AND SemifinishedProductDetails.HandoverApproved = 1 AND ROUND(SemifinishedProductDetails.Quantity - SemifinishedProductDetails.QuantityFinished, " + (int)GlobalEnums.rndQuantity + ") > 0 AND SemifinishedProductDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON SemifinishedProductDetails.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsReceiptDetails ON SemifinishedProductDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";
            queryString = queryString + "                   INNER JOIN SemifinishedProducts ON SemifinishedProductDetails.SemifinishedProductID = SemifinishedProducts.SemifinishedProductID " + "\r\n";

            return queryString;
        }

        private string BUILDSQLEdit()
        {
            string queryString = "";

            queryString = queryString + "       SELECT      FinishedProductDetails.FinishedProductDetailID, FinishedProductDetails.FinishedProductID, SemifinishedProductDetails.FirmOrderID, SemifinishedProductDetails.FirmOrderDetailID, SemifinishedProductDetails.PlannedOrderID, SemifinishedProductDetails.PlannedOrderDetailID, SemifinishedProductDetails.SemifinishedProductID, SemifinishedProductDetails.SemifinishedProductDetailID, SemifinishedProductDetails.SemifinishedHandoverID, SemifinishedProductDetails.EntryDate AS SemifinishedProductEntryDate, SemifinishedProducts.Reference AS SemifinishedProductReference, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, FinishedProductDetails.PiecePerPack, FinishedProductDetails.Remarks, " + "\r\n";
            queryString = queryString + "                   SemifinishedProductDetails.WorkshiftID, Workshifts.EntryDate AS WorkshiftEntryDate, Workshifts.Code AS WorkshiftCode, GoodsReceiptDetails.Reference AS GoodsReceiptReference, GoodsReceiptDetails.Code AS GoodsReceiptCode, GoodsReceiptDetails.BatchEntryDate, ROUND(SemifinishedProductDetails.Quantity - SemifinishedProductDetails.QuantityFinished + FinishedProductDetails.Quantity + FinishedProductDetails.QuantityFailure + FinishedProductDetails.QuantityShortage, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, FinishedProductDetails.Quantity, FinishedProductDetails.QuantityFailure, FinishedProductDetails.QuantityExcess, FinishedProductDetails.QuantityShortage, FinishedProductDetails.Swarfs " + "\r\n";

            queryString = queryString + "       FROM        FinishedProductDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN SemifinishedProductDetails ON FinishedProductDetails.FinishedProductID = @FinishedProductID AND FinishedProductDetails.SemifinishedProductDetailID = SemifinishedProductDetails.SemifinishedProductDetailID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON SemifinishedProductDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON SemifinishedProductDetails.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsReceiptDetails ON SemifinishedProductDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";
            queryString = queryString + "                   INNER JOIN SemifinishedProducts ON SemifinishedProductDetails.SemifinishedProductID = SemifinishedProducts.SemifinishedProductID " + "\r\n";

            return queryString;
        }
       


        private void GetFinishedProductPendingFirmOrders()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          FirmOrders.FirmOrderID, FirmOrders.EntryDate AS FirmOrderEntryDate, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, FirmOrders.Specification AS FirmOrderSpecification, FirmOrders.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName " + "\r\n";
            queryString = queryString + "       FROM            FirmOrders " + "\r\n";//LocationID = @LocationID AND 
            queryString = queryString + "                       INNER JOIN Customers ON FirmOrders.FirmOrderID IN (SELECT DISTINCT FirmOrderID FROM SemifinishedProductDetails WHERE Approved = 1 AND HandoverApproved = 1 AND ROUND(Quantity - QuantityFinished, " + (int)GlobalEnums.rndQuantity + ") > 0) AND FirmOrders.CustomerID = Customers.CustomerID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetFinishedProductPendingFirmOrders", queryString);
        }

        private void FinishedProductSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN  " + "\r\n";

            queryString = queryString + "       DECLARE @msg NVARCHAR(300) ";

            queryString = queryString + "       UPDATE          SemifinishedProductDetails " + "\r\n";
            queryString = queryString + "       SET             SemifinishedProductDetails.QuantityFinished = ROUND(SemifinishedProductDetails.QuantityFinished + (FinishedProductDetails.Quantity + FinishedProductDetails.QuantityFailure + FinishedProductDetails.QuantityShortage) * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "       FROM            FinishedProductDetails " + "\r\n";
            queryString = queryString + "                       INNER JOIN SemifinishedProductDetails ON SemifinishedProductDetails.Approved = 1 AND SemifinishedProductDetails.HandoverApproved = 1 AND FinishedProductDetails.FinishedProductID = @EntityID AND FinishedProductDetails.SemifinishedProductDetailID = SemifinishedProductDetails.SemifinishedProductDetailID " + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = (SELECT COUNT(*) FROM FinishedProductDetails WHERE FinishedProductID = @EntityID) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE  FirmOrderDetails " + "\r\n";
            queryString = queryString + "               SET     FirmOrderDetails.QuantityFinished = ROUND(FirmOrderDetails.QuantityFinished + (FinishedProductSummaries.Quantity + FinishedProductSummaries.QuantityFailure + FinishedProductSummaries.QuantityShortage) * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + "), " + "\r\n";
            queryString = queryString + "                       FirmOrderDetails.QuantityFailure = ROUND(FirmOrderDetails.QuantityFailure + FinishedProductSummaries.QuantityFailure * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + "), " + "\r\n";
            queryString = queryString + "                       FirmOrderDetails.QuantityExcess = ROUND(FirmOrderDetails.QuantityExcess + FinishedProductSummaries.QuantityExcess * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + "), " + "\r\n";
            queryString = queryString + "                       FirmOrderDetails.QuantityShortage = ROUND(FirmOrderDetails.QuantityShortage + FinishedProductSummaries.QuantityShortage * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + "), " + "\r\n";
            queryString = queryString + "                       FirmOrderDetails.Swarfs = ROUND(FirmOrderDetails.Swarfs + FinishedProductSummaries.Swarfs * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "               FROM    FirmOrderDetails INNER JOIN (SELECT FirmOrderDetailID, SUM(Quantity) AS Quantity, SUM(QuantityFailure) AS QuantityFailure, SUM(QuantityExcess) AS QuantityExcess, SUM(QuantityShortage) AS QuantityShortage, SUM(Swarfs) AS Swarfs FROM FinishedProductDetails WHERE FinishedProductID = @EntityID GROUP BY FirmOrderDetailID) AS FinishedProductSummaries ON FirmOrderDetails.FirmOrderDetailID = FinishedProductSummaries.FirmOrderDetailID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               SET         @msg = N'Phiếu BTP không tồn tại, chưa duyệt hoặc đã hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";




            queryString = queryString + "       IF ((SELECT Approved FROM FinishedProducts WHERE FinishedProductID = @EntityID AND Approved = 1) = 1) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE      FinishedProducts  SET Approved = 0 WHERE FinishedProductID = @EntityID AND Approved = 1" + "\r\n"; //CLEAR APPROVE BEFORE CALL FinishedProductToggleApproved
            queryString = queryString + "               IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "                   EXEC        FinishedProductToggleApproved @EntityID, 1 " + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       SET         @msg = N'Dữ liệu không tồn tại hoặc đã duyệt'; " + "\r\n";
            queryString = queryString + "                       THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("FinishedProductSaveRelative", queryString);
        }

        private void FinishedProductPostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày sản xuất phôi: ' + CAST(SemifinishedProducts.EntryDate AS nvarchar) FROM FinishedProductDetails INNER JOIN SemifinishedProducts ON FinishedProductDetails.FinishedProductID = @EntityID AND FinishedProductDetails.SemifinishedProductID = SemifinishedProducts.SemifinishedProductID AND FinishedProductDetails.EntryDate < SemifinishedProducts.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Số lượng đóng gói vượt quá số lượng phôi: ' + CAST(ROUND(Quantity - QuantityFinished, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM SemifinishedProductDetails WHERE (ROUND(Quantity - QuantityFinished, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("FinishedProductPostSaveValidate", queryArray);
        }




        private void FinishedProductApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = FinishedProductID FROM FinishedProducts WHERE FinishedProductID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("FinishedProductApproved", queryArray);
        }


        private void FinishedProductEditable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = FinishedProductID FROM FinishedProductDetails WHERE FinishedProductID = @EntityID AND NOT FinishedHandoverID IS NULL ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = FinishedProductID FROM FinishedHandoverDetails WHERE FinishedProductID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("FinishedProductEditable", queryArray);
        }

        private void FinishedProductToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      FinishedProducts  SET Approved = @Approved, ApprovedDate = GetDate() WHERE FinishedProductID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          FinishedProductDetails  SET Approved = @Approved WHERE FinishedProductID = @EntityID ; " + "\r\n";


            #region INIT FinishedProductPackages
            queryString = queryString + "               IF (@Approved = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       INSERT INTO     FinishedProductPackages (FinishedProductID, EntryDate, LocationID, CustomerID, FirmOrderID, PlannedOrderID, CommodityID, CommodityTypeID, PiecePerPack, Quantity, QuantityFailure, QuantityExcess, QuantityShortage, Swarfs, QuantityReceipted, Packages, OddPackages, QuantityWeights, QuantityFailureWeights, QuantityExcessWeights, QuantityShortageWeights, Remarks, Approved, HandoverApproved, SemifinishedProductReferences) " + "\r\n";
            queryString = queryString + "                       SELECT          MIN(FinishedProductID) AS FinishedProductID, MIN(EntryDate) AS EntryDate, MIN(LocationID) AS LocationID, MIN(CustomerID) AS CustomerID, MIN(FirmOrderID) AS FirmOrderID, MIN(PlannedOrderID) AS PlannedOrderID, CommodityID, MIN(CommodityTypeID) AS CommodityTypeID, MIN(PiecePerPack) AS PiecePerPack, ROUND(SUM(Quantity + QuantityExcess), " + (int)GlobalEnums.rndQuantity + ") AS Quantity, ROUND(SUM(QuantityFailure), " + (int)GlobalEnums.rndQuantity + ") AS QuantityFailure, ROUND(SUM(QuantityExcess), " + (int)GlobalEnums.rndQuantity + ") AS QuantityExcess, ROUND(SUM(QuantityShortage), " + (int)GlobalEnums.rndQuantity + ") AS QuantityShortage, ROUND(SUM(Swarfs), " + (int)GlobalEnums.rndQuantity + ") AS Swarfs, 0 AS QuantityReceipted, IIF(MIN(PiecePerPack) <> 0, CAST(ROUND(SUM(Quantity), " + (int)GlobalEnums.rndQuantity + ") AS int) / MIN(PiecePerPack), 0) AS Packages, IIF(MIN(PiecePerPack) <> 0, ROUND(SUM(Quantity), " + (int)GlobalEnums.rndQuantity + ") % MIN(PiecePerPack), 0) AS OddPackages, 999 AS QuantityWeights, 999 AS QuantityFailureWeights, 999 AS QuantityExcessWeights, 999 AS QuantityShortageWeights, MAX(Remarks) AS Remarks, 1 AS Approved, 0 AS HandoverApproved, " + "\r\n";

            queryString = queryString + "                                       STUFF   ((SELECT ', ' + SemifinishedProducts.Reference " + "\r\n";
            queryString = queryString + "                                       FROM    FinishedProductDetails INNER JOIN SemifinishedProducts ON FinishedProductDetails.SemifinishedProductID = SemifinishedProducts.SemifinishedProductID " + "\r\n";
            queryString = queryString + "                                       WHERE   FinishedProductDetails.FinishedProductID = @EntityID AND FinishedProductDetails.CommodityID = FinishedProductDetailResults.CommodityID " + "\r\n";
            queryString = queryString + "                                       FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)'),1,1,'') AS SemifinishedProductReferences " + "\r\n";

            queryString = queryString + "                       FROM            FinishedProductDetails  FinishedProductDetailResults " + "\r\n";
            queryString = queryString + "                       WHERE           FinishedProductID = @EntityID " + "\r\n";
            queryString = queryString + "                       GROUP BY        CommodityID; " + "\r\n";

            queryString = queryString + "                       UPDATE          FinishedProductDetails " + "\r\n";
            queryString = queryString + "                       SET             FinishedProductDetails.FinishedProductPackageID = FinishedProductPackages.FinishedProductPackageID " + "\r\n";
            queryString = queryString + "                       FROM            FinishedProductDetails INNER JOIN FinishedProductPackages ON FinishedProductDetails.FinishedProductID = @EntityID AND FinishedProductDetails.FinishedProductID = FinishedProductPackages.FinishedProductID AND FinishedProductDetails.CommodityID = FinishedProductPackages.CommodityID; " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       UPDATE          FinishedProductDetails SET FinishedProductPackageID = NULL WHERE FinishedProductID = @EntityID ; " + "\r\n";
            queryString = queryString + "                       DELETE FROM     FinishedProductPackages WHERE FinishedProductID = @EntityID ; " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";
            #endregion INIT FinishedProductPackages


            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, N'hủy', '')  + N' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("FinishedProductToggleApproved", queryString);
        }

        private void FinishedProductInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("FinishedProducts", "FinishedProductID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.FinishedProduct));
            this.totalSmartPortalEntities.CreateTrigger("FinishedProductInitReference", simpleInitReference.CreateQuery());
        }


        #endregion
    }
}
