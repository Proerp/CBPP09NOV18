using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Commons
{
    public class Customer
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public Customer(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetCustomerIndexes();

            this.CustomerEditable();
            this.CustomerDeletable();
            this.CustomerSaveRelative();
        }


        private void GetCustomerIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      CustomerCategories.CustomerCategoryID, CustomerCategories.Name AS Category, Customers.CustomerID, Customers.Code, Customers.Name, Customers.OfficialName, Customers.VATCode, Customers.BillingAddress, Customers.ShippingAddress, PriceCategories.Code AS PriceCategoryCode, PaymentTerms.Name AS PaymentTermName, Employees.Name AS SalespersonName, Territories.Name AS TerritoryName " + "\r\n";
            queryString = queryString + "       FROM        CustomerCategories " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON CustomerCategories.CustomerCategoryID = Customers.CustomerCategoryID " + "\r\n";
            queryString = queryString + "                   INNER JOIN PriceCategories ON Customers.PriceCategoryID = PriceCategories.PriceCategoryID " + "\r\n";
            queryString = queryString + "                   INNER JOIN PaymentTerms ON Customers.PaymentTermID = PaymentTerms.PaymentTermID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Employees ON Customers.SalespersonID = Employees.EmployeeID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Territories ON Customers.TerritoryID = Territories.TerritoryID " + "\r\n";
            queryString = queryString + "       ORDER BY    Customers.Code DESC " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetCustomerIndexes", queryString);
        }


        private void CustomerSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";

            if (GlobalEnums.DMC)
            {
                queryString = queryString + "               INSERT INTO CustomerWarehouses (CustomerID, WarehouseID, WarehouseTaskID, EntryDate, Remarks, InActive) " + "\r\n";
                queryString = queryString + "               SELECT      @EntityID AS CustomerID, Warehouses.WarehouseID, WarehouseTasks.WarehouseTaskID, GETDATE(), '', 0 FROM Warehouses CROSS JOIN (SELECT IDField AS WarehouseTaskID FROM FNSplitUpIds('" + (int)GlobalEnums.WarehouseTaskID.SalesOrder + "," + (int)GlobalEnums.WarehouseTaskID.DeliveryAdvice + "," + (int)GlobalEnums.WarehouseTaskID.SalesReturn + "," + (int)GlobalEnums.WarehouseTaskID.ProductAdjustment + "')) WarehouseTasks " + "\r\n";
            }
            else
            {
                queryString = queryString + "               INSERT INTO CustomerWarehouses (CustomerID, WarehouseID, WarehouseTaskID, EntryDate, Remarks, InActive) " + "\r\n";
                queryString = queryString + "               SELECT      CustomerID, 1 AS WarehouseID, " + (int)GlobalEnums.WarehouseTaskID.MaterialAdjustment + " AS WarehouseTaskID, GETDATE(), '', 0 FROM Customers WHERE CustomerID = @EntityID " + "\r\n";

                queryString = queryString + "               INSERT INTO CustomerWarehouses (CustomerID, WarehouseID, WarehouseTaskID, EntryDate, Remarks, InActive) " + "\r\n";
                queryString = queryString + "               SELECT      CustomerID, 2 AS WarehouseID, " + (int)GlobalEnums.WarehouseTaskID.ItemAdjustment + " AS WarehouseTaskID, GETDATE(), '', 0 FROM Customers WHERE CustomerID = @EntityID " + "\r\n";

                queryString = queryString + "               INSERT INTO CustomerWarehouses (CustomerID, WarehouseID, WarehouseTaskID, EntryDate, Remarks, InActive) " + "\r\n";
                queryString = queryString + "               SELECT      CustomerID, 3 AS WarehouseID, " + (int)GlobalEnums.WarehouseTaskID.ProductAdjustment + " AS WarehouseTaskID, GETDATE(), '', 0 FROM Customers WHERE CustomerID = @EntityID " + "\r\n";


                //queryString = queryString + "               INSERT INTO CustomerWarehouses (CustomerID, WarehouseID, WarehouseTaskID, EntryDate, Remarks, InActive) " + "\r\n";
                //queryString = queryString + "               SELECT      CustomerID, 46 AS WarehouseID, " + (int)GlobalEnums.NmvnTaskID.SalesOrder + " AS WarehouseTaskID, GETDATE(), '', 0 FROM Customers WHERE CustomerID = @EntityID " + "\r\n";

                //queryString = queryString + "               INSERT INTO CustomerWarehouses (CustomerID, WarehouseID, WarehouseTaskID, EntryDate, Remarks, InActive) " + "\r\n"; //ALL CustomerCategoryID NOT IN (4, 5, 7, 9, 10, 11, 12, 15) DEFINED BY Warehouses.WarehouseCategoryID. HERE WE SET Warehouses.WarehouseCategoryID AND CustomerCategories.WarehouseCategoryID PARALLEL
                //queryString = queryString + "               SELECT      Customers.CustomerID, Warehouses.WarehouseID, " + (int)GlobalEnums.NmvnTaskID.DeliveryAdvice + " AS WarehouseTaskID, GETDATE(), '', 0 FROM Customers INNER JOIN CustomerCategories ON Customers.CustomerID = @EntityID AND CustomerCategories.WarehouseCategoryID NOT IN (4, 5, 7, 9, 10, 11, 12, 15) AND Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID INNER JOIN Warehouses ON CustomerCategories.WarehouseCategoryID = Warehouses.WarehouseCategoryID " + "\r\n";

                //queryString = queryString + "               INSERT INTO CustomerWarehouses (CustomerID, WarehouseID, WarehouseTaskID, EntryDate, Remarks, InActive) " + "\r\n"; //MAP ALL CustomerCategoryID IN (4, 5, 7, 9, 10, 11, 12, 15) TO WarehouseID 82: STPX
                //queryString = queryString + "               SELECT      Customers.CustomerID, 82 AS WarehouseID, " + (int)GlobalEnums.NmvnTaskID.DeliveryAdvice + " AS WarehouseTaskID, GETDATE(), '', 0 FROM Customers INNER JOIN CustomerCategories ON Customers.CustomerID = @EntityID AND CustomerCategories.WarehouseCategoryID IN (4, 5, 7, 9, 10, 11, 12, 15) AND Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";
            }
            queryString = queryString + "           END " + "\r\n";
            
            queryString = queryString + "       ELSE " + "\r\n"; //(@SaveRelativeOption = -1) 
            queryString = queryString + "           DELETE      CustomerWarehouses WHERE CustomerID = @EntityID " + "\r\n";

            //queryString = queryString + "       INSERT INTO     ERmgrVCP.dbo.Customers (CustomerID, Code, Name, OfficialName, VendorCode, VendorCategory, PriceCategoryID, MonetaryAccountID, SalespersonID, CustomerCategoryID, CustomerTypeID, TerritoryID, BillingAddress, ShippingAddress, VATCode, Telephone, Facsimile, AttentionName, AttentionTitle, Birthday, LimitAmount, Remarks, InActive, IsCustomer, IsSupplier, IsFemale) " + "\r\n";
            //queryString = queryString + "       SELECT          CustomerID, Code, Name, OfficialName, VendorCode, VendorCategory, PriceCategoryID, MonetaryAccountID, SalespersonID, CustomerCategoryID, CustomerTypeID, TerritoryID, BillingAddress, ShippingAddress, VATCode, Telephone, Facsimile, AttentionName, AttentionTitle, Birthday, LimitAmount, Remarks, InActive, IsCustomer, IsSupplier, IsFemale     FROM    Customers WHERE CustomerID NOT IN (SELECT CustomerID FROM ERmgrVCP.dbo.Customers) " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("CustomerSaveRelative", queryString);
        }


        private void CustomerEditable()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = CustomerID FROM Customers WHERE CustomerID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            //queryArray[1] = " SELECT TOP 1 @FoundEntity = CustomerID FROM GoodsIssueDetails WHERE CustomerID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("CustomerEditable", queryArray);
        }

        private void CustomerDeletable()
        {
            string[] queryArray = new string[27];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = CustomerID FROM PurchaseOrders WHERE CustomerID = @EntityID OR TransporterID = @EntityID";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = CustomerID FROM PlannedOrders WHERE CustomerID = @EntityID";
            queryArray[2] = " SELECT TOP 1 @FoundEntity = CustomerID FROM DeliveryAdvices WHERE CustomerID = @EntityID OR ReceiverID = @EntityID";
            queryArray[3] = " SELECT TOP 1 @FoundEntity = CustomerID FROM WarehouseAdjustments WHERE CustomerID = @EntityID";
            queryArray[4] = " SELECT TOP 1 @FoundEntity = CustomerID FROM AccountInvoices WHERE CustomerID = @EntityID OR ReceiverID = @EntityID OR ConsumerID = @EntityID";
            queryArray[5] = " SELECT TOP 1 @FoundEntity = CustomerID FROM Boms WHERE CustomerID = @EntityID";
            queryArray[6] = " SELECT TOP 1 @FoundEntity = CustomerID FROM CreditNotes WHERE CustomerID = @EntityID";
            queryArray[7] = " SELECT TOP 1 @FoundEntity = CustomerID FROM FinishedHandovers WHERE CustomerID = @EntityID";
            queryArray[8] = " SELECT TOP 1 @FoundEntity = CustomerID FROM FinishedProducts WHERE CustomerID = @EntityID";
            queryArray[9] = " SELECT TOP 1 @FoundEntity = CustomerID FROM FirmOrders WHERE CustomerID = @EntityID";
            queryArray[10] = " SELECT TOP 1 @FoundEntity = CustomerID FROM GoodsArrivals WHERE CustomerID = @EntityID OR TransporterID = @EntityID";
            queryArray[11] = " SELECT TOP 1 @FoundEntity = ReceiverID FROM GoodsDeliveries WHERE ReceiverID = @EntityID";
            queryArray[12] = " SELECT TOP 1 @FoundEntity = CustomerID FROM GoodsDeliveryDetails WHERE CustomerID = @EntityID OR ReceiverID = @EntityID";
            queryArray[13] = " SELECT TOP 1 @FoundEntity = CustomerID FROM GoodsIssues WHERE CustomerID = @EntityID OR ReceiverID = @EntityID";
            queryArray[14] = " SELECT TOP 1 @FoundEntity = CustomerID FROM GoodsReceipts WHERE CustomerID = @EntityID";
            queryArray[15] = " SELECT TOP 1 @FoundEntity = CustomerID FROM HandlingUnits WHERE CustomerID = @EntityID OR ReceiverID = @EntityID";
            queryArray[16] = " SELECT TOP 1 @FoundEntity = CustomerID FROM MaterialIssues WHERE CustomerID = @EntityID";
            queryArray[17] = " SELECT TOP 1 @FoundEntity = CustomerID FROM ProductionOrders WHERE CustomerID = @EntityID";
            queryArray[18] = " SELECT TOP 1 @FoundEntity = CustomerID FROM PromotionCustomers WHERE CustomerID = @EntityID";
            queryArray[19] = " SELECT TOP 1 @FoundEntity = CustomerID FROM PurchaseRequisitions WHERE CustomerID = @EntityID";
            queryArray[20] = " SELECT TOP 1 @FoundEntity = CustomerID FROM Quotations WHERE CustomerID = @EntityID OR ReceiverID = @EntityID";
            queryArray[21] = " SELECT TOP 1 @FoundEntity = CustomerID FROM Receipts WHERE CustomerID = @EntityID";
            queryArray[22] = " SELECT TOP 1 @FoundEntity = CustomerID FROM SalesOrders WHERE CustomerID = @EntityID OR ReceiverID = @EntityID";
            queryArray[23] = " SELECT TOP 1 @FoundEntity = CustomerID FROM SalesReturns WHERE CustomerID = @EntityID OR ReceiverID = @EntityID";
            queryArray[24] = " SELECT TOP 1 @FoundEntity = CustomerID FROM SemifinishedHandovers WHERE CustomerID = @EntityID";
            queryArray[25] = " SELECT TOP 1 @FoundEntity = CustomerID FROM SemifinishedItems WHERE CustomerID = @EntityID";
            queryArray[26] = " SELECT TOP 1 @FoundEntity = CustomerID FROM SemifinishedProducts WHERE CustomerID = @EntityID"; 

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("CustomerDeletable", queryArray);
        }

    }
}
