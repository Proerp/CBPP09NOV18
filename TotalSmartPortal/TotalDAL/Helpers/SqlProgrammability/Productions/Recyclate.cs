using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Productions
{
    public class Recyclate
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public Recyclate(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetRecyclateIndexes();

            this.GetRecyclatePendingWorkshifts();

            this.GetRecyclateViewDetails();
            this.GetRecyclateViewPackages();

            this.RecyclateSaveRelative();
            this.RecyclatePostSaveValidate();

            this.RecyclateApproved();
            this.RecyclateEditable();

            this.RecyclateToggleApproved();

            this.RecyclateInitReference();

            this.RecyclateSheet();
        }


        private void GetRecyclateIndexes()
        {
            string queryString;

            queryString = " @NMVNTaskID int, @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      Recyclates.RecyclateID, CAST(Recyclates.EntryDate AS DATE) AS EntryDate, Recyclates.Reference, Locations.Code AS LocationCode, Workshifts.EntryDate AS WorkshiftEntryDate, Workshifts.Code AS WorkshiftCode, Recyclates.Description, Recyclates.Caption, Recyclates.TotalQuantity, Recyclates.Approved " + "\r\n";
            queryString = queryString + "       FROM        Recyclates " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON Recyclates.NMVNTaskID = @NMVNTaskID AND Recyclates.EntryDate >= @FromDate AND Recyclates.EntryDate <= @ToDate AND Recyclates.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = @NMVNTaskID AND AccessControls.AccessLevel > 0) AND Locations.LocationID = Recyclates.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON Recyclates.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetRecyclateIndexes", queryString);
        }

        private string nmvnTaskTable(GlobalEnums.NmvnTaskID nmvnTaskID)
        {
            return nmvnTaskID == GlobalEnums.NmvnTaskID.SemifinishedProductRecyclate ? "SemifinishedProducts" : (nmvnTaskID == GlobalEnums.NmvnTaskID.FinishedProductRecyclate ? "FinishedProductPackages" : (nmvnTaskID == GlobalEnums.NmvnTaskID.FinishedItemRecyclate ? "FinishedItemDetails" : ""));
        }

        private string nmvnTaskPrimaryKey(GlobalEnums.NmvnTaskID nmvnTaskID)
        {
            return nmvnTaskID == GlobalEnums.NmvnTaskID.SemifinishedProductRecyclate ? "SemifinishedProductID" : (nmvnTaskID == GlobalEnums.NmvnTaskID.FinishedProductRecyclate ? "FinishedProductPackageID" : (nmvnTaskID == GlobalEnums.NmvnTaskID.FinishedItemRecyclate ? "FinishedItemDetailID" : ""));
        }

        private string nmvnTaskFailures(GlobalEnums.NmvnTaskID nmvnTaskID)
        {
            return nmvnTaskID == GlobalEnums.NmvnTaskID.SemifinishedProductRecyclate ? "SemifinishedProducts.RejectWeights" : (nmvnTaskID == GlobalEnums.NmvnTaskID.FinishedProductRecyclate ? "FinishedProductPackages.QuantityFailureWeights" : (nmvnTaskID == GlobalEnums.NmvnTaskID.FinishedItemRecyclate ? "FinishedItemDetails.QuantityFailure" : ""));
        }

        private string nmvnTaskSwarfs(GlobalEnums.NmvnTaskID nmvnTaskID)
        {
            return nmvnTaskID == GlobalEnums.NmvnTaskID.SemifinishedProductRecyclate ? "SemifinishedProducts.FailureWeights" : (nmvnTaskID == GlobalEnums.NmvnTaskID.FinishedProductRecyclate ? "FinishedProductPackages.Swarfs" : (nmvnTaskID == GlobalEnums.NmvnTaskID.FinishedItemRecyclate ? "FinishedItemDetails.Swarfs" : ""));
        }

        private void GetRecyclatePendingWorkshifts()
        {
            string queryString = " @NMVNTaskID int, @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       IF (@NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.SemifinishedProductRecyclate + ") " + "\r\n";
            queryString = queryString + "           " + this.GetRecyclatePendingWorkshiftsSQL(GlobalEnums.NmvnTaskID.SemifinishedProductRecyclate) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           IF (@NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.FinishedProductRecyclate + ") " + "\r\n";
            queryString = queryString + "               " + this.GetRecyclatePendingWorkshiftsSQL(GlobalEnums.NmvnTaskID.FinishedProductRecyclate) + "\r\n";
            queryString = queryString + "           ELSE " + "\r\n"; //GlobalEnums.NmvnTaskID.FinishedItemRecyclate
            queryString = queryString + "               " + this.GetRecyclatePendingWorkshiftsSQL(GlobalEnums.NmvnTaskID.FinishedItemRecyclate) + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetRecyclatePendingWorkshifts", queryString);
        }

        private string GetRecyclatePendingWorkshiftsSQL(GlobalEnums.NmvnTaskID nmvnTaskID)
        {
            string queryString = "";

            queryString = queryString + "       SELECT          Workshifts.WorkshiftID, Workshifts.EntryDate AS WorkshiftEntryDate, Workshifts.Code AS WorkshiftCode, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, Warehouses.Name AS WarehouseName, NMVNTaskRemains.TotalQuantityRemains " + "\r\n";

            queryString = queryString + "       FROM           (SELECT WorkshiftID, ROUND(SUM(" + this.nmvnTaskFailures(nmvnTaskID) + " + " + this.nmvnTaskSwarfs(nmvnTaskID) + " - RecycleWeights), " + (int)GlobalEnums.rndQuantity + ") AS TotalQuantityRemains FROM " + this.nmvnTaskTable(nmvnTaskID) + " WHERE Approved = 1 AND EntryDate >= CONVERT(smalldatetime, '" + new DateTime(2019, 6, 1).ToString("dd/MM/yyyy") + "',103) AND ROUND(" + this.nmvnTaskFailures(nmvnTaskID) + " + " + this.nmvnTaskSwarfs(nmvnTaskID) + " - RecycleWeights - RecycleLoss, " + (int)GlobalEnums.rndQuantity + ") > 0 " + " GROUP BY WorkshiftID) AS NMVNTaskRemains " + "\r\n";
            queryString = queryString + "                       INNER JOIN Workshifts ON NMVNTaskRemains.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Warehouses ON Warehouses.WarehouseID = 1 " + "\r\n";

            return queryString;
        }

        private void GetRecyclateViewDetails()
        {
            string queryString;

            queryString = " @NMVNTaskID int, @RecyclateID Int, @LocationID Int, @WorkshiftID Int, @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.SemifinishedProductRecyclate + ") " + "\r\n";
            queryString = queryString + "           " + this.BUILDSQL(GlobalEnums.NmvnTaskID.SemifinishedProductRecyclate) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           IF (@NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.FinishedProductRecyclate + ") " + "\r\n";
            queryString = queryString + "               " + this.BUILDSQL(GlobalEnums.NmvnTaskID.FinishedProductRecyclate) + "\r\n";
            queryString = queryString + "           ELSE " + "\r\n"; //GlobalEnums.NmvnTaskID.FinishedItemRecyclate
            queryString = queryString + "               " + this.BUILDSQL(GlobalEnums.NmvnTaskID.FinishedItemRecyclate) + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetRecyclateViewDetails", queryString);
        }

        private string BUILDSQL(GlobalEnums.NmvnTaskID nmvnTaskID)
        {
            string queryString = "" + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@RecyclateID > 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BUILDSQLEdit(nmvnTaskID) + "\r\n";
            queryString = queryString + "                   ORDER BY ProductionLineCode, " + this.nmvnTaskTable(nmvnTaskID) + "." + this.nmvnTaskPrimaryKey(nmvnTaskID) + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BUILDSQLNew(nmvnTaskID) + "\r\n";
            queryString = queryString + "                   ORDER BY ProductionLineCode, " + this.nmvnTaskTable(nmvnTaskID) + "." + this.nmvnTaskPrimaryKey(nmvnTaskID) + "\r\n";
            queryString = queryString + "               END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";
            return queryString;
        }

        private string BUILDSQLNew(GlobalEnums.NmvnTaskID nmvnTaskID)
        {
            string queryString = "";

            if (nmvnTaskID == GlobalEnums.NmvnTaskID.SemifinishedProductRecyclate)
            {
                queryString = queryString + "       SELECT      0 AS RecyclateDetailID, 0 AS RecyclateID, SemifinishedProducts.SemifinishedProductID, NULL AS FinishedProductPackageID, NULL AS FinishedItemDetailID, SemifinishedProducts.EntryDate AS RootEntryDate, SemifinishedProducts.Reference AS RootReference, ProductionLines.Code AS ProductionLineCode, FirmOrders.Code AS FirmOrderCode, FirmOrders.Specification, " + "\r\n";
                queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.CommodityTypeID, Commodities.RecycleCommodityID, RecycleCommodities.Code AS RecycleCommodityCode, RecycleCommodities.Name AS RecycleCommodityName, RecycleCommodities.CommodityTypeID AS RecycleCommodityTypeID, SemifinishedProducts.RejectWeights AS QuantityFailures, SemifinishedProducts.FailureWeights AS QuantitySwarfs, ISNULL(ROUND(SemifinishedProducts.RejectWeights + SemifinishedProducts.FailureWeights - SemifinishedProducts.RecycleWeights, " + (int)GlobalEnums.rndQuantity + "), 0) AS QuantityRemains, 0.0 AS Quantity " + "\r\n"; //QuantityRemains NOT INCLUDED RecycleLoss, BUT: CHECK PENDING MUST MINUS RecycleLoss!!!!!!

                queryString = queryString + "       FROM        SemifinishedProducts " + "\r\n";
                queryString = queryString + "                   INNER JOIN ProductionLines ON SemifinishedProducts.WorkshiftID = @WorkshiftID AND SemifinishedProducts.Approved = 1 AND ROUND(SemifinishedProducts.RejectWeights + SemifinishedProducts.FailureWeights - SemifinishedProducts.RecycleWeights - SemifinishedProducts.RecycleLoss, " + (int)GlobalEnums.rndQuantity + ") > 0 AND SemifinishedProducts.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";
                queryString = queryString + "                   INNER JOIN FirmOrders ON SemifinishedProducts.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
                queryString = queryString + "                   INNER JOIN MaterialIssueDetails ON SemifinishedProducts.MaterialIssueDetailID = MaterialIssueDetails.MaterialIssueDetailID " + "\r\n";
                queryString = queryString + "                   INNER JOIN Commodities ON MaterialIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
                queryString = queryString + "                   LEFT  JOIN Commodities AS RecycleCommodities ON Commodities.RecycleCommodityID = RecycleCommodities.CommodityID " + "\r\n";
            }

            if (nmvnTaskID == GlobalEnums.NmvnTaskID.FinishedProductRecyclate)
            {
                queryString = queryString + "       SELECT      0 AS RecyclateDetailID, 0 AS RecyclateID, NULL AS SemifinishedProductID, FinishedProductPackages.FinishedProductPackageID, NULL AS FinishedItemDetailID, FinishedProductPackages.EntryDate AS RootEntryDate, FinishedProductPackages.Reference AS RootReference, NULL AS ProductionLineCode, FirmOrders.Code AS FirmOrderCode, FirmOrders.Specification, " + "\r\n";
                queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.CommodityTypeID, Commodities.RecycleCommodityID, RecycleCommodities.Code AS RecycleCommodityCode, RecycleCommodities.Name AS RecycleCommodityName, RecycleCommodities.CommodityTypeID AS RecycleCommodityTypeID, FinishedProductPackages.QuantityFailureWeights AS QuantityFailures, FinishedProductPackages.Swarfs AS QuantitySwarfs, ISNULL(ROUND(FinishedProductPackages.QuantityFailureWeights + FinishedProductPackages.Swarfs - FinishedProductPackages.RecycleWeights, " + (int)GlobalEnums.rndQuantity + "), 0) AS QuantityRemains, 0.0 AS Quantity " + "\r\n"; //QuantityRemains NOT INCLUDED RecycleLoss, BUT: CHECK PENDING MUST MINUS RecycleLoss!!!!!!

                queryString = queryString + "       FROM        FinishedProductPackages " + "\r\n";
                queryString = queryString + "                   INNER JOIN FirmOrders ON FinishedProductPackages.WorkshiftID = @WorkshiftID AND FinishedProductPackages.Approved = 1 AND ROUND(FinishedProductPackages.QuantityFailureWeights + FinishedProductPackages.Swarfs - FinishedProductPackages.RecycleWeights - FinishedProductPackages.RecycleLoss, " + (int)GlobalEnums.rndQuantity + ") > 0 AND FinishedProductPackages.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
                queryString = queryString + "                   INNER JOIN BomDetails ON FirmOrders.BomID = BomDetails.BomID " + "\r\n"; //NOTE: VERY IMPORTANT: HERE BomDetails HAVE ONLY ONE ROW!!!
                queryString = queryString + "                   INNER JOIN Commodities ON BomDetails.MaterialID = Commodities.CommodityID " + "\r\n";
                queryString = queryString + "                   LEFT  JOIN Commodities AS RecycleCommodities ON Commodities.RecycleCommodityID = RecycleCommodities.CommodityID " + "\r\n";
            }

            if (nmvnTaskID == GlobalEnums.NmvnTaskID.FinishedItemRecyclate)
            {
                queryString = queryString + "       SELECT      0 AS RecyclateDetailID, 0 AS RecyclateID, NULL AS SemifinishedProductID, NULL AS FinishedProductPackageID, FinishedItemDetails.FinishedItemDetailID, FinishedItemDetails.EntryDate AS RootEntryDate, FinishedItemDetails.Reference AS RootReference, ProductionLines.Code AS ProductionLineCode, FirmOrders.Code AS FirmOrderCode, FirmOrders.Specification, " + "\r\n";
                queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.CommodityTypeID, Commodities.RecycleCommodityID, RecycleCommodities.Code AS RecycleCommodityCode, RecycleCommodities.Name AS RecycleCommodityName, RecycleCommodities.CommodityTypeID AS RecycleCommodityTypeID, FinishedItemDetails.QuantityFailure AS QuantityFailures, FinishedItemDetails.Swarfs AS QuantitySwarfs, ISNULL(ROUND(FinishedItemDetails.QuantityFailure + FinishedItemDetails.Swarfs - FinishedItemDetails.RecycleWeights, " + (int)GlobalEnums.rndQuantity + "), 0) AS QuantityRemains, 0.0 AS Quantity " + "\r\n"; //QuantityRemains NOT INCLUDED RecycleLoss, BUT: CHECK PENDING MUST MINUS RecycleLoss!!!!!!

                queryString = queryString + "       FROM        FinishedItemDetails " + "\r\n";
                queryString = queryString + "                   INNER JOIN ProductionLines ON FinishedItemDetails.WorkshiftID = @WorkshiftID AND FinishedItemDetails.Approved = 1 AND ROUND(FinishedItemDetails.QuantityFailure + FinishedItemDetails.Swarfs - FinishedItemDetails.RecycleWeights - FinishedItemDetails.RecycleLoss, " + (int)GlobalEnums.rndQuantity + ") > 0 AND FinishedItemDetails.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";
                queryString = queryString + "                   INNER JOIN FirmOrders ON FinishedItemDetails.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
                queryString = queryString + "                   INNER JOIN Commodities ON FinishedItemDetails.CommodityID = Commodities.CommodityID " + "\r\n";
                queryString = queryString + "                   LEFT  JOIN Commodities AS RecycleCommodities ON Commodities.RecycleCommodityID = RecycleCommodities.CommodityID " + "\r\n";
            }

            return queryString;
        }

        private string BUILDSQLEdit(GlobalEnums.NmvnTaskID nmvnTaskID)
        {
            string queryString = "";

            if (nmvnTaskID == GlobalEnums.NmvnTaskID.SemifinishedProductRecyclate)
            {
                queryString = queryString + "       SELECT      RecyclateDetails.RecyclateDetailID, RecyclateDetails.RecyclateID, RecyclateDetails.SemifinishedProductID, RecyclateDetails.FinishedProductPackageID, RecyclateDetails.FinishedItemDetailID, SemifinishedProducts.EntryDate AS RootEntryDate, SemifinishedProducts.Reference AS RootReference, ProductionLines.Code AS ProductionLineCode, FirmOrders.Code AS FirmOrderCode, FirmOrders.Specification, " + "\r\n";
                queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.CommodityTypeID, Commodities.RecycleCommodityID, RecycleCommodities.Code AS RecycleCommodityCode, RecycleCommodities.Name AS RecycleCommodityName, RecycleCommodities.CommodityTypeID AS RecycleCommodityTypeID, SemifinishedProducts.RejectWeights AS QuantityFailures, SemifinishedProducts.FailureWeights AS QuantitySwarfs, ISNULL(ROUND(SemifinishedProducts.RejectWeights + SemifinishedProducts.FailureWeights - SemifinishedProducts.RecycleWeights + RecyclateDetails.Quantity, " + (int)GlobalEnums.rndQuantity + "), 0) AS QuantityRemains, RecyclateDetails.Quantity " + "\r\n";

                queryString = queryString + "       FROM        RecyclateDetails " + "\r\n";
                queryString = queryString + "                   INNER JOIN SemifinishedProducts ON RecyclateDetails.RecyclateID = @RecyclateID AND RecyclateDetails.SemifinishedProductID = SemifinishedProducts.SemifinishedProductID " + "\r\n";
                queryString = queryString + "                   INNER JOIN ProductionLines ON SemifinishedProducts.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";
                queryString = queryString + "                   INNER JOIN FirmOrders ON SemifinishedProducts.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
                queryString = queryString + "                   INNER JOIN Commodities ON RecyclateDetails.CommodityID = Commodities.CommodityID " + "\r\n";
                queryString = queryString + "                   LEFT  JOIN Commodities AS RecycleCommodities ON RecyclateDetails.RecycleCommodityID = RecycleCommodities.CommodityID " + "\r\n";
            }

            if (nmvnTaskID == GlobalEnums.NmvnTaskID.FinishedProductRecyclate)
            {
                queryString = queryString + "       SELECT      RecyclateDetails.RecyclateDetailID, RecyclateDetails.RecyclateID, RecyclateDetails.SemifinishedProductID, RecyclateDetails.FinishedProductPackageID, RecyclateDetails.FinishedItemDetailID, FinishedProductPackages.EntryDate AS RootEntryDate, FinishedProductPackages.Reference AS RootReference, NULL AS ProductionLineCode, FirmOrders.Code AS FirmOrderCode, FirmOrders.Specification, " + "\r\n";
                queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.CommodityTypeID, Commodities.RecycleCommodityID, RecycleCommodities.Code AS RecycleCommodityCode, RecycleCommodities.Name AS RecycleCommodityName, RecycleCommodities.CommodityTypeID AS RecycleCommodityTypeID, FinishedProductPackages.QuantityFailureWeights AS QuantityFailures, FinishedProductPackages.Swarfs AS QuantitySwarfs, ISNULL(ROUND(FinishedProductPackages.QuantityFailureWeights + FinishedProductPackages.Swarfs - FinishedProductPackages.RecycleWeights + RecyclateDetails.Quantity, " + (int)GlobalEnums.rndQuantity + "), 0) AS QuantityRemains, RecyclateDetails.Quantity " + "\r\n";

                queryString = queryString + "       FROM        RecyclateDetails " + "\r\n";
                queryString = queryString + "                   INNER JOIN FinishedProductPackages ON RecyclateDetails.RecyclateID = @RecyclateID AND RecyclateDetails.FinishedProductPackageID = FinishedProductPackages.FinishedProductPackageID " + "\r\n";
                queryString = queryString + "                   INNER JOIN FirmOrders ON FinishedProductPackages.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
                queryString = queryString + "                   INNER JOIN Commodities ON RecyclateDetails.CommodityID = Commodities.CommodityID " + "\r\n";
                queryString = queryString + "                   LEFT  JOIN Commodities AS RecycleCommodities ON RecyclateDetails.RecycleCommodityID = RecycleCommodities.CommodityID " + "\r\n";
            }

            if (nmvnTaskID == GlobalEnums.NmvnTaskID.FinishedItemRecyclate)
            {
                queryString = queryString + "       SELECT      RecyclateDetails.RecyclateDetailID, RecyclateDetails.RecyclateID, RecyclateDetails.SemifinishedProductID, RecyclateDetails.FinishedProductPackageID, RecyclateDetails.FinishedItemDetailID, FinishedItemDetails.EntryDate AS RootEntryDate, FinishedItemDetails.Reference AS RootReference, ProductionLines.Code AS ProductionLineCode, FirmOrders.Code AS FirmOrderCode, FirmOrders.Specification, " + "\r\n";
                queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.CommodityTypeID, Commodities.RecycleCommodityID, RecycleCommodities.Code AS RecycleCommodityCode, RecycleCommodities.Name AS RecycleCommodityName, RecycleCommodities.CommodityTypeID AS RecycleCommodityTypeID, FinishedItemDetails.QuantityFailure AS QuantityFailures, FinishedItemDetails.Swarfs AS QuantitySwarfs, ISNULL(ROUND(FinishedItemDetails.QuantityFailure + FinishedItemDetails.Swarfs - FinishedItemDetails.RecycleWeights + RecyclateDetails.Quantity, " + (int)GlobalEnums.rndQuantity + "), 0) AS QuantityRemains, RecyclateDetails.Quantity " + "\r\n";

                queryString = queryString + "       FROM        RecyclateDetails " + "\r\n";
                queryString = queryString + "                   INNER JOIN FinishedItemDetails ON RecyclateDetails.RecyclateID = @RecyclateID AND RecyclateDetails.FinishedItemDetailID = FinishedItemDetails.FinishedItemDetailID " + "\r\n";
                queryString = queryString + "                   INNER JOIN ProductionLines ON FinishedItemDetails.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";
                queryString = queryString + "                   INNER JOIN FirmOrders ON FinishedItemDetails.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
                queryString = queryString + "                   INNER JOIN Commodities ON RecyclateDetails.CommodityID = Commodities.CommodityID " + "\r\n";
                queryString = queryString + "                   LEFT  JOIN Commodities AS RecycleCommodities ON RecyclateDetails.RecycleCommodityID = RecycleCommodities.CommodityID " + "\r\n";
            }
            return queryString;
        }

        private void GetRecyclateViewPackages()
        {
            string queryString;

            queryString = " @NMVNTaskID int, @RecyclateID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";

            //queryString = queryString + "       DECLARE @NMVNTaskID int     SET @NMVNTaskID = (SELECT NMVNTaskID FROM RecyclatePackages WHERE RecyclateID = @RecyclateID) " + "\r\n";

            queryString = queryString + "       IF (@NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.SemifinishedProductRecyclate + ") " + "\r\n";
            queryString = queryString + "           " + this.GetRecyclateViewPackagesSQL(GlobalEnums.NmvnTaskID.SemifinishedProductRecyclate) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           IF (@NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.FinishedProductRecyclate + ") " + "\r\n";
            queryString = queryString + "               " + this.GetRecyclateViewPackagesSQL(GlobalEnums.NmvnTaskID.FinishedProductRecyclate) + "\r\n";
            queryString = queryString + "           ELSE " + "\r\n"; //GlobalEnums.NmvnTaskID.FinishedItemRecyclate
            queryString = queryString + "               " + this.GetRecyclateViewPackagesSQL(GlobalEnums.NmvnTaskID.FinishedItemRecyclate) + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetRecyclateViewPackages", queryString);
        }

        private string GetRecyclateViewPackagesSQL(GlobalEnums.NmvnTaskID nmvnTaskID)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "        SELECT         RecyclatePackages.RecyclatePackageID, RecyclatePackages.RecyclateID, RecyclatePackages.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, RecyclatePackages.CommodityTypeID, RecyclatePackages.BatchID, RecyclatePackages.BatchEntryDate, " + "\r\n";
            queryString = queryString + "                       SFP_TABLE.QuantityFailures, SFP_TABLE.QuantitySwarfs, SFP_TABLE.QuantityRemains, RecyclatePackages.Quantity, RecyclatePackages.Remarks " + "\r\n";
            queryString = queryString + "        FROM           RecyclatePackages " + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities ON RecyclatePackages.RecyclateID = @RecyclateID AND RecyclatePackages.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                       INNER JOIN (SELECT      RecyclateDetails.RecycleCommodityID, SUM(" + this.nmvnTaskFailures(nmvnTaskID) + ") AS QuantityFailures, SUM(" + this.nmvnTaskSwarfs(nmvnTaskID) + ") AS QuantitySwarfs, SUM(ROUND(" + this.nmvnTaskFailures(nmvnTaskID) + " + " + this.nmvnTaskSwarfs(nmvnTaskID) + " - " + this.nmvnTaskTable(nmvnTaskID) + ".RecycleWeights + RecyclateDetails.Quantity, " + (int)GlobalEnums.rndQuantity + ")) AS QuantityRemains " + "\r\n";
            queryString = queryString + "                                   FROM        RecyclateDetails " + "\r\n";
            queryString = queryString + "                                   INNER JOIN  " + this.nmvnTaskTable(nmvnTaskID) + " ON RecyclateDetails.RecyclateID = @RecyclateID AND RecyclateDetails." + this.nmvnTaskPrimaryKey(nmvnTaskID) + " = " + this.nmvnTaskTable(nmvnTaskID) + "." + this.nmvnTaskPrimaryKey(nmvnTaskID) + "\r\n";
            queryString = queryString + "                                   GROUP BY    RecyclateDetails.RecycleCommodityID " + "\r\n";
            queryString = queryString + "                                  )SFP_TABLE ON RecyclatePackages.CommodityID = SFP_TABLE.RecycleCommodityID " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private void RecyclateSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       BEGIN  " + "\r\n";

            queryString = queryString + "           DECLARE @NMVNTaskID int, @msg NVARCHAR(300), @affectedRows int ";

            queryString = queryString + "           SET @NMVNTaskID = (SELECT NMVNTaskID FROM Recyclates WHERE RecyclateID = @EntityID) " + "\r\n";

            queryString = queryString + "           IF (@NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.SemifinishedProductRecyclate + ") " + "\r\n";
            queryString = queryString + "               " + this.RecyclateSaveRelativeSQL(GlobalEnums.NmvnTaskID.SemifinishedProductRecyclate) + "\r\n";
            queryString = queryString + "           ELSE " + "\r\n";
            queryString = queryString + "               IF (@NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.FinishedProductRecyclate + ") " + "\r\n";
            queryString = queryString + "                   " + this.RecyclateSaveRelativeSQL(GlobalEnums.NmvnTaskID.FinishedProductRecyclate) + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n"; //GlobalEnums.NmvnTaskID.FinishedItemRecyclate
            queryString = queryString + "                   " + this.RecyclateSaveRelativeSQL(GlobalEnums.NmvnTaskID.FinishedItemRecyclate) + "\r\n";


            queryString = queryString + "           IF @affectedRows <> (SELECT COUNT(*) FROM RecyclateDetails WHERE RecyclateID = @EntityID) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   SET         @msg = N'Chứng từ đã hủy, hoặc chưa duyệt; hoặc số lượng không phù hợp' ; " + "\r\n";
            queryString = queryString + "                   THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";

            queryString = queryString + "       END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("RecyclateSaveRelative", queryString);
        }

        private string RecyclateSaveRelativeSQL(GlobalEnums.NmvnTaskID nmvnTaskID)
        {
            string queryString = " " + "\r\n"; 
            queryString = queryString + "       BEGIN  " + "\r\n";

            queryString = queryString + "           UPDATE          " + this.nmvnTaskTable(nmvnTaskID) + " " + "\r\n";
            queryString = queryString + "           SET             " + this.nmvnTaskTable(nmvnTaskID) + ".RecycleWeights = ROUND(" + this.nmvnTaskTable(nmvnTaskID) + ".RecycleWeights + RecyclateDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + "), " + "\r\n";
            queryString = queryString + "                           " + this.nmvnTaskTable(nmvnTaskID) + ".RecycleLoss = IIF(@SaveRelativeOption = 1, ROUND(" + this.nmvnTaskFailures(nmvnTaskID) + " + " + this.nmvnTaskSwarfs(nmvnTaskID) + " - (" + this.nmvnTaskTable(nmvnTaskID) + ".RecycleWeights + RecyclateDetails.Quantity), " + (int)GlobalEnums.rndQuantity + "), 0) " + "\r\n"; //UPDATE: SET RecycleLoss = Remains, UNDO: SET RecycleLoss = 0            
            queryString = queryString + "           FROM            RecyclateDetails " + "\r\n";
            queryString = queryString + "                           INNER JOIN " + this.nmvnTaskTable(nmvnTaskID) + " ON (" + this.nmvnTaskTable(nmvnTaskID) + ".Approved = 1 OR @SaveRelativeOption = -1) AND RecyclateDetails.RecyclateID = @EntityID AND RecyclateDetails." + this.nmvnTaskPrimaryKey(nmvnTaskID) + " = " + this.nmvnTaskTable(nmvnTaskID) + "." + this.nmvnTaskPrimaryKey(nmvnTaskID) + "\r\n";
            
            queryString = queryString + "           SET             @affectedRows = @@ROWCOUNT; " + "\r\n";

            queryString = queryString + "       END " + "\r\n";
            return queryString;
        }

        private void RecyclatePostSaveValidate()
        {
            string[] queryArray = new string[3];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày nhập kho: ' + CAST(SemifinishedProducts.EntryDate AS nvarchar) FROM RecyclateDetails INNER JOIN SemifinishedProducts ON RecyclateDetails.RecyclateID = @EntityID AND RecyclateDetails.SemifinishedProductID = SemifinishedProducts.SemifinishedProductID AND RecyclateDetails.EntryDate < SemifinishedProducts.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Ngày nhập kho: ' + CAST(FinishedProductPackages.EntryDate AS nvarchar) FROM RecyclateDetails INNER JOIN FinishedProductPackages ON RecyclateDetails.RecyclateID = @EntityID AND RecyclateDetails.FinishedProductPackageID = FinishedProductPackages.FinishedProductPackageID AND RecyclateDetails.EntryDate < FinishedProductPackages.EntryDate ";
            queryArray[2] = " SELECT TOP 1 @FoundEntity = N'Ngày nhập kho: ' + CAST(FinishedItemDetails.EntryDate AS nvarchar) FROM RecyclateDetails INNER JOIN FinishedItemDetails ON RecyclateDetails.RecyclateID = @EntityID AND RecyclateDetails.FinishedItemDetailID = FinishedItemDetails.FinishedItemDetailID AND RecyclateDetails.EntryDate < FinishedItemDetails.EntryDate ";

            //queryArray[3] = NO NEED TO CHECK??? BECAUSE OF RecycleLoss??????   " SELECT TOP 1 @FoundEntity = N'Số lượng kết toán vượt quá số lượng ghi nhận: ' + SemifinishedProducts.Reference + ': ' + CAST(ROUND(RejectWeights + FailureWeights - RecycleWeights - RecycleLoss, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM SemifinishedProducts WHERE (ROUND(RejectWeights + FailureWeights - RecycleWeights - RecycleLoss, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("RecyclatePostSaveValidate", queryArray);
        }




        private void RecyclateApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = RecyclateID FROM Recyclates WHERE RecyclateID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("RecyclateApproved", queryArray);
        }


        private void RecyclateEditable()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = RecyclateID FROM GoodsReceiptDetails WHERE RecyclateID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("RecyclateEditable", queryArray);
        }

        private void RecyclateToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      Recyclates  SET Approved = @Approved, ApprovedDate = GetDate() WHERE RecyclateID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          RecyclateDetails  SET Approved = @Approved WHERE RecyclateID = @EntityID ; " + "\r\n";
            queryString = queryString + "               UPDATE          RecyclatePackages SET Approved = @Approved WHERE RecyclateID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, N'hủy', '')  + N' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("RecyclateToggleApproved", queryString);
        }


        private void RecyclateInitReference()
        {
            SimpleInitReference simpleInitReference = new RecyclateInitReference("Recyclates", "RecyclateID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.Recyclate));
            this.totalSmartPortalEntities.CreateTrigger("RecyclateInitReference", simpleInitReference.CreateQuery());
        }

        private void RecyclateSheet()
        {
            string queryString = " @RecyclateID int " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @LocalRecyclateID int    SET @LocalRecyclateID = @RecyclateID" + "\r\n";

            queryString = queryString + "       SELECT          Recyclates.RecyclateID, Recyclates.EntryDate, Recyclates.Reference, Workshifts.EntryDate AS WorkshiftEntryDate, Workshifts.Code AS WorkshiftCode, Recyclates.Description, " + "\r\n";
            queryString = queryString + "                       NMVNTaskName = IIF(Recyclates.NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.SemifinishedProductRecyclate + ", N'TỔ ĐỊNH HÌNH', IIF(Recyclates.NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.FinishedProductRecyclate + ", N'TỔ ĐÓNG GÓI', N'TỔ TẠO MÀNG')), " + "\r\n";
            queryString = queryString + "                       CrucialWorkers.Name AS CrucialWorkerName, Storekeepers.Name AS StorekeeperName, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, RecyclatePackages.BatchEntryDate, RecyclatePackages.Remarks, RecyclatePackages.Quantity " + "\r\n";

            queryString = queryString + "       FROM            Recyclates " + "\r\n";
            queryString = queryString + "                       INNER JOIN RecyclatePackages ON Recyclates.RecyclateID = @LocalRecyclateID AND Recyclates.RecyclateID = RecyclatePackages.RecyclateID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Workshifts ON Recyclates.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities ON RecyclatePackages.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Employees AS CrucialWorkers ON Recyclates.CrucialWorkerID = CrucialWorkers.EmployeeID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Employees AS Storekeepers ON Recyclates.StorekeeperID = Storekeepers.EmployeeID " + "\r\n";

            queryString = queryString + "       ORDER BY        RecyclatePackages.RecyclatePackageID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("RecyclateSheet", queryString);
        }
    }
}

