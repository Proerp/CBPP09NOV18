using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Purchases
{
    public class GoodsArrival
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public GoodsArrival(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetGoodsArrivalIndexes();
            this.GetGoodsArrivalViewDetails();

            this.GetGoodsArrivalPendingCustomers();
            this.GetGoodsArrivalPendingPurchaseOrders();
            this.GetGoodsArrivalPendingPurchaseOrderDetails();

            this.GoodsArrivalSaveRelative();
            this.GoodsArrivalPostSaveValidate();

            this.GoodsArrivalApproved();
            this.GoodsArrivalEditable();
            this.GoodsArrivalVoidable();

            this.GoodsArrivalToggleApproved();
            this.GoodsArrivalToggleVoid();
            this.GoodsArrivalToggleVoidDetail();

            this.GoodsArrivalInitReference();
        }


        private void GetGoodsArrivalIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime, @PendingOnly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF  (@PendingOnly = 1) " + "\r\n";
            queryString = queryString + "           " + this.GetGoodsArrivalIndexSQL(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetGoodsArrivalIndexSQL(false) + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsArrivalIndexes", queryString);
        }

        private string GetGoodsArrivalIndexSQL(bool pendingOnly)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      GoodsArrivals.GoodsArrivalID, CAST(GoodsArrivals.EntryDate AS DATE) AS EntryDate, GoodsArrivals.Reference, GoodsArrivals.Code, Locations.Code AS LocationCode, Customers.Name AS CustomerName, Transporters.Name AS TransporterName, GoodsArrivals.Description, GoodsArrivals.TotalQuantity, GoodsArrivals.TotalQuantityReceipted, GoodsArrivals.Approved " + "\r\n";
            queryString = queryString + "       FROM        GoodsArrivals " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON " + (pendingOnly ? "GoodsArrivals.GoodsArrivalID IN (SELECT GoodsArrivalID FROM GoodsArrivalDetails WHERE ROUND(Quantity - QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") > 0)" : "GoodsArrivals.EntryDate >= @FromDate AND GoodsArrivals.EntryDate <= @ToDate") + " AND GoodsArrivals.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.GoodsArrival + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = GoodsArrivals.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON GoodsArrivals.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers Transporters ON GoodsArrivals.TransporterID = Transporters.CustomerID " + "\r\n";

            return queryString;
        }

        #region X


        private void GetGoodsArrivalViewDetails()
        {
            string queryString;
            SqlProgrammability.Inventories.Inventories inventories = new Inventories.Inventories(this.totalSmartPortalEntities);

            queryString = " @GoodsArrivalID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @EntryDate DateTime       DECLARE @LocationID varchar(35)       DECLARE @CustomerID int         DECLARE @WarehouseIDList varchar(555)         DECLARE @CommodityIDList varchar(3999)        DECLARE @WarehouseClassList varchar(555) " + "\r\n";
            queryString = queryString + "       SELECT      @EntryDate = EntryDate, @LocationID = LocationID, @CustomerID = CustomerID FROM GoodsArrivals WHERE GoodsArrivalID = @GoodsArrivalID " + "\r\n";
            queryString = queryString + "       IF          @EntryDate IS NULL          SET @EntryDate = CONVERT(Datetime, '31/12/2000', 103)" + "\r\n";
            queryString = queryString + "       SELECT      @WarehouseIDList = STUFF((SELECT ',' + CAST(WarehouseID AS varchar)  FROM GoodsArrivalDetails WHERE GoodsArrivalID = @GoodsArrivalID FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "       SELECT      @CommodityIDList = STUFF((SELECT ',' + CAST(CommodityID AS varchar)  FROM GoodsArrivalDetails WHERE GoodsArrivalID = @GoodsArrivalID FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "       SELECT      @WarehouseClassList = STUFF((SELECT ',' + CAST(WarehouseClassID AS varchar) FROM Warehouses WHERE WarehouseID IN (SELECT * FROM FNSplitUpIds(@WarehouseIDList))        FOR XML PATH('')) ,1,1,'') " + "\r\n";

            queryString = queryString + "       " + inventories.GET_WarehouseJournal_BUILD_SQL("@CommoditiesBalance", "@EntryDate", "@EntryDate", "@WarehouseIDList", "@CommodityIDList", "0", "0", "@WarehouseClassList", null) + "\r\n";

            queryString = queryString + "       SELECT      GoodsArrivalDetails.GoodsArrivalDetailID, GoodsArrivalDetails.GoodsArrivalID, GoodsArrivalDetails.PurchaseOrderID, GoodsArrivalDetails.PurchaseOrderDetailID, PurchaseOrders.Reference AS PurchaseOrderReference, PurchaseOrders.Code AS PurchaseOrderCode, PurchaseOrders.EntryDate AS PurchaseOrderEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, GoodsArrivalDetails.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, VoidTypes.VoidTypeID, VoidTypes.Code AS VoidTypeCode, VoidTypes.Name AS VoidTypeName, VoidTypes.VoidClassID, GoodsArrivalDetails.CalculatingTypeID, " + "\r\n";
            queryString = queryString + "                   ROUND(ISNULL(CommoditiesBalance.QuantityBalance, 0) + GoodsArrivalDetails.QuantityReceipted + GoodsArrivalDetails.FreeQuantityReceipted + CASE WHEN GoodsArrivalDetails.Approved = 1 AND GoodsArrivalDetails.InActive = 0 AND GoodsArrivalDetails.InActivePartial = 0 AND GoodsArrivalDetails.InActiveIssue = 0 THEN GoodsArrivalDetails.Quantity + GoodsArrivalDetails.FreeQuantity - GoodsArrivalDetails.QuantityReceipted - GoodsArrivalDetails.FreeQuantityReceipted ELSE 0 END, 0) AS QuantityAvailable, " + "\r\n";
            queryString = queryString + "                   ROUND(ISNULL(PurchaseOrderDetails.Quantity, 0) - ISNULL(PurchaseOrderDetails.QuantityAdvice, 0) + GoodsArrivalDetails.Quantity, 0) AS QuantityRemains, ROUND(ISNULL(PurchaseOrderDetails.FreeQuantity, 0) - ISNULL(PurchaseOrderDetails.FreeQuantityAdvice, 0) + GoodsArrivalDetails.FreeQuantity, 0) AS FreeQuantityRemains, " + "\r\n";
            queryString = queryString + "                   GoodsArrivalDetails.Quantity, GoodsArrivalDetails.ControlFreeQuantity, GoodsArrivalDetails.FreeQuantity, GoodsArrivalDetails.ListedPrice, GoodsArrivalDetails.DiscountPercent, GoodsArrivalDetails.UnitPrice, GoodsArrivalDetails.TradeDiscountRate, GoodsArrivalDetails.VATPercent, GoodsArrivalDetails.ListedGrossPrice, GoodsArrivalDetails.GrossPrice, GoodsArrivalDetails.ListedAmount, GoodsArrivalDetails.Amount, GoodsArrivalDetails.ListedVATAmount, GoodsArrivalDetails.VATAmount, GoodsArrivalDetails.ListedGrossAmount, GoodsArrivalDetails.GrossAmount, GoodsArrivalDetails.IsBonus, GoodsArrivalDetails.InActivePartial, GoodsArrivalDetails.InActivePartialDate, GoodsArrivalDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        GoodsArrivalDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON GoodsArrivalDetails.GoodsArrivalID = @GoodsArrivalID AND GoodsArrivalDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Warehouses ON GoodsArrivalDetails.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN PurchaseOrderDetails ON GoodsArrivalDetails.PurchaseOrderDetailID = PurchaseOrderDetails.PurchaseOrderDetailID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN PurchaseOrders ON PurchaseOrderDetails.PurchaseOrderID = PurchaseOrders.PurchaseOrderID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN VoidTypes ON GoodsArrivalDetails.VoidTypeID = VoidTypes.VoidTypeID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN @CommoditiesBalance CommoditiesBalance ON GoodsArrivalDetails.WarehouseID = CommoditiesBalance.WarehouseID AND GoodsArrivalDetails.CommodityID = CommoditiesBalance.CommodityID " + "\r\n";

            queryString = queryString + "       ORDER BY    Commodities.CommodityTypeID, GoodsArrivalDetails.GoodsArrivalID, GoodsArrivalDetails.GoodsArrivalDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsArrivalViewDetails", queryString);
        }





        #region Y

        private void GetGoodsArrivalPendingPurchaseOrders()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          PurchaseOrders.PurchaseOrderID, PurchaseOrders.Reference AS PurchaseOrderReference, PurchaseOrders.Code AS PurchaseOrderCode, PurchaseOrders.EntryDate AS PurchaseOrderEntryDate, PurchaseOrders.PriceCategoryID, PriceCategories.Code AS PriceCategoryCode, PurchaseOrders.WarehouseID, Warehouses.Code AS WarehouseCode, Warehouses.Name AS WarehouseName, PurchaseOrders.TradePromotionID, TradePromotions.Specs AS TradePromotionSpecs, PurchaseOrders.TradeDiscountRate, PurchaseOrders.VATPercent, PurchaseOrders.PaymentTermID, PurchaseOrders.SalespersonID, Employees.Name AS SalespersonName, PurchaseOrders.Description, PurchaseOrders.Remarks, " + "\r\n";
            queryString = queryString + "                       PurchaseOrders.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.OfficialName AS CustomerOfficialName, Customers.Birthday AS CustomerBirthday, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.BillingAddress AS CustomerBillingAddress, Customers.ShippingAddress AS CustomerShippingAddress, Customers.TerritoryID AS CustomerTerritoryID, CustomerEntireTerritories.EntireName AS CustomerEntireTerritoryEntireName, Customers.SalespersonID AS CustomerSalespersonID, N'' AS CustomerSalespersonName, Customers.PriceCategoryID AS CustomerPriceCategoryID, N'' AS CustomerPriceCategoryCode, " + "\r\n";
            queryString = queryString + "                       PurchaseOrders.TransporterID, Transporters.Code AS TransporterCode, Transporters.Name AS TransporterName, Transporters.OfficialName AS TransporterOfficialName, Transporters.Birthday AS TransporterBirthday, Transporters.VATCode AS TransporterVATCode, Transporters.AttentionName AS TransporterAttentionName, Transporters.Telephone AS TransporterTelephone, Transporters.BillingAddress AS TransporterBillingAddress, Transporters.ShippingAddress AS TransporterShippingAddress, Transporters.TerritoryID AS TransporterTerritoryID, TransporterEntireTerritories.EntireName AS TransporterEntireTerritoryEntireName, Transporters.SalespersonID AS TransporterSalespersonID, N'' AS TransporterSalespersonName, Transporters.PriceCategoryID AS TransporterPriceCategoryID, N'' AS TransporterPriceCategoryCode, PurchaseOrders.ShippingAddress, PurchaseOrders.Addressee " + "\r\n";

            queryString = queryString + "       FROM            PurchaseOrders " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON PurchaseOrders.PurchaseOrderID IN (SELECT PurchaseOrderID FROM PurchaseOrderDetails WHERE LocationID = @LocationID AND Approved = 1 AND InActive = 0 AND InActivePartial = 0  AND ROUND(Quantity + FreeQuantity - QuantityAdvice - FreeQuantityAdvice, " + (int)GlobalEnums.rndQuantity + ") > 0) AND PurchaseOrders.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers Transporters ON PurchaseOrders.TransporterID = Transporters.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories TransporterEntireTerritories ON Transporters.TerritoryID = TransporterEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN PriceCategories ON PurchaseOrders.PriceCategoryID = PriceCategories.PriceCategoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Warehouses ON PurchaseOrders.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Employees ON PurchaseOrders.SalespersonID = Employees.EmployeeID " + "\r\n";
            queryString = queryString + "                       LEFT  JOIN Promotions AS TradePromotions ON PurchaseOrders.TradePromotionID = TradePromotions.PromotionID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsArrivalPendingPurchaseOrders", queryString);
        }

        private void GetGoodsArrivalPendingCustomers()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          CustomerTransporterPENDING.PriceCategoryID, PriceCategories.Code AS PriceCategoryCode, CustomerTransporterPENDING.WarehouseID, Warehouses.Code AS WarehouseCode, Warehouses.Name AS WarehouseName, CustomerTransporterPENDING.TradePromotionID, TradePromotions.Specs AS TradePromotionSpecs, CustomerTransporterPENDING.TradeDiscountRate, CustomerTransporterPENDING.VATPercent, CASE WHEN Customers.PaymentTermID <> -1 THEN Customers.PaymentTermID ELSE CustomerCategories.PaymentTermID END AS PaymentTermID, Customers.SalespersonID, Employees.Name AS SalespersonName, N'' AS Description, N'' AS Remarks, " + "\r\n";
            queryString = queryString + "                       CustomerTransporterPENDING.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.OfficialName AS CustomerOfficialName, Customers.Birthday AS CustomerBirthday, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.BillingAddress AS CustomerBillingAddress, Customers.ShippingAddress AS CustomerShippingAddress, Customers.TerritoryID AS CustomerTerritoryID, CustomerEntireTerritories.EntireName AS CustomerEntireTerritoryEntireName, Customers.SalespersonID AS CustomerSalespersonID, N'' AS CustomerSalespersonName, Customers.PriceCategoryID AS CustomerPriceCategoryID, N'' AS CustomerPriceCategoryCode, " + "\r\n";
            queryString = queryString + "                       CustomerTransporterPENDING.TransporterID, Transporters.Code AS TransporterCode, Transporters.Name AS TransporterName, Transporters.OfficialName AS TransporterOfficialName, Transporters.Birthday AS TransporterBirthday, Transporters.VATCode AS TransporterVATCode, Transporters.AttentionName AS TransporterAttentionName, Transporters.Telephone AS TransporterTelephone, Transporters.BillingAddress AS TransporterBillingAddress, Transporters.ShippingAddress AS TransporterShippingAddress, Transporters.TerritoryID AS TransporterTerritoryID, TransporterEntireTerritories.EntireName AS TransporterEntireTerritoryEntireName, Transporters.SalespersonID AS TransporterSalespersonID, N'' AS TransporterSalespersonName, Transporters.PriceCategoryID AS TransporterPriceCategoryID, N'' AS TransporterPriceCategoryCode, CustomerTransporterPENDING.ShippingAddress, CustomerTransporterPENDING.Addressee " + "\r\n";

            queryString = queryString + "       FROM           (SELECT DISTINCT CustomerID, TransporterID, PriceCategoryID, WarehouseID, TradePromotionID, TradeDiscountRate, " + (GlobalEnums.VATbyRow ? "0.0 AS" : "") + " VATPercent, ShippingAddress, Addressee FROM PurchaseOrders WHERE PurchaseOrderID IN (SELECT PurchaseOrderID FROM PurchaseOrderDetails WHERE LocationID = @LocationID AND Approved = 1 AND InActive = 0 AND InActivePartial = 0  AND ROUND(Quantity + FreeQuantity - QuantityAdvice - FreeQuantityAdvice, " + (int)GlobalEnums.rndQuantity + ") > 0)) CustomerTransporterPENDING " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON CustomerTransporterPENDING.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers Transporters ON CustomerTransporterPENDING.TransporterID = Transporters.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories TransporterEntireTerritories ON Transporters.TerritoryID = TransporterEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN PriceCategories ON CustomerTransporterPENDING.PriceCategoryID = PriceCategories.PriceCategoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Warehouses ON CustomerTransporterPENDING.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Employees ON Customers.SalespersonID = Employees.EmployeeID " + "\r\n";
            queryString = queryString + "                       LEFT  JOIN Promotions AS TradePromotions ON CustomerTransporterPENDING.TradePromotionID = TradePromotions.PromotionID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsArrivalPendingCustomers", queryString);
        }



        private void GetGoodsArrivalPendingPurchaseOrderDetails()
        {
            string queryString;

            SqlProgrammability.Inventories.Inventories inventories = new SqlProgrammability.Inventories.Inventories(this.totalSmartPortalEntities);

            queryString = " @LocationID Int, @GoodsArrivalID Int, @PurchaseOrderID Int, @CustomerID Int, @TransporterID Int, @PriceCategoryID Int, @WarehouseID Int, @ShippingAddress nvarchar(200), @Addressee nvarchar(200), @TradePromotionID int, @VATPercent decimal(18, 2), @EntryDate DateTime, @PurchaseOrderDetailIDs varchar(3999), @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE @WarehouseIDList varchar(35)         DECLARE @CommodityIDList varchar(3999)             DECLARE @WarehouseClassList varchar(555) " + "\r\n";

            queryString = queryString + "       IF (@PurchaseOrderID > 0) ";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               SELECT      @WarehouseIDList = STUFF((SELECT ',' + CAST(WarehouseID AS varchar)  FROM (SELECT DISTINCT WarehouseID FROM PurchaseOrderDetails WHERE PurchaseOrderID = @PurchaseOrderID) DistinctWarehouses FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "               SELECT      @CommodityIDList = STUFF((SELECT ',' + CAST(CommodityID AS varchar)  FROM PurchaseOrderDetails WHERE PurchaseOrderID = @PurchaseOrderID FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE ";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @WarehouseCommodities TABLE (WarehouseID int NOT NULL, CommodityID int NOT NULL) " + "\r\n";
            queryString = queryString + "               INSERT INTO @WarehouseCommodities       SELECT      PurchaseOrderDetails.WarehouseID, PurchaseOrderDetails.CommodityID                        FROM    PurchaseOrderDetails INNER JOIN PurchaseOrders ON PurchaseOrderDetails.LocationID = @LocationID AND PurchaseOrderDetails.CustomerID = @CustomerID AND PurchaseOrderDetails.TransporterID = @TransporterID AND PurchaseOrders.PriceCategoryID = @PriceCategoryID AND PurchaseOrders.WarehouseID = @WarehouseID AND PurchaseOrders.ShippingAddress = @ShippingAddress AND PurchaseOrders.Addressee = @Addressee AND (PurchaseOrders.TradePromotionID = @TradePromotionID OR (PurchaseOrders.TradePromotionID IS NULL AND @TradePromotionID IS NULL)) " + (GlobalEnums.VATbyRow ? "" : "AND PurchaseOrders.VATPercent = @VATPercent") + " AND PurchaseOrderDetails.Approved = 1 AND PurchaseOrderDetails.InActive = 0 AND PurchaseOrderDetails.InActivePartial = 0 AND (ROUND(PurchaseOrderDetails.Quantity - PurchaseOrderDetails.QuantityAdvice, " + (int)GlobalEnums.rndQuantity + ") > 0 OR ROUND(PurchaseOrderDetails.FreeQuantity - PurchaseOrderDetails.FreeQuantityAdvice, " + (int)GlobalEnums.rndQuantity + ") > 0) AND PurchaseOrderDetails.PurchaseOrderID = PurchaseOrders.PurchaseOrderID " + "\r\n";
            queryString = queryString + "               INSERT INTO @WarehouseCommodities       SELECT      GoodsArrivalDetails.WarehouseID, GoodsArrivalDetails.CommodityID                FROM    GoodsArrivalDetails WHERE GoodsArrivalID = @GoodsArrivalID ";

            queryString = queryString + "               SELECT      @WarehouseIDList = STUFF((SELECT ',' + CAST(WarehouseID AS varchar)  FROM (SELECT DISTINCT WarehouseID FROM @WarehouseCommodities) PendingWarehouses FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "               SELECT      @CommodityIDList = STUFF((SELECT ',' + CAST(CommodityID AS varchar)  FROM (SELECT DISTINCT CommodityID FROM @WarehouseCommodities) PendingCommodities FOR XML PATH('')) ,1,1,'') " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       SELECT      @WarehouseClassList = STUFF((SELECT ',' + CAST(WarehouseClassID AS varchar) FROM Warehouses WHERE WarehouseID IN (SELECT * FROM FNSplitUpIds(@WarehouseIDList))        FOR XML PATH('')) ,1,1,'') " + "\r\n";

            queryString = queryString + "       " + inventories.GET_WarehouseJournal_BUILD_SQL("@CommoditiesBalance", "@EntryDate", "@EntryDate", "@WarehouseIDList", "@CommodityIDList", "0", "0", "@WarehouseClassList", null) + "\r\n";


            queryString = queryString + "       IF  (@PurchaseOrderID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPurchaseOrder(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPurchaseOrder(false) + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsArrivalPendingPurchaseOrderDetails", queryString);
        }

        private string BuildSQLPurchaseOrder(bool isPurchaseOrderID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@PurchaseOrderDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPurchaseOrderPurchaseOrderDetailIDs(isPurchaseOrderID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPurchaseOrderPurchaseOrderDetailIDs(isPurchaseOrderID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLPurchaseOrderPurchaseOrderDetailIDs(bool isPurchaseOrderID, bool isPurchaseOrderDetailIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@GoodsArrivalID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLNew(isPurchaseOrderID, isPurchaseOrderDetailIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY PurchaseOrders.EntryDate, PurchaseOrders.PurchaseOrderID, PurchaseOrderDetails.PurchaseOrderDetailID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLEdit(isPurchaseOrderID, isPurchaseOrderDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY PurchaseOrders.EntryDate, PurchaseOrders.PurchaseOrderID, PurchaseOrderDetails.PurchaseOrderDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLNew(isPurchaseOrderID, isPurchaseOrderDetailIDs) + " WHERE PurchaseOrderDetails.PurchaseOrderDetailID NOT IN (SELECT PurchaseOrderDetailID FROM GoodsArrivalDetails WHERE GoodsArrivalID = @GoodsArrivalID) " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLEdit(isPurchaseOrderID, isPurchaseOrderDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY PurchaseOrders.EntryDate, PurchaseOrders.PurchaseOrderID, PurchaseOrderDetails.PurchaseOrderDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLNew(bool isPurchaseOrderID, bool isPurchaseOrderDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      PurchaseOrders.PurchaseOrderID, PurchaseOrderDetails.PurchaseOrderDetailID, PurchaseOrders.Reference AS PurchaseOrderReference, PurchaseOrders.Code AS PurchaseOrderCode, PurchaseOrders.EntryDate AS PurchaseOrderEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, PurchaseOrderDetails.CalculatingTypeID, PurchaseOrderDetails.VATbyRow, " + "\r\n";
            queryString = queryString + "                   ISNULL(CommoditiesBalance.QuantityBalance, 0) AS QuantityAvailable, ROUND(PurchaseOrderDetails.Quantity - PurchaseOrderDetails.QuantityAdvice, 0) AS QuantityRemains, ROUND(PurchaseOrderDetails.FreeQuantity - PurchaseOrderDetails.FreeQuantityAdvice, 0) AS FreeQuantityRemains, " + "\r\n";
            queryString = queryString + "                   0 AS Quantity, PurchaseOrderDetails.ControlFreeQuantity, 0 AS FreeQuantity, PurchaseOrderDetails.ListedPrice, PurchaseOrderDetails.DiscountPercent, PurchaseOrderDetails.UnitPrice, PurchaseOrderDetails.TradeDiscountRate, PurchaseOrderDetails.VATPercent, PurchaseOrderDetails.ListedGrossPrice, PurchaseOrderDetails.GrossPrice, 0 AS ListedAmount, 0 AS Amount, 0 AS ListedVATAmount, 0 AS VATAmount, 0 AS ListedGrossAmount, 0 AS GrossAmount, PurchaseOrderDetails.IsBonus, PurchaseOrders.Description, PurchaseOrderDetails.Remarks, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        PurchaseOrders " + "\r\n";
            queryString = queryString + "                   INNER JOIN PurchaseOrderDetails ON " + (isPurchaseOrderID ? " PurchaseOrders.PurchaseOrderID = @PurchaseOrderID " : "PurchaseOrders.LocationID = @LocationID AND PurchaseOrders.CustomerID = @CustomerID AND PurchaseOrders.TransporterID = @TransporterID AND PurchaseOrders.PriceCategoryID = @PriceCategoryID AND PurchaseOrders.WarehouseID = @WarehouseID AND PurchaseOrders.ShippingAddress = @ShippingAddress AND PurchaseOrders.Addressee = @Addressee AND (PurchaseOrders.TradePromotionID = @TradePromotionID OR (PurchaseOrders.TradePromotionID IS NULL AND @TradePromotionID IS NULL)) " + (GlobalEnums.VATbyRow ? "" : "AND PurchaseOrders.VATPercent = @VATPercent")) + " AND PurchaseOrderDetails.Approved = 1 AND PurchaseOrderDetails.InActive = 0 AND PurchaseOrderDetails.InActivePartial = 0 AND ROUND(PurchaseOrderDetails.Quantity + PurchaseOrderDetails.FreeQuantity - PurchaseOrderDetails.QuantityAdvice - PurchaseOrderDetails.FreeQuantityAdvice, " + (int)GlobalEnums.rndQuantity + ") > 0 AND PurchaseOrders.PurchaseOrderID = PurchaseOrderDetails.PurchaseOrderID" + (isPurchaseOrderDetailIDs ? " AND PurchaseOrderDetails.PurchaseOrderDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@PurchaseOrderDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON PurchaseOrderDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Warehouses ON PurchaseOrderDetails.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN @CommoditiesBalance CommoditiesBalance ON PurchaseOrderDetails.WarehouseID = CommoditiesBalance.WarehouseID AND PurchaseOrderDetails.CommodityID = CommoditiesBalance.CommodityID " + "\r\n";

            return queryString;
        }

        private string BuildSQLEdit(bool isPurchaseOrderID, bool isPurchaseOrderDetailIDs)
        {
            string queryString = "";
            //NO NEED TO UNDO QuantityAvailable -THE WAREHOUSE BALANCE- FOR THIS EDIT QUERY: BECAUSE: THIS STORED PROCEDURE ONLY BE CALLED WHEN Approved = 0 => BECAUSE OF THIS (HAVE NOT UPPROVED YET): THIS DELIVERYADVICE QUANTITY DOES NOT EFFECT THE WAREHOUSE BALANCE
            queryString = queryString + "       SELECT      PurchaseOrders.PurchaseOrderID, PurchaseOrderDetails.PurchaseOrderDetailID, PurchaseOrders.Reference AS PurchaseOrderReference, PurchaseOrders.Code AS PurchaseOrderCode, PurchaseOrders.EntryDate AS PurchaseOrderEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, PurchaseOrderDetails.CalculatingTypeID, PurchaseOrderDetails.VATbyRow, " + "\r\n";
            queryString = queryString + "                   ISNULL(CommoditiesBalance.QuantityBalance, 0) AS QuantityAvailable, ROUND(PurchaseOrderDetails.Quantity - PurchaseOrderDetails.QuantityAdvice + GoodsArrivalDetails.Quantity, 0) AS QuantityRemains, ROUND(PurchaseOrderDetails.FreeQuantity - PurchaseOrderDetails.FreeQuantityAdvice + GoodsArrivalDetails.FreeQuantity, 0) AS FreeQuantityRemains, " + "\r\n";
            queryString = queryString + "                   0 AS Quantity, PurchaseOrderDetails.ControlFreeQuantity, 0 AS FreeQuantity, PurchaseOrderDetails.ListedPrice, PurchaseOrderDetails.DiscountPercent, PurchaseOrderDetails.UnitPrice, PurchaseOrderDetails.TradeDiscountRate, PurchaseOrderDetails.VATPercent, PurchaseOrderDetails.ListedGrossPrice, PurchaseOrderDetails.GrossPrice, 0 AS ListedAmount, 0 AS Amount, 0 AS ListedVATAmount, 0 AS VATAmount, 0 AS ListedGrossAmount, 0 AS GrossAmount, PurchaseOrderDetails.IsBonus, PurchaseOrders.Description, PurchaseOrderDetails.Remarks, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        PurchaseOrderDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsArrivalDetails ON GoodsArrivalDetails.GoodsArrivalID = @GoodsArrivalID AND PurchaseOrderDetails.PurchaseOrderDetailID = GoodsArrivalDetails.PurchaseOrderDetailID" + (isPurchaseOrderDetailIDs ? " AND PurchaseOrderDetails.PurchaseOrderDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@PurchaseOrderDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON PurchaseOrderDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Warehouses ON PurchaseOrderDetails.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                   INNER JOIN PurchaseOrders ON PurchaseOrderDetails.PurchaseOrderID = PurchaseOrders.PurchaseOrderID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN @CommoditiesBalance CommoditiesBalance ON PurchaseOrderDetails.WarehouseID = CommoditiesBalance.WarehouseID AND PurchaseOrderDetails.CommodityID = CommoditiesBalance.CommodityID " + "\r\n";

            return queryString;
        }

        #endregion Y




        private void GoodsArrivalSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   IF (SELECT HasPurchaseOrder FROM GoodsArrivals WHERE GoodsArrivalID = @EntityID) = 1 " + "\r\n";
            queryString = queryString + "       BEGIN " + "\r\n";
            queryString = queryString + "           UPDATE          PurchaseOrderDetails " + "\r\n";
            queryString = queryString + "           SET             PurchaseOrderDetails.QuantityAdvice = ROUND(PurchaseOrderDetails.QuantityAdvice + GoodsArrivalDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + "), PurchaseOrderDetails.FreeQuantityAdvice = ROUND(PurchaseOrderDetails.FreeQuantityAdvice + GoodsArrivalDetails.FreeQuantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "           FROM            GoodsArrivalDetails " + "\r\n";
            queryString = queryString + "                           INNER JOIN PurchaseOrderDetails ON ((PurchaseOrderDetails.Approved = 1 AND PurchaseOrderDetails.InActive = 0 AND PurchaseOrderDetails.InActivePartial = 0) OR @SaveRelativeOption = -1) AND GoodsArrivalDetails.GoodsArrivalID = @EntityID AND GoodsArrivalDetails.PurchaseOrderDetailID = PurchaseOrderDetails.PurchaseOrderDetailID " + "\r\n";

            queryString = queryString + "           IF @@ROWCOUNT <> (SELECT COUNT(*) FROM GoodsArrivalDetails WHERE GoodsArrivalID = @EntityID) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   DECLARE     @msg NVARCHAR(300) = N'Đơn hàng không tồn tại, chưa duyệt hoặc đã hủy' ; " + "\r\n";
            queryString = queryString + "                   THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GoodsArrivalSaveRelative", queryString);

        }

        private void GoodsArrivalPostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày đặt hàng: ' + CAST(PurchaseOrders.EntryDate AS nvarchar) FROM GoodsArrivalDetails INNER JOIN PurchaseOrders ON GoodsArrivalDetails.GoodsArrivalID = @EntityID AND GoodsArrivalDetails.PurchaseOrderID = PurchaseOrders.PurchaseOrderID AND GoodsArrivalDetails.EntryDate < PurchaseOrders.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Số lượng xuất vượt quá số lượng đặt hàng: ' + CAST(ROUND(Quantity - QuantityAdvice, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) + ' OR free quantity: ' + CAST(ROUND(FreeQuantity - FreeQuantityAdvice, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM PurchaseOrderDetails WHERE (ROUND(Quantity - QuantityAdvice, " + (int)GlobalEnums.rndQuantity + ") < 0) OR (ROUND(FreeQuantity - FreeQuantityAdvice, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("GoodsArrivalPostSaveValidate", queryArray);
        }




        private void GoodsArrivalApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = GoodsArrivalID FROM GoodsArrivals WHERE GoodsArrivalID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("GoodsArrivalApproved", queryArray);
        }


        private void GoodsArrivalEditable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = GoodsArrivalID FROM GoodsArrivals WHERE GoodsArrivalID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            queryArray[1] = " SELECT TOP 1 @FoundEntity = GoodsArrivalID FROM GoodsReceiptDetails WHERE GoodsArrivalID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("GoodsArrivalEditable", queryArray);
        }

        private void GoodsArrivalVoidable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = GoodsArrivalID FROM GoodsArrivals WHERE GoodsArrivalID = @EntityID AND Approved = 0"; //Must approve in order to allow void
            queryArray[1] = " SELECT TOP 1 @FoundEntity = GoodsArrivalID FROM GoodsReceiptDetails WHERE GoodsArrivalID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("GoodsArrivalVoidable", queryArray);
        }


        private void GoodsArrivalToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      GoodsArrivals  SET Approved = @Approved, ApprovedDate = GetDate(), InActive = 0, InActivePartial = 0, InActiveDate = NULL, VoidTypeID = NULL WHERE GoodsArrivalID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          GoodsArrivalDetails  SET Approved = @Approved, InActive = 0, InActivePartial = 0, InActivePartialDate = NULL, VoidTypeID = NULL WHERE GoodsArrivalID = @EntityID ; " + "\r\n";

            queryString = queryString + "               UPDATE          ERmgrVCP.dbo.GoodsArrivals  SET Approved = @Approved, ApprovedDate = GetDate(), InActive = 0, InActivePartial = 0, InActiveDate = NULL, VoidTypeID = NULL WHERE GoodsArrivalID = @EntityID " + "\r\n";
            queryString = queryString + "               UPDATE          ERmgrVCP.dbo.GoodsArrivalDetails  SET Approved = @Approved, InActive = 0, InActivePartial = 0, InActivePartialDate = NULL, VoidTypeID = NULL WHERE GoodsArrivalID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, N'hủy', '')  + N' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GoodsArrivalToggleApproved", queryString);
        }

        private void GoodsArrivalToggleVoid()
        {
            string queryString = " @EntityID int, @InActive bit, @VoidTypeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      GoodsArrivals  SET InActive = @InActive, InActiveDate = GetDate(), VoidTypeID = IIF(@InActive = 1, @VoidTypeID, NULL) WHERE GoodsArrivalID = @EntityID AND InActive = ~@InActive" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          GoodsArrivalDetails  SET InActive = @InActive WHERE GoodsArrivalID = @EntityID ; " + "\r\n";

            queryString = queryString + "               UPDATE          ERmgrVCP.dbo.GoodsArrivals  SET InActive = @InActive, InActiveDate = GetDate(), VoidTypeID = IIF(@InActive = 1, @VoidTypeID, NULL) WHERE GoodsArrivalID = @EntityID " + "\r\n";
            queryString = queryString + "               UPDATE          ERmgrVCP.dbo.GoodsArrivalDetails  SET InActive = @InActive WHERE GoodsArrivalID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@InActive = 0, 'phục hồi lệnh', '')  + ' hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            this.totalSmartPortalEntities.CreateStoredProcedure("GoodsArrivalToggleVoid", queryString);
        }

        private void GoodsArrivalToggleVoidDetail()
        {
            string queryString = " @EntityID int, @EntityDetailID int, @InActivePartial bit, @VoidTypeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      GoodsArrivalDetails  SET InActivePartial = @InActivePartial, InActivePartialDate = GetDate(), VoidTypeID = IIF(@InActivePartial = 1, @VoidTypeID, NULL) WHERE GoodsArrivalID = @EntityID AND GoodsArrivalDetailID = @EntityDetailID AND InActivePartial = ~@InActivePartial ; " + "\r\n";
            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE         @MaxInActivePartial bit     SET @MaxInActivePartial = (SELECT MAX(CAST(InActivePartial AS int)) FROM GoodsArrivalDetails WHERE GoodsArrivalID = @EntityID) ;" + "\r\n";
            queryString = queryString + "               UPDATE          GoodsArrivals  SET InActivePartial = @MaxInActivePartial WHERE GoodsArrivalID = @EntityID ; " + "\r\n";

            queryString = queryString + "               UPDATE          ERmgrVCP.dbo.GoodsArrivalDetails  SET InActivePartial = @InActivePartial, InActivePartialDate = GetDate(), VoidTypeID = IIF(@InActivePartial = 1, @VoidTypeID, NULL) WHERE GoodsArrivalID = @EntityID AND GoodsArrivalDetailID = @EntityDetailID ; " + "\r\n";
            queryString = queryString + "               UPDATE          ERmgrVCP.dbo.GoodsArrivals  SET InActivePartial = @MaxInActivePartial WHERE GoodsArrivalID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@InActivePartial = 0, 'phục hồi lệnh', '')  + ' hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            this.totalSmartPortalEntities.CreateStoredProcedure("GoodsArrivalToggleVoidDetail", queryString);
        }


        private void GoodsArrivalInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("GoodsArrivals", "GoodsArrivalID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.GoodsArrival));
            this.totalSmartPortalEntities.CreateTrigger("GoodsArrivalInitReference", simpleInitReference.CreateQuery());
        }

        #endregion
    }
}
