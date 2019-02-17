using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Productions
{
    public class SemifinishedHandover
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public SemifinishedHandover(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetSemifinishedHandoverIndexes();

            this.GetSemifinishedHandoverViewDetails();

            this.GetSemifinishedHandoverPendingWorkshifts();
            this.GetSemifinishedHandoverPendingCustomers();
            this.GetSemifinishedHandoverPendingDetails();

            this.SemifinishedHandoverSaveRelative();
            this.SemifinishedHandoverPostSaveValidate();

            this.SemifinishedHandoverApproved();
            this.SemifinishedHandoverEditable();

            this.SemifinishedHandoverToggleApproved();

            this.SemifinishedHandoverInitReference();


            this.SemifinishedHandoverSheet();
        }

        private void GetSemifinishedHandoverIndexes()
        {
            string queryString;

            queryString = " @NMVNTaskID int, @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      SemifinishedHandovers.SemifinishedHandoverID, CAST(SemifinishedHandovers.EntryDate AS DATE) AS EntryDate, SemifinishedHandovers.Reference, Locations.Code AS LocationCode, ISNULL(Customers.Name + ',    ' + Customers.BillingAddress, IIF(@NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.SemifinishedItemHandover + ", N'Bàn giao hỗn hợp', N'Bàn giao phôi định hình')) AS CustomerDescription, Workshifts.EntryDate AS WorkshiftEntryDate, Workshifts.Code AS WorkshiftCode, SemifinishedHandovers.Description, SemifinishedHandovers.TotalQuantity, SemifinishedHandovers.Approved " + "\r\n";
            queryString = queryString + "       FROM        SemifinishedHandovers " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON SemifinishedHandovers.NMVNTaskID = @NMVNTaskID AND SemifinishedHandovers.EntryDate >= @FromDate AND SemifinishedHandovers.EntryDate <= @ToDate AND SemifinishedHandovers.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = @NMVNTaskID AND AccessControls.AccessLevel > 0) AND Locations.LocationID = SemifinishedHandovers.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON SemifinishedHandovers.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN Customers ON SemifinishedHandovers.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetSemifinishedHandoverIndexes", queryString);
        }

        private void GetSemifinishedHandoverViewDetails()
        {
            string queryString;

            queryString = " @SemifinishedHandoverID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF ((SELECT NMVNTaskID FROM SemifinishedHandovers WHERE SemifinishedHandoverID = @SemifinishedHandoverID) = " + (int)GlobalEnums.NmvnTaskID.SemifinishedProductHandover + ") " + "\r\n";
            queryString = queryString + "           " + this.GetSemifinishedHandoverViewDetailSQL(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetSemifinishedHandoverViewDetailSQL(false) + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetSemifinishedHandoverViewDetails", queryString);
        }

        private string GetSemifinishedHandoverViewDetailSQL(bool productVsItem)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       SELECT      SemifinishedHandoverDetails.SemifinishedHandoverDetailID, SemifinishedHandoverDetails.SemifinishedHandoverID, SemifinishedHandoverDetails.SemifinishedItemID, SemifinishedHandoverDetails.SemifinishedProductID, SemifinishedProtems.EntryDate AS SemifinishedProtemEntryDate, SemifinishedProtems.Reference AS SemifinishedProtemReference, SemifinishedProtems.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, " + "\r\n";
            queryString = queryString + "                   SemifinishedProtems.ProductionLineID, ProductionLines.Code AS ProductionLineCode, SemifinishedProtems.CrucialWorkerID, Employees.Name AS CrucialWorkerName, SemifinishedHandoverDetails.Quantity, SemifinishedHandoverDetails.Remarks " + "\r\n";

            queryString = queryString + "       FROM        SemifinishedHandoverDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN " + this.productVsItemTable(productVsItem) + " SemifinishedProtems ON SemifinishedHandoverDetails.SemifinishedHandoverID = @SemifinishedHandoverID AND SemifinishedHandoverDetails." + this.productVsItemPrimaryKey(productVsItem) + " = SemifinishedProtems." + this.productVsItemPrimaryKey(productVsItem) + " " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON SemifinishedProtems.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN ProductionLines ON SemifinishedProtems.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Employees ON SemifinishedProtems.CrucialWorkerID = Employees.EmployeeID " + "\r\n";

            queryString = queryString + "       ORDER BY    SemifinishedHandoverDetails.SemifinishedHandoverDetailID " + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }







        private string productVsItemTable(bool productVsItem)
        {
            return productVsItem ? "SemifinishedProducts" : "SemifinishedItems";
        }

        private string productVsItemTableDetail(bool productVsItem)
        {
            return productVsItem ? "SemifinishedProductDetails" : "SemifinishedItemDetails";
        }

        private string productVsItemPrimaryKey(bool productVsItem)
        {
            return productVsItem ? "SemifinishedProductID" : "SemifinishedItemID";
        }

        private void GetSemifinishedHandoverPendingWorkshifts()
        {
            string queryString = " @NMVNTaskID int, @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          Workshifts.WorkshiftID, Workshifts.EntryDate, Workshifts.Code AS WorkshiftCode " + "\r\n";
            queryString = queryString + "       FROM            Workshifts " + "\r\n";
            queryString = queryString + "       WHERE           WorkshiftID IN (SELECT DISTINCT WorkshiftID FROM SemifinishedProducts WHERE @NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.SemifinishedProductHandover + " AND SemifinishedHandoverID IS NULL AND Approved = 1 UNION ALL SELECT DISTINCT WorkshiftID FROM SemifinishedItems WHERE @NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.SemifinishedItemHandover + " AND SemifinishedHandoverID IS NULL AND Approved = 1) " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetSemifinishedHandoverPendingWorkshifts", queryString);
        }

        private void GetSemifinishedHandoverPendingCustomers()
        {
            string queryString = " @NMVNTaskID int, @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          Workshifts.WorkshiftID, Workshifts.EntryDate, Workshifts.Code AS WorkshiftCode, Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName " + "\r\n";
            queryString = queryString + "       FROM            Workshifts " + "\r\n";
            queryString = queryString + "                       INNER JOIN (SELECT WorkshiftID, CustomerID FROM SemifinishedProducts WHERE @NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.SemifinishedProductHandover + " AND SemifinishedHandoverID IS NULL AND Approved = 1 GROUP BY WorkshiftID, CustomerID UNION ALL SELECT WorkshiftID, CustomerID FROM SemifinishedItems WHERE @NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.SemifinishedItemHandover + " AND SemifinishedHandoverID IS NULL AND Approved = 1 GROUP BY WorkshiftID, CustomerID) SemifinishedProtems ON Workshifts.WorkshiftID = SemifinishedProtems.WorkshiftID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON SemifinishedProtems.CustomerID = Customers.CustomerID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetSemifinishedHandoverPendingCustomers", queryString);
        }


        private void GetSemifinishedHandoverPendingDetails()
        {
            string queryString;

            queryString = " @NMVNTaskID int, @SemifinishedHandoverID Int, @WorkshiftID Int, @CustomerID Int, @SemifinishedItemIDs varchar(3999), @SemifinishedProductIDs varchar(3999) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.SemifinishedProductHandover + ") " + "\r\n";
            queryString = queryString + "           " + this.GetPendingBUILDSQL(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingBUILDSQL(false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetSemifinishedHandoverPendingDetails", queryString);
        }

        private string GetPendingBUILDSQL(bool productVsItem)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@CustomerID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.GetPendingBUILDSQL(productVsItem, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingBUILDSQL(productVsItem, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPendingBUILDSQL(bool productVsItem, bool isCustomerID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@SemifinishedProductIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.GetPendingBUILDSQL(productVsItem, isCustomerID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetPendingBUILDSQL(productVsItem, isCustomerID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPendingBUILDSQL(bool productVsItem, bool isCustomerID, bool isSemifinishedProductIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@SemifinishedHandoverID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.GetPendingBUILDSQLNew(productVsItem, isCustomerID, isSemifinishedProductIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY ProductionLines.Code, SemifinishedProtems.EntryDate " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.GetPendingBUILDSQLNew(productVsItem, isCustomerID, isSemifinishedProductIDs) + "\r\n";
            queryString = queryString + "                   UNION ALL " + "\r\n";
            queryString = queryString + "                   " + this.GetPendingBUILDSQLEdit(productVsItem, isCustomerID, isSemifinishedProductIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY ProductionLines.Code, SemifinishedProtems.EntryDate " + "\r\n";
            queryString = queryString + "               END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetPendingBUILDSQLNew(bool productVsItem, bool isCustomerID, bool isSemifinishedProductIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      " + (productVsItem ? "NULL AS SemifinishedItemID" : "SemifinishedProtems.SemifinishedItemID") + ", " + (productVsItem ? "SemifinishedProtems.SemifinishedProductID" : "NULL AS SemifinishedProductID") + ", SemifinishedProtems.EntryDate AS SemifinishedProtemEntryDate, SemifinishedProtems.Reference AS SemifinishedProtemReference, SemifinishedProtems.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, " + "\r\n";
            queryString = queryString + "                   SemifinishedProtems.ProductionLineID, ProductionLines.Code AS ProductionLineCode, SemifinishedProtems.CrucialWorkerID, Employees.Name AS CrucialWorkerName, SemifinishedProtems.TotalQuantity AS Quantity, CAST(1 AS bit) AS IsSelected " + "\r\n";


            queryString = queryString + "       FROM        " + this.productVsItemTable(productVsItem) + " SemifinishedProtems" + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON SemifinishedProtems.WorkshiftID = @WorkshiftID AND SemifinishedProtems.Approved = 1 " + (isCustomerID ? " AND SemifinishedProtems.CustomerID = @CustomerID " : "") + " AND SemifinishedProtems.SemifinishedHandoverID IS NULL AND SemifinishedProtems.CustomerID = Customers.CustomerID " + (isSemifinishedProductIDs ? " AND SemifinishedProtems." + this.productVsItemPrimaryKey(productVsItem) + " NOT IN (SELECT Id FROM dbo.SplitToIntList (" + (productVsItem ? "@SemifinishedProductIDs" : "@SemifinishedItemIDs") + "))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN ProductionLines ON SemifinishedProtems.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Employees ON SemifinishedProtems.CrucialWorkerID = Employees.EmployeeID " + "\r\n";

            return queryString;
        }

        private string GetPendingBUILDSQLEdit(bool productVsItem, bool isCustomerID, bool isSemifinishedProductIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      " + (productVsItem ? "NULL AS SemifinishedItemID" : "SemifinishedProtems.SemifinishedItemID") + ", " + (productVsItem ? "SemifinishedProtems.SemifinishedProductID" : "NULL AS SemifinishedProductID") + ", SemifinishedProtems.EntryDate AS SemifinishedProtemEntryDate, SemifinishedProtems.Reference AS SemifinishedProtemReference, SemifinishedProtems.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, " + "\r\n";
            queryString = queryString + "                   SemifinishedProtems.ProductionLineID, ProductionLines.Code AS ProductionLineCode, SemifinishedProtems.CrucialWorkerID, Employees.Name AS CrucialWorkerName, SemifinishedProtems.TotalQuantity AS Quantity, CAST(1 AS bit) AS IsSelected " + "\r\n";


            queryString = queryString + "       FROM        " + this.productVsItemTable(productVsItem) + " SemifinishedProtems" + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON SemifinishedProtems.SemifinishedHandoverID = @SemifinishedHandoverID AND SemifinishedProtems.CustomerID = Customers.CustomerID " + (isSemifinishedProductIDs ? " AND SemifinishedProtems." + this.productVsItemPrimaryKey(productVsItem) + " NOT IN (SELECT Id FROM dbo.SplitToIntList (" + (productVsItem ? "@SemifinishedProductIDs" : "@SemifinishedItemIDs") + "))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN ProductionLines ON SemifinishedProtems.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Employees ON SemifinishedProtems.CrucialWorkerID = Employees.EmployeeID " + "\r\n";

            return queryString;
        }


        private void SemifinishedHandoverSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE @msg NVARCHAR(300) " + "\r\n";

            queryString = queryString + "       IF ((SELECT NMVNTaskID FROM SemifinishedHandovers WHERE SemifinishedHandoverID = @EntityID) = " + (int)GlobalEnums.NmvnTaskID.SemifinishedProductHandover + ") " + "\r\n";
            queryString = queryString + "           " + this.SemifinishedHandoverSaveRelativeSQL(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.SemifinishedHandoverSaveRelativeSQL(false) + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("SemifinishedHandoverSaveRelative", queryString);
        }


        private string SemifinishedHandoverSaveRelativeSQL(bool productVsItem)
        {
            string queryString = "";

            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";

            queryString = queryString + "               UPDATE      SemifinishedProtems" + "\r\n";
            queryString = queryString + "               SET         SemifinishedProtems.SemifinishedHandoverID = SemifinishedHandoverDetails.SemifinishedHandoverID " + "\r\n";
            queryString = queryString + "               FROM        " + this.productVsItemTable(productVsItem) + " SemifinishedProtems INNER JOIN" + "\r\n";
            queryString = queryString + "                           SemifinishedHandoverDetails ON SemifinishedHandoverDetails.SemifinishedHandoverID = @EntityID AND SemifinishedProtems.Approved = 1 AND SemifinishedProtems." + this.productVsItemPrimaryKey(productVsItem) + " = SemifinishedHandoverDetails." + this.productVsItemPrimaryKey(productVsItem) + " " + "\r\n";

            queryString = queryString + "               IF @@ROWCOUNT = (SELECT COUNT(*) FROM SemifinishedHandoverDetails WHERE SemifinishedHandoverID = @EntityID) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       UPDATE      " + this.productVsItemTableDetail(productVsItem) + "\r\n";
            queryString = queryString + "                       SET         SemifinishedHandoverID = @EntityID " + "\r\n";
            queryString = queryString + "                       WHERE       " + this.productVsItemPrimaryKey(productVsItem) + " IN (SELECT " + this.productVsItemPrimaryKey(productVsItem) + " FROM " + this.productVsItemTable(productVsItem) + " WHERE SemifinishedHandoverID = @EntityID) " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       SET         @msg = N'Dữ liệu không tồn tại hoặc chưa duyệt' ; " + "\r\n";
            queryString = queryString + "                       THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";


            queryString = queryString + "               IF ((SELECT Approved FROM SemifinishedHandovers WHERE SemifinishedHandoverID = @EntityID AND Approved = 1) = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       UPDATE      SemifinishedHandovers SET Approved = 0 WHERE SemifinishedHandoverID = @EntityID AND Approved = 1" + "\r\n"; //CLEAR APPROVE BEFORE CALL SemifinishedHandoverToggleApproved
            queryString = queryString + "                       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "                           EXEC        SemifinishedHandoverToggleApproved @EntityID, 1 " + "\r\n";
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
            queryString = queryString + "               SET         SemifinishedHandoverID = NULL " + "\r\n";
            queryString = queryString + "               WHERE       SemifinishedHandoverID = @EntityID " + "\r\n";

            queryString = queryString + "               UPDATE      " + this.productVsItemTableDetail(productVsItem) + "\r\n";
            queryString = queryString + "               SET         SemifinishedHandoverID = NULL " + "\r\n";
            queryString = queryString + "               WHERE       SemifinishedHandoverID = @EntityID " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            return queryString;
        }


        private void SemifinishedHandoverPostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày đóng hàng: ' + CAST(SemifinishedItems.EntryDate AS nvarchar) FROM SemifinishedHandovers INNER JOIN SemifinishedItems ON SemifinishedHandovers.SemifinishedHandoverID = @EntityID AND SemifinishedHandovers.SemifinishedHandoverID = SemifinishedItems.SemifinishedHandoverID AND SemifinishedHandovers.EntryDate < SemifinishedItems.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Ngày đóng hàng: ' + CAST(SemifinishedProducts.EntryDate AS nvarchar) FROM SemifinishedHandovers INNER JOIN SemifinishedProducts ON SemifinishedHandovers.SemifinishedHandoverID = @EntityID AND SemifinishedHandovers.SemifinishedHandoverID = SemifinishedProducts.SemifinishedHandoverID AND SemifinishedHandovers.EntryDate < SemifinishedProducts.EntryDate ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("SemifinishedHandoverPostSaveValidate", queryArray);
        }




        private void SemifinishedHandoverApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = SemifinishedHandoverID FROM SemifinishedHandovers WHERE SemifinishedHandoverID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("SemifinishedHandoverApproved", queryArray);
        }


        private void SemifinishedHandoverEditable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = SemifinishedHandoverID FROM FinishedProductDetails WHERE SemifinishedHandoverID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("SemifinishedHandoverEditable", queryArray);
        }

        private void SemifinishedHandoverToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      SemifinishedHandovers  SET Approved = @Approved, ApprovedDate = GetDate() WHERE SemifinishedHandoverID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          SemifinishedHandoverDetails     SET Approved            = @Approved WHERE SemifinishedHandoverID = @EntityID ; " + "\r\n";

            queryString = queryString + "               IF ((SELECT NMVNTaskID FROM SemifinishedHandovers WHERE SemifinishedHandoverID = @EntityID) = " + (int)GlobalEnums.NmvnTaskID.SemifinishedProductHandover + ") " + "\r\n";
            queryString = queryString + "                   " + this.SemifinishedHandoverToggleApprovedSQL(true) + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n";
            queryString = queryString + "                   " + this.SemifinishedHandoverToggleApprovedSQL(false) + "\r\n";

            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, N'hủy', '')  + N' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("SemifinishedHandoverToggleApproved", queryString);
        }

        private string SemifinishedHandoverToggleApprovedSQL(bool productVsItem)
        {
            string queryString = "";

            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "               UPDATE          " + this.productVsItemTable(productVsItem) + "          SET HandoverApproved    = @Approved WHERE SemifinishedHandoverID = @EntityID ; " + "\r\n";
            queryString = queryString + "               UPDATE          " + this.productVsItemTableDetail(productVsItem) + "    SET HandoverApproved    = @Approved WHERE SemifinishedHandoverID = @EntityID ; " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            return queryString;
        }

        private void SemifinishedHandoverInitReference()
        {
            SimpleInitReference simpleInitReference = new SemifinishedHandoverInitReference("SemifinishedHandovers", "SemifinishedHandoverID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.SemifinishedHandover));
            this.totalSmartPortalEntities.CreateTrigger("SemifinishedHandoverInitReference", simpleInitReference.CreateQuery());
        }






        private void SemifinishedHandoverSheet()
        {
            string queryString;

            queryString = " @SemifinishedHandoverID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SET NOCOUNT ON" + "\r\n";

            queryString = queryString + "       DECLARE     @LocalSemifinishedHandoverID int      SET @LocalSemifinishedHandoverID = @SemifinishedHandoverID" + "\r\n";

            queryString = queryString + "       IF ((SELECT NMVNTaskID FROM SemifinishedHandovers WHERE SemifinishedHandoverID = @SemifinishedHandoverID) = " + (int)GlobalEnums.NmvnTaskID.SemifinishedProductHandover + ") " + "\r\n";
            queryString = queryString + "           " + this.SemifinishedHandoverSheetSQL(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.SemifinishedHandoverSheetSQL(false) + "\r\n";

            queryString = queryString + "       SET NOCOUNT OFF" + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("SemifinishedHandoverSheet", queryString);
        }


        private string SemifinishedHandoverSheetSQL(bool productVsItem)
        {
            string queryString = "";

            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      SemifinishedHandovers.SemifinishedHandoverID, SemifinishedHandovers.EntryDate, SemifinishedHandovers.Reference, Workshifts.EntryDate AS WorkshiftEntryDate, Workshifts.Code AS WorkshiftCode, ProductionLines.Code AS ProductionLineCode, " + "\r\n";
            queryString = queryString + "                   FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, Customers.Name AS CustomerName, SemifinishedProtems.Reference AS SemifinishedProtemReference, SemifinishedProtems.Caption, SemifinishedLeaders.Name AS SemifinishedLeaderName, FinishedLeaders.Name AS FinishedLeaderName, SemifinishedHandoverDetails.Quantity " + "\r\n";

            queryString = queryString + "       FROM        SemifinishedHandovers " + "\r\n";
            queryString = queryString + "                   INNER JOIN SemifinishedHandoverDetails ON SemifinishedHandovers.SemifinishedHandoverID = @LocalSemifinishedHandoverID AND SemifinishedHandovers.SemifinishedHandoverID = SemifinishedHandoverDetails.SemifinishedHandoverID " + "\r\n";
            queryString = queryString + "                   INNER JOIN " + this.productVsItemTable(productVsItem) + " SemifinishedProtems ON SemifinishedHandoverDetails." + this.productVsItemPrimaryKey(productVsItem) + " = SemifinishedProtems." + this.productVsItemPrimaryKey(productVsItem) + "\r\n";
            queryString = queryString + "                   INNER JOIN FirmOrders ON SemifinishedProtems.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON SemifinishedProtems.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN ProductionLines ON SemifinishedProtems.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON SemifinishedProtems.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Employees AS SemifinishedLeaders ON SemifinishedHandovers.SemifinishedLeaderID = SemifinishedLeaders.EmployeeID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Employees AS FinishedLeaders ON SemifinishedHandovers.FinishedLeaderID = FinishedLeaders.EmployeeID" + "\r\n";

            queryString = queryString + "       ORDER BY    ProductionLines.Code, SemifinishedProtems.EntryDate " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            return queryString;
        }
    }
}