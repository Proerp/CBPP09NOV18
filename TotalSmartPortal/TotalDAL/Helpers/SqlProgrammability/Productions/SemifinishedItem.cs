using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Productions
{
    public class SemifinishedItem
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public SemifinishedItem(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetSemifinishedItemIndexes();

            this.GetSemifinishedItemPendingMaterialIssues();

            this.GetSemifinishedItemViewDetails();

            this.SemifinishedItemSaveRelative();
            this.SemifinishedItemPostSaveValidate();

            this.SemifinishedItemApproved();
            this.SemifinishedItemEditable();

            this.SemifinishedItemToggleApproved();

            this.SemifinishedItemInitReference();

            this.SemifinishedItemSheet();
        }


        private void GetSemifinishedItemIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      SemifinishedItems.SemifinishedItemID, CAST(SemifinishedItems.EntryDate AS DATE) AS EntryDate, SemifinishedItems.Reference, Locations.Code AS LocationCode, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, ProductionLines.Code AS ProductionLineCode, SemifinishedItems.WorkshiftID, Workshifts.EntryDate AS WorkshiftEntryDate, Workshifts.Code AS WorkshiftCode, FirmOrders.Reference AS FirmOrdersReference, FirmOrders.Code AS FirmOrdersCode, FirmOrders.Specification, Boms.Code AS BomCode, SemifinishedItems.Description, SemifinishedItems.TotalQuantity, SemifinishedItems.TotalQuantityFailure, SemifinishedItems.Approved " + "\r\n";
            queryString = queryString + "       FROM        SemifinishedItems " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON SemifinishedItems.EntryDate >= @FromDate AND SemifinishedItems.EntryDate <= @ToDate AND SemifinishedItems.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.SemifinishedItem + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = SemifinishedItems.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN FirmOrders ON SemifinishedItems.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON SemifinishedItems.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON SemifinishedItems.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN ProductionLines ON SemifinishedItems.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Boms ON FirmOrders.BomID = Boms.BomID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetSemifinishedItemIndexes", queryString);
        }

        #region X


        private void GetSemifinishedItemViewDetails()
        {
            string queryString;

            queryString = " @SemifinishedItemID Int, @FirmOrderID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT          ISNULL(SemifinishedItemDetails.SemifinishedItemDetailID, 0) AS SemifinishedItemDetailID, ISNULL(SemifinishedItemDetails.SemifinishedItemID, 0) AS SemifinishedItemID, FirmOrderDetails.FirmOrderDetailID, FirmOrderDetails.PlannedOrderID, FirmOrderDetails.PlannedOrderDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, FirmOrderDetails.PiecePerPack, FirmOrderDetails.BomID, Boms.Code AS BomCode, " + "\r\n";
            queryString = queryString + "                       FirmOrderDetails.MoldQuantity, ROUND(FirmOrderDetails.Quantity - (FirmOrderDetails.QuantitySemifinished - FirmOrderDetails.QuantityShortage - FirmOrderDetails.QuantityFailure + FirmOrderDetails.QuantityExcess) + ISNULL(SemifinishedItemDetails.Quantity, 0) + ISNULL(SemifinishedItemDetails.QuantityFailure, 0), " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, ISNULL(SemifinishedItemDetails.Quantity, 0) AS Quantity, ISNULL(SemifinishedItemDetails.QuantityFailure, 0) AS QuantityFailure, SemifinishedItemDetails.Remarks " + "\r\n";

            queryString = queryString + "       FROM            FirmOrderDetails " + "\r\n"; //CHIA RAA NHIEU BAO/ NHIEU CUON ==> PHAAI XU LY LAI
            queryString = queryString + "                       INNER JOIN Commodities ON FirmOrderDetails.FirmOrderID = @FirmOrderID AND FirmOrderDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Boms ON FirmOrderDetails.BomID = Boms.BomID " + "\r\n";
            queryString = queryString + "                       LEFT JOIN SemifinishedItemDetails ON SemifinishedItemDetails.SemifinishedItemID = @SemifinishedItemID AND FirmOrderDetails.FirmOrderDetailID = SemifinishedItemDetails.FirmOrderDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetSemifinishedItemViewDetails", queryString);
        }

        private void GetSemifinishedItemPendingMaterialIssues()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          MaterialIssues.MaterialIssueID, MaterialIssues.PlannedOrderID, MaterialIssues.FirmOrderID, FirmOrders.EntryDate AS FirmOrderEntryDate, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, FirmOrders.Specification AS FirmOrderSpecification, FirmOrders.BomID, Boms.Code AS BomCode, " + "\r\n";
            queryString = queryString + "                       MaterialIssues.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, MaterialIssues.WorkshiftID AS MaterialIssueWorkshiftID, Workshifts.EntryDate AS MaterialIssueWorkshiftEntryDate, Workshifts.Code AS MaterialIssueWorkshiftCode, MaterialIssues.ProductionLineID, ProductionLines.Code AS ProductionLineCode, MaterialIssues.CrucialWorkerID, CrucialWorkers.Code AS CrucialWorkerCode, CrucialWorkers.Name AS CrucialWorkerName, MaterialIssues.TotalQuantity AS MaterialTotalQuantity, ROUND(MaterialIssues.TotalQuantity - MaterialIssues.TotalQuantitySemifinished - MaterialIssues.TotalQuantityFailure - MaterialIssues.TotalQuantityReceipted - MaterialIssues.TotalQuantityLoss, " + (int)GlobalEnums.rndQuantity + ") AS MaterialQuantityRemains " + "\r\n";

            queryString = queryString + "       FROM            MaterialIssues  " + "\r\n";
            queryString = queryString + "                       INNER JOIN FirmOrders ON FirmOrders.InActive = 0 AND MaterialIssues.MaterialIssueID IN (SELECT MaterialIssueID FROM MaterialIssueDetails WHERE NMVNTaskID = " + (int)GlobalEnums.NmvnTaskID.MaterialStaging + " AND LocationID = @LocationID AND Approved = 1 AND ROUND(Quantity - QuantitySemifinished - QuantityFailure - QuantityReceipted - QuantityLoss, " + (int)GlobalEnums.rndQuantity + ") > 0) AND MaterialIssues.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON MaterialIssues.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Workshifts ON MaterialIssues.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                       INNER JOIN ProductionLines ON MaterialIssues.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Employees CrucialWorkers ON MaterialIssues.CrucialWorkerID = CrucialWorkers.EmployeeID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Boms ON FirmOrders.BomID = Boms.BomID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetSemifinishedItemPendingMaterialIssues", queryString);
        }

        private void SemifinishedItemSaveRelative()
        {
            string queryString = " @EntityID int, @MaterialIssueID int, @BomID int, @BomDetailID int, @MaterialID int, @Quantity decimal(18, 2) OUTPUT  " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN  " + "\r\n";
            queryString = queryString + "       DECLARE         @MaterialIssueDetailID int, @QuantityRemains decimal(18, 2), @QuantitySemifinished decimal(18, 2); " + "\r\n";

            queryString = queryString + "       DECLARE         CURSORMaterialIssueDetails CURSOR LOCAL FOR  SELECT MaterialIssueDetailID, ROUND(Quantity - QuantitySemifinished - QuantityFailure - QuantityReceipted - QuantityLoss, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains FROM MaterialIssueDetails WHERE MaterialIssueID = @MaterialIssueID AND BomDetailID = @BomDetailID AND ROUND(Quantity - QuantitySemifinished - QuantityFailure - QuantityReceipted - QuantityLoss, " + (int)GlobalEnums.rndQuantity + ") > 0; " + "\r\n";
            queryString = queryString + "       OPEN            CURSORMaterialIssueDetails; " + "\r\n";
            queryString = queryString + "       FETCH NEXT FROM CURSORMaterialIssueDetails INTO @MaterialIssueDetailID, @QuantityRemains; " + "\r\n";

            queryString = queryString + "       WHILE @@FETCH_STATUS = 0   " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               SET @QuantitySemifinished = IIF(@Quantity > @QuantityRemains, @QuantityRemains, @Quantity) " + "\r\n";

            queryString = queryString + "               INSERT INTO     SemifinishedItemMaterials   (SemifinishedItemID, MaterialIssueID, MaterialIssueDetailID, BomID, BomDetailID, MaterialID, Quantity) " + "\r\n";
            queryString = queryString + "               VALUES                                      (@EntityID, @MaterialIssueID, @MaterialIssueDetailID, @BomID, @BomDetailID, @MaterialID, @QuantitySemifinished); " + "\r\n";

            queryString = queryString + "               SET @Quantity = ROUND(@Quantity - @QuantitySemifinished, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "               IF (@Quantity = 0) BREAK " + "\r\n";

            queryString = queryString + "               FETCH NEXT FROM CURSORMaterialIssueDetails INTO @MaterialIssueDetailID, @QuantityRemains; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "   END " + "\r\n";


            this.totalSmartPortalEntities.CreateStoredProcedure("SemifinishedItemSaveMaterials", queryString);


            queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN  " + "\r\n";

            queryString = queryString + "       DECLARE @msg NVARCHAR(300) ";


            #region UPDATE WorkshiftID
            queryString = queryString + "       DECLARE         @EntryDate Datetime, @ShiftID int, @WorkshiftID int " + "\r\n";
            queryString = queryString + "       SELECT          @EntryDate = CONVERT(date, EntryDate), @ShiftID = ShiftID FROM SemifinishedItems WHERE SemifinishedItemID = @EntityID " + "\r\n";
            queryString = queryString + "       SET             @WorkshiftID = (SELECT TOP 1 WorkshiftID FROM Workshifts WHERE EntryDate = @EntryDate AND ShiftID = @ShiftID) " + "\r\n";

            queryString = queryString + "       IF             (@WorkshiftID IS NULL) " + "\r\n";
            queryString = queryString + "           BEGIN ";
            queryString = queryString + "               INSERT INTO     Workshifts(EntryDate, ShiftID, Code, Name, WorkingHours, Remarks) SELECT @EntryDate, ShiftID, Code, Name, WorkingHours, Remarks FROM Shifts WHERE ShiftID = @ShiftID " + "\r\n";
            queryString = queryString + "               SELECT          @WorkshiftID = SCOPE_IDENTITY(); " + "\r\n";
            queryString = queryString + "           END ";

            queryString = queryString + "       UPDATE          SemifinishedItems        SET WorkshiftID = @WorkshiftID WHERE SemifinishedItemID = @EntityID " + "\r\n";
            queryString = queryString + "       UPDATE          SemifinishedItemDetails  SET WorkshiftID = @WorkshiftID WHERE SemifinishedItemID = @EntityID " + "\r\n";
            #endregion UPDATE WorkshiftID


            queryString = queryString + "       DECLARE         @SemifinishedItemDetails TABLE (FirmOrderDetailID int NOT NULL PRIMARY KEY, PlannedOrderDetailID int NOT NULL, Quantity decimal(18, 2) NOT NULL, QuantityFailure decimal(18, 2) NOT NULL)" + "\r\n";
            queryString = queryString + "       INSERT INTO     @SemifinishedItemDetails (FirmOrderDetailID, PlannedOrderDetailID, Quantity, QuantityFailure) SELECT FirmOrderDetailID, PlannedOrderDetailID, SUM(Quantity) AS Quantity, SUM(QuantityFailure) AS QuantityFailure FROM SemifinishedItemDetails WHERE SemifinishedItemID = @EntityID GROUP BY FirmOrderDetailID, PlannedOrderDetailID " + "\r\n";

            queryString = queryString + "       UPDATE          FirmOrderDetails " + "\r\n";
            queryString = queryString + "       SET             FirmOrderDetails.QuantitySemifinished = ROUND(FirmOrderDetails.QuantitySemifinished + SemifinishedItemDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + "), FirmOrderDetails.QuantityFailure = ROUND(FirmOrderDetails.QuantityFailure + SemifinishedItemDetails.QuantityFailure * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "       FROM            FirmOrderDetails " + "\r\n";
            queryString = queryString + "                       INNER JOIN @SemifinishedItemDetails SemifinishedItemDetails ON ((FirmOrderDetails.Approved = 1 AND FirmOrderDetails.InActive = 0 AND FirmOrderDetails.InActivePartial = 0) OR @SaveRelativeOption = -1) AND FirmOrderDetails.FirmOrderDetailID = SemifinishedItemDetails.FirmOrderDetailID " + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT <> (SELECT COUNT(*) FROM @SemifinishedItemDetails) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               SET         @msg = N'Kế hoạch sản xuất đã hủy, chưa duyệt hoặc đã xóa.' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       UPDATE          PlannedOrderDetails " + "\r\n";
            queryString = queryString + "       SET             PlannedOrderDetails.QuantitySemifinished = ROUND(PlannedOrderDetails.QuantitySemifinished + SemifinishedItemDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "       FROM            PlannedOrderDetails " + "\r\n";
            queryString = queryString + "                       INNER JOIN @SemifinishedItemDetails SemifinishedItemDetails ON ((PlannedOrderDetails.Approved = 1 AND PlannedOrderDetails.InActive = 0 AND PlannedOrderDetails.InActivePartial = 0) OR @SaveRelativeOption = -1) AND PlannedOrderDetails.PlannedOrderDetailID = SemifinishedItemDetails.PlannedOrderDetailID " + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT <> (SELECT COUNT(*) FROM @SemifinishedItemDetails) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               SET         @msg = N'Kế hoạch sản xuất đã hủy, chưa duyệt hoặc đã xóa.' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";



            #region INIT FirmOrders
            queryString = queryString + "       IF (@SaveRelativeOption = 1) " + "\r\n"; //Update
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE         @MaterialIssueID int, @BomID int, @TotalQuantity decimal(18, 2); " + "\r\n";
            queryString = queryString + "               DECLARE         @BomDetailID int, @MaterialID int, @MaterialQuantity decimal(18, 2), @MaterialCode nvarchar(50), @MaterialName nvarchar(200); " + "\r\n";

            queryString = queryString + "               SELECT          @MaterialIssueID = MaterialIssueID, @BomID = BomID, @TotalQuantity = TotalQuantity + TotalQuantityFailure FROM SemifinishedItems WHERE SemifinishedItemID = @EntityID " + "\r\n";

            queryString = queryString + "               DECLARE         CURSORSemifinishedItems CURSOR LOCAL FOR  SELECT BomDetails.BomDetailID, BomDetails.MaterialID, ROUND(((@TotalQuantity * BomDetails.BlockUnit)/ 100) * (BomDetails.BlockQuantity/ BomDetails.LayerQuantity), " + (int)GlobalEnums.rndQuantity + ") AS MaterialQuantity, Commodities.Code AS MaterialCode, Commodities.Name AS MaterialName FROM BomDetails INNER JOIN Commodities ON BomDetails.MaterialID = Commodities.CommodityID WHERE BomID = @BomID; " + "\r\n";
            queryString = queryString + "               OPEN            CURSORSemifinishedItems; " + "\r\n";
            queryString = queryString + "               FETCH NEXT FROM CURSORSemifinishedItems INTO @BomDetailID, @MaterialID, @MaterialQuantity, @MaterialCode, @MaterialName; " + "\r\n";

            queryString = queryString + "               WHILE @@FETCH_STATUS = 0   " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       EXECUTE SemifinishedItemSaveMaterials @EntityID, @MaterialIssueID, @BomID, @BomDetailID, @MaterialID, @Quantity = @MaterialQuantity OUTPUT;" + "\r\n";

            queryString = queryString + "                       IF @MaterialQuantity <> 0 " + "\r\n";
            queryString = queryString + "                           BEGIN " + "\r\n";
            queryString = queryString + "                               SET         @msg = N'Khối lượng sử dụng vượt khối lượng nguyên liệu đã cấp, ' + @MaterialCode + '-' + @MaterialName + ': ' + CAST(@MaterialQuantity AS nvarchar) + '.' ; " + "\r\n";
            queryString = queryString + "                               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "                           END " + "\r\n";

            queryString = queryString + "                       FETCH NEXT FROM CURSORSemifinishedItems INTO @BomDetailID, @MaterialID, @MaterialQuantity, @MaterialCode, @MaterialName; " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            queryString = queryString + "       UPDATE          MaterialIssueDetails " + "\r\n";
            queryString = queryString + "       SET             MaterialIssueDetails.QuantitySemifinished = ROUND(MaterialIssueDetails.QuantitySemifinished + SemifinishedItemMaterials.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n"; //, MaterialIssueDetails.QuantityFailure = ROUND(MaterialIssueDetails.QuantityFailure + SemifinishedItems.FailureWeights * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") 
            queryString = queryString + "       FROM            MaterialIssueDetails " + "\r\n";
            queryString = queryString + "                       INNER JOIN (SELECT MaterialIssueDetailID, SUM(Quantity) AS Quantity FROM SemifinishedItemMaterials WHERE SemifinishedItemID = @EntityID GROUP BY MaterialIssueDetailID) SemifinishedItemMaterials ON MaterialIssueDetails.MaterialIssueDetailID = SemifinishedItemMaterials.MaterialIssueDetailID " + "\r\n";

            queryString = queryString + "       UPDATE          MaterialIssues " + "\r\n";
            queryString = queryString + "       SET             MaterialIssues.TotalQuantitySemifinished = ROUND(MaterialIssues.TotalQuantitySemifinished + SemifinishedItemMaterials.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n"; //, MaterialIssues.QuantityFailure = ROUND(MaterialIssues.QuantityFailure + SemifinishedItems.FailureWeights * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") 
            queryString = queryString + "       FROM            MaterialIssues " + "\r\n";
            queryString = queryString + "                       INNER JOIN (SELECT MaterialIssueID, SUM(Quantity) AS Quantity FROM SemifinishedItemMaterials WHERE SemifinishedItemID = @EntityID GROUP BY MaterialIssueID) SemifinishedItemMaterials ON MaterialIssues.MaterialIssueID = SemifinishedItemMaterials.MaterialIssueID " + "\r\n";

            //queryString = queryString + "       IF @@ROWCOUNT <> 1 " + "\r\n";
            //queryString = queryString + "           BEGIN " + "\r\n";
            //queryString = queryString + "               SET         @msg = N'Phiếu xuất NVL đã hủy, chưa duyệt hoặc đã xóa.' ; " + "\r\n";
            //queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            //queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       IF (@SaveRelativeOption <> 1) " + "\r\n"; //Undo
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DELETE FROM     SemifinishedItemMaterials WHERE SemifinishedItemID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            #endregion INIT FirmOrders


            queryString = queryString + "   END " + "\r\n";


            this.totalSmartPortalEntities.CreateStoredProcedure("SemifinishedItemSaveRelative", queryString);
        }

        private void SemifinishedItemPostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày xuất nguyên liệu: ' + CAST(MaterialIssues.EntryDate AS nvarchar) FROM SemifinishedItemDetails INNER JOIN MaterialIssues ON SemifinishedItemDetails.SemifinishedItemID = @EntityID AND SemifinishedItemDetails.MaterialIssueID = MaterialIssues.MaterialIssueID AND SemifinishedItemDetails.EntryDate < MaterialIssues.EntryDate ";            
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Số lượng NVL sử dụng vượt quá số lượng đã xuất kho cho sản xuất: ' + CAST(ROUND(Quantity - QuantitySemifinished - QuantityFailure - QuantityReceipted - QuantityLoss, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM MaterialIssueDetails WHERE (ROUND(Quantity - QuantitySemifinished - QuantityFailure - QuantityReceipted - QuantityLoss, " + (int)GlobalEnums.rndQuantity + ") < 0) ";
            //queryArray[2] = " SELECT TOP 1 @FoundEntity = N'Số lượng xuất vượt quá số lượng đặt hàng: ' + CAST(ROUND(Quantity - (QuantitySemifinished - QuantityShortage - QuantityFailure + QuantityExcess), " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM FirmOrderDetails WHERE 1 = 0 AND (ROUND(Quantity - (QuantitySemifinished - QuantityShortage - QuantityFailure + QuantityExcess), " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("SemifinishedItemPostSaveValidate", queryArray);
        }




        private void SemifinishedItemApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = SemifinishedItemID FROM SemifinishedItems WHERE SemifinishedItemID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("SemifinishedItemApproved", queryArray);
        }


        private void SemifinishedItemEditable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = SemifinishedItemID FROM SemifinishedItems WHERE SemifinishedItemID = @EntityID AND NOT SemifinishedHandoverID IS NULL ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = SemifinishedItemID FROM SemifinishedHandoverDetails WHERE SemifinishedItemID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("SemifinishedItemEditable", queryArray);
        }

        private void SemifinishedItemToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      SemifinishedItems  SET Approved = @Approved, ApprovedDate = GetDate() WHERE SemifinishedItemID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          SemifinishedItemDetails  SET Approved = @Approved WHERE SemifinishedItemID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, N'hủy', '')  + N' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("SemifinishedItemToggleApproved", queryString);
        }

        private void SemifinishedItemInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("SemifinishedItems", "SemifinishedItemID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.SemifinishedItem));
            this.totalSmartPortalEntities.CreateTrigger("SemifinishedItemInitReference", simpleInitReference.CreateQuery());
        }


        #endregion


        private void SemifinishedItemSheet()
        {
            string queryString = " @WorkshiftID int, @SemifinishedItemID int, @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @LocalWorkshiftID int, @LocalSemifinishedItemID int, @LocalFromDate DateTime, @LocalToDate DateTime     " + "\r\n";
            queryString = queryString + "       SET             @LocalWorkshiftID = @WorkshiftID    SET @LocalSemifinishedItemID = @SemifinishedItemID        SET @LocalFromDate = @FromDate      SET @LocalToDate = @ToDate " + "\r\n";

            queryString = queryString + "       IF  (@LocalWorkshiftID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.SemifinishedItemSheetSQL(true, false) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               IF  (@LocalSemifinishedItemID <> 0) " + "\r\n";
            queryString = queryString + "                   " + this.SemifinishedItemSheetSQL(false, true) + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n";
            queryString = queryString + "                   " + this.SemifinishedItemSheetSQL(false, false) + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "    END " + "\r\n";


            this.totalSmartPortalEntities.CreateStoredProcedure("SemifinishedItemSheet", queryString);
        }

        private string SemifinishedItemSheetSQL(bool workshiftID, bool SemifinishedItemID)
        {
            string queryString = " " + "\r\n";

            queryString = queryString + "       SELECT          SemifinishedItems.SemifinishedItemID, SemifinishedItems.Reference, SemifinishedItems.EntryDate, Workshifts.Code AS WorkshiftCode, Workshifts.EntryDate AS WorkshiftEntryDate, CrucialWorkers.Name AS CrucialWorkerName, CrucialWorkers.LastName AS CrucialWorkerLastName, ProductionLines.Code AS ProductionLineCode, " + "\r\n";
            queryString = queryString + "                       FirmOrders.EntryDate AS FirmOrderEntryDate, FirmOrders.Reference AS FirmOrderReference, FirmOrders.Code AS FirmOrderCode, FirmOrders.DeliveryDate, Customers.Name AS CustomerName, MaterialIssues.EntryDate AS MaterialIssueEntryDate, MaterialIssues.Code AS MaterialIssueCode, Boms.Code AS BomCode, FirmOrderDetails.Quantity AS FirmOrderQuantity, FirmOrderDetails.QuantitySemifinished - FirmOrderDetails.QuantityShortage - FirmOrderDetails.QuantityFailure + FirmOrderDetails.QuantityExcess AS QuantityProduced, FirmOrderDetails.Quantity - (FirmOrderDetails.QuantitySemifinished - FirmOrderDetails.QuantityShortage - FirmOrderDetails.QuantityFailure + FirmOrderDetails.QuantityExcess) AS QuantityRemains, " + "\r\n"; //ACCUMMULATED
            queryString = queryString + "                       SemifinishedItems.StartDate, SemifinishedItems.StopDate, Commodities.Code, Commodities.Name, Molds.Code AS MoldCode, SemifinishedItemDetails.Quantity, SemifinishedItemDetails.MoldQuantity, SemifinishedItemDetails.PiecePerPack, ISNULL(FirmOrders.Description, '') + ISNULL(' [ĐHCK: ' + SemifinishedItems.Description + ']', '') AS Description " + "\r\n";

            queryString = queryString + "       FROM            SemifinishedItems " + "\r\n";
            queryString = queryString + "                       INNER JOIN SemifinishedItemDetails ON " + this.SemifinishedItemSheetOption(workshiftID, SemifinishedItemID) + " AND SemifinishedItems.SemifinishedItemID = SemifinishedItemDetails.SemifinishedItemID " + "\r\n";
            queryString = queryString + "                       INNER JOIN FirmOrders ON SemifinishedItems.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "                       INNER JOIN FirmOrderDetails ON SemifinishedItemDetails.FirmOrderDetailID = FirmOrderDetails.FirmOrderDetailID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Molds ON FirmOrderDetails.MoldID = Molds.MoldID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON SemifinishedItems.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN MaterialIssues ON SemifinishedItems.MaterialIssueID = MaterialIssues.MaterialIssueID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Boms ON SemifinishedItems.BomID = Boms.BomID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Workshifts ON SemifinishedItems.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Employees AS CrucialWorkers ON SemifinishedItems.CrucialWorkerID = CrucialWorkers.EmployeeID " + "\r\n";
            queryString = queryString + "                       INNER JOIN ProductionLines ON SemifinishedItems.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities ON SemifinishedItemDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            //queryString = queryString + "                       LEFT JOIN  (SELECT FirmOrderID, MAX(EntryDate) AS ItemEntryDate, MAX(CommodityID) AS ItemID, SUM(Quantity) AS ItemQuantity, SUM(QuantitySemifinished) AS ItemQuantitySemifinished, SUM(QuantityFailure) AS ItemQuantityFailure, SUM(QuantityReceipted) AS ItemQuantityReceipted, SUM(QuantityLoss) AS ItemQuantityLoss FROM MaterialIssueDetails WHERE FirmOrderID IN (SELECT FirmOrderID FROM SemifinishedItems WHERE " + this.SemifinishedItemSheetOption(workshiftID, SemifinishedItemID) + ") GROUP BY FirmOrderID) AS MaterialIssueSummaries ON FirmOrderDetails.FirmOrderID = MaterialIssueSummaries.FirmOrderID " + "\r\n";

            queryString = queryString + "       ORDER BY        SemifinishedItemDetails.SemifinishedItemDetailID " + "\r\n";

            return queryString;
        }

        private string SemifinishedItemSheetOption(bool workshiftID, bool SemifinishedItemID) { return (workshiftID ? "SemifinishedItems.WorkshiftID = @LocalWorkshiftID" : (SemifinishedItemID ? "SemifinishedItems.SemifinishedItemID = @LocalSemifinishedItemID" : "SemifinishedItems.EntryDate >= @LocalFromDate AND SemifinishedItems.EntryDate <= @LocalToDate")); }

    }
}
