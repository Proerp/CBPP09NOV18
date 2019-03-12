using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Productions
{
    public class FinishedItem
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public FinishedItem(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetFinishedItemIndexes();

            this.GetFinishedItemPendingFirmOrders();

            this.GetFinishedItemViewDetails();
            this.GetFinishedItemViewLots();

            this.FinishedItemSaveRelative();
            this.FinishedItemPostSaveValidate();

            this.FinishedItemApproved();
            this.FinishedItemEditable();

            this.FinishedItemToggleApproved();

            this.FinishedItemInitReference();

            //this.FinishedItemSheet();
        }


        private void GetFinishedItemIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      FinishedItems.FinishedItemID, CAST(FinishedItems.EntryDate AS DATE) AS EntryDate, FinishedItems.Reference, Locations.Code AS LocationCode, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, FirmOrders.Specs AS FirmOrderSpecs, FirmOrders.Specification AS FirmOrderSpecification, FinishedItems.Description, FinishedItems.WorkshiftID, Workshifts.Name AS WorkshiftName, Workshifts.EntryDate AS WorkshiftEntryDate, FinishedItems.TotalQuantity + FinishedItems.TotalQuantityExcess AS TotalQuantity, FinishedItems.TotalQuantityFailure, FinishedItems.TotalQuantityExcess, FinishedItems.TotalQuantityShortage, FinishedItems.TotalSwarfs, FinishedItems.Approved " + "\r\n";
            queryString = queryString + "       FROM        FinishedItems " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON FinishedItems.EntryDate >= @FromDate AND FinishedItems.EntryDate <= @ToDate AND FinishedItems.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.FinishedItem + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = FinishedItems.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN FirmOrders ON FinishedItems.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON FinishedItems.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON FinishedItems.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetFinishedItemIndexes", queryString);
        }

        #region X


        private void GetFinishedItemViewDetails()
        {
            string queryString;

            queryString = " @FinishedItemID Int, @LocationID Int, @FirmOrderID Int, @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@FinishedItemID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BUILDSQLNew() + "\r\n";
            queryString = queryString + "                   ORDER BY CommodityCode, SemifinishedItemEntryDate, SemifinishedItemDetails.SemifinishedItemID, SemifinishedItemDetails.SemifinishedItemDetailID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BUILDSQLEdit() + "\r\n";
            queryString = queryString + "                       ORDER BY CommodityCode, SemifinishedItemEntryDate, SemifinishedItemDetails.SemifinishedItemID, SemifinishedItemDetails.SemifinishedItemDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BUILDSQLNew() + " AND SemifinishedItemDetails.SemifinishedItemDetailID NOT IN (SELECT SemifinishedItemDetailID FROM FinishedItemDetails WHERE FinishedItemID = @FinishedItemID) " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       " + this.BUILDSQLEdit() + "\r\n";
            queryString = queryString + "                       ORDER BY CommodityCode, SemifinishedItemEntryDate, SemifinishedItemDetails.SemifinishedItemID, SemifinishedItemDetails.SemifinishedItemDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetFinishedItemViewDetails", queryString);
        }

        private string BUILDSQLNew()
        {
            string queryString = "";

            queryString = queryString + "       SELECT      0 AS FinishedItemDetailID, 0 AS FinishedItemID, SemifinishedItemDetails.FirmOrderID, SemifinishedItemDetails.FirmOrderDetailID, SemifinishedItemDetails.PlannedOrderID, SemifinishedItemDetails.PlannedOrderDetailID, SemifinishedItemDetails.SemifinishedItemID, SemifinishedItemDetails.SemifinishedItemDetailID, SemifinishedItemDetails.SemifinishedHandoverID, SemifinishedItemDetails.EntryDate AS SemifinishedItemEntryDate, SemifinishedItems.Reference AS SemifinishedItemReference, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, SemifinishedItemDetails.PiecePerPack, N'' AS Remarks, " + "\r\n";
            queryString = queryString + "                   SemifinishedItemDetails.WorkshiftID, Workshifts.EntryDate AS WorkshiftEntryDate, Workshifts.Code AS WorkshiftCode, ISNULL(ROUND(SemifinishedItemDetails.Quantity - SemifinishedItemDetails.QuantityFinished, " + (int)GlobalEnums.rndQuantity + "), 0) AS QuantityRemains, 0.0 AS Quantity, 0.0 AS QuantityFailure, 0.0 AS QuantityExcess, 0.0 AS QuantityShortage, 0.0 AS Swarfs " + "\r\n";

            queryString = queryString + "       FROM        SemifinishedItemDetails " + "\r\n"; //AND SemifinishedItemDetails.LocationID = @LocationID 
            queryString = queryString + "                   INNER JOIN Commodities ON SemifinishedItemDetails.FirmOrderID = @FirmOrderID AND SemifinishedItemDetails.Approved = 1 AND SemifinishedItemDetails.HandoverApproved = 1 AND ROUND(SemifinishedItemDetails.Quantity - SemifinishedItemDetails.QuantityFinished, " + (int)GlobalEnums.rndQuantity + ") > 0 AND SemifinishedItemDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON SemifinishedItemDetails.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN SemifinishedItems ON SemifinishedItemDetails.SemifinishedItemID = SemifinishedItems.SemifinishedItemID " + "\r\n";

            return queryString;
        }

        private string BUILDSQLEdit()
        {
            string queryString = "";

            queryString = queryString + "       SELECT      FinishedItemDetails.FinishedItemDetailID, FinishedItemDetails.FinishedItemID, SemifinishedItemDetails.FirmOrderID, SemifinishedItemDetails.FirmOrderDetailID, SemifinishedItemDetails.PlannedOrderID, SemifinishedItemDetails.PlannedOrderDetailID, SemifinishedItemDetails.SemifinishedItemID, SemifinishedItemDetails.SemifinishedItemDetailID, SemifinishedItemDetails.SemifinishedHandoverID, SemifinishedItemDetails.EntryDate AS SemifinishedItemEntryDate, SemifinishedItems.Reference AS SemifinishedItemReference, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, FinishedItemDetails.PiecePerPack, FinishedItemDetails.Remarks, " + "\r\n";
            queryString = queryString + "                   SemifinishedItemDetails.WorkshiftID, Workshifts.EntryDate AS WorkshiftEntryDate, Workshifts.Code AS WorkshiftCode, ISNULL(ROUND(SemifinishedItemDetails.Quantity - SemifinishedItemDetails.QuantityFinished + FinishedItemDetails.Quantity + FinishedItemDetails.QuantityFailure + FinishedItemDetails.QuantityShortage, " + (int)GlobalEnums.rndQuantity + "), 0) AS QuantityRemains, FinishedItemDetails.Quantity, FinishedItemDetails.QuantityFailure, FinishedItemDetails.QuantityExcess, FinishedItemDetails.QuantityShortage, FinishedItemDetails.Swarfs " + "\r\n";

            queryString = queryString + "       FROM        FinishedItemDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN SemifinishedItemDetails ON FinishedItemDetails.FinishedItemID = @FinishedItemID AND FinishedItemDetails.SemifinishedItemDetailID = SemifinishedItemDetails.SemifinishedItemDetailID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON SemifinishedItemDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON SemifinishedItemDetails.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN SemifinishedItems ON SemifinishedItemDetails.SemifinishedItemID = SemifinishedItems.SemifinishedItemID " + "\r\n";

            return queryString;
        }


        private void GetFinishedItemViewLots()
        {
            string queryString;

            queryString = " @FinishedItemID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "        SELECT         FinishedItemLots.FinishedItemLotID, FinishedItemLots.FinishedItemID, FinishedItemLots.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, FinishedItemLots.CommodityTypeID, FinishedItemLots.BatchID, FinishedItemLots.BatchEntryDate, " + "\r\n";
            queryString = queryString + "                       FinishedItemLots.PiecePerPack, FinishedItemLots.Quantity, FinishedItemLots.QuantityFailure, FinishedItemLots.QuantityExcess, FinishedItemLots.QuantityShortage, FinishedItemLots.Swarfs, FinishedItemLots.Packages, FinishedItemLots.OddPackages, FinishedItemLots.Remarks " + "\r\n";
            queryString = queryString + "        FROM           FinishedItemLots " + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities ON FinishedItemLots.FinishedItemID = @FinishedItemID AND FinishedItemLots.CommodityID = Commodities.CommodityID " + "\r\n";
           
            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetFinishedItemViewLots", queryString);
        }


        private void GetFinishedItemPendingFirmOrders()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          FirmOrders.PlannedOrderID, FirmOrders.FirmOrderID, FirmOrders.EntryDate AS FirmOrderEntryDate, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, FirmOrders.Specification AS FirmOrderSpecification, FirmOrders.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName " + "\r\n";
            queryString = queryString + "       FROM            FirmOrders " + "\r\n";//LocationID = @LocationID AND 
            queryString = queryString + "                       INNER JOIN Customers ON FirmOrders.FirmOrderID IN (SELECT DISTINCT FirmOrderID FROM SemifinishedItemDetails WHERE Approved = 1 AND HandoverApproved = 1 AND ROUND(Quantity - QuantityFinished, " + (int)GlobalEnums.rndQuantity + ") > 0) AND FirmOrders.CustomerID = Customers.CustomerID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetFinishedItemPendingFirmOrders", queryString);
        }

        private void FinishedItemSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN  " + "\r\n";

            queryString = queryString + "       DECLARE @msg NVARCHAR(300) ";

            queryString = queryString + "       IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            #region UPDATE WorkshiftID
            queryString = queryString + "               DECLARE         @EntryDate Datetime, @ShiftID int, @WorkshiftID int " + "\r\n";
            queryString = queryString + "               SELECT          @EntryDate = CONVERT(date, EntryDate), @ShiftID = ShiftID FROM FinishedItems WHERE FinishedItemID = @EntityID " + "\r\n";
            queryString = queryString + "               SET             @WorkshiftID = (SELECT TOP 1 WorkshiftID FROM Workshifts WHERE EntryDate = @EntryDate AND ShiftID = @ShiftID) " + "\r\n";

            queryString = queryString + "               IF             (@WorkshiftID IS NULL) " + "\r\n";
            queryString = queryString + "                   BEGIN ";
            queryString = queryString + "                       INSERT INTO     Workshifts(EntryDate, ShiftID, Code, Name, WorkingHours, Remarks) SELECT @EntryDate, ShiftID, Code, Name, WorkingHours, Remarks FROM Shifts WHERE ShiftID = @ShiftID " + "\r\n";
            queryString = queryString + "                       SELECT          @WorkshiftID = SCOPE_IDENTITY(); " + "\r\n";
            queryString = queryString + "                   END ";

            queryString = queryString + "               UPDATE          FinishedItems        SET WorkshiftID = @WorkshiftID WHERE FinishedItemID = @EntityID " + "\r\n";
            queryString = queryString + "               UPDATE          FinishedItemDetails  SET WorkshiftID = @WorkshiftID WHERE FinishedItemID = @EntityID " + "\r\n";
            queryString = queryString + "               UPDATE          FinishedItemLots SET WorkshiftID = @WorkshiftID WHERE FinishedItemID = @EntityID " + "\r\n";
            #endregion UPDATE WorkshiftID
            queryString = queryString + "           END " + "\r\n";


            queryString = queryString + "       UPDATE          SemifinishedItemDetails " + "\r\n";
            queryString = queryString + "       SET             SemifinishedItemDetails.QuantityFinished = ROUND(SemifinishedItemDetails.QuantityFinished + (FinishedItemDetails.Quantity + FinishedItemDetails.QuantityFailure + FinishedItemDetails.QuantityShortage) * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "       FROM            FinishedItemDetails " + "\r\n";
            queryString = queryString + "                       INNER JOIN SemifinishedItemDetails ON SemifinishedItemDetails.Approved = 1 AND SemifinishedItemDetails.HandoverApproved = 1 AND FinishedItemDetails.FinishedItemID = @EntityID AND FinishedItemDetails.SemifinishedItemDetailID = SemifinishedItemDetails.SemifinishedItemDetailID " + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = (SELECT COUNT(*) FROM FinishedItemDetails WHERE FinishedItemID = @EntityID) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE  FirmOrderDetails " + "\r\n";
            queryString = queryString + "               SET     FirmOrderDetails.QuantityFinished = ROUND(FirmOrderDetails.QuantityFinished + (FinishedItemSummaries.Quantity + FinishedItemSummaries.QuantityFailure + FinishedItemSummaries.QuantityShortage) * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + "), " + "\r\n";
            queryString = queryString + "                       FirmOrderDetails.QuantityFailure = ROUND(FirmOrderDetails.QuantityFailure + FinishedItemSummaries.QuantityFailure * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + "), " + "\r\n";
            queryString = queryString + "                       FirmOrderDetails.QuantityExcess = ROUND(FirmOrderDetails.QuantityExcess + FinishedItemSummaries.QuantityExcess * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + "), " + "\r\n";
            queryString = queryString + "                       FirmOrderDetails.QuantityShortage = ROUND(FirmOrderDetails.QuantityShortage + FinishedItemSummaries.QuantityShortage * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + "), " + "\r\n";
            queryString = queryString + "                       FirmOrderDetails.Swarfs = ROUND(FirmOrderDetails.Swarfs + FinishedItemSummaries.Swarfs * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "               FROM    FirmOrderDetails INNER JOIN (SELECT FirmOrderDetailID, SUM(Quantity) AS Quantity, SUM(QuantityFailure) AS QuantityFailure, SUM(QuantityExcess) AS QuantityExcess, SUM(QuantityShortage) AS QuantityShortage, SUM(Swarfs) AS Swarfs FROM FinishedItemDetails WHERE FinishedItemID = @EntityID GROUP BY FirmOrderDetailID) AS FinishedItemSummaries ON FirmOrderDetails.FirmOrderDetailID = FinishedItemSummaries.FirmOrderDetailID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               SET         @msg = N'Phiếu BTP không tồn tại, chưa duyệt hoặc đã hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";




            queryString = queryString + "       IF ((SELECT Approved FROM FinishedItems WHERE FinishedItemID = @EntityID AND Approved = 1) = 1) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE      FinishedItems  SET Approved = 0 WHERE FinishedItemID = @EntityID AND Approved = 1" + "\r\n"; //CLEAR APPROVE BEFORE CALL FinishedItemToggleApproved
            queryString = queryString + "               IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "                   EXEC        FinishedItemToggleApproved @EntityID, 1 " + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       SET         @msg = N'Dữ liệu không tồn tại hoặc đã duyệt'; " + "\r\n";
            queryString = queryString + "                       THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("FinishedItemSaveRelative", queryString);
        }

        private void FinishedItemPostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày sản xuất phôi: ' + CAST(SemifinishedItems.EntryDate AS nvarchar) FROM FinishedItemDetails INNER JOIN SemifinishedItems ON FinishedItemDetails.FinishedItemID = @EntityID AND FinishedItemDetails.SemifinishedItemID = SemifinishedItems.SemifinishedItemID AND FinishedItemDetails.EntryDate < SemifinishedItems.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Số lượng đóng gói vượt quá số lượng phôi: ' + CAST(ROUND(Quantity - QuantityFinished, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM SemifinishedItemDetails WHERE (ROUND(Quantity - QuantityFinished, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("FinishedItemPostSaveValidate", queryArray);
        }




        private void FinishedItemApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = FinishedItemID FROM FinishedItems WHERE FinishedItemID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("FinishedItemApproved", queryArray);
        }


        private void FinishedItemEditable()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = FinishedItemID FROM FinishedItemDetails WHERE FinishedItemID = @EntityID AND NOT FinishedHandoverID IS NULL ";
            //queryArray[1] = " SELECT TOP 1 @FoundEntity = FinishedItemID FROM FinishedHandoverDetails WHERE FinishedItemID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("FinishedItemEditable", queryArray);
        }

        private void FinishedItemToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      FinishedItems  SET Approved = @Approved, ApprovedDate = GetDate() WHERE FinishedItemID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          FinishedItemDetails  SET Approved = @Approved WHERE FinishedItemID = @EntityID ; " + "\r\n";


            //#region INIT FinishedItemPackages - NO NEED
            //queryString = queryString + "               IF (@Approved = 1) " + "\r\n";
            //queryString = queryString + "                   BEGIN " + "\r\n";
            //queryString = queryString + "                       INSERT INTO     FinishedItemPackages (FinishedItemID, EntryDate, LocationID, ShiftID, WorkshiftID, CustomerID, FirmOrderID, PlannedOrderID, CommodityID, CommodityTypeID, PiecePerPack, Quantity, QuantityFailure, QuantityExcess, QuantityShortage, Swarfs, QuantityReceipted, Packages, OddPackages, Remarks, Approved, HandoverApproved, SemifinishedItemReferences) " + "\r\n";
            //queryString = queryString + "                       SELECT          MIN(FinishedItemID) AS FinishedItemID, MIN(EntryDate) AS EntryDate, MIN(LocationID) AS LocationID, MIN(ShiftID) AS ShiftID, MIN(WorkshiftID) AS WorkshiftID, MIN(CustomerID) AS CustomerID, MIN(FirmOrderID) AS FirmOrderID, MIN(PlannedOrderID) AS PlannedOrderID, CommodityID, MIN(CommodityTypeID) AS CommodityTypeID, MIN(PiecePerPack) AS PiecePerPack, ROUND(SUM(Quantity + QuantityExcess), " + (int)GlobalEnums.rndQuantity + ") AS Quantity, ROUND(SUM(QuantityFailure), " + (int)GlobalEnums.rndQuantity + ") AS QuantityFailure, ROUND(SUM(QuantityExcess), " + (int)GlobalEnums.rndQuantity + ") AS QuantityExcess, ROUND(SUM(QuantityShortage), " + (int)GlobalEnums.rndQuantity + ") AS QuantityShortage, ROUND(SUM(Swarfs), " + (int)GlobalEnums.rndQuantity + ") AS Swarfs, 0 AS QuantityReceipted, IIF(MIN(PiecePerPack) <> 0, CAST(ROUND(SUM(Quantity + QuantityExcess), " + (int)GlobalEnums.rndQuantity + ") AS int) / MIN(PiecePerPack), 0) AS Packages, IIF(MIN(PiecePerPack) <> 0, ROUND(SUM(Quantity + QuantityExcess), " + (int)GlobalEnums.rndQuantity + ") % MIN(PiecePerPack), 0) AS OddPackages, MAX(Remarks) AS Remarks, 1 AS Approved, 0 AS HandoverApproved, " + "\r\n";

            //queryString = queryString + "                                       STUFF   ((SELECT ', ' + SemifinishedItems.Reference " + "\r\n";
            //queryString = queryString + "                                       FROM    FinishedItemDetails INNER JOIN SemifinishedItems ON FinishedItemDetails.SemifinishedItemID = SemifinishedItems.SemifinishedItemID " + "\r\n";
            //queryString = queryString + "                                       WHERE   FinishedItemDetails.FinishedItemID = @EntityID AND FinishedItemDetails.CommodityID = FinishedItemDetailResults.CommodityID " + "\r\n";
            //queryString = queryString + "                                       FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)'),1,1,'') AS SemifinishedItemReferences " + "\r\n";

            //queryString = queryString + "                       FROM            FinishedItemDetails  FinishedItemDetailResults " + "\r\n";
            //queryString = queryString + "                       WHERE           FinishedItemID = @EntityID " + "\r\n";
            //queryString = queryString + "                       GROUP BY        CommodityID; " + "\r\n";

            //queryString = queryString + "                       UPDATE          FinishedItemDetails " + "\r\n";
            //queryString = queryString + "                       SET             FinishedItemDetails.FinishedItemPackageID = FinishedItemPackages.FinishedItemPackageID " + "\r\n";
            //queryString = queryString + "                       FROM            FinishedItemDetails INNER JOIN FinishedItemPackages ON FinishedItemDetails.FinishedItemID = @EntityID AND FinishedItemDetails.FinishedItemID = FinishedItemPackages.FinishedItemID AND FinishedItemDetails.CommodityID = FinishedItemPackages.CommodityID; " + "\r\n";
            //queryString = queryString + "                   END " + "\r\n";

            //queryString = queryString + "               ELSE " + "\r\n";
            //queryString = queryString + "                   BEGIN " + "\r\n";
            //queryString = queryString + "                       UPDATE          FinishedItemDetails SET FinishedItemPackageID = NULL WHERE FinishedItemID = @EntityID ; " + "\r\n";
            //queryString = queryString + "                       DELETE FROM     FinishedItemPackages WHERE FinishedItemID = @EntityID ; " + "\r\n";
            //queryString = queryString + "                   END " + "\r\n";
            //#endregion INIT FinishedItemPackages


            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, N'hủy', '')  + N' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("FinishedItemToggleApproved", queryString);
        }

        private void FinishedItemInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("FinishedItems", "FinishedItemID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.FinishedItem));
            this.totalSmartPortalEntities.CreateTrigger("FinishedItemInitReference", simpleInitReference.CreateQuery());
        }


        private void FinishedItemSheet()
        {
            string queryString;

            queryString = " @WorkshiftID int, @FinishedItemID int, @FromDate DateTime, @ToDate DateTime " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";
            queryString = queryString + "       SET NOCOUNT ON" + "\r\n";

            queryString = queryString + "       DECLARE     @LocalWorkshiftID int, @LocalFinishedItemID int, @LocalFromDate DateTime, @LocalToDate DateTime      " + "\r\n";
            queryString = queryString + "       SET         @LocalWorkshiftID = @WorkshiftID    SET @LocalFinishedItemID = @FinishedItemID        SET @LocalFromDate = @FromDate      SET @LocalToDate = @ToDate " + "\r\n";

            queryString = queryString + "       IF  (@LocalWorkshiftID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.FinishedItemSheetSQL(true, false) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               IF  (@LocalFinishedItemID <> 0) " + "\r\n";
            queryString = queryString + "                   " + this.FinishedItemSheetSQL(false, true) + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n";
            queryString = queryString + "                   " + this.FinishedItemSheetSQL(false, false) + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            queryString = queryString + "       SET NOCOUNT OFF" + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("FinishedItemSheet", queryString);
        }

        private string FinishedItemSheetSQL(bool workshiftID, bool finishedItemID)
        {
            string queryString = " " + "\r\n";
            queryString = queryString + "       SELECT      FinishedItems.FinishedItemID, FinishedItemPackages.FinishedItemPackageID, FinishedItems.EntryDate, FinishedItems.Reference, Workshifts.EntryDate AS WorkshiftEntryDate, Workshifts.Code AS WorkshiftCode, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, FirmOrders.EntryDate AS FirmOrderEntryDate, FirmOrders.DeliveryDate, Customers.Name AS CustomerName, " + "\r\n";
            queryString = queryString + "                   Commodities.Code, Commodities.Name, CommodityClasses.Code AS CommodityClassCode, Molds.Code AS MoldCode, FirmOrderDetails.FirmOrderQuantity, FinishedItemSummaries.FirmOrderQuantityReceipted, FirmOrderDetails.FirmOrderQuantity - FinishedItemSummaries.FirmOrderQuantityReceipted AS FirmOrderQuantityRemains, FinishedItemSummaries.FinishedQuantityRemains + ISNULL(SemifinishedItemSummaries.SemifinishedQuantityRemains, 0) AS FinishedQuantityRemains, FinishedItemPackages.PiecePerPack, FinishedItemPackages.Quantity, FinishedItemPackages.Packages, FinishedItemPackages.OddPackages, FinishedItemPackages.QuantityFailure, FinishedItemPackages.Swarfs, CrucialWorkers.Name AS CrucialWorkerName, CrucialWorkers.LastName AS CrucialWorkerLastName, FinishedItems.Description " + "\r\n";

            queryString = queryString + "       FROM        FinishedItems " + "\r\n";
            queryString = queryString + "                   INNER JOIN FinishedItemPackages ON " + this.FinishedItemSheetOption(workshiftID, finishedItemID) + " AND FinishedItems.FinishedItemID = FinishedItemPackages.FinishedItemID " + "\r\n";
            queryString = queryString + "                   INNER JOIN FirmOrders ON FinishedItemPackages.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";

            queryString = queryString + "                   INNER JOIN (SELECT FirmOrderID, CommodityID, MIN(MoldID) AS MoldID, SUM(Quantity) AS FirmOrderQuantity FROM FirmOrderDetails WHERE FirmOrderID IN (SELECT DISTINCT FirmOrderID FROM FinishedItems WHERE " + this.FinishedItemSheetOption(workshiftID, finishedItemID) + ") GROUP BY FirmOrderID, CommodityID) FirmOrderDetails ON FinishedItemPackages.FirmOrderID = FirmOrderDetails.FirmOrderID AND FinishedItemPackages.CommodityID = FirmOrderDetails.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN (SELECT FirmOrderID, CommodityID, SUM(QuantityReceipted) AS FirmOrderQuantityReceipted, SUM(Quantity - QuantityReceipted) AS FinishedQuantityRemains FROM FinishedItemPackages WHERE FirmOrderID IN (SELECT DISTINCT FirmOrderID FROM FinishedItems WHERE " + this.FinishedItemSheetOption(workshiftID, finishedItemID) + ") GROUP BY FirmOrderID, CommodityID) FinishedItemSummaries ON FinishedItemPackages.FirmOrderID = FinishedItemSummaries.FirmOrderID AND FinishedItemPackages.CommodityID = FinishedItemSummaries.CommodityID " + "\r\n";

            queryString = queryString + "                   INNER JOIN Molds ON FirmOrderDetails.MoldID = Molds.MoldID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON FinishedItemPackages.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON FinishedItemPackages.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CommodityClasses ON Commodities.CommodityClassID = CommodityClasses.CommodityClassID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON FinishedItems.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Employees AS CrucialWorkers ON FinishedItems.CrucialWorkerID = CrucialWorkers.EmployeeID " + "\r\n";

            queryString = queryString + "                   LEFT JOIN (SELECT FirmOrderID, CommodityID, SUM(Quantity - QuantityFinished) AS SemifinishedQuantityRemains FROM SemifinishedItemDetails WHERE FirmOrderID IN (SELECT DISTINCT FirmOrderID FROM FinishedItems WHERE " + this.FinishedItemSheetOption(workshiftID, finishedItemID) + ") AND ROUND(Quantity - QuantityFinished, " + (int)GlobalEnums.rndQuantity + ") > 0 GROUP BY FirmOrderID, CommodityID) SemifinishedItemSummaries ON FinishedItemPackages.FirmOrderID = SemifinishedItemSummaries.FirmOrderID AND FinishedItemPackages.CommodityID = SemifinishedItemSummaries.CommodityID " + "\r\n";

            queryString = queryString + "       ORDER BY    FirmOrders.Code, Commodities.Name " + "\r\n";

            return queryString;
        }

        private string FinishedItemSheetOption(bool workshiftID, bool finishedItemID) { return (workshiftID ? "FinishedItems.WorkshiftID = @LocalWorkshiftID" : (finishedItemID ? "FinishedItems.FinishedItemID = @LocalFinishedItemID" : "FinishedItems.EntryDate >= @LocalFromDate AND FinishedItems.EntryDate <= @LocalToDate")); }

        #endregion
    }
}
