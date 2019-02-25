using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Inventories
{
    public class MaterialIssue
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public MaterialIssue(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetMaterialIssueIndexes();

            this.GetMaterialIssueViewDetails();

            this.GetMaterialIssuePendingFirmOrders();
            this.GetMaterialIssuePendingFirmOrderMaterials();

            this.MaterialIssueSaveRelative();
            this.MaterialIssuePostSaveValidate();

            this.MaterialIssueApproved();
            this.MaterialIssueEditable();

            //DO NOT ALLOW MaterialIssueToggleApproved DEPENDENTLY. BECAUSE: WE NEED TO RUN CHECK BOM MaterialIssueSaveRelative
            //IMPORTANT: 'CHECK BOM' AT: MaterialIssueSaveRelative: MUST RUN BEFORE CALL SAVE UPDATE TO FirmOrderMaterials ==> IN ORDER TO GET SQL_FirmOrderRemains CORRECTLY
            this.MaterialIssueToggleApproved(); 

            this.MaterialIssueInitReference();

            this.MaterialIssueSheet();
        }


        private void GetMaterialIssueIndexes()
        {
            string queryString;

            queryString = " @NMVNTaskID int, @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      MaterialIssues.MaterialIssueID, CAST(MaterialIssues.EntryDate AS DATE) AS EntryDate, MaterialIssues.Reference, Locations.Code AS LocationCode, Workshifts.Name AS WorkshiftName, Customers.Name AS CustomerName, ISNULL(FirmOrders.Description, '') + ' ' + ISNULL(MaterialIssues.Description, '') AS Description, MaterialIssues.TotalQuantity, MaterialIssues.Approved, " + "\r\n";
            queryString = queryString + "                   Workshifts.EntryDate AS WorkshiftEntryDate, ProductionLines.Code AS ProductionLinesCode, FirmOrders.Reference AS FirmOrdersReference, FirmOrders.Code AS FirmOrdersCode, FirmOrders.EntryDate AS FirmOrderEntryDate, FirmOrders.Specs AS FirmOrderSpecs, FirmOrders.Specification AS FirmOrderSpecification, Boms.Code AS BomCode " + "\r\n";
            queryString = queryString + "       FROM        MaterialIssues " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON MaterialIssues.NMVNTaskID = @NMVNTaskID AND MaterialIssues.EntryDate >= @FromDate AND MaterialIssues.EntryDate <= @ToDate AND MaterialIssues.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = @NMVNTaskID AND AccessControls.AccessLevel > 0) AND Locations.LocationID = MaterialIssues.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON MaterialIssues.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN ProductionLines ON MaterialIssues.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";
            queryString = queryString + "                   INNER JOIN FirmOrders ON MaterialIssues.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON MaterialIssues.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Boms ON FirmOrders.BomID = Boms.BomID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetMaterialIssueIndexes", queryString);
        }


        private void GetMaterialIssueViewDetails()
        {
            string queryString;

            queryString = " @MaterialIssueID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @IssueAll bit = 0 " + "\r\n";
            queryString = queryString + "       DECLARE     @FirmOrderID int, @QuantityMaterialEstimated decimal(18, 2) " + "\r\n";
            queryString = queryString + "       SELECT      TOP 1 @FirmOrderID = FirmOrderID, @QuantityMaterialEstimated = QuantityMaterialEstimated FROM MaterialIssues WHERE MaterialIssueID = @MaterialIssueID " + "\r\n";

            queryString = queryString + "       " + this.SQL_IssueAll() + "\r\n";

            queryString = queryString + "       SELECT      MaterialIssueDetails.MaterialIssueDetailID, MaterialIssueDetails.MaterialIssueID, FirmOrderMaterials.FirmOrderMaterialID, FirmOrderMaterials.BomID, FirmOrderMaterials.BomDetailID, BomDetails.LayerCode, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails.GoodsReceiptID, GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceiptDetails.Reference AS GoodsReceiptReference, GoodsReceiptDetails.Code AS GoodsReceiptCode, GoodsReceiptDetails.EntryDate AS GoodsReceiptEntryDate, GoodsReceiptDetails.BatchID, GoodsReceiptDetails.BatchEntryDate, GoodsReceiptDetails.LabID, GoodsReceiptDetails.ProductionDate, GoodsReceiptDetails.ExpiryDate, GoodsReceiptDetails.Barcode, GoodsReceiptDetails.BatchCode, GoodsReceiptDetails.SealCode, GoodsReceiptDetails.LabCode, " + "\r\n";
            queryString = queryString + "                   " + this.SQL_FirmOrderRemains("Issued_FirmOrderMaterials.QuantityIssued") + " AS FirmOrderRemains, ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued + Issued_GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityAvailables, MaterialIssueDetails.Quantity, MaterialIssueDetails.Remarks " + "\r\n";

            queryString = queryString + "       FROM        MaterialIssueDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN FirmOrderMaterials ON MaterialIssueDetails.MaterialIssueID = @MaterialIssueID AND MaterialIssueDetails.FirmOrderMaterialID = FirmOrderMaterials.FirmOrderMaterialID " + "\r\n";
            queryString = queryString + "                   INNER JOIN BomDetails ON FirmOrderMaterials.BomDetailID = BomDetails.BomDetailID " + "\r\n";

            queryString = queryString + "                   INNER JOIN (SELECT FirmOrderMaterialID, SUM(Quantity) AS QuantityIssued FROM MaterialIssueDetails WHERE MaterialIssueID = @MaterialIssueID GROUP BY FirmOrderMaterialID) AS Issued_FirmOrderMaterials ON MaterialIssueDetails.FirmOrderMaterialID = Issued_FirmOrderMaterials.FirmOrderMaterialID " + "\r\n";

            queryString = queryString + "                   INNER JOIN Commodities ON FirmOrderMaterials.MaterialID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "                   INNER JOIN GoodsReceiptDetails ON MaterialIssueDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";
            queryString = queryString + "                   INNER JOIN (SELECT GoodsReceiptDetailID, SUM(Quantity) AS QuantityIssued FROM MaterialIssueDetails WHERE MaterialIssueID = @MaterialIssueID GROUP BY GoodsReceiptDetailID) AS Issued_GoodsReceiptDetails ON MaterialIssueDetails.GoodsReceiptDetailID = Issued_GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";

            queryString = queryString + "       ORDER BY    MaterialIssueDetails.MaterialIssueID, MaterialIssueDetails.MaterialIssueDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetMaterialIssueViewDetails", queryString);
        }

        private string SQL_IssueAll() { return "SET         @IssueAll = (SELECT TOP 1 IIF(ROUND(QuantityMaterialEstimated - QuantityMaterialEstimatedIssued, " + (int)GlobalEnums.rndQuantity + ") <= @QuantityMaterialEstimated, 1, 0) FROM FirmOrders WHERE FirmOrderID = @FirmOrderID)"; }
        private string SQL_FirmOrderRemains(string undoSQL) { return "ROUND(IIF(@IssueAll = 1, FirmOrderMaterials.Quantity - FirmOrderMaterials.QuantityIssued" + (undoSQL != "" ? " + " + undoSQL : "") + ", (((@QuantityMaterialEstimated * BomDetails.BlockUnit)/ 100) * (BomDetails.BlockQuantity/ BomDetails.LayerQuantity)))" + ", " + (int)GlobalEnums.rndQuantity + ")"; }




        private void GetMaterialIssuePendingFirmOrders()
        {
            string queryString = " @LocationID int, @NMVNTaskID int, @FirmOrderID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          " + (int)@GlobalEnums.MaterialIssueTypeID.FirmOrders + " AS MaterialIssueTypeID, ProductionOrderDetails.ProductionOrderDetailID, ProductionOrderDetails.ProductionOrderID, ProductionOrderDetails.PlannedOrderID, ProductionOrderDetails.FirmOrderID, FirmOrders.Code AS FirmOrderCode, FirmOrders.Reference AS FirmOrderReference, FirmOrders.EntryDate AS FirmOrderEntryDate, FirmOrders.Specs AS FirmOrderSpecs, FirmOrders.Specification AS FirmOrderSpecification, FirmOrders.BomID, FirmOrders.TotalQuantity, FirmOrderRemains.TotalQuantityRemains, ROUND(QuantityMaterialEstimated - QuantityMaterialEstimatedIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityMaterialEstimatedRemains, " + "\r\n";
            queryString = queryString + "                       ProductionOrderDetails.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Warehouses.WarehouseID, Warehouses.Code AS WarehouseCode, Warehouses.Name AS WarehouseName " + "\r\n";

            queryString = queryString + "       FROM           (SELECT FirmOrderID, ROUND(SUM(Quantity - (QuantitySemifinished - QuantityShortage - QuantityFailure + QuantityExcess)), " + (int)GlobalEnums.rndQuantity + ") AS TotalQuantityRemains FROM FirmOrderDetails WHERE NMVNTaskID = @NMVNTaskID + 671977 AND Approved = 1 AND InActive = 0 AND InActivePartial = 0 AND (@FirmOrderID IS NULL OR FirmOrderID = @FirmOrderID) AND ROUND(Quantity - (QuantitySemifinished - QuantityShortage - QuantityFailure + QuantityExcess), " + (int)GlobalEnums.rndQuantity + ") > 0 GROUP BY FirmOrderID) AS FirmOrderRemains " + "\r\n";
            queryString = queryString + "                       INNER JOIN ProductionOrderDetails ON ProductionOrderDetails.Approved = 1 AND ProductionOrderDetails.InActive = 0 AND ProductionOrderDetails.InActivePartial = 0 AND FirmOrderRemains.FirmOrderID = ProductionOrderDetails.FirmOrderID " + "\r\n";//LocationID = @LocationID AND 
            queryString = queryString + "                       INNER JOIN FirmOrders ON (@NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.ItemStaging + " OR ROUND(FirmOrders.QuantityMaterialEstimated - FirmOrders.QuantityMaterialEstimatedIssued, " + (int)GlobalEnums.rndQuantity + ") >0) AND FirmOrderRemains.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";

            //ProductionOrderDetails.FirmOrderID IN 
            queryString = queryString + "                       INNER JOIN Customers ON ProductionOrderDetails.CustomerID = Customers.CustomerID " + "\r\n";

            queryString = queryString + "                       INNER JOIN Warehouses ON Warehouses.WarehouseID = IIF(@NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.ItemStaging + ", 6, 1) " + "\r\n";


            this.totalSmartPortalEntities.CreateStoredProcedure("GetMaterialIssuePendingFirmOrders", queryString);
        }

        private void GetMaterialIssuePendingFirmOrderMaterials()
        {
            string queryString;

            queryString = " @LocationID Int, @MaterialIssueID Int, @FirmOrderID Int, @QuantityMaterialEstimated decimal(18, 2), @WarehouseID Int, @GoodsReceiptDetailIDs varchar(3999), @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE @IssueAll bit = 0 " + "\r\n";
            queryString = queryString + "       " + this.SQL_IssueAll() + "\r\n";

            queryString = queryString + "       IF  (@GoodsReceiptDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPendingDetails(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPendingDetails(false) + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetMaterialIssuePendingFirmOrderMaterials", queryString);
        }

        private string BuildSQLPendingDetails(bool isGoodsReceiptDetailIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@MaterialIssueID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLNew(isGoodsReceiptDetailIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY FirmOrderMaterials.FirmOrderMaterialID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLNew(isGoodsReceiptDetailIDs) + " WHERE FirmOrderMaterials.FirmOrderMaterialID NOT IN (SELECT FirmOrderMaterialID FROM MaterialIssueDetails WHERE MaterialIssueID = @MaterialIssueID) " + "\r\n";
            queryString = queryString + "                   UNION ALL " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLEdit(isGoodsReceiptDetailIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY FirmOrderMaterials.FirmOrderMaterialID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLNew(bool isGoodsReceiptDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      FirmOrderMaterials.FirmOrderMaterialID, FirmOrderMaterials.BomID, FirmOrderMaterials.BomDetailID, BomDetails.LayerCode, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.OfficialCode, Commodities.CodePartA, Commodities.CodePartB, Commodities.CodePartC, Commodities.CodePartD, Commodities.CodePartE, Commodities.CodePartF, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails.GoodsReceiptID, GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceipts.Reference AS GoodsReceiptReference, GoodsReceipts.Code AS GoodsReceiptCode, GoodsReceipts.EntryDate AS GoodsReceiptEntryDate, GoodsReceiptDetails.BatchID, GoodsReceiptDetails.BatchEntryDate, GoodsReceiptDetails.LabID, GoodsReceiptDetails.ProductionDate, GoodsReceiptDetails.ExpiryDate, GoodsReceiptDetails.Barcode, GoodsReceiptDetails.BatchCode, GoodsReceiptDetails.SealCode, GoodsReceiptDetails.LabCode, " + "\r\n";
            queryString = queryString + "                   " + this.SQL_FirmOrderRemains("") + " AS FirmOrderRemains, ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityAvailables, 0 AS Quantity, CAST(0 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        FirmOrderMaterials " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON FirmOrderMaterials.FirmOrderID = @FirmOrderID AND FirmOrderMaterials.Approved = 1 AND FirmOrderMaterials.InActive = 0 AND FirmOrderMaterials.InActivePartial = 0 AND FirmOrderMaterials.MaterialID = Commodities.CommodityID " + "\r\n"; //AND ROUND(FirmOrderMaterials.Quantity - FirmOrderMaterials.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 
            queryString = queryString + "                   INNER JOIN BomDetails ON FirmOrderMaterials.BomDetailID = BomDetails.BomDetailID " + "\r\n";

            queryString = queryString + "                   LEFT JOIN GoodsReceiptDetails ON GoodsReceiptDetails.WarehouseID = @WarehouseID AND FirmOrderMaterials.MaterialID = GoodsReceiptDetails.CommodityID AND GoodsReceiptDetails.Approved = 1 AND ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 " + (isGoodsReceiptDetailIDs ? " AND GoodsReceiptDetails.GoodsReceiptDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@GoodsReceiptDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   LEFT JOIN GoodsReceipts ON GoodsReceiptDetails.GoodsReceiptID = GoodsReceipts.GoodsReceiptID " + "\r\n";

            return queryString;
        }

        private string BuildSQLEdit(bool isGoodsReceiptDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      FirmOrderMaterials.FirmOrderMaterialID, FirmOrderMaterials.BomID, FirmOrderMaterials.BomDetailID, BomDetails.LayerCode, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.OfficialCode, Commodities.CodePartA, Commodities.CodePartB, Commodities.CodePartC, Commodities.CodePartD, Commodities.CodePartE, Commodities.CodePartF, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails.GoodsReceiptID, GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceipts.Reference AS GoodsReceiptReference, GoodsReceipts.Code AS GoodsReceiptCode, GoodsReceipts.EntryDate AS GoodsReceiptEntryDate, GoodsReceiptDetails.BatchID, GoodsReceiptDetails.BatchEntryDate, GoodsReceiptDetails.LabID, GoodsReceiptDetails.ProductionDate, GoodsReceiptDetails.ExpiryDate, GoodsReceiptDetails.Barcode, GoodsReceiptDetails.BatchCode, GoodsReceiptDetails.SealCode, GoodsReceiptDetails.LabCode, " + "\r\n";
            queryString = queryString + "                   " + this.SQL_FirmOrderRemains("MaterialIssueDetails.Quantity") + " AS FirmOrderRemains, ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued + ISNULL(IssuedGoodsReceiptDetails.Quantity, 0), " + (int)GlobalEnums.rndQuantity + ") AS QuantityAvailables, 0 AS Quantity, CAST(0 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        FirmOrderMaterials " + "\r\n";
            queryString = queryString + "                   INNER JOIN (SELECT FirmOrderMaterialID, SUM(Quantity) AS Quantity FROM MaterialIssueDetails WHERE MaterialIssueID = @MaterialIssueID GROUP BY FirmOrderMaterialID) AS MaterialIssueDetails ON FirmOrderMaterials.FirmOrderMaterialID = MaterialIssueDetails.FirmOrderMaterialID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON FirmOrderMaterials.MaterialID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN BomDetails ON FirmOrderMaterials.BomDetailID = BomDetails.BomDetailID " + "\r\n";

            queryString = queryString + "                   LEFT JOIN GoodsReceiptDetails ON GoodsReceiptDetails.WarehouseID = @WarehouseID AND FirmOrderMaterials.MaterialID = GoodsReceiptDetails.CommodityID AND GoodsReceiptDetails.Approved = 1 AND (ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 OR GoodsReceiptDetails.GoodsReceiptDetailID IN (SELECT GoodsReceiptDetailID FROM MaterialIssueDetails WHERE MaterialIssueID = @MaterialIssueID)) " + (isGoodsReceiptDetailIDs ? " AND GoodsReceiptDetails.GoodsReceiptDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@GoodsReceiptDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   LEFT JOIN (SELECT GoodsReceiptDetailID, SUM(Quantity) AS Quantity FROM MaterialIssueDetails WHERE MaterialIssueID = @MaterialIssueID GROUP BY GoodsReceiptDetailID) AS IssuedGoodsReceiptDetails ON GoodsReceiptDetails.GoodsReceiptDetailID = IssuedGoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN GoodsReceipts ON GoodsReceiptDetails.GoodsReceiptID = GoodsReceipts.GoodsReceiptID " + "\r\n";

            return queryString;
        }


        private void MaterialIssueSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       BEGIN  " + "\r\n";

            queryString = queryString + "           DECLARE @msg NVARCHAR(300) = '' ";

            queryString = queryString + "           IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   IF (NOT EXISTS(SELECT ProductionOrderDetailID FROM ProductionOrderDetails WHERE ProductionOrderDetailID = (SELECT TOP 1 ProductionOrderDetailID FROM MaterialIssues WHERE MaterialIssueID = @EntityID) AND ProductionOrderDetails.Approved = 1 AND ProductionOrderDetails.InActive = 0 AND ProductionOrderDetails.InActivePartial = 0)) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       SET         @msg = N'Lệnh sản xuất đã hủy, chưa duyệt hoặc đã xóa.' ; " + "\r\n";
            queryString = queryString + "                       THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";


            #region CHECK BOM
            //IMPORTANT: 'CHECK BOM' AT: MaterialIssueSaveRelative: MUST RUN BEFORE CALL SAVE UPDATE TO FirmOrderMaterials ==> IN ORDER TO GET SQL_FirmOrderRemains CORRECTLY
            queryString = queryString + "                   IF ((SELECT Approved FROM MaterialIssues WHERE MaterialIssueID = @EntityID AND Approved = 1 AND NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.MaterialStaging + ") = 1) " + "\r\n";
            queryString = queryString + "                       BEGIN " + "\r\n";

            queryString = queryString + "                           DECLARE     @IssueAll bit = 0 " + "\r\n";
            queryString = queryString + "                           DECLARE     @FirmOrderID int, @QuantityMaterialEstimated decimal(18, 2) " + "\r\n";
            queryString = queryString + "                           DECLARE     @Code nvarchar(50), @Name nvarchar(200), @LayerCode varchar(5), @VerifyQuantity decimal(18, 2) " + "\r\n";
            queryString = queryString + "                           SELECT      TOP 1 @FirmOrderID = FirmOrderID, @QuantityMaterialEstimated = QuantityMaterialEstimated FROM MaterialIssues WHERE MaterialIssueID = @EntityID " + "\r\n";

            queryString = queryString + "                           " + this.SQL_IssueAll() + "\r\n";

            queryString = queryString + "                           DECLARE         @VerifyBomDetails TABLE (MaterialID int NOT NULL, LayerCode varchar(5) NOT NULL, QuantityBOM decimal(18, 2) NOT NULL, QuantityISSUE decimal(18, 2) NOT NULL)" + "\r\n";

            queryString = queryString + "                           INSERT INTO     @VerifyBomDetails (MaterialID, LayerCode, QuantityBOM, QuantityISSUE) " + "\r\n";
            queryString = queryString + "                           SELECT          FirmOrderMaterials.MaterialID, BomDetails.LayerCode, " + this.SQL_FirmOrderRemains("") + " AS QuantityBOM, 0 AS QuantityISSUE " + "\r\n";
            queryString = queryString + "                           FROM            FirmOrderMaterials " + "\r\n";
            queryString = queryString + "                                           INNER JOIN BomDetails ON FirmOrderMaterials.FirmOrderID = @FirmOrderID AND FirmOrderMaterials.Approved = 1 AND FirmOrderMaterials.InActive = 0 AND FirmOrderMaterials.InActivePartial = 0 AND FirmOrderMaterials.BomDetailID = BomDetails.BomDetailID " + "\r\n";

            queryString = queryString + "                           INSERT INTO     @VerifyBomDetails (MaterialID, LayerCode, QuantityBOM, QuantityISSUE) " + "\r\n";
            queryString = queryString + "                           SELECT          MaterialIssueDetails.CommodityID, BomDetails.LayerCode, 0 AS QuantityBOM, MaterialIssueDetails.Quantity AS QuantityISSUE " + "\r\n";
            queryString = queryString + "                           FROM            MaterialIssueDetails " + "\r\n";
            queryString = queryString + "                                           INNER JOIN BomDetails ON MaterialIssueDetails.MaterialIssueID = @EntityID AND MaterialIssueDetails.BomDetailID = BomDetails.BomDetailID " + "\r\n";

            queryString = queryString + "                           DECLARE         VerifyBomDetails CURSOR LOCAL FOR SELECT MIN(Commodities.Code) AS Code, MIN(Commodities.Name) AS Name, VerifyBomDetails.LayerCode, ROUND(SUM(VerifyBomDetails.QuantityBOM - VerifyBomDetails.QuantityISSUE), " + (int)GlobalEnums.rndQuantity + ") AS VerifyQuantity FROM @VerifyBomDetails VerifyBomDetails INNER JOIN Commodities ON VerifyBomDetails.MaterialID = Commodities.CommodityID GROUP BY VerifyBomDetails.MaterialID, VerifyBomDetails.LayerCode HAVING ROUND(SUM(VerifyBomDetails.QuantityBOM - VerifyBomDetails.QuantityISSUE), " + (int)GlobalEnums.rndQuantity + ") <> 0; " + "\r\n";
            queryString = queryString + "                           OPEN            VerifyBomDetails; " + "\r\n";
            queryString = queryString + "                           FETCH NEXT FROM VerifyBomDetails INTO @Code, @Name, @LayerCode, @VerifyQuantity; " + "\r\n";
            queryString = queryString + "                           WHILE @@FETCH_STATUS = 0   " + "\r\n";
            queryString = queryString + "                               BEGIN " + "\r\n";
            queryString = queryString + "                                   SET   @msg = @msg + " + "\r\n" + " + ' [' + @Code + '] ' + @Name + N': xuất ' + iif(@VerifyQuantity > 0, N'thiếu: ', N'thừa: ') + CAST(@VerifyQuantity AS nvarchar); " + "\r\n";
            queryString = queryString + "                                   FETCH NEXT FROM VerifyBomDetails INTO @Code, @Name, @LayerCode, @VerifyQuantity; " + "\r\n";
            queryString = queryString + "                               END " + "\r\n";

            queryString = queryString + "                           IF  (@msg <> '') THROW 61001, @msg, 1; " + "\r\n";

            queryString = queryString + "                       END " + "\r\n";
            #endregion CHECK BOM



            #region UPDATE WorkshiftID
            queryString = queryString + "                   DECLARE         @EntryDate Datetime, @ShiftID int, @WorkshiftID int " + "\r\n";
            queryString = queryString + "                   SELECT          @EntryDate = CONVERT(date, EntryDate), @ShiftID = ShiftID FROM MaterialIssues WHERE MaterialIssueID = @EntityID " + "\r\n";
            queryString = queryString + "                   SET             @WorkshiftID = (SELECT TOP 1 WorkshiftID FROM Workshifts WHERE EntryDate = @EntryDate AND ShiftID = @ShiftID) " + "\r\n";

            queryString = queryString + "                   IF             (@WorkshiftID IS NULL) " + "\r\n";
            queryString = queryString + "                       BEGIN ";
            queryString = queryString + "                           INSERT INTO     Workshifts(EntryDate, ShiftID, Code, Name, WorkingHours, Remarks) SELECT @EntryDate, ShiftID, Code, Name, WorkingHours, Remarks FROM Shifts WHERE ShiftID = @ShiftID " + "\r\n";
            queryString = queryString + "                           SELECT          @WorkshiftID = SCOPE_IDENTITY(); " + "\r\n";
            queryString = queryString + "                       END ";

            queryString = queryString + "                   UPDATE          MaterialIssues        SET WorkshiftID = @WorkshiftID WHERE MaterialIssueID = @EntityID " + "\r\n";
            queryString = queryString + "                   UPDATE          MaterialIssueDetails  SET WorkshiftID = @WorkshiftID WHERE MaterialIssueID = @EntityID " + "\r\n";
            #endregion UPDATE WorkshiftID



            queryString = queryString + "               END " + "\r\n";


            queryString = queryString + "           UPDATE          FirmOrders " + "\r\n";
            queryString = queryString + "           SET             FirmOrders.QuantityMaterialEstimatedIssued = ROUND(FirmOrders.QuantityMaterialEstimatedIssued + MaterialIssues.QuantityMaterialEstimated * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "           FROM            FirmOrders  " + "\r\n";
            queryString = queryString + "                           INNER JOIN MaterialIssues ON MaterialIssues.MaterialIssueID = @EntityID AND FirmOrders.FirmOrderID = MaterialIssues.FirmOrderID " + "\r\n";


            queryString = queryString + "           DECLARE         @MaterialIssueDetails TABLE (GoodsReceiptDetailID int NOT NULL PRIMARY KEY, MaterialIssueTypeID int NOT NULL, Quantity decimal(18, 2) NOT NULL)" + "\r\n";
            queryString = queryString + "           INSERT INTO     @MaterialIssueDetails (GoodsReceiptDetailID, MaterialIssueTypeID, Quantity) SELECT GoodsReceiptDetailID, MIN(MaterialIssueTypeID) AS MaterialIssueTypeID, SUM(Quantity) AS Quantity FROM MaterialIssueDetails WHERE MaterialIssueID = @EntityID GROUP BY GoodsReceiptDetailID " + "\r\n";

            queryString = queryString + "           DECLARE         @MaterialIssueTypeID int, @AffectedROWCOUNT int ";
            queryString = queryString + "           SELECT          @MaterialIssueTypeID = MaterialIssueTypeID FROM @MaterialIssueDetails ";

            #region UPDATE GoodsReceiptDetails
            queryString = queryString + "           UPDATE          GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "           SET             GoodsReceiptDetails.QuantityIssued = ROUND(GoodsReceiptDetails.QuantityIssued + MaterialIssueDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "           FROM            GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                           INNER JOIN @MaterialIssueDetails MaterialIssueDetails ON GoodsReceiptDetails.GoodsReceiptDetailID = MaterialIssueDetails.GoodsReceiptDetailID AND GoodsReceiptDetails.Approved = 1 " + "\r\n";

            queryString = queryString + "           IF @@ROWCOUNT <> (SELECT COUNT(*) FROM @MaterialIssueDetails) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   SET         @msg = N'Phiếu nhập kho đã hủy, chưa duyệt hoặc đã xóa.' ; " + "\r\n";
            queryString = queryString + "                   THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            #endregion


            #region ISSUE ADVICE
            queryString = queryString + "           IF (@MaterialIssueTypeID > 0) " + "\r\n";
            queryString = queryString + "               BEGIN  " + "\r\n";

            queryString = queryString + "                   IF (@MaterialIssueTypeID = " + (int)GlobalEnums.MaterialIssueTypeID.FirmOrders + ") " + "\r\n";
            queryString = queryString + "                       BEGIN  " + "\r\n";

            queryString = queryString + "                           DECLARE         @IssueFirmOrderMaterials TABLE (FirmOrderMaterialID int NOT NULL PRIMARY KEY, Quantity decimal(18, 2) NOT NULL)" + "\r\n";
            queryString = queryString + "                           INSERT INTO     @IssueFirmOrderMaterials (FirmOrderMaterialID, Quantity) SELECT FirmOrderMaterialID, SUM(Quantity) AS Quantity FROM MaterialIssueDetails WHERE MaterialIssueID = @EntityID GROUP BY FirmOrderMaterialID " + "\r\n";

            queryString = queryString + "                           UPDATE          FirmOrderMaterials " + "\r\n";
            queryString = queryString + "                           SET             FirmOrderMaterials.QuantityIssued = ROUND(FirmOrderMaterials.QuantityIssued + IssueFirmOrderMaterials.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "                           FROM            FirmOrderMaterials " + "\r\n";
            queryString = queryString + "                                           INNER JOIN @IssueFirmOrderMaterials IssueFirmOrderMaterials ON ((FirmOrderMaterials.Approved = 1 AND FirmOrderMaterials.InActive = 0 AND FirmOrderMaterials.InActivePartial = 0) OR @SaveRelativeOption = -1) AND FirmOrderMaterials.FirmOrderMaterialID = IssueFirmOrderMaterials.FirmOrderMaterialID " + "\r\n";

            queryString = queryString + "                           IF @@ROWCOUNT <> (SELECT COUNT(*) FROM @IssueFirmOrderMaterials) " + "\r\n";
            queryString = queryString + "                               BEGIN " + "\r\n";
            queryString = queryString + "                                   SET         @msg = N'Kế hoạch sản xuất đã hủy, chưa duyệt hoặc đã xóa.' ; " + "\r\n";
            queryString = queryString + "                                   THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "                               END " + "\r\n";

            queryString = queryString + "                       END " + "\r\n";

            queryString = queryString + "               END  " + "\r\n";
            #endregion

            queryString = queryString + "       END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("MaterialIssueSaveRelative", queryString);
        }

        private void MaterialIssuePostSaveValidate()
        {
            string[] queryArray = new string[3];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày nhập kho: ' + CAST(GoodsReceipts.EntryDate AS nvarchar) FROM MaterialIssueDetails INNER JOIN GoodsReceipts ON MaterialIssueDetails.MaterialIssueID = @EntityID AND MaterialIssueDetails.GoodsReceiptID = GoodsReceipts.GoodsReceiptID AND MaterialIssueDetails.EntryDate < GoodsReceipts.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Lệnh sản xuất: ' + CAST(ProductionOrders.EntryDate AS nvarchar) FROM MaterialIssues INNER JOIN ProductionOrders ON MaterialIssues.MaterialIssueID = @EntityID AND MaterialIssues.ProductionOrderID = ProductionOrders.ProductionOrderID AND MaterialIssues.EntryDate < ProductionOrders.EntryDate ";
            queryArray[2] = " SELECT TOP 1 @FoundEntity = N'Số lượng xuất vượt quá số lượng tồn kho: ' + CAST(ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM GoodsReceiptDetails WHERE (ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") < 0) ";
            //ALLOW TO ISSUE OVER FirmOrderMaterials: queryArray[3] = " SELECT TOP 1 @FoundEntity = N'Số lượng xuất vượt quá số định mức nguyên vật liệu: ' + CAST(ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM FirmOrderMaterials WHERE (ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("MaterialIssuePostSaveValidate", queryArray);
        }




        private void MaterialIssueApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = MaterialIssueID FROM MaterialIssues WHERE MaterialIssueID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("MaterialIssueApproved", queryArray);
        }


        private void MaterialIssueEditable()
        {
            string[] queryArray = new string[3];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = MaterialIssueID FROM SemifinishedItems WHERE MaterialIssueID = @EntityID ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = MaterialIssueID FROM SemifinishedProducts WHERE MaterialIssueID = @EntityID ";
            queryArray[2] = " SELECT TOP 1 @FoundEntity = MaterialIssueID FROM GoodsReceiptDetails WHERE MaterialIssueID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("MaterialIssueEditable", queryArray);
        }

        private void MaterialIssueToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      MaterialIssues  SET Approved = @Approved, ApprovedDate = GetDate() WHERE MaterialIssueID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE      MaterialIssueDetails  SET Approved = @Approved WHERE MaterialIssueID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, N'hủy', '')  + N' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("MaterialIssueToggleApproved", queryString);
        }


        private void MaterialIssueInitReference()
        {
            SimpleInitReference simpleInitReference = new MaterialIssueInitReference("MaterialIssues", "MaterialIssueID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.MaterialIssue));
            this.totalSmartPortalEntities.CreateTrigger("MaterialIssueInitReference", simpleInitReference.CreateQuery());
        }

        private void MaterialIssueSheet()
        {
            string queryString = " @MaterialIssueID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @LocalMaterialIssueID int    SET @LocalMaterialIssueID = @MaterialIssueID" + "\r\n";

            queryString = queryString + "       SELECT          MaterialIssues.MaterialIssueID, MaterialIssues.EntryDate AS MaterialIssueEntryDate, MaterialIssues.Reference, Workshifts.Code AS WorkshiftCode, ProductionLines.Code AS ProductionLineCode, " + "\r\n";
            queryString = queryString + "                       FirmOrders.EntryDate AS FirmOrderEntryDate, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, FirmOrders.Specs, FirmOrders.Specification, FirmOrders.TotalQuantity AS FirmOrderTotalQuantity, Customers.Name AS CustomerName, " + "\r\n";
            queryString = queryString + "                       Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, MaterialIssueDetails.BatchEntryDate, MaterialIssueDetails.Quantity, ISNULL(FirmOrders.Description, '') + ' ' + ISNULL(MaterialIssues.Description, '') AS Description " + "\r\n";

            queryString = queryString + "       FROM            MaterialIssues " + "\r\n";
            queryString = queryString + "                       INNER JOIN MaterialIssueDetails ON MaterialIssues.MaterialIssueID = @LocalMaterialIssueID AND MaterialIssues.MaterialIssueID = MaterialIssueDetails.MaterialIssueID " + "\r\n";
            queryString = queryString + "                       INNER JOIN FirmOrders ON MaterialIssues.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON MaterialIssues.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Workshifts ON MaterialIssues.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                       INNER JOIN ProductionLines ON MaterialIssues.ProductionLineID = ProductionLines.ProductionLineID" + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities ON MaterialIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "       ORDER BY        MaterialIssueDetails.MaterialIssueDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("MaterialIssueSheet", queryString);
        }

    }
}

