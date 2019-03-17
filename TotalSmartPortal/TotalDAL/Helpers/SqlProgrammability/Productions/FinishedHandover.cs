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
            this.GetFinishedHandoverPendingWorkshifts();
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

            queryString = " @NMVNTaskID int, @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      FinishedHandovers.FinishedHandoverID, CAST(FinishedHandovers.EntryDate AS DATE) AS EntryDate, FinishedHandovers.Reference, Locations.Code AS LocationCode, Workshifts.EntryDate AS WorkshiftEntryDate, Workshifts.Code AS WorkshiftCode, ISNULL(Customers.Name, N'Phiếu tổng hợp') AS CustomerDescription, FinishedHandovers.Caption, FinishedHandovers.Description, FinishedHandovers.TotalQuantity, FinishedHandovers.Approved " + "\r\n";
            queryString = queryString + "       FROM        FinishedHandovers " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON FinishedHandovers.NMVNTaskID = @NMVNTaskID AND FinishedHandovers.EntryDate >= @FromDate AND FinishedHandovers.EntryDate <= @ToDate AND FinishedHandovers.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = @NMVNTaskID AND AccessControls.AccessLevel > 0) AND Locations.LocationID = FinishedHandovers.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON FinishedHandovers.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
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

            queryString = queryString + "       IF ((SELECT NMVNTaskID FROM FinishedHandovers WHERE FinishedHandoverID = @FinishedHandoverID) = " + (int)GlobalEnums.NmvnTaskID.FinishedProductHandover + ") " + "\r\n";
            queryString = queryString + "           " + this.GetFinishedHandoverViewDetailSQL(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetFinishedHandoverViewDetailSQL(false) + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetFinishedHandoverViewDetails", queryString);
        }

        private string GetFinishedHandoverViewDetailSQL(bool productVsItem)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       SELECT      FinishedHandoverDetails.FinishedHandoverDetailID, FinishedHandoverDetails.FinishedHandoverID, FinishedHandoverDetails.FinishedItemID, FinishedHandoverDetails.FinishedItemPackageID, FinishedHandoverDetails.FinishedProductID, FinishedHandoverDetails.FinishedProductPackageID, FinishedProtemPackages.EntryDate AS FinishedProtemEntryDate, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, FirmOrders.EntryDate AS FirmOrderEntryDate, " + "\r\n";
            queryString = queryString + "                   FinishedProtemPackages.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, FinishedProtemPackages.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   FinishedHandoverDetails.Quantity, FinishedProtemPackages." + this.productVsItemSemifinishedReferences(productVsItem) + ", FinishedHandoverDetails.Remarks " + "\r\n";

            queryString = queryString + "       FROM        FinishedHandoverDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN " + this.productVsItemTable(productVsItem) + " FinishedProtemPackages ON FinishedHandoverDetails.FinishedHandoverID = @FinishedHandoverID AND FinishedHandoverDetails." + this.productVsItemPrimaryKey(productVsItem) + " = FinishedProtemPackages." + this.productVsItemPrimaryKey(productVsItem) + "\r\n";
            queryString = queryString + "                   INNER JOIN FirmOrders ON FinishedProtemPackages.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON FinishedProtemPackages.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON FinishedProtemPackages.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "       ORDER BY    FinishedHandoverDetails.FinishedHandoverDetailID " + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }








        private string productVsItemTable(bool productVsItem)
        {
            return productVsItem ? "FinishedProductPackages" : "FinishedItemPackages";
        }

        private string productVsItemPrimaryKey(bool productVsItem)
        {
            return productVsItem ? "FinishedProductPackageID" : "FinishedItemPackageID";
        }

        private string productVsItemSemifinishedReferences(bool productVsItem)
        {
            return (productVsItem ? "SemifinishedProductReferences" : "SemifinishedItemReferences") + " AS SemifinishedProtemReferences";
        }


        private void GetFinishedHandoverPendingPlannedOrders()
        {
            string queryString = " @NMVNTaskID int, @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          PlannedOrders.PlannedOrderID, PlannedOrders.EntryDate AS PlannedOrderEntryDate, PlannedOrders.Code AS PlannedOrderCode, Workshifts.WorkshiftID, Workshifts.EntryDate AS WorkshiftEntryDate, Workshifts.Code AS WorkshiftCode, Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName " + "\r\n";
            queryString = queryString + "       FROM            PlannedOrders " + "\r\n";
            queryString = queryString + "                       INNER JOIN (SELECT WorkshiftID, PlannedOrderID FROM FinishedProductPackages WHERE @NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.FinishedProductHandover + " AND FinishedHandoverID IS NULL AND Approved = 1 GROUP BY WorkshiftID, PlannedOrderID UNION ALL SELECT WorkshiftID, PlannedOrderID FROM FinishedItemPackages WHERE @NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.FinishedItemHandover + " AND FinishedHandoverID IS NULL AND Approved = 1 GROUP BY WorkshiftID, PlannedOrderID) FinishedProtemPackages ON PlannedOrders.PlannedOrderID = FinishedProtemPackages.PlannedOrderID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Workshifts ON FinishedProtemPackages.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON PlannedOrders.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "       ORDER BY        Workshifts.EntryDate DESC, Workshifts.Code DESC " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetFinishedHandoverPendingPlannedOrders", queryString);
        }

        private void GetFinishedHandoverPendingCustomers()
        {
            string queryString = " @NMVNTaskID int, @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          Workshifts.WorkshiftID, Workshifts.EntryDate AS WorkshiftEntryDate, Workshifts.Code AS WorkshiftCode, Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName " + "\r\n";
            queryString = queryString + "       FROM            Workshifts " + "\r\n";
            queryString = queryString + "                       INNER JOIN (SELECT WorkshiftID, CustomerID FROM FinishedProductPackages WHERE @NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.FinishedProductHandover + " AND FinishedHandoverID IS NULL AND Approved = 1 GROUP BY WorkshiftID, CustomerID UNION ALL SELECT WorkshiftID, CustomerID FROM FinishedItemPackages WHERE @NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.FinishedItemHandover + " AND FinishedHandoverID IS NULL AND Approved = 1 GROUP BY WorkshiftID, CustomerID) FinishedProtemPackages ON Workshifts.WorkshiftID = FinishedProtemPackages.WorkshiftID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON FinishedProtemPackages.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "       ORDER BY        Workshifts.EntryDate DESC, Workshifts.Code DESC " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetFinishedHandoverPendingCustomers", queryString);
        }

        private void GetFinishedHandoverPendingWorkshifts()
        {
            string queryString = " @NMVNTaskID int, @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          Workshifts.WorkshiftID, Workshifts.EntryDate AS WorkshiftEntryDate, Workshifts.Code AS WorkshiftCode " + "\r\n";
            queryString = queryString + "       FROM            Workshifts " + "\r\n";
            queryString = queryString + "                       INNER JOIN (SELECT WorkshiftID FROM FinishedProductPackages WHERE @NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.FinishedProductHandover + " AND FinishedHandoverID IS NULL AND Approved = 1 GROUP BY WorkshiftID UNION ALL SELECT WorkshiftID FROM FinishedItemPackages WHERE @NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.FinishedItemHandover + " AND FinishedHandoverID IS NULL AND Approved = 1 GROUP BY WorkshiftID) FinishedProtemPackages ON Workshifts.WorkshiftID = FinishedProtemPackages.WorkshiftID " + "\r\n";
            queryString = queryString + "       ORDER BY        Workshifts.EntryDate DESC, Workshifts.Code DESC " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetFinishedHandoverPendingWorkshifts", queryString);
        }

        private void GetFinishedHandoverPendingDetails()
        {
            string queryString;

            queryString = " @NMVNTaskID int, @FinishedHandoverID Int, @WorkshiftID Int, @PlannedOrderID Int, @CustomerID Int, @FinishedItemPackageIDs varchar(3999), @FinishedProductPackageIDs varchar(3999) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.FinishedProductHandover + ") " + "\r\n";
            queryString = queryString + "           " + this.GetPendingBUILDSQL(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingBUILDSQL(false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetFinishedHandoverPendingDetails", queryString);
        }

        private string GetPendingBUILDSQL(bool productVsItem)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@PlannedOrderID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.GetPendingBUILDSQL(productVsItem, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingBUILDSQL(productVsItem, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPendingBUILDSQL(bool productVsItem, bool isPlannedOrderID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@CustomerID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.GetPendingBUILDSQL(productVsItem, isPlannedOrderID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingBUILDSQL(productVsItem, isPlannedOrderID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPendingBUILDSQL(bool productVsItem, bool isPlannedOrderID, bool isCustomerID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (" + (productVsItem ? "@FinishedProductPackageIDs" : "@FinishedItemPackageIDs") + " <> '') " + "\r\n";
            queryString = queryString + "           " + this.GetPendingBUILDSQL(productVsItem, isPlannedOrderID, isCustomerID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingBUILDSQL(productVsItem, isPlannedOrderID, isCustomerID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPendingBUILDSQL(bool productVsItem, bool isPlannedOrderID, bool isCustomerID, bool isFinishedProductPackageIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@FinishedHandoverID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.GetPendingBUILDSQLNew(productVsItem, isPlannedOrderID, isCustomerID, isFinishedProductPackageIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY Customers.Name, Customers.Code, Commodities.Code, FinishedProtemPackages.EntryDate " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.GetPendingBUILDSQLNew(productVsItem, isPlannedOrderID, isCustomerID, isFinishedProductPackageIDs) + "\r\n";
            queryString = queryString + "                   UNION ALL " + "\r\n";
            queryString = queryString + "                   " + this.GetPendingBUILDSQLEdit(productVsItem, isPlannedOrderID, isCustomerID, isFinishedProductPackageIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY Customers.Name, Customers.Code, Commodities.Code, FinishedProtemPackages.EntryDate " + "\r\n";
            queryString = queryString + "               END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPendingBUILDSQLNew(bool productVsItem, bool isPlannedOrderID, bool isCustomerID, bool isFinishedProductPackageIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      " + (productVsItem ? "NULL AS FinishedItemID" : "FinishedProtemPackages.FinishedItemID") + ", " + (productVsItem ? "NULL AS FinishedItemPackageID" : "FinishedProtemPackages.FinishedItemPackageID") + ", " + (productVsItem ? "FinishedProtemPackages.FinishedProductID" : "NULL AS FinishedProductID") + ", " + (productVsItem ? "FinishedProtemPackages.FinishedProductPackageID" : "NULL AS FinishedProductPackageID") + ", FinishedProtemPackages.EntryDate AS FinishedProtemEntryDate, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, FirmOrders.EntryDate AS FirmOrderEntryDate, FinishedProtemPackages.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, " + "\r\n";
            queryString = queryString + "                   FinishedProtemPackages.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, FinishedProtemPackages.Quantity, FinishedProtemPackages." + this.productVsItemSemifinishedReferences(productVsItem) + ", CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        " + this.productVsItemTable(productVsItem) + " FinishedProtemPackages " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON FinishedProtemPackages.Approved = 1 AND FinishedProtemPackages.WorkshiftID = @WorkshiftID " + (isCustomerID ? " AND FinishedProtemPackages.CustomerID = @CustomerID " : "") + (isPlannedOrderID ? " AND FinishedProtemPackages.PlannedOrderID = @PlannedOrderID " : "") + " AND FinishedProtemPackages.FinishedHandoverID IS NULL AND FinishedProtemPackages.CustomerID = Customers.CustomerID " + (isFinishedProductPackageIDs ? " AND FinishedProtemPackages." + this.productVsItemPrimaryKey(productVsItem) + " NOT IN (SELECT Id FROM dbo.SplitToIntList (" + (productVsItem ? "@FinishedProductPackageIDs" : "@FinishedItemPackageIDs") + "))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON FinishedProtemPackages.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN FirmOrders ON FinishedProtemPackages.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";

            return queryString;
        }

        private string GetPendingBUILDSQLEdit(bool productVsItem, bool isPlannedOrderID, bool isCustomerID, bool isFinishedProductPackageIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      " + (productVsItem ? "NULL AS FinishedItemID" : "FinishedProtemPackages.FinishedItemID") + ", " + (productVsItem ? "NULL AS FinishedItemPackageID" : "FinishedProtemPackages.FinishedItemPackageID") + ", " + (productVsItem ? "FinishedProtemPackages.FinishedProductID" : "NULL AS FinishedProductID") + ", " + (productVsItem ? "FinishedProtemPackages.FinishedProductPackageID" : "NULL AS FinishedProductPackageID") + ", FinishedProtemPackages.EntryDate AS FinishedProtemEntryDate, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, FirmOrders.EntryDate AS FirmOrderEntryDate, FinishedProtemPackages.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, " + "\r\n";
            queryString = queryString + "                   FinishedProtemPackages.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, FinishedProtemPackages.Quantity, FinishedProtemPackages." + this.productVsItemSemifinishedReferences(productVsItem) + ", CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        " + this.productVsItemTable(productVsItem) + " FinishedProtemPackages " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON FinishedProtemPackages.FinishedHandoverID = @FinishedHandoverID AND FinishedProtemPackages.CustomerID = Customers.CustomerID " + (isFinishedProductPackageIDs ? " AND FinishedProtemPackages." + this.productVsItemPrimaryKey(productVsItem) + " NOT IN (SELECT Id FROM dbo.SplitToIntList (" + (productVsItem ? "@FinishedProductPackageIDs" : "@FinishedItemPackageIDs") + "))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON FinishedProtemPackages.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN FirmOrders ON FinishedProtemPackages.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";

            return queryString;
        }


        private void FinishedHandoverSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE @msg NVARCHAR(300) " + "\r\n";

            queryString = queryString + "       IF ((SELECT NMVNTaskID FROM FinishedHandovers WHERE FinishedHandoverID = @EntityID) = " + (int)GlobalEnums.NmvnTaskID.FinishedProductHandover + ") " + "\r\n";
            queryString = queryString + "           " + this.FinishedHandoverSaveRelativeSQL(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.FinishedHandoverSaveRelativeSQL(false) + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("FinishedHandoverSaveRelative", queryString);
        }

        private string FinishedHandoverSaveRelativeSQL(bool productVsItem)
        {
            string queryString = "";

            queryString = queryString + "    BEGIN " + "\r\n";
            queryString = queryString + "       IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";

            queryString = queryString + "               UPDATE      FinishedProtemPackages" + "\r\n";
            queryString = queryString + "               SET         FinishedProtemPackages.FinishedHandoverID = FinishedHandoverDetails.FinishedHandoverID, FinishedProtemPackages.FinishedHandoverDate = FinishedHandoverDetails.EntryDate " + "\r\n";
            queryString = queryString + "               FROM        " + this.productVsItemTable(productVsItem) + " FinishedProtemPackages INNER JOIN" + "\r\n";
            queryString = queryString + "                           FinishedHandoverDetails ON FinishedHandoverDetails.FinishedHandoverID = @EntityID AND FinishedProtemPackages.Approved = 1 AND FinishedProtemPackages." + this.productVsItemPrimaryKey(productVsItem) + " = FinishedHandoverDetails." + this.productVsItemPrimaryKey(productVsItem) + " " + "\r\n";

            queryString = queryString + "               IF @@ROWCOUNT <> (SELECT COUNT(*) FROM FinishedHandoverDetails WHERE FinishedHandoverID = @EntityID) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       SET         @msg = N'Dữ liệu không tồn tại hoặc chưa duyệt' + CAST(@@ROWCOUNT AS NVARCHAR) ; " + "\r\n";
            queryString = queryString + "                       THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";
            if (productVsItem)
            {
                queryString = queryString + "           ELSE " + "\r\n";
                queryString = queryString + "               BEGIN " + "\r\n";
                queryString = queryString + "                   UPDATE      FinishedProductDetails" + "\r\n";
                queryString = queryString + "                   SET         FinishedHandoverID = @EntityID " + "\r\n";
                queryString = queryString + "                   WHERE       FinishedProductPackageID IN (SELECT FinishedProductPackageID FROM FinishedProductPackages WHERE FinishedHandoverID = @EntityID) " + "\r\n";
                queryString = queryString + "               END " + "\r\n";
            }


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
            queryString = queryString + "               UPDATE      " + this.productVsItemTable(productVsItem) + "\r\n";
            queryString = queryString + "               SET         FinishedHandoverID = NULL " + "\r\n";
            queryString = queryString + "               WHERE       FinishedHandoverID = @EntityID " + "\r\n";
            if (productVsItem)
            {
                queryString = queryString + "           UPDATE      FinishedProductDetails " + "\r\n";
                queryString = queryString + "           SET         FinishedHandoverID = NULL " + "\r\n";
                queryString = queryString + "           WHERE       FinishedHandoverID = @EntityID " + "\r\n";
            }
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            return queryString;
        }

        private void FinishedHandoverPostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày đóng hàng: ' + CAST(FinishedItemPackages.EntryDate AS nvarchar) FROM FinishedHandovers INNER JOIN FinishedItemPackages ON FinishedHandovers.FinishedHandoverID = @EntityID AND FinishedHandovers.FinishedHandoverID = FinishedItemPackages.FinishedHandoverID AND FinishedHandovers.EntryDate < FinishedItemPackages.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Ngày đóng hàng: ' + CAST(FinishedProductPackages.EntryDate AS nvarchar) FROM FinishedHandovers INNER JOIN FinishedProductPackages ON FinishedHandovers.FinishedHandoverID = @EntityID AND FinishedHandovers.FinishedHandoverID = FinishedProductPackages.FinishedHandoverID AND FinishedHandovers.EntryDate < FinishedProductPackages.EntryDate ";

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

            queryString = queryString + "               IF ((SELECT NMVNTaskID FROM FinishedHandovers WHERE FinishedHandoverID = @EntityID) = " + (int)GlobalEnums.NmvnTaskID.FinishedProductHandover + ") " + "\r\n";
            queryString = queryString + "                   " + this.FinishedHandoverToggleApprovedSQL(true) + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n";
            queryString = queryString + "                   " + this.FinishedHandoverToggleApprovedSQL(false) + "\r\n";

            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, N'hủy', '')  + N' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("FinishedHandoverToggleApproved", queryString);
        }

        private string FinishedHandoverToggleApprovedSQL(bool productVsItem)
        {
            string queryString = "";

            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "               UPDATE          " + this.productVsItemTable(productVsItem) + " SET HandoverApproved    = @Approved WHERE FinishedHandoverID = @EntityID ; " + "\r\n";
            if (productVsItem)
                queryString = queryString + "           UPDATE          FinishedProductDetails      SET HandoverApproved    = @Approved WHERE FinishedProductPackageID IN (SELECT FinishedProductPackageID FROM FinishedProductPackages WHERE FinishedHandoverID = @EntityID) ; " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            return queryString;
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

            queryString = queryString + "       IF ((SELECT NMVNTaskID FROM FinishedHandovers WHERE FinishedHandoverID = @FinishedHandoverID) = " + (int)GlobalEnums.NmvnTaskID.FinishedProductHandover + ") " + "\r\n";
            queryString = queryString + "           " + this.FinishedHandoverSheetSQL(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.FinishedHandoverSheetSQL(false) + "\r\n";

            queryString = queryString + "       SET NOCOUNT OFF" + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("FinishedHandoverSheet", queryString);
        }

        private string FinishedHandoverSheetSQL(bool productVsItem)
        {
            string queryString = "";

            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      FinishedHandovers.FinishedHandoverID, FinishedHandoverDetails.FinishedHandoverDetailID, FinishedHandovers.EntryDate, FinishedHandovers.Reference, Workshifts.EntryDate AS WorkshiftEntryDate, Workshifts.Code AS WorkshiftCode, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, FirmOrders.EntryDate AS FirmOrderEntryDate, Customers.Name AS CustomerName, " + "\r\n";
            queryString = queryString + "                   Commodities.Code, Commodities.Name, " + (productVsItem ? "FinishedProtemPackages.PiecePerPack" : "FinishedProtemPackages.Quantity AS PiecePerPack") + ", FinishedProtemPackages.Quantity, " + (productVsItem ? "FinishedProtemPackages.Packages" : "1 AS Packages") + ", " + (productVsItem ? "FinishedProtemPackages.OddPackages" : "0 AS OddPackages") + ", FinishedLeaders.Name AS FinishedLeaderName, Storekeepers.Name AS StorekeeperName, FinishedProtemPackages." + this.productVsItemSemifinishedReferences(productVsItem) + ", FinishedHandovers.Description " + "\r\n";

            queryString = queryString + "       FROM        FinishedHandovers " + "\r\n";
            queryString = queryString + "                   INNER JOIN FinishedHandoverDetails ON FinishedHandovers.FinishedHandoverID = @LocalFinishedHandoverID AND FinishedHandovers.FinishedHandoverID = FinishedHandoverDetails.FinishedHandoverID " + "\r\n";
            queryString = queryString + "                   INNER JOIN " + this.productVsItemTable(productVsItem) + " FinishedProtemPackages ON FinishedHandoverDetails." + this.productVsItemPrimaryKey(productVsItem) + " = FinishedProtemPackages." + this.productVsItemPrimaryKey(productVsItem) + "\r\n";
            queryString = queryString + "                   INNER JOIN FirmOrders ON FinishedProtemPackages.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON FinishedProtemPackages.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON FinishedProtemPackages.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON FinishedHandovers.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Employees AS FinishedLeaders ON FinishedHandovers.FinishedLeaderID = FinishedLeaders.EmployeeID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Employees AS Storekeepers ON FinishedHandovers.StorekeeperID = Storekeepers.EmployeeID " + "\r\n";

            queryString = queryString + "       ORDER BY    FinishedHandoverDetails.FinishedHandoverDetailID " + "\r\n";

            queryString = queryString + "       SET NOCOUNT OFF" + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            return queryString;
        }

    }
}