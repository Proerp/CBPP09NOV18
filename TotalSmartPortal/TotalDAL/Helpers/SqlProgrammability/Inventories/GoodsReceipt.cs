using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Inventories
{
    public class GoodsReceipt
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public GoodsReceipt(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetGoodsReceiptIndexes();

            this.GetGoodsReceiptViewDetails();

            this.GetGoodsReceiptPendingCustomers();
            this.GetGoodsReceiptPendingPurchaseRequisitions();
            this.GetGoodsReceiptPendingPurchaseRequisitionDetails();

            this.GetGoodsReceiptPendingWarehouses();
            this.GetGoodsReceiptPendingWarehouseTransfers();
            this.GetGoodsReceiptPendingWarehouseTransferDetails();

            this.GetGoodsReceiptPendingPlannedOrders();
            this.GetGoodsReceiptPendingPlannedOrderCustomers();
            this.GetGoodsReceiptPendingPlannedOrderDetails();
            
            this.GetGoodsReceiptPendingMaterialIssueDetails();

            GenerateSQLPendingDetails generatePendingWarehouseAdjustmentDetails = new GenerateSQLPendingDetails(this.totalSmartPortalEntities, GlobalEnums.GoodsReceiptTypeID.WarehouseAdjustments, "WarehouseAdjustments", "WarehouseAdjustmentDetails", "WarehouseAdjustmentID", "@WarehouseAdjustmentID", "WarehouseAdjustmentDetailID", "@WarehouseAdjustmentDetailIDs", "WarehouseReceiptID", "PrimaryReference", "PrimaryEntryDate");
            generatePendingWarehouseAdjustmentDetails.GetPendingPickupDetails("GetPendingWarehouseAdjustmentDetails");

            this.GoodsReceiptSaveRelative();
            this.GoodsReceiptPostSaveValidate();

            this.GoodsReceiptApproved();
            this.GoodsReceiptEditable();

            this.GoodsReceiptToggleApproved();

            this.GoodsReceiptInitReference();

            this.GetGoodsReceiptIDofWarehouseAdjustment();
            this.GetGoodsReceiptDetailAvailables();
        }


        private void GetGoodsReceiptIndexes()
        {
            string queryString;

            queryString = " @NMVNTaskID int, @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      GoodsReceipts.GoodsReceiptID, CAST(GoodsReceipts.EntryDate AS DATE) AS EntryDate, GoodsReceipts.Reference, Locations.Code AS LocationCode, Customers.Name AS CustomerName, ISNULL(PlannedOrders.Reference, '') +  ISNULL(' (' + PlannedOrders.Code + ')', '') + ISNULL(WarehouseIssues.Code, '') + IIF(GoodsReceipts.GoodsReceiptTypeID = 7, N'Thu hồi màng đã cấp', '') AS GoodsReceiptTypeCaption, GoodsReceipts.Caption, GoodsReceipts.Description, GoodsReceipts.TotalQuantity, GoodsReceipts.Approved " + "\r\n";
            queryString = queryString + "       FROM        GoodsReceipts " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON (GoodsReceipts.GoodsReceiptTypeID = 7 OR GoodsReceipts.GoodsReceiptTypeID = 8 OR GoodsReceipts.GoodsReceiptTypeID = 9) AND GoodsReceipts.NMVNTaskID = @NMVNTaskID AND GoodsReceipts.EntryDate >= @FromDate AND GoodsReceipts.EntryDate <= @ToDate AND GoodsReceipts.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = @NMVNTaskID AND AccessControls.AccessLevel > 0) AND Locations.LocationID = GoodsReceipts.LocationID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN Customers ON GoodsReceipts.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN PlannedOrders ON GoodsReceipts.PlannedOrderID = PlannedOrders.PlannedOrderID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN Warehouses WarehouseIssues ON GoodsReceipts.WarehouseIssueID = WarehouseIssues.WarehouseID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsReceiptIndexes", queryString);
        }
        

        private void GetGoodsReceiptViewDetails()
        {
            string queryString;

            queryString = " @GoodsReceiptID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceiptDetails.GoodsReceiptID, GoodsReceiptDetails.PurchaseRequisitionID, GoodsReceiptDetails.PurchaseRequisitionDetailID, PurchaseRequisitions.Reference AS PurchaseRequisitionReference, PurchaseRequisitions.Code AS PurchaseRequisitionCode, PurchaseRequisitions.EntryDate AS PurchaseRequisitionEntryDate, GoodsReceiptDetails.WarehouseTransferID, GoodsReceiptDetails.WarehouseTransferDetailID, WarehouseTransfers.Reference AS WarehouseTransferReference, WarehouseTransfers.EntryDate AS WarehouseTransferEntryDate, WarehouseTransferGoodsReceiptDetails.Reference AS GoodsReceiptReference, WarehouseTransferGoodsReceiptDetails.EntryDate AS GoodsReceiptEntryDate, GoodsReceiptDetails.BatchEntryDate, GoodsReceiptDetails.WarehouseAdjustmentID, GoodsReceiptDetails.WarehouseAdjustmentDetailID, WarehouseAdjustmentDetails.Reference AS WarehouseAdjustmentReference, WarehouseAdjustmentDetails.EntryDate AS WarehouseAdjustmentEntryDate, WarehouseAdjustmentDetails.WarehouseAdjustmentTypeID, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, GoodsReceiptDetails.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   ROUND(ISNULL(IIF(PurchaseRequisitionDetails.Approved = 1 AND PurchaseRequisitionDetails.InActive = 0 AND PurchaseRequisitionDetails.InActivePartial = 0, PurchaseRequisitionDetails.Quantity - PurchaseRequisitionDetails.QuantityReceipted, 0), 0) + ISNULL(WarehouseTransferDetails.Quantity - WarehouseTransferDetails.QuantityReceipted, 0) + ISNULL(MaterialIssueDetails.Quantity - MaterialIssueDetails.QuantitySemifinished - MaterialIssueDetails.QuantityFailure - MaterialIssueDetails.QuantityReceipted, 0) + ISNULL(WarehouseAdjustmentDetails.Quantity - WarehouseAdjustmentDetails.QuantityReceipted, 0) + ISNULL(FinishedProductPackages.Quantity - FinishedProductPackages.QuantityReceipted, 0) + GoodsReceiptDetails.Quantity, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, GoodsReceiptDetails.Quantity, GoodsReceiptDetails.Remarks, " + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails.MaterialIssueDetailID, MaterialIssueDetails.MaterialIssueID, MaterialIssueDetails.EntryDate AS MaterialIssueEntryDate, Workshifts.Name AS WorkshiftName, Workshifts.EntryDate AS WorkshiftEntryDate, ProductionLines.Code AS ProductionLinesCode, " + "\r\n";

            queryString = queryString + "                   GoodsReceiptDetails.FinishedProductPackageID, FinishedProductPackages.FinishedProductID, FinishedProductPackages.EntryDate AS FinishedProductEntryDate, ISNULL(FirmOrders.Reference, MaterialIssueFirmOrders.Reference) AS FirmOrderReference, ISNULL(FirmOrders.Code, MaterialIssueFirmOrders.Code) AS FirmOrderCode, ISNULL(FirmOrders.Specs, MaterialIssueFirmOrders.Specs) AS FirmOrderSpecs, FinishedProductPackages.SemifinishedProductReferences " + "\r\n";

            queryString = queryString + "       FROM        GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON GoodsReceiptDetails.GoodsReceiptID = @GoodsReceiptID AND GoodsReceiptDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "                   LEFT JOIN MaterialIssueDetails ON GoodsReceiptDetails.MaterialIssueDetailID = MaterialIssueDetails.MaterialIssueDetailID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN FirmOrders MaterialIssueFirmOrders ON MaterialIssueDetails.FirmOrderID = MaterialIssueFirmOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN Workshifts ON MaterialIssueDetails.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN ProductionLines ON MaterialIssueDetails.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";

            queryString = queryString + "                   LEFT JOIN PurchaseRequisitionDetails ON GoodsReceiptDetails.PurchaseRequisitionDetailID = PurchaseRequisitionDetails.PurchaseRequisitionDetailID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN PurchaseRequisitions ON PurchaseRequisitionDetails.PurchaseRequisitionID = PurchaseRequisitions.PurchaseRequisitionID " + "\r\n";

            queryString = queryString + "                   LEFT JOIN WarehouseTransferDetails ON GoodsReceiptDetails.WarehouseTransferDetailID = WarehouseTransferDetails.WarehouseTransferDetailID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN WarehouseTransfers ON WarehouseTransferDetails.WarehouseTransferID = WarehouseTransfers.WarehouseTransferID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN GoodsReceiptDetails WarehouseTransferGoodsReceiptDetails ON WarehouseTransferDetails.GoodsReceiptDetailID = WarehouseTransferGoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";

            queryString = queryString + "                   LEFT JOIN WarehouseAdjustmentDetails ON GoodsReceiptDetails.WarehouseAdjustmentDetailID = WarehouseAdjustmentDetails.WarehouseAdjustmentDetailID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN FinishedProductPackages ON GoodsReceiptDetails.FinishedProductPackageID = FinishedProductPackages.FinishedProductPackageID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN FirmOrders ON FinishedProductPackages.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";

            queryString = queryString + "       ORDER BY    Commodities.CommodityTypeID, GoodsReceiptDetails.GoodsReceiptID, GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsReceiptViewDetails", queryString);
        }




        #region PurchaseRequisition
        private void GetGoodsReceiptPendingPurchaseRequisitions()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          " + (int)@GlobalEnums.GoodsReceiptTypeID.PurchaseRequisition + " AS GoodsReceiptTypeID, PurchaseRequisitions.PurchaseRequisitionID, PurchaseRequisitions.Reference AS PurchaseRequisitionReference, PurchaseRequisitions.Code AS PurchaseRequisitionCode, PurchaseRequisitions.EntryDate AS PurchaseRequisitionEntryDate, PurchaseRequisitions.Description, PurchaseRequisitions.Remarks, " + "\r\n";
            queryString = queryString + "                       PurchaseRequisitions.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.OfficialName AS CustomerOfficialName, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, Warehouses.Name AS WarehouseName " + "\r\n";

            queryString = queryString + "       FROM            PurchaseRequisitions " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON PurchaseRequisitions.PurchaseRequisitionID IN (SELECT PurchaseRequisitionID FROM PurchaseRequisitionDetails WHERE LocationID = @LocationID AND Approved = 1 AND InActive = 0 AND InActivePartial = 0 AND ROUND(Quantity - QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") > 0) AND PurchaseRequisitions.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";


            queryString = queryString + "                       INNER JOIN Warehouses ON Warehouses.WarehouseID = 6 " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsReceiptPendingPurchaseRequisitions", queryString);
        }

        private void GetGoodsReceiptPendingCustomers()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          " + (int)@GlobalEnums.GoodsReceiptTypeID.PurchaseRequisition + " AS GoodsReceiptTypeID, Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.OfficialName AS CustomerOfficialName, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.TerritoryID AS CustomerTerritoryID, CustomerEntireTerritories.EntireName AS CustomerEntireTerritoryEntireName, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, Warehouses.Name AS WarehouseName " + "\r\n";

            queryString = queryString + "       FROM           (SELECT DISTINCT CustomerID FROM PurchaseRequisitions WHERE PurchaseRequisitionID IN (SELECT PurchaseRequisitionID FROM PurchaseRequisitionDetails WHERE LocationID = @LocationID AND Approved = 1 AND InActive = 0 AND InActivePartial = 0 AND ROUND(Quantity - QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") > 0)) CustomerPENDING " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON CustomerPENDING.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";

            
            queryString = queryString + "                       INNER JOIN Warehouses ON Warehouses.WarehouseID = 2 " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsReceiptPendingCustomers", queryString);
        }


        private void GetGoodsReceiptPendingPurchaseRequisitionDetails()
        {
            string queryString;

            queryString = " @LocationID Int, @GoodsReceiptID Int, @PurchaseRequisitionID Int, @CustomerID Int, @PurchaseRequisitionDetailIDs varchar(3999), @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF  (@PurchaseRequisitionID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPurchaseRequisition(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPurchaseRequisition(false) + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsReceiptPendingPurchaseRequisitionDetails", queryString);
        }

        private string BuildSQLPurchaseRequisition(bool isPurchaseRequisitionID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@PurchaseRequisitionDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPurchaseRequisition(isPurchaseRequisitionID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPurchaseRequisition(isPurchaseRequisitionID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLPurchaseRequisition(bool isPurchaseRequisitionID, bool isPurchaseRequisitionDetailIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@GoodsReceiptID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLPurchaseRequisitionNew(isPurchaseRequisitionID, isPurchaseRequisitionDetailIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY PurchaseRequisitions.EntryDate, PurchaseRequisitions.PurchaseRequisitionID, PurchaseRequisitionDetails.PurchaseRequisitionDetailID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLPurchaseRequisitionEdit(isPurchaseRequisitionID, isPurchaseRequisitionDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY PurchaseRequisitions.EntryDate, PurchaseRequisitions.PurchaseRequisitionID, PurchaseRequisitionDetails.PurchaseRequisitionDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLPurchaseRequisitionNew(isPurchaseRequisitionID, isPurchaseRequisitionDetailIDs) + " WHERE PurchaseRequisitionDetails.PurchaseRequisitionDetailID NOT IN (SELECT PurchaseRequisitionDetailID FROM GoodsReceiptDetails WHERE GoodsReceiptID = @GoodsReceiptID) " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLPurchaseRequisitionEdit(isPurchaseRequisitionID, isPurchaseRequisitionDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY PurchaseRequisitions.EntryDate, PurchaseRequisitions.PurchaseRequisitionID, PurchaseRequisitionDetails.PurchaseRequisitionDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLPurchaseRequisitionNew(bool isPurchaseRequisitionID, bool isPurchaseRequisitionDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      PurchaseRequisitions.PurchaseRequisitionID, PurchaseRequisitionDetails.PurchaseRequisitionDetailID, PurchaseRequisitions.Reference AS PurchaseRequisitionReference, PurchaseRequisitions.Code AS PurchaseRequisitionCode, PurchaseRequisitions.EntryDate AS PurchaseRequisitionEntryDate, GETDATE() AS BatchEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   ROUND(PurchaseRequisitionDetails.Quantity - PurchaseRequisitionDetails.QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   0.0 AS Quantity, PurchaseRequisitions.Description, PurchaseRequisitionDetails.Remarks, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        PurchaseRequisitions " + "\r\n";
            queryString = queryString + "                   INNER JOIN PurchaseRequisitionDetails ON " + (isPurchaseRequisitionID ? " PurchaseRequisitions.PurchaseRequisitionID = @PurchaseRequisitionID " : "PurchaseRequisitions.LocationID = @LocationID AND PurchaseRequisitions.CustomerID = @CustomerID ") + " AND PurchaseRequisitionDetails.Approved = 1 AND PurchaseRequisitionDetails.InActive = 0 AND PurchaseRequisitionDetails.InActivePartial = 0 AND ROUND(PurchaseRequisitionDetails.Quantity - PurchaseRequisitionDetails.QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") > 0 AND PurchaseRequisitions.PurchaseRequisitionID = PurchaseRequisitionDetails.PurchaseRequisitionID" + (isPurchaseRequisitionDetailIDs ? " AND PurchaseRequisitionDetails.PurchaseRequisitionDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@PurchaseRequisitionDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON PurchaseRequisitionDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            return queryString;
        }

        private string BuildSQLPurchaseRequisitionEdit(bool isPurchaseRequisitionID, bool isPurchaseRequisitionDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      PurchaseRequisitions.PurchaseRequisitionID, PurchaseRequisitionDetails.PurchaseRequisitionDetailID, PurchaseRequisitions.Reference AS PurchaseRequisitionReference, PurchaseRequisitions.Code AS PurchaseRequisitionCode, PurchaseRequisitions.EntryDate AS PurchaseRequisitionEntryDate, GETDATE() AS BatchEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   ROUND(PurchaseRequisitionDetails.Quantity - PurchaseRequisitionDetails.QuantityReceipted + GoodsReceiptDetails.Quantity, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   0.0 AS Quantity, PurchaseRequisitions.Description, PurchaseRequisitionDetails.Remarks, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        PurchaseRequisitionDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsReceiptDetails ON GoodsReceiptDetails.GoodsReceiptID = @GoodsReceiptID AND PurchaseRequisitionDetails.PurchaseRequisitionDetailID = GoodsReceiptDetails.PurchaseRequisitionDetailID" + (isPurchaseRequisitionDetailIDs ? " AND PurchaseRequisitionDetails.PurchaseRequisitionDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@PurchaseRequisitionDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON PurchaseRequisitionDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN PurchaseRequisitions ON PurchaseRequisitionDetails.PurchaseRequisitionID = PurchaseRequisitions.PurchaseRequisitionID " + "\r\n";

            return queryString;
        }
        #endregion PurchaseRequisition





        #region WarehouseTransfer
        private void GetGoodsReceiptPendingWarehouseTransfers()
        {
            string queryString = " @LocationID int, @NMVNTaskID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          " + (int)@GlobalEnums.GoodsReceiptTypeID.WarehouseTransfer + " AS GoodsReceiptTypeID, WarehouseTransfers.WarehouseTransferID, WarehouseTransfers.Reference AS WarehouseTransferReference, WarehouseTransfers.EntryDate AS WarehouseTransferEntryDate, WarehouseTransfers.Caption, WarehouseTransfers.Description, WarehouseTransfers.Remarks, " + "\r\n";
            queryString = queryString + "                       Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, Warehouses.Name AS WarehouseName, WarehouseIssues.WarehouseID AS WarehouseIssueID, WarehouseIssues.Code AS WarehouseIssueCode, WarehouseIssues.Name AS WarehouseIssueName " + "\r\n";

            queryString = queryString + "       FROM            WarehouseTransfers " + "\r\n";
            queryString = queryString + "                       INNER JOIN Warehouses ON WarehouseTransfers.WarehouseTransferID IN (SELECT WarehouseTransferID FROM WarehouseTransferDetails WHERE LocationReceiptID = @LocationID AND NMVNTaskID = @NMVNTaskID - 1000000 AND Approved = 1 AND ROUND(Quantity - QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") > 0) AND WarehouseTransfers.WarehouseReceiptID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Warehouses WarehouseIssues ON WarehouseTransfers.WarehouseID = WarehouseIssues.WarehouseID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsReceiptPendingWarehouseTransfers", queryString);
        }

        private void GetGoodsReceiptPendingWarehouses()
        {
            string queryString = " @LocationID int, @NMVNTaskID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          " + (int)@GlobalEnums.GoodsReceiptTypeID.WarehouseTransfer + " AS GoodsReceiptTypeID, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, Warehouses.Name AS WarehouseName, WarehouseIssues.WarehouseID AS WarehouseIssueID, WarehouseIssues.Code AS WarehouseIssueCode, WarehouseIssues.Name AS WarehouseIssueName " + "\r\n";

            queryString = queryString + "       FROM           (SELECT WarehouseReceiptID, WarehouseID FROM WarehouseTransfers WHERE WarehouseTransferID IN (SELECT WarehouseTransferID FROM WarehouseTransferDetails WHERE LocationReceiptID = @LocationID AND NMVNTaskID = @NMVNTaskID - 1000000 AND Approved = 1 AND ROUND(Quantity - QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") > 0) GROUP BY WarehouseReceiptID, WarehouseID) WarehousePENDING " + "\r\n";
            queryString = queryString + "                       INNER JOIN Warehouses ON WarehousePENDING.WarehouseReceiptID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Warehouses WarehouseIssues ON WarehousePENDING.WarehouseID = WarehouseIssues.WarehouseID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsReceiptPendingWarehouses", queryString);
        }


        private void GetGoodsReceiptPendingWarehouseTransferDetails()
        {
            string queryString;

            queryString = " @NMVNTaskID int, @GoodsReceiptID Int, @WarehouseTransferID Int, @WarehouseID Int, @WarehouseIssueID Int, @WarehouseTransferDetailIDs varchar(3999), @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF  (@WarehouseTransferID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLWarehouseTransfer(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLWarehouseTransfer(false) + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsReceiptPendingWarehouseTransferDetails", queryString);
        }

        private string BuildSQLWarehouseTransfer(bool isWarehouseTransferID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@WarehouseTransferDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLWarehouseTransfer(isWarehouseTransferID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLWarehouseTransfer(isWarehouseTransferID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLWarehouseTransfer(bool isWarehouseTransferID, bool isWarehouseTransferDetailIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@GoodsReceiptID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLWarehouseTransferNew(isWarehouseTransferID, isWarehouseTransferDetailIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY WarehouseTransfers.EntryDate, WarehouseTransfers.WarehouseTransferID, WarehouseTransferDetails.WarehouseTransferDetailID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLWarehouseTransferEdit(isWarehouseTransferID, isWarehouseTransferDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY WarehouseTransfers.EntryDate, WarehouseTransfers.WarehouseTransferID, WarehouseTransferDetails.WarehouseTransferDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLWarehouseTransferNew(isWarehouseTransferID, isWarehouseTransferDetailIDs) + " WHERE WarehouseTransferDetails.WarehouseTransferDetailID NOT IN (SELECT WarehouseTransferDetailID FROM GoodsReceiptDetails WHERE GoodsReceiptID = @GoodsReceiptID) " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLWarehouseTransferEdit(isWarehouseTransferID, isWarehouseTransferDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY WarehouseTransfers.EntryDate, WarehouseTransfers.WarehouseTransferID, WarehouseTransferDetails.WarehouseTransferDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLWarehouseTransferNew(bool isWarehouseTransferID, bool isWarehouseTransferDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      WarehouseTransfers.WarehouseTransferID, WarehouseTransferDetails.WarehouseTransferDetailID, WarehouseTransfers.Reference AS WarehouseTransferReference, WarehouseTransfers.EntryDate AS WarehouseTransferEntryDate, WarehouseTransferGoodsReceiptDetails.Reference AS GoodsReceiptReference, WarehouseTransferGoodsReceiptDetails.EntryDate AS GoodsReceiptEntryDate, WarehouseTransferGoodsReceiptDetails.BatchEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   ROUND(WarehouseTransferDetails.Quantity - WarehouseTransferDetails.QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   0.0 AS Quantity, WarehouseTransfers.Description, WarehouseTransferDetails.Remarks, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        WarehouseTransfers " + "\r\n";
            queryString = queryString + "                   INNER JOIN WarehouseTransferDetails ON " + (isWarehouseTransferID ? " WarehouseTransfers.WarehouseTransferID = @WarehouseTransferID " : "WarehouseTransfers.NMVNTaskID = @NMVNTaskID - 1000000 AND WarehouseTransfers.WarehouseReceiptID = @WarehouseID AND WarehouseTransfers.WarehouseID = @WarehouseIssueID ") + " AND WarehouseTransferDetails.Approved = 1 AND ROUND(WarehouseTransferDetails.Quantity - WarehouseTransferDetails.QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") > 0 AND WarehouseTransfers.WarehouseTransferID = WarehouseTransferDetails.WarehouseTransferID" + (isWarehouseTransferDetailIDs ? " AND WarehouseTransferDetails.WarehouseTransferDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@WarehouseTransferDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON WarehouseTransferDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsReceiptDetails WarehouseTransferGoodsReceiptDetails ON WarehouseTransferDetails.GoodsReceiptDetailID = WarehouseTransferGoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";

            return queryString;
        }

        private string BuildSQLWarehouseTransferEdit(bool isWarehouseTransferID, bool isWarehouseTransferDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      WarehouseTransfers.WarehouseTransferID, WarehouseTransferDetails.WarehouseTransferDetailID, WarehouseTransfers.Reference AS WarehouseTransferReference, WarehouseTransfers.EntryDate AS WarehouseTransferEntryDate, WarehouseTransferGoodsReceiptDetails.Reference AS GoodsReceiptReference, WarehouseTransferGoodsReceiptDetails.EntryDate AS GoodsReceiptEntryDate, WarehouseTransferGoodsReceiptDetails.BatchEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   ROUND(WarehouseTransferDetails.Quantity - WarehouseTransferDetails.QuantityReceipted + GoodsReceiptDetails.Quantity, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   0.0 AS Quantity, WarehouseTransfers.Description, WarehouseTransferDetails.Remarks, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        WarehouseTransferDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsReceiptDetails ON GoodsReceiptDetails.GoodsReceiptID = @GoodsReceiptID AND WarehouseTransferDetails.WarehouseTransferDetailID = GoodsReceiptDetails.WarehouseTransferDetailID" + (isWarehouseTransferDetailIDs ? " AND WarehouseTransferDetails.WarehouseTransferDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@WarehouseTransferDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON WarehouseTransferDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN WarehouseTransfers ON WarehouseTransferDetails.WarehouseTransferID = WarehouseTransfers.WarehouseTransferID " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsReceiptDetails WarehouseTransferGoodsReceiptDetails ON WarehouseTransferDetails.GoodsReceiptDetailID = WarehouseTransferGoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";

            return queryString;
        }
        #endregion WarehouseTransfer


        #region FinishedProduct
        private void GetGoodsReceiptPendingPlannedOrders()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          " + (int)@GlobalEnums.GoodsReceiptTypeID.FinishedProduct + " AS GoodsReceiptTypeID, PlannedOrders.PlannedOrderID, PlannedOrders.EntryDate AS PlannedOrderEntryDate, PlannedOrders.Reference AS PlannedOrderReference, PlannedOrders.Code AS PlannedOrderCode, PlannedOrders.Caption AS PlannedOrderCaption, Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, Warehouses.Name AS WarehouseName " + "\r\n";
            queryString = queryString + "       FROM            Customers " + "\r\n";


            queryString = queryString + "                       INNER JOIN Warehouses ON Warehouses.WarehouseID = 3 " + "\r\n";


            queryString = queryString + "                       INNER JOIN PlannedOrders ON PlannedOrders.PlannedOrderID IN (SELECT DISTINCT PlannedOrderID FROM FinishedProductPackages WHERE LocationID = @LocationID AND Approved = 1 AND HandoverApproved = 1 AND ROUND(Quantity - QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") > 0) AND PlannedOrders.CustomerID = Customers.CustomerID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsReceiptPendingPlannedOrders", queryString);
        }

        private void GetGoodsReceiptPendingPlannedOrderCustomers()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          " + (int)@GlobalEnums.GoodsReceiptTypeID.FinishedProduct + " AS GoodsReceiptTypeID, Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, Warehouses.Name AS WarehouseName " + "\r\n";
            queryString = queryString + "       FROM            Customers " + "\r\n";

            queryString = queryString + "                       INNER JOIN Warehouses ON Warehouses.WarehouseID = 3 " + "\r\n";

            queryString = queryString + "       WHERE           CustomerID IN (SELECT DISTINCT CustomerID FROM FinishedProductPackages WHERE LocationID = @LocationID AND Approved = 1 AND HandoverApproved = 1 AND ROUND(Quantity - QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") > 0 GROUP BY CustomerID) " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsReceiptPendingPlannedOrderCustomers", queryString);
        }


        private void GetGoodsReceiptPendingPlannedOrderDetails()
        {
            string queryString;

            queryString = " @LocationID Int, @GoodsReceiptID Int, @PlannedOrderID Int, @CustomerID Int, @FinishedProductPackageIDs varchar(3999), @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF  (@PlannedOrderID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLFinishedProduct(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLFinishedProduct(false) + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsReceiptPendingPlannedOrderDetails", queryString);
        }

        private string BuildSQLFinishedProduct(bool isFinishedProductID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@FinishedProductPackageIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLFinishedProduct(isFinishedProductID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLFinishedProduct(isFinishedProductID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLFinishedProduct(bool isFinishedProductID, bool isFinishedProductPackageIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@GoodsReceiptID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLFinishedProductNew(isFinishedProductID, isFinishedProductPackageIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY FinishedProductPackages.EntryDate, FinishedProductPackages.FinishedProductID, FinishedProductPackages.FinishedProductPackageID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLFinishedProductEdit(isFinishedProductID, isFinishedProductPackageIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY FinishedProductPackages.EntryDate, FinishedProductPackages.FinishedProductID, FinishedProductPackages.FinishedProductPackageID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLFinishedProductNew(isFinishedProductID, isFinishedProductPackageIDs) + " WHERE FinishedProductPackages.FinishedProductPackageID NOT IN (SELECT FinishedProductPackageID FROM GoodsReceiptDetails WHERE GoodsReceiptID = @GoodsReceiptID) " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLFinishedProductEdit(isFinishedProductID, isFinishedProductPackageIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY FinishedProductPackages.EntryDate, FinishedProductPackages.FinishedProductID, FinishedProductPackages.FinishedProductPackageID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLFinishedProductNew(bool isFinishedProductID, bool isFinishedProductPackageIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      FinishedProductPackages.FinishedProductID, FinishedProductPackages.FinishedProductPackageID, FinishedProductPackages.EntryDate AS FinishedProductEntryDate, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, FinishedProductPackages.SemifinishedProductReferences, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   ROUND(FinishedProductPackages.Quantity - FinishedProductPackages.QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   0.0 AS Quantity, FinishedProductPackages.Remarks, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        FirmOrders " + "\r\n";
            queryString = queryString + "                   INNER JOIN FinishedProductPackages ON " + (isFinishedProductID ? " FinishedProductPackages.PlannedOrderID = @PlannedOrderID " : "FinishedProductPackages.LocationID = @LocationID AND FinishedProductPackages.CustomerID = @CustomerID ") + " AND FinishedProductPackages.Approved = 1 AND FinishedProductPackages.HandoverApproved = 1 AND ROUND(FinishedProductPackages.Quantity - FinishedProductPackages.QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") > 0 AND FirmOrders.FirmOrderID = FinishedProductPackages.FirmOrderID " + (isFinishedProductPackageIDs ? " AND FinishedProductPackages.FinishedProductPackageID NOT IN (SELECT Id FROM dbo.SplitToIntList (@FinishedProductPackageIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON FinishedProductPackages.CommodityID = Commodities.CommodityID " + "\r\n";

            return queryString;
        }

        private string BuildSQLFinishedProductEdit(bool isFinishedProductID, bool isFinishedProductPackageIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      FinishedProductPackages.FinishedProductID, FinishedProductPackages.FinishedProductPackageID, FinishedProductPackages.EntryDate AS FinishedProductEntryDate, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, FinishedProductPackages.SemifinishedProductReferences, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   ROUND(FinishedProductPackages.Quantity - FinishedProductPackages.QuantityReceipted + GoodsReceiptDetails.Quantity, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   0.0 AS Quantity, FinishedProductPackages.Remarks, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        FinishedProductPackages " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsReceiptDetails ON GoodsReceiptDetails.GoodsReceiptID = @GoodsReceiptID AND FinishedProductPackages.FinishedProductPackageID = GoodsReceiptDetails.FinishedProductPackageID" + (isFinishedProductPackageIDs ? " AND FinishedProductPackages.FinishedProductPackageID NOT IN (SELECT Id FROM dbo.SplitToIntList (@FinishedProductPackageIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN FirmOrders ON FinishedProductPackages.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON FinishedProductPackages.CommodityID = Commodities.CommodityID " + "\r\n";

            return queryString;
        }

        #endregion FinishedProduct




        #region MaterialIssue
        private void GetGoodsReceiptPendingMaterialIssueDetails()
        {
            string queryString;

            queryString = " @LocationID Int, @GoodsReceiptID Int, @MaterialIssueDetailIDs varchar(3999), @IsReadonly bit " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF  (@MaterialIssueDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLMaterialIssue(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLMaterialIssue(false) + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsReceiptPendingMaterialIssueDetails", queryString);
        }
        
        private string BuildSQLMaterialIssue(bool isMaterialIssueDetailIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@GoodsReceiptID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLMaterialIssueNew(isMaterialIssueDetailIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY MaterialIssueDetails.EntryDate, MaterialIssueDetails.MaterialIssueID, MaterialIssueDetails.MaterialIssueDetailID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLMaterialIssueEdit(isMaterialIssueDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY MaterialIssueDetails.EntryDate, MaterialIssueDetails.MaterialIssueID, MaterialIssueDetails.MaterialIssueDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLMaterialIssueNew(isMaterialIssueDetailIDs) + " WHERE MaterialIssueDetails.MaterialIssueDetailID NOT IN (SELECT MaterialIssueDetailID FROM GoodsReceiptDetails WHERE GoodsReceiptID = @GoodsReceiptID) " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLMaterialIssueEdit(isMaterialIssueDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY MaterialIssueDetails.EntryDate, MaterialIssueDetails.MaterialIssueID, MaterialIssueDetails.MaterialIssueDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLMaterialIssueNew(bool isMaterialIssueDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      MaterialIssueDetails.MaterialIssueID, MaterialIssueDetails.MaterialIssueDetailID, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, FirmOrders.Specs AS FirmOrderSpecs, MaterialIssueDetails.EntryDate AS MaterialIssueEntryDate, MaterialIssueDetails.BatchEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   ROUND(MaterialIssueDetails.Quantity - MaterialIssueDetails.QuantitySemifinished - MaterialIssueDetails.QuantityFailure - MaterialIssueDetails.QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, " + "\r\n"; //QuantityRemains NOT INCLUDED QuantityLoss, BUT: CHECK PENDING MUST MINUS QuantityLoss!!!!!!
            queryString = queryString + "                   0.0 AS Quantity, MaterialIssueDetails.Remarks, CAST(0 AS bit) AS IsSelected, " + "\r\n";
            queryString = queryString + "                   Workshifts.Name AS WorkshiftName, Workshifts.EntryDate AS WorkshiftEntryDate, ProductionLines.Code AS ProductionLinesCode " + "\r\n";

            queryString = queryString + "       FROM        FirmOrders " + "\r\n";
            queryString = queryString + "                   INNER JOIN MaterialIssueDetails ON MaterialIssueDetails.LocationID = @LocationID AND MaterialIssueDetails.Approved = 1 AND ROUND(MaterialIssueDetails.Quantity - MaterialIssueDetails.QuantitySemifinished - MaterialIssueDetails.QuantityFailure - MaterialIssueDetails.QuantityReceipted - MaterialIssueDetails.QuantityLoss, " + (int)GlobalEnums.rndQuantity + ") > 0 AND FirmOrders.FirmOrderID = MaterialIssueDetails.FirmOrderID" + (isMaterialIssueDetailIDs ? " AND MaterialIssueDetails.MaterialIssueDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@MaterialIssueDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON MaterialIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON MaterialIssueDetails.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN ProductionLines ON MaterialIssueDetails.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";

            return queryString;
        }

        private string BuildSQLMaterialIssueEdit(bool isMaterialIssueDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      MaterialIssueDetails.MaterialIssueID, MaterialIssueDetails.MaterialIssueDetailID, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, FirmOrders.Specs AS FirmOrderSpecs, MaterialIssueDetails.EntryDate AS MaterialIssueEntryDate, MaterialIssueDetails.BatchEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   ROUND(MaterialIssueDetails.Quantity - MaterialIssueDetails.QuantitySemifinished - MaterialIssueDetails.QuantityFailure - MaterialIssueDetails.QuantityReceipted + GoodsReceiptDetails.Quantity, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   0.0 AS Quantity, MaterialIssueDetails.Remarks, CAST(0 AS bit) AS IsSelected, " + "\r\n";
            queryString = queryString + "                   Workshifts.Name AS WorkshiftName, Workshifts.EntryDate AS WorkshiftEntryDate, ProductionLines.Code AS ProductionLinesCode " + "\r\n";

            queryString = queryString + "       FROM        MaterialIssueDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsReceiptDetails ON GoodsReceiptDetails.GoodsReceiptID = @GoodsReceiptID AND MaterialIssueDetails.MaterialIssueDetailID = GoodsReceiptDetails.MaterialIssueDetailID" + (isMaterialIssueDetailIDs ? " AND MaterialIssueDetails.MaterialIssueDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@MaterialIssueDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON MaterialIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN FirmOrders ON MaterialIssueDetails.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON MaterialIssueDetails.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN ProductionLines ON MaterialIssueDetails.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";

            return queryString;
        }
        #endregion MaterialIssue



        private void GoodsReceiptSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       BEGIN " + "\r\n";

            queryString = queryString + "           IF (@SaveRelativeOption = 1) ";
            queryString = queryString + "               BEGIN ";
            queryString = queryString + "                   UPDATE          GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                   SET             GoodsReceiptDetails.Reference = GoodsReceipts.Reference " + "\r\n";
            queryString = queryString + "                   FROM            GoodsReceipts INNER JOIN GoodsReceiptDetails ON GoodsReceipts.GoodsReceiptID = @EntityID AND GoodsReceipts.GoodsReceiptID = GoodsReceiptDetails.GoodsReceiptID " + "\r\n";
            queryString = queryString + "               END ";

            queryString = queryString + "           DECLARE @GoodsReceiptTypeID int, @AffectedROWCOUNT int ";
            queryString = queryString + "           SELECT  @GoodsReceiptTypeID = GoodsReceiptTypeID FROM GoodsReceipts WHERE GoodsReceiptID = @EntityID ";

            queryString = queryString + "           IF (@GoodsReceiptTypeID > 0) " + "\r\n";
            queryString = queryString + "               BEGIN  " + "\r\n";

            queryString = queryString + "                   IF (@GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseRequisition + ") " + "\r\n";
            queryString = queryString + "                       BEGIN  " + "\r\n";
            queryString = queryString + "                           UPDATE          PurchaseRequisitionDetails " + "\r\n";
            queryString = queryString + "                           SET             PurchaseRequisitionDetails.QuantityReceipted = ROUND(PurchaseRequisitionDetails.QuantityReceipted + GoodsReceiptDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "                           FROM            GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                                           INNER JOIN PurchaseRequisitionDetails ON ((PurchaseRequisitionDetails.Approved = 1 AND PurchaseRequisitionDetails.InActive = 0 AND PurchaseRequisitionDetails.InActivePartial = 0) OR @SaveRelativeOption = -1) AND GoodsReceiptDetails.GoodsReceiptID = @EntityID AND GoodsReceiptDetails.PurchaseRequisitionDetailID = PurchaseRequisitionDetails.PurchaseRequisitionDetailID " + "\r\n";
            queryString = queryString + "                           SET @AffectedROWCOUNT = @@ROWCOUNT " + "\r\n";
            queryString = queryString + "                       END " + "\r\n";

            queryString = queryString + "                   IF (@GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.WarehouseTransfer + ") " + "\r\n";
            queryString = queryString + "                       BEGIN  " + "\r\n";
            queryString = queryString + "                           UPDATE          WarehouseTransferDetails " + "\r\n";
            queryString = queryString + "                           SET             WarehouseTransferDetails.QuantityReceipted = ROUND(WarehouseTransferDetails.QuantityReceipted + GoodsReceiptDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "                           FROM            GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                                           INNER JOIN WarehouseTransferDetails ON (WarehouseTransferDetails.Approved = 1 OR @SaveRelativeOption = -1) AND GoodsReceiptDetails.GoodsReceiptID = @EntityID AND GoodsReceiptDetails.WarehouseTransferDetailID = WarehouseTransferDetails.WarehouseTransferDetailID " + "\r\n";
            queryString = queryString + "                           SET @AffectedROWCOUNT = @@ROWCOUNT " + "\r\n";
            queryString = queryString + "                       END " + "\r\n";

            queryString = queryString + "                   IF (@GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.FinishedProduct + ") " + "\r\n";
            queryString = queryString + "                       BEGIN  " + "\r\n";
            queryString = queryString + "                           UPDATE          FinishedProductPackages " + "\r\n";
            queryString = queryString + "                           SET             FinishedProductPackages.QuantityReceipted = ROUND(FinishedProductPackages.QuantityReceipted + GoodsReceiptDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "                           FROM            GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                                           INNER JOIN FinishedProductPackages ON ((FinishedProductPackages.Approved = 1 AND FinishedProductPackages.HandoverApproved = 1) OR @SaveRelativeOption = -1) AND GoodsReceiptDetails.GoodsReceiptID = @EntityID AND GoodsReceiptDetails.FinishedProductPackageID = FinishedProductPackages.FinishedProductPackageID " + "\r\n";
            queryString = queryString + "                           SET @AffectedROWCOUNT = @@ROWCOUNT " + "\r\n";
            queryString = queryString + "                       END " + "\r\n";


            //queryString = queryString + "                   IF (@GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.GoodsIssueTransfer + ") " + "\r\n";
            //queryString = queryString + "                       BEGIN  " + "\r\n";
            //queryString = queryString + "                           UPDATE          GoodsIssueTransferDetails " + "\r\n";
            //queryString = queryString + "                           SET             GoodsIssueTransferDetails.QuantityReceipted = ROUND(GoodsIssueTransferDetails.QuantityReceipted + GoodsReceiptDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            //queryString = queryString + "                           FROM            GoodsReceiptDetails " + "\r\n";
            //queryString = queryString + "                                           INNER JOIN GoodsIssueTransferDetails ON GoodsReceiptDetails.GoodsReceiptID = @EntityID AND GoodsIssueTransferDetails.Approved = 1 AND GoodsReceiptDetails.GoodsIssueTransferDetailID = GoodsIssueTransferDetails.GoodsIssueTransferDetailID " + "\r\n";
            //queryString = queryString + "                           SET @AffectedROWCOUNT = @@ROWCOUNT " + "\r\n";
            //queryString = queryString + "                       END " + "\r\n";


            queryString = queryString + "                   IF (@GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.MaterialIssue + ") " + "\r\n";
            queryString = queryString + "                       BEGIN  " + "\r\n";
            queryString = queryString + "                           UPDATE          MaterialIssueDetails " + "\r\n";
            queryString = queryString + "                           SET             MaterialIssueDetails.QuantityReceipted = ROUND(MaterialIssueDetails.QuantityReceipted + GoodsReceiptDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + "), " + "\r\n";
            queryString = queryString + "                                           MaterialIssueDetails.QuantityLoss = IIF(@SaveRelativeOption = 1, ROUND(MaterialIssueDetails.Quantity - MaterialIssueDetails.QuantitySemifinished - MaterialIssueDetails.QuantityFailure - (MaterialIssueDetails.QuantityReceipted + GoodsReceiptDetails.Quantity), " + (int)GlobalEnums.rndQuantity + "), 0) " + "\r\n"; //UPDATE: SET QuantityLoss = Remains, UNDO: SET QuantityLoss = 0            
            queryString = queryString + "                           FROM            GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                                           INNER JOIN MaterialIssueDetails ON (MaterialIssueDetails.Approved = 1 OR @SaveRelativeOption = -1) AND GoodsReceiptDetails.GoodsReceiptID = @EntityID AND GoodsReceiptDetails.MaterialIssueDetailID = MaterialIssueDetails.MaterialIssueDetailID " + "\r\n";
            queryString = queryString + "                           SET @AffectedROWCOUNT = @@ROWCOUNT " + "\r\n";
            queryString = queryString + "                       END " + "\r\n";


            queryString = queryString + "                   IF (@GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.WarehouseAdjustments + ") " + "\r\n";
            queryString = queryString + "                       BEGIN  " + "\r\n";
            queryString = queryString + "                           UPDATE          WarehouseAdjustmentDetails " + "\r\n";
            queryString = queryString + "                           SET             WarehouseAdjustmentDetails.QuantityReceipted = ROUND(WarehouseAdjustmentDetails.QuantityReceipted + GoodsReceiptDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "                           FROM            GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                                           INNER JOIN WarehouseAdjustmentDetails ON GoodsReceiptDetails.GoodsReceiptID = @EntityID AND WarehouseAdjustmentDetails.Quantity > 0 AND GoodsReceiptDetails.WarehouseAdjustmentDetailID = WarehouseAdjustmentDetails.WarehouseAdjustmentDetailID " + "\r\n";
            queryString = queryString + "                           SET @AffectedROWCOUNT = @@ROWCOUNT " + "\r\n";
            //------------------------------------------------------SHOULD UPDATE GoodsReceiptID, GoodsReceiptDetailID BACK TO WarehouseAdjustmentDetails FOR GoodsReceipts OF WarehouseAdjustmentDetails? THE ANSWER: WE CAN DO IT HERE, BUT IT BREAK THE RELATIONSHIP (cyclic redundancy relationship: GoodsReceiptDetails => WarehouseAdjustmentDetails => THUS: WE SHOULD NOT MAKE ANOTHER RELATIONSHIP FROM WarehouseAdjustmentDetails => GoodsReceiptDetails ) => SO: SHOULD NOT!!!
            queryString = queryString + "                       END " + "\r\n";

            queryString = queryString + "                   IF @AffectedROWCOUNT <> (SELECT COUNT(*) FROM GoodsReceiptDetails WHERE GoodsReceiptID = @EntityID) " + "\r\n";
            queryString = queryString + "                       BEGIN " + "\r\n";
            queryString = queryString + "                           DECLARE     @msg NVARCHAR(300) = N'Phiếu giao hàng đã hủy, hoặc chưa duyệt' ; " + "\r\n";
            queryString = queryString + "                           THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "                       END " + "\r\n";

            queryString = queryString + "               END  " + "\r\n";

            queryString = queryString + "       END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GoodsReceiptSaveRelative", queryString);
        }

        private void GoodsReceiptPostSaveValidate()
        {
            string[] queryArray = new string[8];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày đặt hàng: ' + CAST(PurchaseRequisitions.EntryDate AS nvarchar) FROM GoodsReceiptDetails INNER JOIN PurchaseRequisitions ON GoodsReceiptDetails.GoodsReceiptID = @EntityID AND GoodsReceiptDetails.PurchaseRequisitionID = PurchaseRequisitions.PurchaseRequisitionID AND GoodsReceiptDetails.EntryDate < PurchaseRequisitions.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Số lượng xuất vượt quá số lượng đặt hàng: ' + CAST(ROUND(Quantity - QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM PurchaseRequisitionDetails WHERE (ROUND(Quantity - QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            queryArray[2] = " SELECT TOP 1 @FoundEntity = N'Ngày đặt hàng: ' + CAST(FinishedProducts.EntryDate AS nvarchar) FROM GoodsReceiptDetails INNER JOIN FinishedProducts ON GoodsReceiptDetails.GoodsReceiptID = @EntityID AND GoodsReceiptDetails.FinishedProductID = FinishedProducts.FinishedProductID AND GoodsReceiptDetails.EntryDate < FinishedProducts.EntryDate ";
            queryArray[3] = " SELECT TOP 1 @FoundEntity = N'Số lượng xuất vượt quá số lượng đặt hàng: ' + CAST(ROUND(Quantity - QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM FinishedProductPackages WHERE (ROUND(Quantity - QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            queryArray[4] = " SELECT TOP 1 @FoundEntity = N'Ngày đặt hàng: ' + CAST(MaterialIssues.EntryDate AS nvarchar) FROM GoodsReceiptDetails INNER JOIN MaterialIssues ON GoodsReceiptDetails.GoodsReceiptID = @EntityID AND GoodsReceiptDetails.MaterialIssueID = MaterialIssues.MaterialIssueID AND GoodsReceiptDetails.EntryDate < MaterialIssues.EntryDate ";
            queryArray[5] = " SELECT TOP 1 @FoundEntity = N'Số lượng NVL sử dụng vượt quá số lượng đã xuất kho cho sản xuất: ' + CAST(ROUND(Quantity - QuantitySemifinished - QuantityFailure - QuantityReceipted - QuantityLoss, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM MaterialIssueDetails WHERE (ROUND(Quantity - QuantitySemifinished - QuantityFailure - QuantityReceipted - QuantityLoss, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            queryArray[6] = " SELECT TOP 1 @FoundEntity = N'Ngày đặt hàng: ' + CAST(WarehouseTransfers.EntryDate AS nvarchar) FROM GoodsReceiptDetails INNER JOIN WarehouseTransfers ON GoodsReceiptDetails.GoodsReceiptID = @EntityID AND GoodsReceiptDetails.WarehouseTransferID = WarehouseTransfers.WarehouseTransferID AND GoodsReceiptDetails.EntryDate < WarehouseTransfers.EntryDate ";
            queryArray[7] = " SELECT TOP 1 @FoundEntity = N'Số lượng xuất vượt quá số lượng đặt hàng: ' + CAST(ROUND(Quantity - QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM WarehouseTransferDetails WHERE (ROUND(Quantity - QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") < 0) ";


            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("GoodsReceiptPostSaveValidate", queryArray);
        }




        private void GoodsReceiptApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = GoodsReceiptID FROM GoodsReceipts WHERE GoodsReceiptID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("GoodsReceiptApproved", queryArray);
        }


        private void GoodsReceiptEditable()
        {
            string[] queryArray = new string[3]; //IMPORTANT: THESE QUERIES SHOULD BE COPIED TO WarehouseAdjustmentEditable

            queryArray[0] = " SELECT TOP 1 @FoundEntity = GoodsReceiptID FROM MaterialIssueDetails WHERE GoodsReceiptID = @EntityID ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = GoodsReceiptID FROM WarehouseTransferDetails WHERE GoodsReceiptID = @EntityID ";
            queryArray[2] = " SELECT TOP 1 @FoundEntity = GoodsReceiptID FROM WarehouseAdjustmentDetails WHERE GoodsReceiptID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("GoodsReceiptEditable", queryArray);
        }

        private void GoodsReceiptToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      GoodsReceipts  SET Approved = @Approved, ApprovedDate = GetDate() WHERE GoodsReceiptID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          GoodsReceiptDetails  SET Approved = @Approved WHERE GoodsReceiptID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, N'hủy', '')  + N' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GoodsReceiptToggleApproved", queryString);
        }
        

        private void GoodsReceiptInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("GoodsReceipts", "GoodsReceiptID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.GoodsReceipt));
            this.totalSmartPortalEntities.CreateTrigger("GoodsReceiptInitReference", simpleInitReference.CreateQuery());
        }




        private void GetGoodsReceiptIDofWarehouseAdjustment()
        {
            string queryString;

            queryString = " @WarehouseAdjustmentID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   SELECT TOP 1 GoodsReceiptID FROM GoodsReceipts WHERE WarehouseAdjustmentID = @WarehouseAdjustmentID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsReceiptIDofWarehouseAdjustment", queryString);
        }

        private void GetGoodsReceiptDetailAvailables()
        {
            string queryString = " @LocationID Int, @WarehouseID Int, @CommodityID Int, @CommodityIDs varchar(3999), @BatchID Int, @GoodsReceiptDetailIDs varchar(3999), @OnlyApproved bit, @OnlyIssuable bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF  (@WarehouseID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.GetGoodsReceiptDetailAvailableSQL(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetGoodsReceiptDetailAvailableSQL(false) + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsReceiptDetailAvailables", queryString);
        }

        private string GetGoodsReceiptDetailAvailableSQL(bool isWarehouseID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF  (@CommodityID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.GetGoodsReceiptDetailAvailableSQL(isWarehouseID, true, false) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               IF  (@CommodityIDs <> '') " + "\r\n";
            queryString = queryString + "                   " + this.GetGoodsReceiptDetailAvailableSQL(isWarehouseID, false, true) + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n";
            queryString = queryString + "                   " + this.GetGoodsReceiptDetailAvailableSQL(isWarehouseID, false, false) + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetGoodsReceiptDetailAvailableSQL(bool isWarehouseID, bool isCommodityID, bool isCommodityIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (NOT @BatchID IS NULL) " + "\r\n";
            queryString = queryString + "           " + this.GetGoodsReceiptDetailAvailableSQL(isWarehouseID, isCommodityID, isCommodityIDs, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetGoodsReceiptDetailAvailableSQL(isWarehouseID, isCommodityID, isCommodityIDs, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetGoodsReceiptDetailAvailableSQL(bool isWarehouseID, bool isCommodityID, bool isCommodityIDs, bool isBatchID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@GoodsReceiptDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.GetGoodsReceiptDetailAvailableSQL(isWarehouseID, isCommodityID, isCommodityIDs, isBatchID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.GetGoodsReceiptDetailAvailableSQL(isWarehouseID, isCommodityID, isCommodityIDs, isBatchID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string GetGoodsReceiptDetailAvailableSQL(bool isWarehouseID, bool isCommodityID, bool isCommodityIDs, bool isBatchID, bool isGoodsReceiptDetailIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      GoodsReceiptDetails.GoodsReceiptID, GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceiptDetails.Reference AS GoodsReceiptReference, GoodsReceiptDetails.Code AS GoodsReceiptCode, GoodsReceiptDetails.EntryDate AS GoodsReceiptEntryDate, GoodsReceiptDetails.BatchID, GoodsReceiptDetails.BatchEntryDate, GoodsReceiptDetails.WarehouseID, Warehouses.Code AS WarehouseCode, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.OfficialCode, Commodities.CodePartA, Commodities.CodePartB, Commodities.CodePartC, Commodities.CodePartD, Commodities.CodePartE, Commodities.CodePartF, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   ISNULL('Production: ' + ' ' + PurchaseRequisitions.Reference, ISNULL('From: ' + ' ' + WarehouseIssues.Name, ISNULL(WarehouseAdjustmentTypes.Name  + ' ' + WarehouseAdjustments.AdjustmentJobs, ''))) AS LineReferences, GoodsReceiptDetails.Approved, GoodsReceiptDetails.Remarks, Warehouses.Issuable, " + "\r\n";
            queryString = queryString + "                   ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + GlobalEnums.rndQuantity + ") AS QuantityAvailables, ISNULL(CAST(0 AS bit), CAST(0 AS bit)) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Warehouses ON ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + GlobalEnums.rndQuantity + ") > 0 AND GoodsReceiptDetails.LocationID = @LocationID " + (isWarehouseID ? " AND GoodsReceiptDetails.WarehouseID = @WarehouseID" : "") + (isCommodityID ? " AND GoodsReceiptDetails.CommodityID = @CommodityID" : "") + (isCommodityIDs ? " AND GoodsReceiptDetails.CommodityID IN (SELECT Id FROM dbo.SplitToIntList (@CommodityIDs))" : "") + " AND (@OnlyApproved = 0 OR GoodsReceiptDetails.Approved = 1) AND (@OnlyIssuable = 0 OR Warehouses.Issuable = 1) AND GoodsReceiptDetails.WarehouseID = Warehouses.WarehouseID " + (isBatchID ? " AND GoodsReceiptDetails.BatchID = @BatchID" : "") + (isGoodsReceiptDetailIDs ? " AND GoodsReceiptDetails.GoodsReceiptDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@GoodsReceiptDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON GoodsReceiptDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "                   LEFT JOIN PurchaseRequisitions ON GoodsReceiptDetails.PurchaseRequisitionID = PurchaseRequisitions.PurchaseRequisitionID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN Warehouses WarehouseIssues ON GoodsReceiptDetails.WarehouseIssueID = WarehouseIssues.WarehouseID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN WarehouseAdjustments ON GoodsReceiptDetails.WarehouseAdjustmentID = WarehouseAdjustments.WarehouseAdjustmentID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN WarehouseAdjustmentTypes ON WarehouseAdjustments.WarehouseAdjustmentTypeID = WarehouseAdjustmentTypes.WarehouseAdjustmentTypeID " + "\r\n";

            queryString = queryString + "       ORDER BY    GoodsReceiptDetails.BatchEntryDate, GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }




        #region Generate Pending

        private class GenerateSQLPendingDetails
        {
            private readonly GlobalEnums.GoodsReceiptTypeID goodsReceiptTypeID;

            private readonly string primaryTables;
            private readonly string primaryTableDetails;

            private readonly string primaryKey;
            private readonly string paraPrimaryKey;

            private readonly string primaryKeyDetail;
            private readonly string paraPrimaryKeyDetails;

            private readonly string fieldNameWarehouseID;

            private readonly string primaryReference;
            private readonly string primaryEntryDate;

            private readonly TotalSmartPortalEntities totalSmartPortalEntities;

            public GenerateSQLPendingDetails(TotalSmartPortalEntities totalSmartPortalEntities, GlobalEnums.GoodsReceiptTypeID goodsReceiptTypeID, string primaryTables, string primaryTableDetails, string primaryKey, string paraPrimaryKey, string primaryKeyDetails, string paraPrimaryKeyDetails, string fieldNameWarehouseID, string primaryReference, string primaryEntryDate)
            {
                this.totalSmartPortalEntities = totalSmartPortalEntities;

                this.goodsReceiptTypeID = goodsReceiptTypeID;

                this.primaryTables = primaryTables;
                this.primaryTableDetails = primaryTableDetails;

                this.primaryKey = primaryKey;
                this.paraPrimaryKey = paraPrimaryKey;

                this.primaryKeyDetail = primaryKeyDetails;
                this.paraPrimaryKeyDetails = paraPrimaryKeyDetails;

                this.fieldNameWarehouseID = fieldNameWarehouseID;

                this.primaryReference = primaryReference;
                this.primaryEntryDate = primaryEntryDate;
            }





            public void GetPendingPickups(string myfunctionName)
            {
                string queryString = " @LocationID int " + "\r\n";
                //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
                queryString = queryString + " AS " + "\r\n";

                queryString = queryString + "       SELECT          " + this.primaryTables + "." + this.primaryKey + ", " + this.primaryTables + ".Reference AS " + this.primaryReference + ", " + this.primaryTables + ".EntryDate AS " + this.primaryEntryDate + ", Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, Warehouses.Name AS WarehouseName, " + (int)this.goodsReceiptTypeID + " AS GoodsReceiptTypeID, (SELECT TOP 1 Name FROM GoodsReceiptTypes WHERE GoodsReceiptTypeID = " + (int)this.goodsReceiptTypeID + ") AS GoodsReceiptTypeName, " + this.primaryTables + ".Description, " + this.primaryTables + ".Remarks " + (this.goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.GoodsIssueTransfer ? ", GoodsIssues.VoucherCodes, SourceWarehouses.Name AS SourceWarehouseName " : "") + "\r\n";
                queryString = queryString + "       FROM           (SELECT " + this.primaryKey + ", " + fieldNameWarehouseID + (this.goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.GoodsIssueTransfer && fieldNameWarehouseID.IndexOf("WarehouseID") == -1 ? ", WarehouseID" : "") + ", Reference, EntryDate, Description, Remarks" + (this.goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.GoodsIssueTransfer ? ", VoucherCodes" : "") + " FROM " + this.primaryTables + " WHERE " + this.primaryKey + " IN (SELECT " + this.primaryKey + " FROM " + this.primaryTableDetails + " WHERE LocationID = @LocationID AND Approved = 1 AND ROUND(Quantity - QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") > 0)) AS " + this.primaryTables + "\r\n";
                queryString = queryString + "                       INNER JOIN Warehouses ON " + this.primaryTables + "." + fieldNameWarehouseID + " = Warehouses.WarehouseID " + "\r\n";
                if (this.goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.GoodsIssueTransfer)
                    queryString = queryString + "                   INNER JOIN Warehouses SourceWarehouses ON GoodsIssues.WarehouseID = SourceWarehouses.WarehouseID " + "\r\n";

                this.totalSmartPortalEntities.CreateStoredProcedure(myfunctionName, queryString);
            }

            public void GetPendingPickupWarehouses(string myfunctionName)
            {
                string queryString = " @LocationID int " + "\r\n";
                //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
                queryString = queryString + " AS " + "\r\n";

                queryString = queryString + "       SELECT          Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, Warehouses.Name AS WarehouseName, " + (int)this.goodsReceiptTypeID + " AS GoodsReceiptTypeID, (SELECT TOP 1 Name FROM GoodsReceiptTypes WHERE GoodsReceiptTypeID = " + (int)this.goodsReceiptTypeID + ") AS GoodsReceiptTypeName " + "\r\n";
                queryString = queryString + "       FROM           (SELECT DISTINCT " + fieldNameWarehouseID + " FROM " + this.primaryTableDetails + " WHERE LocationID = @LocationID AND Approved = 1 AND ROUND(Quantity - QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") > 0) WarehousePENDING " + "\r\n";
                queryString = queryString + "                       INNER JOIN Warehouses ON WarehousePENDING." + fieldNameWarehouseID + " = Warehouses.WarehouseID " + "\r\n";

                this.totalSmartPortalEntities.CreateStoredProcedure(myfunctionName, queryString);
            }




















            public void GetPendingPickupDetails(string myfunctionName)
            {
                string queryString;

                queryString = " @LocationID Int, @GoodsReceiptID Int, " + this.paraPrimaryKey + " Int, @WarehouseID Int, " + this.paraPrimaryKeyDetails + " varchar(3999), @IsReadonly bit " + "\r\n";
                //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
                queryString = queryString + " AS " + "\r\n";

                queryString = queryString + "   BEGIN " + "\r\n";

                queryString = queryString + "       IF  (" + this.paraPrimaryKey + " <> 0) " + "\r\n";
                queryString = queryString + "           " + this.BuildSQLPickup(true) + "\r\n";
                queryString = queryString + "       ELSE " + "\r\n";
                queryString = queryString + "           " + this.BuildSQLPickup(false) + "\r\n";

                queryString = queryString + "   END " + "\r\n";

                this.totalSmartPortalEntities.CreateStoredProcedure(myfunctionName, queryString);
            }

            private string BuildSQLPickup(bool isPickupID)
            {
                string queryString = "";
                queryString = queryString + "   BEGIN " + "\r\n";
                queryString = queryString + "       IF  (" + this.paraPrimaryKeyDetails + " <> '') " + "\r\n";
                queryString = queryString + "           " + this.BuildSQLPickupPickupDetailIDs(isPickupID, true) + "\r\n";
                queryString = queryString + "       ELSE " + "\r\n";
                queryString = queryString + "           " + this.BuildSQLPickupPickupDetailIDs(isPickupID, false) + "\r\n";
                queryString = queryString + "   END " + "\r\n";

                return queryString;
            }

            private string BuildSQLPickupPickupDetailIDs(bool isPickupID, bool isPickupDetailIDs)
            {
                string queryString = "";
                queryString = queryString + "   BEGIN " + "\r\n";

                queryString = queryString + "       IF (@GoodsReceiptID <= 0) " + "\r\n";
                queryString = queryString + "               BEGIN " + "\r\n";
                queryString = queryString + "                   " + this.BuildSQLNew(isPickupID, isPickupDetailIDs) + "\r\n";
                queryString = queryString + "                   ORDER BY " + this.primaryTableDetails + ".EntryDate, " + this.primaryTableDetails + "." + this.primaryKey + ", " + this.primaryTableDetails + "." + this.primaryKeyDetail + " " + "\r\n";
                queryString = queryString + "               END " + "\r\n";
                queryString = queryString + "       ELSE " + "\r\n";

                queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
                queryString = queryString + "                   BEGIN " + "\r\n";
                queryString = queryString + "                       " + this.BuildSQLEdit(isPickupID, isPickupDetailIDs) + "\r\n";
                queryString = queryString + "                       ORDER BY " + this.primaryTableDetails + ".EntryDate, " + this.primaryTableDetails + "." + this.primaryKey + ", " + this.primaryTableDetails + "." + this.primaryKeyDetail + " " + "\r\n";
                queryString = queryString + "                   END " + "\r\n";

                queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

                queryString = queryString + "                   BEGIN " + "\r\n";
                queryString = queryString + "                       " + this.BuildSQLNew(isPickupID, isPickupDetailIDs) + " WHERE " + this.primaryTableDetails + "." + this.primaryKeyDetail + " NOT IN (SELECT " + this.primaryKeyDetail + " FROM GoodsReceiptDetails WHERE GoodsReceiptID = @GoodsReceiptID) " + "\r\n";
                queryString = queryString + "                       UNION ALL " + "\r\n";
                queryString = queryString + "                       " + this.BuildSQLEdit(isPickupID, isPickupDetailIDs) + "\r\n";
                queryString = queryString + "                       ORDER BY " + this.primaryTableDetails + ".EntryDate, " + this.primaryTableDetails + "." + this.primaryKey + ", " + this.primaryTableDetails + "." + this.primaryKeyDetail + " " + "\r\n";
                queryString = queryString + "                   END " + "\r\n";

                queryString = queryString + "   END " + "\r\n";

                return queryString;
            }

            private string BuildSQLNew(bool isPickupID, bool isPickupDetailIDs)
            {
                string queryString = "";

                queryString = queryString + "       SELECT      " + this.primaryTableDetails + "." + this.primaryKey + ", " + this.primaryTableDetails + "." + this.primaryKeyDetail + ", " + this.primaryTableDetails + ".Reference " + (this.goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.GoodsIssueTransfer ? " + ' [' + GoodsIssues.VoucherCodes + ']' " : "") + " AS " + this.primaryReference + ", " + this.primaryTableDetails + ".EntryDate AS " + this.primaryEntryDate + ", " + "\r\n";
                queryString = queryString + "                   " + (this.goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.GoodsIssueTransfer ? this.primaryTableDetails + ".LocationIssueID + 0" : "NULL") + " AS LocationIssueID, " + (this.goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.GoodsIssueTransfer ? this.primaryTableDetails + ".WarehouseID + 0" : "NULL") + " AS WarehouseIssueID, " + (this.goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.WarehouseAdjustments ? this.primaryTableDetails + ".WarehouseAdjustmentTypeID + 0" : "NULL") + " AS WarehouseAdjustmentTypeID, " + "\r\n";
                queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, " + this.primaryTableDetails + ".BatchID, " + this.primaryTableDetails + ".BatchEntryDate, " + "\r\n";
                queryString = queryString + "                   ROUND(" + this.primaryTableDetails + ".Quantity - " + this.primaryTableDetails + ".QuantityReceipted,  " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, CAST(0 AS decimal(18, 2)) AS Quantity, " + this.primaryTableDetails + ".Remarks, CAST(" + (this.goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.Pickup ? "1" : "0") + " AS bit) AS IsSelected " + "\r\n";

                queryString = queryString + "       FROM        " + this.primaryTableDetails + " " + "\r\n";
                queryString = queryString + "                   INNER JOIN Commodities ON " + (isPickupID ? " " + this.primaryTableDetails + "." + this.primaryKey + " = " + this.paraPrimaryKey + " " : "" + this.primaryTableDetails + ".LocationID = @LocationID AND " + this.primaryTableDetails + "." + fieldNameWarehouseID + " = @WarehouseID ") + " AND " + this.primaryTableDetails + (this.primaryTableDetails == "WarehouseAdjustmentDetails" ? ".Quantity > 0" : ".Approved = 1") + " AND ROUND(" + this.primaryTableDetails + ".Quantity - " + this.primaryTableDetails + ".QuantityReceipted, " + (int)GlobalEnums.rndQuantity + ") > 0 AND " + this.primaryTableDetails + ".CommodityID = Commodities.CommodityID " + (isPickupDetailIDs ? " AND " + this.primaryTableDetails + "." + this.primaryKeyDetail + " NOT IN (SELECT Id FROM dbo.SplitToIntList (" + this.paraPrimaryKeyDetails + "))" : "") + "\r\n";

                if (this.goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.GoodsIssueTransfer)
                    queryString = queryString + "               INNER JOIN GoodsIssues ON GoodsIssueTransferDetails.GoodsIssueID = GoodsIssues.GoodsIssueID " + "\r\n";

                return queryString;
            }

            private string BuildSQLEdit(bool isPickupID, bool isPickupDetailIDs)
            {
                string queryString = "";

                queryString = queryString + "       SELECT      " + this.primaryTableDetails + "." + this.primaryKey + ", " + this.primaryTableDetails + "." + this.primaryKeyDetail + ", " + this.primaryTableDetails + ".Reference " + (this.goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.GoodsIssueTransfer ? " + ' [' + GoodsIssues.VoucherCodes + ']' " : "") + " AS " + this.primaryReference + ", " + this.primaryTableDetails + ".EntryDate AS " + this.primaryEntryDate + ", " + "\r\n";
                queryString = queryString + "                   " + (this.goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.GoodsIssueTransfer ? this.primaryTableDetails + ".LocationIssueID + 0" : "NULL") + " AS LocationIssueID, " + (this.goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.GoodsIssueTransfer ? this.primaryTableDetails + ".WarehouseID + 0" : "NULL") + " AS WarehouseIssueID, " + (this.goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.WarehouseAdjustments ? this.primaryTableDetails + ".WarehouseAdjustmentTypeID + 0" : "NULL") + " AS WarehouseAdjustmentTypeID, " + "\r\n";
                queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, " + this.primaryTableDetails + ".BatchID, " + this.primaryTableDetails + ".BatchEntryDate, " + "\r\n";
                queryString = queryString + "                   ROUND(" + this.primaryTableDetails + ".Quantity - " + this.primaryTableDetails + ".QuantityReceipted + GoodsReceiptDetails.Quantity,  " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, CAST(0 AS decimal(18, 2)) AS Quantity, " + this.primaryTableDetails + ".Remarks, CAST(" + (this.goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.Pickup ? "1" : "0") + " AS bit) AS IsSelected " + "\r\n";

                queryString = queryString + "       FROM        " + this.primaryTableDetails + " " + "\r\n";
                queryString = queryString + "                   INNER JOIN GoodsReceiptDetails ON GoodsReceiptDetails.GoodsReceiptID = @GoodsReceiptID AND " + this.primaryTableDetails + "." + this.primaryKeyDetail + " = GoodsReceiptDetails." + this.primaryKeyDetail + "" + (isPickupDetailIDs ? " AND " + this.primaryTableDetails + "." + this.primaryKeyDetail + " NOT IN (SELECT Id FROM dbo.SplitToIntList (" + this.paraPrimaryKeyDetails + "))" : "") + "\r\n";

                if (this.goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.GoodsIssueTransfer)
                    queryString = queryString + "               INNER JOIN GoodsIssues ON GoodsIssueTransferDetails.GoodsIssueID = GoodsIssues.GoodsIssueID " + "\r\n";

                queryString = queryString + "                   INNER JOIN Commodities ON GoodsReceiptDetails.CommodityID = Commodities.CommodityID " + "\r\n";

                return queryString;
            }
        }
        #endregion Generate Pending
    }
}
