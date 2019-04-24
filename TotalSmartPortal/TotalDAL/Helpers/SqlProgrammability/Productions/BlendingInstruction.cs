using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Productions
{
    public class BlendingInstruction
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public BlendingInstruction(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetBlendingInstructionIndexes();

            this.GetBlendingInstructionRunnings();

            this.GetBlendingInstructionViewDetails();
            this.BlendingInstructionSaveRelative();
            this.BlendingInstructionPostSaveValidate();

            this.BlendingInstructionApproved();
            this.BlendingInstructionEditable();
            this.BlendingInstructionVoidable();

            this.BlendingInstructionToggleApproved();
            this.BlendingInstructionToggleVoid();
            this.BlendingInstructionToggleVoidDetail();

            this.BlendingInstructionInitReference();

            this.GetBlendingInstructionLogs();
            this.BlendingInstructionSheet();
        }


        private void GetBlendingInstructionIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime, @LabOptionID int, @FilterOptionID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalAspUserID nvarchar(128), @LocalFromDate DateTime, @LocalToDate DateTime, @LocalLabOptionID int, @LocalFilterOptionID int " + "\r\n";
            queryString = queryString + "       SET         @LocalAspUserID = @AspUserID       SET @LocalFromDate = @FromDate      SET @LocalToDate = @ToDate          SET @LocalLabOptionID = @LabOptionID            SET @LocalFilterOptionID = @FilterOptionID" + "\r\n";

            queryString = queryString + "       DECLARE     @BlendingInstructionIndexes TABLE ( BlendingInstructionID int NOT NULL, EntryDate datetime NOT NULL, Reference nvarchar(10) NOT NULL, Code nvarchar(50) NULL, VoucherDate datetime NULL, ProductCode nvarchar(50) NULL, ProductName nvarchar(200) NULL, Description nvarchar(100) NULL, Jobs nvarchar(100) NULL, " + "\r\n";
            queryString = queryString + "                                                       BlendingInstructionDetailID int NULL, CommodityCode nvarchar(50) NULL, CommodityName nvarchar(200) NULL, Approved bit NOT NULL, InActive bit NOT NULL, InActivePartial bit NOT NULL, VoidTypeName nvarchar(50) NULL, " + "\r\n";
            queryString = queryString + "                                                       Quantity decimal(18, 2) NULL, QuantityIssued decimal(18, 2) NULL, QuantityRemains decimal(18, 2) NULL, QuantityAvailableArrivals decimal(18, 2) NULL, QuantityAvailableLocation1 decimal(18, 2) NULL, QuantityAvailableLocation2 decimal(18, 2) NULL) " + "\r\n";

            queryString = queryString + "       IF  (@LocalFilterOptionID = 0) " + "\r\n";
            queryString = queryString + "           " + this.GetBlendingInstructionIndexSQL(0) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               IF  (@LocalFilterOptionID = 10) " + "\r\n";
            queryString = queryString + "                   " + this.GetBlendingInstructionIndexSQL(10) + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       IF  (@LocalFilterOptionID = 11) " + "\r\n";
            queryString = queryString + "                           " + this.GetBlendingInstructionIndexSQL(11) + "\r\n";
            queryString = queryString + "                       ELSE " + "\r\n";
            queryString = queryString + "                           BEGIN " + "\r\n";
            queryString = queryString + "                               IF  (@LocalFilterOptionID = 12) " + "\r\n";
            queryString = queryString + "                                   " + this.GetBlendingInstructionIndexSQL(12) + "\r\n";
            queryString = queryString + "                               ELSE " + "\r\n";
            queryString = queryString + "                                   " + this.GetBlendingInstructionIndexSQL(20) + "\r\n";
            queryString = queryString + "                           END " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            queryString = queryString + "       SELECT      BlendingInstructionID, EntryDate, Reference, Code, VoucherDate, ProductCode, ProductName, Description, Jobs, " + "\r\n";
            queryString = queryString + "                   BlendingInstructionDetailID, CommodityCode, CommodityName, Approved, InActive, InActivePartial, VoidTypeName, " + "\r\n";
            queryString = queryString + "                   Quantity, IIF(QuantityIssued = 0, NULL, QuantityIssued) AS QuantityIssued, IIF(QuantityRemains = 0, NULL, QuantityRemains) AS QuantityRemains, IIF(QuantityAvailableArrivals = 0, NULL, QuantityAvailableArrivals) AS QuantityAvailableArrivals, IIF(QuantityAvailableLocation1 = 0, NULL, QuantityAvailableLocation1) AS QuantityAvailableLocation1, IIF(QuantityAvailableLocation2 = 0, NULL, QuantityAvailableLocation2) AS QuantityAvailableLocation2 " + "\r\n";

            queryString = queryString + "       FROM        @BlendingInstructionIndexes " + "\r\n"; //NOTE: QuantityProduced: QuantitySemifinishedRemains + QuantityAndExcess
            queryString = queryString + "       ORDER BY    EntryDate DESC, CommodityCode " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetBlendingInstructionIndexes", queryString);
        }

        private string GetBlendingInstructionIndexSQL(int filterOptionID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@LocalLabOptionID = 1) " + "\r\n";
            queryString = queryString + "           " + this.GetBlendingInstructionIndexSQL(filterOptionID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetBlendingInstructionIndexSQL(filterOptionID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetBlendingInstructionIndexSQL(int filterOptionID, bool labOptionID)
        {

            //filterOptionID: 0: NORMAL  BlendingInstructionDetails LEFT JOIN BlendingInstructionDetails: FROM TO
            //filterOptionID: 10: PENDING BlendingInstructionDetails LEFT JOIN BlendingInstructionDetails WITH: NOT InActive AND (BlendingInstructionDetails IS NULL (NOT APPROVED YET) OR BlendingInstructionDetails.QuantityPending > 0))
            //filterOptionID: 11: 10 AND NOT MATERIAL
            //filterOptionID: 12: 10 AND WITH MATERIAL
            //filterOptionID: 20: FINISH  BlendingInstructionDetails INNER JOIN BlendingInstructionDetails: FROM TO

            string queryString = "";

            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       INSERT INTO @BlendingInstructionIndexes (BlendingInstructionID, EntryDate, Reference, Code, VoucherDate, ProductCode, ProductName, Description, Jobs, " + "\r\n";
            queryString = queryString + "                                                BlendingInstructionDetailID, CommodityCode, CommodityName, Approved, InActive, InActivePartial, VoidTypeName, " + "\r\n";
            queryString = queryString + "                                                Quantity, QuantityIssued, QuantityRemains, QuantityAvailableArrivals, QuantityAvailableLocation1, QuantityAvailableLocation2) " + "\r\n";

            queryString = queryString + "       SELECT      BlendingInstructions.BlendingInstructionID, CAST(" + "BlendingInstructions.EntryDate" + " AS DATE) AS EntryDate, BlendingInstructions.Reference, BlendingInstructions.Code, BlendingInstructions.VoucherDate, Products.Code AS ProductCode, Products.Name AS ProductName, BlendingInstructionDetails.Remarks, BlendingInstructions.Jobs, " + "\r\n";
            queryString = queryString + "                   BlendingInstructionDetails.BlendingInstructionDetailID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, BlendingInstructions.Approved, BlendingInstructions.InActive, BlendingInstructionDetails.InActivePartial, ISNULL(VoidTypes.Name, VoidTypeDetails.Name) AS VoidTypeName, " + "\r\n";
            queryString = queryString + "                   BlendingInstructionDetails.Quantity, BlendingInstructionDetails.QuantityIssued, ROUND(BlendingInstructionDetails.Quantity - BlendingInstructionDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, GoodsArrivalAvailables.QuantityAvailableArrivals, GoodsReceiptAvailables.QuantityAvailableLocation1, GoodsReceiptAvailables.QuantityAvailableLocation2 " + "\r\n";

            queryString = queryString + "       FROM        BlendingInstructions " + "\r\n";
            queryString = queryString + "                   INNER JOIN  Commodities Products ON " + (filterOptionID == 0 || filterOptionID == 20 ? "BlendingInstructions.EntryDate" + " >= @LocalFromDate AND " + "BlendingInstructions.EntryDate" + " <= @LocalToDate AND" : "") + " BlendingInstructions.OrganizationalUnitID IN (SELECT DISTINCT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @LocalAspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.BlendingInstruction + " AND AccessControls.AccessLevel > 0) AND BlendingInstructions.CommodityID = Products.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN  BlendingInstructionDetails ON " + this.SQLPendingVsFinished(filterOptionID) + (filterOptionID == 11 || filterOptionID == 12 ? " BlendingInstructions.BlendingInstructionID " + (filterOptionID == 11 ? "NOT IN" : "IN") + "(SELECT BlendingInstructionID FROM BlendingInstructionDetails WHERE QuantityIssued <> 0)" + " AND " : "") + " BlendingInstructions.BlendingInstructionID = BlendingInstructionDetails.BlendingInstructionID " + "\r\n";
            queryString = queryString + "                   INNER JOIN  Commodities ON BlendingInstructionDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "                   LEFT JOIN   VoidTypes ON BlendingInstructions.VoidTypeID = VoidTypes.VoidTypeID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN   VoidTypes VoidTypeDetails ON BlendingInstructionDetails.VoidTypeID = VoidTypeDetails.VoidTypeID " + "\r\n";

            queryString = queryString + "                   LEFT JOIN  (SELECT CommodityID, ROUND(SUM(Quantity - QuantityReceipted), " + (int)GlobalEnums.rndQuantity + ") AS QuantityAvailableArrivals FROM GoodsArrivalPackages WHERE ROUND(Quantity - QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") > 0 " + (labOptionID ? " AND LabID IN (SELECT LabID FROM Labs WHERE Approved = 1 AND InActive = 0)" : "") + " GROUP BY CommodityID) AS GoodsArrivalAvailables ON BlendingInstructionDetails.CommodityID = GoodsArrivalAvailables.CommodityID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN  (SELECT GoodsReceiptDetails.CommodityID, ROUND(SUM(CASE WHEN Warehouses.LocationID = 1 THEN GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued ELSE 0 END), " + (int)GlobalEnums.rndQuantity + ") AS QuantityAvailableLocation1, ROUND(SUM(CASE WHEN Warehouses.LocationID = 2 THEN GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued ELSE 0 END), " + (int)GlobalEnums.rndQuantity + ") AS QuantityAvailableLocation2 FROM GoodsReceiptDetails INNER JOIN Warehouses ON GoodsReceiptDetails.WarehouseID = Warehouses.WarehouseID WHERE ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 " + (labOptionID ? " AND GoodsReceiptDetails.LabID IN (SELECT LabID FROM Labs WHERE Approved = 1 AND InActive = 0)" : "") + " GROUP BY GoodsReceiptDetails.CommodityID) AS GoodsReceiptAvailables ON BlendingInstructionDetails.CommodityID = GoodsReceiptAvailables.CommodityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            return queryString;
        }

        private string SQLPendingVsFinished(int filterOptionID)
        {
            bool pendingVsFinished = filterOptionID == 10 || filterOptionID == 11 || filterOptionID == 12;
            return filterOptionID == 0 ? "" : ("(BlendingInstructionDetails.InActive " + (pendingVsFinished ? "=" : "<>") + " 0 " + (pendingVsFinished ? "AND" : "OR") + " BlendingInstructionDetails.InActivePartial " + (pendingVsFinished ? "=" : "<>") + " 0 " + (pendingVsFinished ? "AND" : "OR") + " ROUND(BlendingInstructionDetails.Quantity - BlendingInstructionDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") " + (pendingVsFinished ? ">" : "<=") + " 0) AND ");
        }


        #region X


        private void GetBlendingInstructionViewDetails()
        {
            string queryString;

            queryString = " @BlendingInstructionID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      BlendingInstructionDetails.BlendingInstructionDetailID, BlendingInstructionDetails.BlendingInstructionID, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, BlendingInstructionDetails.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   VoidTypes.VoidTypeID, VoidTypes.Code AS VoidTypeCode, VoidTypes.Name AS VoidTypeName, VoidTypes.VoidClassID, " + "\r\n";
            queryString = queryString + "                   BlendingInstructionDetails.Quantity, BlendingInstructionDetails.QuantityIssued, BlendingInstructionDetails.InActivePartial, BlendingInstructionDetails.InActivePartialDate, BlendingInstructionDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        BlendingInstructionDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON BlendingInstructionDetails.BlendingInstructionID = @BlendingInstructionID AND BlendingInstructionDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN VoidTypes ON BlendingInstructionDetails.VoidTypeID = VoidTypes.VoidTypeID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetBlendingInstructionViewDetails", queryString);
        }

        private void GetBlendingInstructionRunnings()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          BlendingInstructions.BlendingInstructionID, BlendingInstructions.Code AS BlendingInstructionCode, BlendingInstructions.Reference AS BlendingInstructionReference, BlendingInstructions.EntryDate AS BlendingInstructionEntryDate, BlendingInstructions.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, BlendingInstructions.Description, BlendingInstructions.TotalQuantity " + "\r\n";
            queryString = queryString + "       FROM            BlendingInstructions " + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities ON BlendingInstructions.ParentID IS NULL AND BlendingInstructions.Approved = 1 AND BlendingInstructions.InActive = 0 AND BlendingInstructions.InActivePartial = 0 AND BlendingInstructions.CommodityID = Commodities.CommodityID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetBlendingInstructionRunnings", queryString);
        }

        private void BlendingInstructionSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";
            queryString = queryString + "       IF (@SaveRelativeOption = 1) ";
            queryString = queryString + "           BEGIN ";
            queryString = queryString + "               UPDATE          BlendingInstructionDetails " + "\r\n";
            queryString = queryString + "               SET             BlendingInstructionDetails.Reference = BlendingInstructions.Reference " + "\r\n";
            queryString = queryString + "               FROM            BlendingInstructions INNER JOIN BlendingInstructionDetails ON BlendingInstructions.BlendingInstructionID = @EntityID AND BlendingInstructions.BlendingInstructionID = BlendingInstructionDetails.BlendingInstructionID " + "\r\n";
            queryString = queryString + "           END ";

            queryString = queryString + "       IF ((SELECT Approved FROM BlendingInstructions WHERE BlendingInstructionID = @EntityID AND Approved = 1) = 1) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE      BlendingInstructions  SET Approved = 0 WHERE BlendingInstructionID = @EntityID AND Approved = 1" + "\r\n"; //CLEAR APPROVE BEFORE CALL BlendingInstructionToggleApproved
            queryString = queryString + "               IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "                   EXEC        BlendingInstructionToggleApproved @EntityID, 1 " + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã duyệt'; " + "\r\n";
            queryString = queryString + "                       THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("BlendingInstructionSaveRelative", queryString);
        }

        private void BlendingInstructionPostSaveValidate()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = 'TEST Date: ' + CAST(EntryDate AS nvarchar) FROM BlendingInstructions WHERE BlendingInstructionID = @EntityID "; //FOR TEST TO BREAK OUT WHEN SAVE -> CHECK ROLL BACK OF TRANSACTION
            //queryArray[0] = " SELECT TOP 1 @FoundEntity = 'Service Date: ' + CAST(ServiceInvoices.EntryDate AS nvarchar) FROM BlendingInstructions INNER JOIN BlendingInstructions AS ServiceInvoices ON BlendingInstructions.BlendingInstructionID = @EntityID AND BlendingInstructions.ServiceInvoiceID = ServiceInvoices.BlendingInstructionID AND BlendingInstructions.EntryDate < ServiceInvoices.EntryDate ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("BlendingInstructionPostSaveValidate", queryArray);
        }




        private void BlendingInstructionApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = BlendingInstructionID FROM BlendingInstructions WHERE BlendingInstructionID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("BlendingInstructionApproved", queryArray);
        }


        private void BlendingInstructionEditable()
        {
            string[] queryArray = new string[3];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = BlendingInstructionID FROM BlendingInstructions WHERE BlendingInstructionID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            queryArray[1] = " SELECT TOP 1 @FoundEntity = BlendingInstructionID FROM PackageIssues WHERE BlendingInstructionID = @EntityID ";
            queryArray[2] = " SELECT TOP 1 @FoundEntity = BlendingInstructionID FROM PackageIssueDetails WHERE BlendingInstructionID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("BlendingInstructionEditable", queryArray);
        }

        private void BlendingInstructionVoidable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = BlendingInstructionID FROM BlendingInstructions WHERE BlendingInstructionID = @EntityID AND Approved = 0"; //Must approve in order to allow void

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("BlendingInstructionVoidable", queryArray);
        }

        private void BlendingInstructionToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      BlendingInstructions  SET Approved = @Approved, ApprovedDate = GetDate() WHERE BlendingInstructionID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          BlendingInstructionDetails  SET Approved = @Approved WHERE BlendingInstructionID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, N'hủy', '')  + N' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("BlendingInstructionToggleApproved", queryString);
        }

        private void BlendingInstructionToggleVoid()
        {
            string queryString = " @EntityID int, @InActive bit, @VoidTypeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      BlendingInstructions  SET InActive = @InActive, InActiveDate = GetDate(), VoidTypeID = IIF(@InActive = 1, @VoidTypeID, NULL) WHERE BlendingInstructionID = @EntityID AND InActive = ~@InActive" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          BlendingInstructionDetails     SET InActive = @InActive WHERE BlendingInstructionID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@InActive = 0, 'phục hồi lệnh', '')  + ' hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            this.totalSmartPortalEntities.CreateStoredProcedure("BlendingInstructionToggleVoid", queryString);
        }

        private void BlendingInstructionToggleVoidDetail()
        {
            string queryString = " @EntityID int, @EntityDetailID int, @InActivePartial bit, @VoidTypeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      BlendingInstructionDetails     SET InActivePartial = @InActivePartial, InActivePartialDate = GetDate(), VoidTypeID = IIF(@InActivePartial = 1, @VoidTypeID, NULL) WHERE BlendingInstructionID = @EntityID AND BlendingInstructionDetailID = @EntityDetailID AND InActivePartial = ~@InActivePartial ; " + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE         @MaxInActivePartial bit     SET @MaxInActivePartial = (SELECT MAX(CAST(InActivePartial AS int)) FROM BlendingInstructionDetails WHERE BlendingInstructionID = @EntityID) ;" + "\r\n";
            queryString = queryString + "               UPDATE          BlendingInstructions  SET InActivePartial = @MaxInActivePartial WHERE BlendingInstructionID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@InActivePartial = 0, 'phục hồi lệnh', '')  + ' hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            this.totalSmartPortalEntities.CreateStoredProcedure("BlendingInstructionToggleVoidDetail", queryString);
        }


        private void BlendingInstructionInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("BlendingInstructions", "BlendingInstructionID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.BlendingInstruction));
            this.totalSmartPortalEntities.CreateTrigger("BlendingInstructionInitReference", simpleInitReference.CreateQuery());
        }

        private void GetBlendingInstructionLogs()
        {
            string queryString = " @BlendingInstructionID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT          PackageIssueDetails.PackageIssueDetailID, PackageIssueDetails.EntryDate, ProductionLines.Code AS ProductionLineCode, IIF(Workshifts.EntryDate IS NULL, NULL, PackageIssueDetails.EntryDate) AS PackageIssueWorkshiftEntryDate, Workshifts.Code AS PackageIssueWorkshiftCode, GoodsReceiptDetails.Reference AS GoodsReceiptReference, GoodsReceiptDetails.BatchEntryDate, PackageIssueDetails.Quantity AS PackageIssueDetailQuantity, NULL AS SemifinishedProductWorkshiftEntryDate, NULL AS SemifinishedProductWorkshiftCode, NULL AS CommoditiyCode, NULL AS Quantity, NULL AS Weights " + "\r\n";
            queryString = queryString + "       FROM            PackageIssueDetails " + "\r\n";
            queryString = queryString + "                       INNER JOIN Workshifts ON PackageIssueDetails.BlendingInstructionID = @BlendingInstructionID AND PackageIssueDetails.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                       INNER JOIN ProductionLines ON PackageIssueDetails.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";
            queryString = queryString + "                       INNER JOIN GoodsReceiptDetails ON PackageIssueDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";

            queryString = queryString + "       ORDER BY        PackageIssueDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetBlendingInstructionLogs", queryString);
        }


        private void BlendingInstructionSheet()
        {
            string queryString = " @BlendingInstructionID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @LocalBlendingInstructionID int    SET @LocalBlendingInstructionID = @BlendingInstructionID" + "\r\n";

            queryString = queryString + "       SELECT          BlendingInstructions.BlendingInstructionID, BlendingInstructions.EntryDate, BlendingInstructions.Reference, BlendingInstructions.Code, BlendingInstructions.VoucherDate, BlendingInstructions.Jobs, BlendingInstructions.Description, BlendingInstructions.CommodityID AS ProductID, Products.Code AS ProductCode, Products.Name AS ProductName, " + "\r\n";
            queryString = queryString + "                       BlendingInstructionDetails.BlendingInstructionDetailID, BlendingInstructionDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, BlendingInstructionDetails.Quantity, BlendingInstructionDetails.QuantityIssued, BlendingInstructionDetails.Quantity - BlendingInstructionDetails.QuantityIssued AS QuantityRemains, BlendingInstructionDetails.InActive, BlendingInstructionDetails.InActivePartial, ISNULL(VoidTypes.Name, VoidTypeDetails.Name) AS VoidTypeName, " + "\r\n";
            queryString = queryString + "                       PackageIssueDetails.PackageIssueDetailID, PackageIssueDetails.PackageIssueImage1ID, PackageIssueDetails.PackageIssueImage2ID, PackageIssueDetails.SealCode, PackageIssueDetails.BatchCode, PackageIssueDetails.LabCode, PackageIssueDetails.Barcode, GoodsReceiptDetails.ProductionDate, GoodsReceiptDetails.ExpiryDate, PackageIssueDetails.Quantity AS PackageIssueQuantity " + "\r\n";

            queryString = queryString + "       FROM            BlendingInstructions " + "\r\n";
            queryString = queryString + "                       INNER JOIN BlendingInstructionDetails ON BlendingInstructions.BlendingInstructionID = @LocalBlendingInstructionID AND BlendingInstructions.BlendingInstructionID = ISNULL(BlendingInstructionDetails.ParentID, BlendingInstructionDetails.BlendingInstructionID) " + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities AS Products ON BlendingInstructions.CommodityID = Products.CommodityID  " + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities ON BlendingInstructionDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            
            queryString = queryString + "                       LEFT  JOIN PackageIssueDetails ON BlendingInstructionDetails.BlendingInstructionDetailID = PackageIssueDetails.BlendingInstructionDetailID " + "\r\n";
            queryString = queryString + "                       LEFT  JOIN GoodsReceiptDetails ON PackageIssueDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";

            queryString = queryString + "                       LEFT  JOIN   VoidTypes ON BlendingInstructions.VoidTypeID = VoidTypes.VoidTypeID " + "\r\n";
            queryString = queryString + "                       LEFT  JOIN   VoidTypes VoidTypeDetails ON BlendingInstructionDetails.VoidTypeID = VoidTypeDetails.VoidTypeID " + "\r\n";

            queryString = queryString + "       ORDER BY        BlendingInstructionDetails.BlendingInstructionDetailID, PackageIssueDetails.PackageIssueDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("BlendingInstructionSheet", queryString);
        }

        #endregion
    }
}
