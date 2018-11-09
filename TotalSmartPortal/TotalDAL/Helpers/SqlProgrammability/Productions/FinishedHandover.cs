using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Productions
{
    public class FinishedHandover
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public FinishedHandover(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetFinishedHandoverIndexes();

            this.GetFinishedHandoverViewDetails();

            this.GetFinishedHandoverPendingPlannedOrders();
            this.GetFinishedHandoverPendingCustomers();
            this.GetFinishedHandoverPendingDetails();

            this.FinishedHandoverSaveRelative();
            this.FinishedHandoverPostSaveValidate();

            this.FinishedHandoverApproved();
            this.FinishedHandoverEditable();

            this.FinishedHandoverToggleApproved();

            this.FinishedHandoverInitReference();


            this.FinishedHandoverSheet();
        }

        private void GetFinishedHandoverIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      FinishedHandovers.FinishedHandoverID, CAST(FinishedHandovers.EntryDate AS DATE) AS EntryDate, FinishedHandovers.Reference, Locations.Code AS LocationCode, ISNULL(Customers.Name, N'Phiếu tổng hợp') AS CustomerDescription, FinishedHandovers.Caption, FinishedHandovers.Description, FinishedHandovers.TotalQuantity, FinishedHandovers.Approved " + "\r\n";
            queryString = queryString + "       FROM        FinishedHandovers " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON FinishedHandovers.EntryDate >= @FromDate AND FinishedHandovers.EntryDate <= @ToDate AND FinishedHandovers.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.FinishedHandover + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = FinishedHandovers.LocationID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN Customers ON FinishedHandovers.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetFinishedHandoverIndexes", queryString);
        }

        private void GetFinishedHandoverViewDetails()
        {
            string queryString;

            queryString = " @FinishedHandoverID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      FinishedHandoverDetails.FinishedHandoverDetailID, FinishedHandoverDetails.FinishedHandoverID, FinishedProductPackages.FinishedProductID, FinishedProductPackages.FinishedProductPackageID, FinishedProductPackages.EntryDate AS FinishedProductEntryDate, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, FirmOrders.EntryDate AS FirmOrderEntryDate, " + "\r\n";
            queryString = queryString + "                   FinishedProductPackages.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, FinishedProductPackages.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   FinishedHandoverDetails.Quantity, FinishedProductPackages.SemifinishedProductReferences, FinishedHandoverDetails.Remarks" + "\r\n";

            queryString = queryString + "       FROM        FinishedHandoverDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN FinishedProductPackages ON FinishedHandoverDetails.FinishedHandoverID = @FinishedHandoverID AND FinishedHandoverDetails.FinishedProductPackageID = FinishedProductPackages.FinishedProductPackageID " + "\r\n";
            queryString = queryString + "                   INNER JOIN FirmOrders ON FinishedProductPackages.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON FinishedProductPackages.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON FinishedProductPackages.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "       ORDER BY    FinishedHandoverDetails.FinishedHandoverDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetFinishedHandoverViewDetails", queryString);
        }











        private void GetFinishedHandoverPendingPlannedOrders()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          PlannedOrders.PlannedOrderID, PlannedOrders.EntryDate, PlannedOrders.Code AS PlannedOrderCode, Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName " + "\r\n";
            queryString = queryString + "       FROM            Customers " + "\r\n";
            queryString = queryString + "                       INNER JOIN PlannedOrders ON PlannedOrders.PlannedOrderID IN (SELECT DISTINCT PlannedOrderID FROM FinishedProductPackages WHERE FinishedHandoverID IS NULL AND Approved = 1) AND PlannedOrders.CustomerID = Customers.CustomerID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetFinishedHandoverPendingPlannedOrders", queryString);
        }

        private void GetFinishedHandoverPendingCustomers()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName " + "\r\n";
            queryString = queryString + "       FROM            Customers " + "\r\n";
            queryString = queryString + "       WHERE           CustomerID IN (SELECT DISTINCT CustomerID FROM FinishedProductPackages WHERE FinishedHandoverID IS NULL AND Approved = 1 GROUP BY CustomerID) " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetFinishedHandoverPendingCustomers", queryString);
        }


        private void GetFinishedHandoverPendingDetails()
        {
            string queryString;

            queryString = " @FinishedHandoverID Int, @PlannedOrderID Int, @CustomerID Int, @FinishedProductPackageIDs varchar(3999), @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@PlannedOrderID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.GetPendingBUILDSQL(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingBUILDSQL(false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetFinishedHandoverPendingDetails", queryString);
        }

        private string GetPendingBUILDSQL(bool isPlannedOrderID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@CustomerID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.GetPendingBUILDSQL(isPlannedOrderID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingBUILDSQL(isPlannedOrderID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPendingBUILDSQL(bool isPlannedOrderID, bool isCustomerID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@FinishedProductPackageIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.GetPendingBUILDSQL(isPlannedOrderID, isCustomerID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingBUILDSQL(isPlannedOrderID, isCustomerID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPendingBUILDSQL(bool isPlannedOrderID, bool isCustomerID, bool isFinishedProductPackageIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@FinishedHandoverID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.GetPendingBUILDSQLNew(isPlannedOrderID, isCustomerID, isFinishedProductPackageIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY Customers.Name, Customers.Code, Commodities.Code, FinishedProductPackages.EntryDate " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.GetPendingBUILDSQLEdit(isPlannedOrderID, isCustomerID, isFinishedProductPackageIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY Customers.Name, Customers.Code, Commodities.Code, FinishedProductPackages.EntryDate " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.GetPendingBUILDSQLNew(isPlannedOrderID, isCustomerID, isFinishedProductPackageIDs) + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       " + this.GetPendingBUILDSQLEdit(isPlannedOrderID, isCustomerID, isFinishedProductPackageIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY Customers.Name, Customers.Code, Commodities.Code, FinishedProductPackages.EntryDate " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPendingBUILDSQLNew(bool isPlannedOrderID, bool isCustomerID, bool isFinishedProductPackageIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      FinishedProductPackages.FinishedProductID, FinishedProductPackages.FinishedProductPackageID, FinishedProductPackages.EntryDate AS FinishedProductEntryDate, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, FirmOrders.EntryDate AS FirmOrderEntryDate, FinishedProductPackages.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, " + "\r\n";
            queryString = queryString + "                   FinishedProductPackages.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, FinishedProductPackages.Quantity, FinishedProductPackages.SemifinishedProductReferences, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        FinishedProductPackages " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON FinishedProductPackages.Approved = 1 " + (isCustomerID ? " AND FinishedProductPackages.CustomerID = @CustomerID " : "") + (isPlannedOrderID ? " AND FinishedProductPackages.PlannedOrderID = @PlannedOrderID " : "") + " AND FinishedProductPackages.FinishedHandoverID IS NULL AND FinishedProductPackages.CustomerID = Customers.CustomerID " + (isFinishedProductPackageIDs ? " AND FinishedProductPackages.FinishedProductPackageID NOT IN (SELECT Id FROM dbo.SplitToIntList (@FinishedProductPackageIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON FinishedProductPackages.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN FirmOrders ON FinishedProductPackages.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";

            return queryString;
        }

        private string GetPendingBUILDSQLEdit(bool isPlannedOrderID, bool isCustomerID, bool isFinishedProductPackageIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      FinishedProductPackages.FinishedProductID, FinishedProductPackages.FinishedProductPackageID, FinishedProductPackages.EntryDate AS FinishedProductEntryDate, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, FirmOrders.EntryDate AS FirmOrderEntryDate, FinishedProductPackages.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, " + "\r\n";
            queryString = queryString + "                   FinishedProductPackages.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, FinishedProductPackages.Quantity, FinishedProductPackages.SemifinishedProductReferences, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        FinishedProductPackages " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON FinishedProductPackages.FinishedHandoverID = @FinishedHandoverID AND FinishedProductPackages.CustomerID = Customers.CustomerID " + (isFinishedProductPackageIDs ? " AND FinishedProductPackages.FinishedProductPackageID NOT IN (SELECT Id FROM dbo.SplitToIntList (@FinishedProductPackageIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON FinishedProductPackages.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN FirmOrders ON FinishedProductPackages.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";

            return queryString;
        }


        private void FinishedHandoverSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";

            queryString = queryString + "               DECLARE @msg NVARCHAR(300) " + "\r\n"; ;

            queryString = queryString + "               UPDATE      FinishedProductPackages" + "\r\n";
            queryString = queryString + "               SET         FinishedProductPackages.FinishedHandoverID = FinishedHandoverDetails.FinishedHandoverID " + "\r\n";
            queryString = queryString + "               FROM        FinishedProductPackages INNER JOIN" + "\r\n";
            queryString = queryString + "                           FinishedHandoverDetails ON FinishedHandoverDetails.FinishedHandoverID = @EntityID AND FinishedProductPackages.Approved = 1 AND FinishedProductPackages.FinishedProductPackageID = FinishedHandoverDetails.FinishedProductPackageID " + "\r\n";

            queryString = queryString + "               IF @@ROWCOUNT = (SELECT COUNT(*) FROM FinishedProductPackages WHERE FinishedHandoverID = @EntityID) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       UPDATE      FinishedProductDetails" + "\r\n";
            queryString = queryString + "                       SET         FinishedHandoverID = @EntityID " + "\r\n";
            queryString = queryString + "                       WHERE       FinishedProductPackageID IN (SELECT FinishedProductPackageID FROM FinishedProductPackages WHERE FinishedHandoverID = @EntityID) " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       SET         @msg = N'Dữ liệu không tồn tại hoặc chưa duyệt' ; " + "\r\n";
            queryString = queryString + "                       THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";


            queryString = queryString + "               IF ((SELECT Approved FROM FinishedHandovers WHERE FinishedHandoverID = @EntityID AND Approved = 1) = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       UPDATE      FinishedHandovers  SET Approved = 0 WHERE FinishedHandoverID = @EntityID AND Approved = 1" + "\r\n"; //CLEAR APPROVE BEFORE CALL FinishedHandoverToggleApproved
            queryString = queryString + "                       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "                           EXEC        FinishedHandoverToggleApproved @EntityID, 1 " + "\r\n";
            queryString = queryString + "                       ELSE " + "\r\n";
            queryString = queryString + "                           BEGIN " + "\r\n";
            queryString = queryString + "                               SET         @msg = N'Dữ liệu không tồn tại hoặc đã duyệt'; " + "\r\n";
            queryString = queryString + "                               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "                           END " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";


            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       ELSE " + "\r\n"; //(@SaveRelativeOption = -1) 
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE      FinishedProductPackages" + "\r\n";
            queryString = queryString + "               SET         FinishedHandoverID = NULL " + "\r\n";
            queryString = queryString + "               WHERE       FinishedHandoverID = @EntityID " + "\r\n";

            queryString = queryString + "               UPDATE      FinishedProductDetails " + "\r\n";
            queryString = queryString + "               SET         FinishedHandoverID = NULL " + "\r\n";
            queryString = queryString + "               WHERE       FinishedHandoverID = @EntityID " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("FinishedHandoverSaveRelative", queryString);
        }

        private void FinishedHandoverPostSaveValidate()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày đóng hàng: ' + CAST(FinishedProductPackages.EntryDate AS nvarchar) FROM FinishedHandovers INNER JOIN FinishedProductPackages ON FinishedHandovers.FinishedHandoverID = @EntityID AND FinishedHandovers.FinishedHandoverID = FinishedProductPackages.FinishedHandoverID AND FinishedHandovers.EntryDate < FinishedProductPackages.EntryDate ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("FinishedHandoverPostSaveValidate", queryArray);
        }




        private void FinishedHandoverApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = FinishedHandoverID FROM FinishedHandovers WHERE FinishedHandoverID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("FinishedHandoverApproved", queryArray);
        }


        private void FinishedHandoverEditable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = GoodsReceiptDetailID FROM GoodsReceiptDetails WHERE FinishedProductPackageID IN (SELECT FinishedProductPackageID FROM FinishedProductPackages WHERE FinishedHandoverID = @EntityID) ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("FinishedHandoverEditable", queryArray);
        }

        private void FinishedHandoverToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      FinishedHandovers  SET Approved = @Approved, ApprovedDate = GetDate() WHERE FinishedHandoverID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          FinishedHandoverDetails     SET Approved            = @Approved WHERE FinishedHandoverID = @EntityID ; " + "\r\n";
            queryString = queryString + "               UPDATE          FinishedProductPackages     SET HandoverApproved    = @Approved WHERE FinishedHandoverID = @EntityID ; " + "\r\n";
            queryString = queryString + "               UPDATE          FinishedProductDetails      SET HandoverApproved    = @Approved WHERE FinishedProductPackageID IN (SELECT FinishedProductPackageID FROM FinishedProductPackages WHERE FinishedHandoverID = @EntityID) ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, N'hủy', '')  + N' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("FinishedHandoverToggleApproved", queryString);
        }

        private void FinishedHandoverInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("FinishedHandovers", "FinishedHandoverID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.FinishedHandover));
            this.totalSmartPortalEntities.CreateTrigger("FinishedHandoverInitReference", simpleInitReference.CreateQuery());
        }






        private void FinishedHandoverSheet()
        {
            string queryString;

            queryString = " @FinishedHandoverID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SET NOCOUNT ON" + "\r\n";

            queryString = queryString + "       DECLARE     @LocalFinishedHandoverID int      SET @LocalFinishedHandoverID = @FinishedHandoverID" + "\r\n";

            queryString = queryString + "       SELECT      FinishedHandovers.FinishedHandoverID, FinishedHandoverDetails.FinishedHandoverDetailID, FinishedHandovers.EntryDate, FinishedHandovers.Reference, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, FirmOrders.EntryDate AS FirmOrderEntryDate, Customers.Name AS CustomerName, " + "\r\n";
            queryString = queryString + "                   Commodities.Code, Commodities.Name, FinishedProductPackages.PiecePerPack, FinishedProductPackages.Quantity, FinishedProductPackages.Packages, FinishedProductPackages.OddPackages, FinishedLeaders.Name AS FinishedLeaderName, Storekeepers.Name AS StorekeeperName, FinishedProductPackages.SemifinishedProductReferences, FinishedHandovers.Description " + "\r\n";

            queryString = queryString + "       FROM        FinishedHandovers " + "\r\n";
            queryString = queryString + "                   INNER JOIN FinishedHandoverDetails ON FinishedHandovers.FinishedHandoverID = @LocalFinishedHandoverID AND FinishedHandovers.FinishedHandoverID = FinishedHandoverDetails.FinishedHandoverID " + "\r\n";
            queryString = queryString + "                   INNER JOIN FinishedProductPackages ON FinishedHandoverDetails.FinishedProductPackageID = FinishedProductPackages.FinishedProductPackageID " + "\r\n";
            queryString = queryString + "                   INNER JOIN FirmOrders ON FinishedProductPackages.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON FinishedProductPackages.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON FinishedProductPackages.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Employees AS FinishedLeaders ON FinishedHandovers.FinishedLeaderID = FinishedLeaders.EmployeeID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Employees AS Storekeepers ON FinishedHandovers.StorekeeperID = Storekeepers.EmployeeID " + "\r\n";

            queryString = queryString + "       ORDER BY    FinishedHandoverDetails.FinishedHandoverDetailID " + "\r\n";

            queryString = queryString + "       SET NOCOUNT OFF" + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("FinishedHandoverSheet", queryString);
        }

    }
}