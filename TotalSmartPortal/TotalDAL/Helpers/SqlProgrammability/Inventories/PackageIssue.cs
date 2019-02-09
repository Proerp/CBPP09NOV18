using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Inventories
{
    public class PackageIssue
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public PackageIssue(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetPackageIssueIndexes();

            this.GetPackageIssueViewDetails();

            this.GetPackageIssuePendingBlendingInstructions();
            this.GetPackageIssuePendingBlendingInstructionDetails();

            this.PackageIssueSaveRelative();
            this.PackageIssuePostSaveValidate();

            this.PackageIssueApproved();
            this.PackageIssueEditable();

            this.PackageIssueToggleApproved();

            this.PackageIssueInitReference();

            this.PackageIssueSheet();
        }


        private void GetPackageIssueIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      PackageIssues.PackageIssueID, CAST(PackageIssues.EntryDate AS DATE) AS EntryDate, PackageIssues.Reference, Locations.Code AS LocationCode, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Workshifts.Name AS WorkshiftName, Users.FirstName AS UserFirstName, Users.LastName AS UserLastName, ISNULL(BlendingInstructions.Description, '') + ' ' + ISNULL(PackageIssues.Description, '') AS Description, PackageIssues.Caption, PackageIssues.TotalQuantity, PackageIssues.Approved, " + "\r\n";
            queryString = queryString + "                   Workshifts.EntryDate AS WorkshiftEntryDate, PackageIssues.BlendingInstructionID, BlendingInstructions.Reference AS BlendingInstructionsReference, BlendingInstructions.Code AS BlendingInstructionsCode, BlendingInstructions.EntryDate AS BlendingInstructionEntryDate " + "\r\n";
            queryString = queryString + "       FROM        PackageIssues " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON PackageIssues.EntryDate >= @FromDate AND PackageIssues.EntryDate <= @ToDate AND PackageIssues.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.PackageIssue + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = PackageIssues.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON PackageIssues.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON PackageIssues.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Users ON PackageIssues.UserID = Users.UserID " + "\r\n";
            queryString = queryString + "                   INNER JOIN BlendingInstructions ON PackageIssues.BlendingInstructionID = BlendingInstructions.BlendingInstructionID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetPackageIssueIndexes", queryString);
        }


        private void GetPackageIssueViewDetails()
        {
            string queryString;

            queryString = " @PackageIssueID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      PackageIssueDetails.PackageIssueDetailID, PackageIssueDetails.PackageIssueID, BlendingInstructionDetails.BlendingInstructionDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails.GoodsReceiptID, GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceiptDetails.Reference AS GoodsReceiptReference, GoodsReceiptDetails.Code AS GoodsReceiptCode, GoodsReceiptDetails.EntryDate AS GoodsReceiptEntryDate, GoodsReceiptDetails.ExpiryDate, GoodsReceiptDetails.BatchID, GoodsReceiptDetails.BatchEntryDate, PackageIssueDetails.BinLocationID, BinLocations.Code AS BinLocationCode, PackageIssueDetails.Barcode, PackageIssueDetails.BatchCode, PackageIssueDetails.SealCode, PackageIssueDetails.LabCode, " + "\r\n";
            queryString = queryString + "                   BlendingInstructionDetails.Quantity AS QuantityBIS, ROUND(BlendingInstructionDetails.Quantity - BlendingInstructionDetails.QuantityIssued + Issued_BlendingInstructionDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued + Issued_GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityAvailables, PackageIssueDetails.Quantity, PackageIssueDetails.Remarks " + "\r\n";

            queryString = queryString + "       FROM        PackageIssueDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN BlendingInstructionDetails ON PackageIssueDetails.PackageIssueID = @PackageIssueID AND PackageIssueDetails.BlendingInstructionDetailID = BlendingInstructionDetails.BlendingInstructionDetailID " + "\r\n";
            queryString = queryString + "                   INNER JOIN (SELECT BlendingInstructionDetailID, SUM(Quantity) AS QuantityIssued FROM PackageIssueDetails WHERE PackageIssueID = @PackageIssueID GROUP BY BlendingInstructionDetailID) AS Issued_BlendingInstructionDetails ON PackageIssueDetails.BlendingInstructionDetailID = Issued_BlendingInstructionDetails.BlendingInstructionDetailID " + "\r\n";

            queryString = queryString + "                   INNER JOIN Commodities ON PackageIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN BinLocations ON PackageIssueDetails.BinLocationID = BinLocations.BinLocationID " + "\r\n";

            queryString = queryString + "                   INNER JOIN GoodsReceiptDetails ON PackageIssueDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";
            queryString = queryString + "                   INNER JOIN (SELECT GoodsReceiptDetailID, SUM(Quantity) AS QuantityIssued FROM PackageIssueDetails WHERE PackageIssueID = @PackageIssueID GROUP BY GoodsReceiptDetailID) AS Issued_GoodsReceiptDetails ON PackageIssueDetails.GoodsReceiptDetailID = Issued_GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";

            queryString = queryString + "       ORDER BY    PackageIssueDetails.PackageIssueID, PackageIssueDetails.PackageIssueDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetPackageIssueViewDetails", queryString);
        }





        private void GetPackageIssuePendingBlendingInstructions()
        {
            string queryString = " @LocationID int, @BlendingInstructionID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          BlendingInstructions.BlendingInstructionID, BlendingInstructions.Code AS BlendingInstructionCode, BlendingInstructions.Reference AS BlendingInstructionReference, BlendingInstructions.EntryDate AS BlendingInstructionEntryDate, BlendingInstructions.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, BlendingInstructions.Description, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, Warehouses.Name AS WarehouseName, BlendingInstructions.TotalQuantity, BlendingInstructionRemains.TotalQuantityRemains " + "\r\n";

            queryString = queryString + "       FROM           (SELECT BlendingInstructionID, ROUND(SUM(Quantity - QuantityIssued), " + (int)GlobalEnums.rndQuantity + ") AS TotalQuantityRemains FROM BlendingInstructionDetails WHERE Approved = 1 AND InActive = 0 AND InActivePartial = 0 AND (@BlendingInstructionID IS NULL OR BlendingInstructionID = @BlendingInstructionID) AND ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 GROUP BY BlendingInstructionID) AS BlendingInstructionRemains " + "\r\n";
            queryString = queryString + "                       INNER JOIN BlendingInstructions ON BlendingInstructionRemains.BlendingInstructionID = BlendingInstructions.BlendingInstructionID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities ON BlendingInstructions.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "                       INNER JOIN Warehouses ON Warehouses.WarehouseID = 6 " + "\r\n";


            this.totalSmartPortalEntities.CreateStoredProcedure("GetPackageIssuePendingBlendingInstructions", queryString);
        }

        private void GetPackageIssuePendingBlendingInstructionDetails()
        {
            string queryString;

            queryString = " @WebAPI bit, @LocationID Int, @PackageIssueID Int, @BlendingInstructionID Int, @WarehouseID Int, @Barcode nvarchar(60), @GoodsReceiptDetailIDs varchar(3999) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (NOT @Barcode IS NULL AND @Barcode <> '') " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPendingDetails(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPendingDetails(false) + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetPackageIssuePendingBlendingInstructionDetails", queryString);
        }

        private string BuildSQLPendingDetails(bool isBarcode)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF  (@GoodsReceiptDetailIDs <> '' AND @GoodsReceiptDetailIDs <> '0') " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPendingDetails(isBarcode, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPendingDetails(isBarcode, false) + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLPendingDetails(bool isBarcode, bool isGoodsReceiptDetailIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@PackageIssueID <= 0 OR @WebAPI = 1) " + "\r\n"; //@WebAPI = 1: SHOULD SAVE BEFORE CALL NEXT
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLNew(isBarcode, isGoodsReceiptDetailIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY BlendingInstructionDetails.BlendingInstructionDetailID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLNew(isBarcode, isGoodsReceiptDetailIDs) + " WHERE BlendingInstructionDetails.BlendingInstructionDetailID NOT IN (SELECT BlendingInstructionDetailID FROM PackageIssueDetails WHERE PackageIssueID = @PackageIssueID) " + "\r\n";
            queryString = queryString + "                   UNION ALL " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLEdit(isBarcode, isGoodsReceiptDetailIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY BlendingInstructionDetails.BlendingInstructionDetailID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLNew(bool isBarcode, bool isGoodsReceiptDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      BlendingInstructionDetails.BlendingInstructionDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.OfficialCode, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails.GoodsReceiptID, GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceipts.Reference AS GoodsReceiptReference, GoodsReceipts.Code AS GoodsReceiptCode, GoodsReceipts.EntryDate AS GoodsReceiptEntryDate, GoodsReceiptDetails.ExpiryDate, GoodsReceiptDetails.BatchID, GoodsReceiptDetails.BatchEntryDate, GoodsReceiptDetails.SealCode, GoodsReceiptDetails.BatchCode, GoodsReceiptDetails.LabCode, GoodsReceiptDetails.Barcode, GoodsReceiptDetails.BinLocationID, BinLocations.Code AS BinLocationCode, " + "\r\n";
            queryString = queryString + "                   BlendingInstructionDetails.Quantity AS QuantityBIS, ROUND(BlendingInstructionDetails.Quantity - BlendingInstructionDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityAvailables, 0 AS Quantity, CAST(0 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        BlendingInstructionDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON BlendingInstructionDetails.BlendingInstructionID = @BlendingInstructionID AND BlendingInstructionDetails.Approved = 1 AND BlendingInstructionDetails.InActive = 0 AND BlendingInstructionDetails.InActivePartial = 0 AND ROUND(BlendingInstructionDetails.Quantity - BlendingInstructionDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 AND BlendingInstructionDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "                   LEFT JOIN GoodsReceiptDetails ON GoodsReceiptDetails.WarehouseID = @WarehouseID AND BlendingInstructionDetails.CommodityID = GoodsReceiptDetails.CommodityID AND GoodsReceiptDetails.Approved = 1 AND ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 " + (isBarcode ? " AND GoodsReceiptDetails.Barcode = @Barcode" : "") + (isGoodsReceiptDetailIDs ? " AND GoodsReceiptDetails.GoodsReceiptDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@GoodsReceiptDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   LEFT JOIN GoodsReceipts ON GoodsReceiptDetails.GoodsReceiptID = GoodsReceipts.GoodsReceiptID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN BinLocations ON GoodsReceiptDetails.BinLocationID = BinLocations.BinLocationID " + "\r\n";

            return queryString;
        }

        private string BuildSQLEdit(bool isBarcode, bool isGoodsReceiptDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      BlendingInstructionDetails.BlendingInstructionDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.OfficialCode, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails.GoodsReceiptID, GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceipts.Reference AS GoodsReceiptReference, GoodsReceipts.Code AS GoodsReceiptCode, GoodsReceipts.EntryDate AS GoodsReceiptEntryDate, GoodsReceiptDetails.ExpiryDate, GoodsReceiptDetails.BatchID, GoodsReceiptDetails.BatchEntryDate, GoodsReceiptDetails.SealCode, GoodsReceiptDetails.BatchCode, GoodsReceiptDetails.LabCode, GoodsReceiptDetails.Barcode, GoodsReceiptDetails.BinLocationID, BinLocations.Code AS BinLocationCode, " + "\r\n";
            queryString = queryString + "                   BlendingInstructionDetails.Quantity AS QuantityBIS, ROUND(BlendingInstructionDetails.Quantity - BlendingInstructionDetails.QuantityIssued + PackageIssueDetails.Quantity, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued + ISNULL(IssuedGoodsReceiptDetails.Quantity, 0), " + (int)GlobalEnums.rndQuantity + ") AS QuantityAvailables, 0 AS Quantity, CAST(0 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        BlendingInstructionDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN (SELECT BlendingInstructionDetailID, SUM(Quantity) AS Quantity FROM PackageIssueDetails WHERE PackageIssueID = @PackageIssueID GROUP BY BlendingInstructionDetailID) AS PackageIssueDetails ON BlendingInstructionDetails.BlendingInstructionDetailID = PackageIssueDetails.BlendingInstructionDetailID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON BlendingInstructionDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "                   LEFT JOIN GoodsReceiptDetails ON GoodsReceiptDetails.WarehouseID = @WarehouseID AND BlendingInstructionDetails.CommodityID = GoodsReceiptDetails.CommodityID AND GoodsReceiptDetails.Approved = 1 AND (ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 OR GoodsReceiptDetails.GoodsReceiptDetailID IN (SELECT GoodsReceiptDetailID FROM PackageIssueDetails WHERE PackageIssueID = @PackageIssueID)) " + (isBarcode ? " AND GoodsReceiptDetails.Barcode = @Barcode" : "") + (isGoodsReceiptDetailIDs ? " AND GoodsReceiptDetails.GoodsReceiptDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@GoodsReceiptDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   LEFT JOIN (SELECT GoodsReceiptDetailID, SUM(Quantity) AS Quantity FROM PackageIssueDetails WHERE PackageIssueID = @PackageIssueID GROUP BY GoodsReceiptDetailID) AS IssuedGoodsReceiptDetails ON GoodsReceiptDetails.GoodsReceiptDetailID = IssuedGoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN GoodsReceipts ON GoodsReceiptDetails.GoodsReceiptID = GoodsReceipts.GoodsReceiptID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN BinLocations ON GoodsReceiptDetails.BinLocationID = BinLocations.BinLocationID " + "\r\n";

            return queryString;
        }


        private void PackageIssueSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       BEGIN  " + "\r\n";

            queryString = queryString + "           DECLARE @msg NVARCHAR(300) ";

            queryString = queryString + "           IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";

            #region UPDATE WorkshiftID
            queryString = queryString + "                   DECLARE         @EntryDate Datetime, @ShiftID int, @WorkshiftID int " + "\r\n";
            queryString = queryString + "                   SELECT          @EntryDate = CONVERT(date, EntryDate), @ShiftID = ShiftID FROM PackageIssues WHERE PackageIssueID = @EntityID " + "\r\n";
            queryString = queryString + "                   SET             @WorkshiftID = (SELECT TOP 1 WorkshiftID FROM Workshifts WHERE EntryDate = @EntryDate AND ShiftID = @ShiftID) " + "\r\n";

            queryString = queryString + "                   IF             (@WorkshiftID IS NULL) " + "\r\n";
            queryString = queryString + "                       BEGIN ";
            queryString = queryString + "                           INSERT INTO     Workshifts(EntryDate, ShiftID, Code, Name, WorkingHours, Remarks) SELECT @EntryDate, ShiftID, Code, Name, WorkingHours, Remarks FROM Shifts WHERE ShiftID = @ShiftID " + "\r\n";
            queryString = queryString + "                           SELECT          @WorkshiftID = SCOPE_IDENTITY(); " + "\r\n";
            queryString = queryString + "                       END ";

            queryString = queryString + "                   UPDATE          PackageIssues        SET WorkshiftID = @WorkshiftID WHERE PackageIssueID = @EntityID " + "\r\n";
            queryString = queryString + "                   UPDATE          PackageIssueDetails  SET WorkshiftID = @WorkshiftID WHERE PackageIssueID = @EntityID " + "\r\n";
            #endregion UPDATE WorkshiftID

            queryString = queryString + "               END " + "\r\n";


            queryString = queryString + "           DECLARE         @PackageIssueDetails TABLE (GoodsReceiptDetailID int NOT NULL PRIMARY KEY, Quantity decimal(18, 2) NOT NULL)" + "\r\n";
            queryString = queryString + "           INSERT INTO     @PackageIssueDetails (GoodsReceiptDetailID, Quantity) SELECT GoodsReceiptDetailID, SUM(Quantity) AS Quantity FROM PackageIssueDetails WHERE PackageIssueID = @EntityID GROUP BY GoodsReceiptDetailID " + "\r\n";

            queryString = queryString + "           DECLARE         @AffectedROWCOUNT int ";

            #region UPDATE GoodsReceiptDetails
            queryString = queryString + "           UPDATE          GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "           SET             GoodsReceiptDetails.QuantityIssued = ROUND(GoodsReceiptDetails.QuantityIssued + PackageIssueDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "           FROM            GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                           INNER JOIN @PackageIssueDetails PackageIssueDetails ON GoodsReceiptDetails.GoodsReceiptDetailID = PackageIssueDetails.GoodsReceiptDetailID AND GoodsReceiptDetails.Approved = 1 " + "\r\n";

            queryString = queryString + "           IF @@ROWCOUNT <> (SELECT COUNT(*) FROM @PackageIssueDetails) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   SET         @msg = N'Phiếu nhập kho đã hủy, chưa duyệt hoặc đã xóa.' ; " + "\r\n";
            queryString = queryString + "                   THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            #endregion


            #region UPDATE BlendingInstructions
            queryString = queryString + "           DECLARE         @IssueBlendingInstructionDetails TABLE (BlendingInstructionDetailID int NOT NULL PRIMARY KEY, Quantity decimal(18, 2) NOT NULL)" + "\r\n";
            queryString = queryString + "           INSERT INTO     @IssueBlendingInstructionDetails (BlendingInstructionDetailID, Quantity) SELECT BlendingInstructionDetailID, SUM(Quantity) AS Quantity FROM PackageIssueDetails WHERE PackageIssueID = @EntityID GROUP BY BlendingInstructionDetailID " + "\r\n";

            queryString = queryString + "           UPDATE          BlendingInstructionDetails " + "\r\n";
            queryString = queryString + "           SET             BlendingInstructionDetails.QuantityIssued = ROUND(BlendingInstructionDetails.QuantityIssued + IssueBlendingInstructionDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "           FROM            BlendingInstructionDetails " + "\r\n";
            queryString = queryString + "                           INNER JOIN @IssueBlendingInstructionDetails IssueBlendingInstructionDetails ON ((BlendingInstructionDetails.Approved = 1 AND BlendingInstructionDetails.InActive = 0 AND BlendingInstructionDetails.InActivePartial = 0) OR @SaveRelativeOption = -1) AND BlendingInstructionDetails.BlendingInstructionDetailID = IssueBlendingInstructionDetails.BlendingInstructionDetailID " + "\r\n";

            queryString = queryString + "           IF @@ROWCOUNT <> (SELECT COUNT(*) FROM @IssueBlendingInstructionDetails) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   SET         @msg = N'BIS đã hủy, chưa duyệt hoặc đã xóa.' ; " + "\r\n";
            queryString = queryString + "                   THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            #endregion


            queryString = queryString + "       END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("PackageIssueSaveRelative", queryString);
        }

        private void PackageIssuePostSaveValidate()
        {
            string[] queryArray = new string[3];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày nhập kho: ' + CAST(GoodsReceipts.EntryDate AS nvarchar) FROM PackageIssueDetails INNER JOIN GoodsReceipts ON PackageIssueDetails.PackageIssueID = @EntityID AND PackageIssueDetails.GoodsReceiptID = GoodsReceipts.GoodsReceiptID AND PackageIssueDetails.EntryDate < GoodsReceipts.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Số lượng xuất vượt quá số lượng tồn kho: ' + CAST(ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM GoodsReceiptDetails WHERE (ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") < 0) ";
            queryArray[2] = " SELECT TOP 1 @FoundEntity = N'Số lượng xuất vượt quá số lượng yêu cầu: ' + CAST(ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM BlendingInstructionDetails WHERE (ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("PackageIssuePostSaveValidate", queryArray);
        }




        private void PackageIssueApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = PackageIssueID FROM PackageIssues WHERE PackageIssueID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("PackageIssueApproved", queryArray);
        }


        private void PackageIssueEditable()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = PackageIssueID FROM GoodsReceiptDetails WHERE PackageIssueID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("PackageIssueEditable", queryArray);
        }

        private void PackageIssueToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      PackageIssues  SET Approved = @Approved, ApprovedDate = GetDate() WHERE PackageIssueID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          PackageIssueDetails  SET Approved = @Approved WHERE PackageIssueID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, N'hủy', '')  + N' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("PackageIssueToggleApproved", queryString);
        }


        private void PackageIssueInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("PackageIssues", "PackageIssueID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.PackageIssue));
            this.totalSmartPortalEntities.CreateTrigger("PackageIssueInitReference", simpleInitReference.CreateQuery());

            string queryString = " @PackageIssueID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT TOP 1 Reference FROM PackageIssues WHERE PackageIssueID = @PackageIssueID " + "\r\n";
            this.totalSmartPortalEntities.CreateStoredProcedure("PackageIssueGetReference", queryString);
        }

        private void PackageIssueSheet()
        {
            string queryString = " @PackageIssueID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @LocalPackageIssueID int    SET @LocalPackageIssueID = @PackageIssueID" + "\r\n";

            queryString = queryString + "       SELECT          PackageIssues.PackageIssueID, PackageIssues.EntryDate AS PackageIssueEntryDate, PackageIssues.Reference, Workshifts.Code AS WorkshiftCode, ProductionLines.Code AS ProductionLineCode, " + "\r\n";
            queryString = queryString + "                       BlendingInstructions.EntryDate AS BlendingInstructionEntryDate, BlendingInstructions.Reference AS BlendingInstructionReference, BlendingInstructions.Code AS BlendingInstructionCode, BlendingInstructions.Specs, BlendingInstructions.Specification, BlendingInstructions.TotalQuantity AS BlendingInstructionTotalQuantity, Customers.Name AS CustomerName, " + "\r\n";
            queryString = queryString + "                       Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, PackageIssueDetails.BatchEntryDate, PackageIssueDetails.Quantity, ISNULL(BlendingInstructions.Description, '') + ' ' + ISNULL(PackageIssues.Description, '') AS Description " + "\r\n";

            queryString = queryString + "       FROM            PackageIssues " + "\r\n";
            queryString = queryString + "                       INNER JOIN PackageIssueDetails ON PackageIssues.PackageIssueID = @LocalPackageIssueID AND PackageIssues.PackageIssueID = PackageIssueDetails.PackageIssueID " + "\r\n";
            queryString = queryString + "                       INNER JOIN BlendingInstructions ON PackageIssues.BlendingInstructionID = BlendingInstructions.BlendingInstructionID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON PackageIssues.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Workshifts ON PackageIssues.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                       INNER JOIN ProductionLines ON PackageIssues.ProductionLineID = ProductionLines.ProductionLineID" + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities ON PackageIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "       ORDER BY        PackageIssueDetails.PackageIssueDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            //this.totalSmartPortalEntities.CreateStoredProcedure("PackageIssueSheet", queryString);
        }

    }
}

