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

            this.GoodsArrivalToggleApproved();

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

            queryString = " @GoodsArrivalID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      GoodsArrivalDetails.GoodsArrivalDetailID, GoodsArrivalDetails.GoodsArrivalID, GoodsArrivalDetails.PurchaseOrderID, GoodsArrivalDetails.PurchaseOrderDetailID, PurchaseOrders.Reference AS PurchaseOrderReference, PurchaseOrders.Code AS PurchaseOrderCode, PurchaseOrders.EntryDate AS PurchaseOrderEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, GoodsArrivalDetails.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   ROUND(ISNULL(PurchaseOrderDetails.Quantity, 0) - ISNULL(PurchaseOrderDetails.QuantityArrived, 0) + GoodsArrivalDetails.Quantity, 0) AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   GoodsArrivalDetails.Quantity, GoodsArrivalDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        GoodsArrivalDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON GoodsArrivalDetails.GoodsArrivalID = @GoodsArrivalID AND GoodsArrivalDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN PurchaseOrderDetails ON GoodsArrivalDetails.PurchaseOrderDetailID = PurchaseOrderDetails.PurchaseOrderDetailID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN PurchaseOrders ON PurchaseOrderDetails.PurchaseOrderID = PurchaseOrders.PurchaseOrderID " + "\r\n";

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

            queryString = queryString + "       SELECT          PurchaseOrders.PurchaseOrderID, PurchaseOrders.Reference AS PurchaseOrderReference, PurchaseOrders.Code AS PurchaseOrderCode, PurchaseOrders.EntryDate AS PurchaseOrderEntryDate, PurchaseOrders.Description, PurchaseOrders.Remarks, " + "\r\n";
            queryString = queryString + "                       PurchaseOrders.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.OfficialName AS CustomerOfficialName, Customers.Birthday AS CustomerBirthday, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.BillingAddress AS CustomerBillingAddress, Customers.ShippingAddress AS CustomerShippingAddress, " + "\r\n";
            queryString = queryString + "                       PurchaseOrders.TransporterID, Transporters.Code AS TransporterCode, Transporters.Name AS TransporterName, Transporters.OfficialName AS TransporterOfficialName, Transporters.Birthday AS TransporterBirthday, Transporters.VATCode AS TransporterVATCode, Transporters.AttentionName AS TransporterAttentionName, Transporters.Telephone AS TransporterTelephone, Transporters.BillingAddress AS TransporterBillingAddress, Transporters.ShippingAddress AS TransporterShippingAddress, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, Warehouses.Name AS WarehouseName " + "\r\n";

            queryString = queryString + "       FROM            PurchaseOrders " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON PurchaseOrders.PurchaseOrderID IN (SELECT PurchaseOrderID FROM PurchaseOrderDetails WHERE LocationID = @LocationID AND Approved = 1 AND InActive = 0 AND InActivePartial = 0  AND ROUND(Quantity - QuantityArrived, " + (int)GlobalEnums.rndQuantity + ") > 0) AND PurchaseOrders.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers Transporters ON PurchaseOrders.TransporterID = Transporters.CustomerID " + "\r\n";

            queryString = queryString + "                       INNER JOIN Warehouses ON Warehouses.WarehouseID = 1 " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsArrivalPendingPurchaseOrders", queryString);
        }

        private void GetGoodsArrivalPendingCustomers()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          CustomerTransporterPENDING.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.OfficialName AS CustomerOfficialName, Customers.Birthday AS CustomerBirthday, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.BillingAddress AS CustomerBillingAddress, Customers.ShippingAddress AS CustomerShippingAddress, " + "\r\n";
            queryString = queryString + "                       CustomerTransporterPENDING.TransporterID, Transporters.Code AS TransporterCode, Transporters.Name AS TransporterName, Transporters.OfficialName AS TransporterOfficialName, Transporters.Birthday AS TransporterBirthday, Transporters.VATCode AS TransporterVATCode, Transporters.AttentionName AS TransporterAttentionName, Transporters.Telephone AS TransporterTelephone, Transporters.BillingAddress AS TransporterBillingAddress, Transporters.ShippingAddress AS TransporterShippingAddress, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, Warehouses.Name AS WarehouseName, N'' AS Description, N'' AS Remarks " + "\r\n";

            queryString = queryString + "       FROM           (SELECT DISTINCT CustomerID, TransporterID FROM PurchaseOrders WHERE PurchaseOrderID IN (SELECT PurchaseOrderID FROM PurchaseOrderDetails WHERE LocationID = @LocationID AND Approved = 1 AND InActive = 0 AND InActivePartial = 0  AND ROUND(Quantity - QuantityArrived, " + (int)GlobalEnums.rndQuantity + ") > 0)) CustomerTransporterPENDING " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON CustomerTransporterPENDING.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers Transporters ON CustomerTransporterPENDING.TransporterID = Transporters.CustomerID " + "\r\n";

            queryString = queryString + "                       INNER JOIN Warehouses ON Warehouses.WarehouseID = 1 " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsArrivalPendingCustomers", queryString);
        }



        private void GetGoodsArrivalPendingPurchaseOrderDetails()
        {
            string queryString;

            queryString = " @LocationID Int, @GoodsArrivalID Int, @PurchaseOrderID Int, @CustomerID Int, @TransporterID Int, @PurchaseOrderDetailIDs varchar(3999) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

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
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLNew(isPurchaseOrderID, isPurchaseOrderDetailIDs) + " WHERE PurchaseOrderDetails.PurchaseOrderDetailID NOT IN (SELECT PurchaseOrderDetailID FROM GoodsArrivalDetails WHERE GoodsArrivalID = @GoodsArrivalID) " + "\r\n";
            queryString = queryString + "                   UNION ALL " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLEdit(isPurchaseOrderID, isPurchaseOrderDetailIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY PurchaseOrders.EntryDate, PurchaseOrders.PurchaseOrderID, PurchaseOrderDetails.PurchaseOrderDetailID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLNew(bool isPurchaseOrderID, bool isPurchaseOrderDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      PurchaseOrders.PurchaseOrderID, PurchaseOrderDetails.PurchaseOrderDetailID, PurchaseOrders.Reference AS PurchaseOrderReference, PurchaseOrders.Code AS PurchaseOrderCode, PurchaseOrders.EntryDate AS PurchaseOrderEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   ROUND(PurchaseOrderDetails.Quantity - PurchaseOrderDetails.QuantityArrived, 0) AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   0 AS Quantity, PurchaseOrders.Description, PurchaseOrderDetails.Remarks, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        PurchaseOrders " + "\r\n";
            queryString = queryString + "                   INNER JOIN PurchaseOrderDetails ON " + (isPurchaseOrderID ? " PurchaseOrders.PurchaseOrderID = @PurchaseOrderID " : "PurchaseOrders.LocationID = @LocationID AND PurchaseOrders.CustomerID = @CustomerID AND PurchaseOrders.TransporterID = @TransporterID ") + " AND PurchaseOrderDetails.Approved = 1 AND PurchaseOrderDetails.InActive = 0 AND PurchaseOrderDetails.InActivePartial = 0 AND ROUND(PurchaseOrderDetails.Quantity - PurchaseOrderDetails.QuantityArrived, " + (int)GlobalEnums.rndQuantity + ") > 0 AND PurchaseOrders.PurchaseOrderID = PurchaseOrderDetails.PurchaseOrderID" + (isPurchaseOrderDetailIDs ? " AND PurchaseOrderDetails.PurchaseOrderDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@PurchaseOrderDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON PurchaseOrderDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            return queryString;
        }

        private string BuildSQLEdit(bool isPurchaseOrderID, bool isPurchaseOrderDetailIDs)
        {
            string queryString = "";
            //NO NEED TO UNDO QuantityAvailable -THE WAREHOUSE BALANCE- FOR THIS EDIT QUERY: BECAUSE: THIS STORED PROCEDURE ONLY BE CALLED WHEN Approved = 0 => BECAUSE OF THIS (HAVE NOT UPPROVED YET): THIS DELIVERYADVICE QUANTITY DOES NOT EFFECT THE WAREHOUSE BALANCE
            queryString = queryString + "       SELECT      PurchaseOrders.PurchaseOrderID, PurchaseOrderDetails.PurchaseOrderDetailID, PurchaseOrders.Reference AS PurchaseOrderReference, PurchaseOrders.Code AS PurchaseOrderCode, PurchaseOrders.EntryDate AS PurchaseOrderEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   ROUND(PurchaseOrderDetails.Quantity - PurchaseOrderDetails.QuantityArrived + GoodsArrivalDetails.Quantity, 0) AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   0 AS Quantity, PurchaseOrders.Description, PurchaseOrderDetails.Remarks, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        PurchaseOrderDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsArrivalDetails ON GoodsArrivalDetails.GoodsArrivalID = @GoodsArrivalID AND PurchaseOrderDetails.PurchaseOrderDetailID = GoodsArrivalDetails.PurchaseOrderDetailID" + (isPurchaseOrderDetailIDs ? " AND PurchaseOrderDetails.PurchaseOrderDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@PurchaseOrderDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON PurchaseOrderDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN PurchaseOrders ON PurchaseOrderDetails.PurchaseOrderID = PurchaseOrders.PurchaseOrderID " + "\r\n";

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
            queryString = queryString + "           SET             PurchaseOrderDetails.QuantityArrived = ROUND(PurchaseOrderDetails.QuantityArrived + GoodsArrivalDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
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
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Số lượng xuất vượt quá số lượng đặt hàng: ' + CAST(ROUND(Quantity - QuantityArrived, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM PurchaseOrderDetails WHERE (ROUND(Quantity - QuantityArrived, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

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

        private void GoodsArrivalInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("GoodsArrivals", "GoodsArrivalID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.GoodsArrival));
            this.totalSmartPortalEntities.CreateTrigger("GoodsArrivalInitReference", simpleInitReference.CreateQuery());
        }

        #endregion
    }
}
