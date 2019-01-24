using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Commons
{
    public class Commodity
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public Commodity(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetCommodityIndexes();

            this.CommoditySaveRelative();

            this.CommodityEditable();
            this.CommodityDeletable();

            this.GetCommodityBases();
        }


        private void GetCommodityIndexes()
        {
            string queryString;

            queryString = " @NMVNTaskID int, @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      Commodities.CommodityID, EntireCommodityCategories.Name1 AS CommodityCategoryName1, EntireCommodityCategories.Name2 AS CommodityCategoryName2, Commodities.Code, Commodities.OfficialCode, Commodities.CodePartA, Commodities.CodePartB, Commodities.CodePartC, Commodities.CodePartD, Commodities.CodePartE, Commodities.CodePartF, Commodities.Name, Commodities.OfficialName, Commodities.OriginalName, Commodities.SalesUnit, LTRIM(RTRIM(Commodities.Specification + ' ' + ISNULL(Commodities.Description, ''))) AS Specification, LTRIM(RTRIM(ISNULL(Commodities.Remarks, '') + ' ' + ISNULL(Commodities.Caption, ''))) AS Remarks, Commodities.Discontinue, Commodities.InActive, " + "\r\n";
            queryString = queryString + "                   CommodityBoms.BomID, CommodityBoms.Code AS BomCode, CommodityBoms.Name AS BomName, CommodityBoms.BlockUnit, CommodityBoms.BlockQuantity, CommodityMolds.MoldID, CommodityMolds.Code AS MoldCode, CommodityMolds.Name AS MoldName, CommodityMolds.Quantity AS MoldQuantity, Commodities.Weight, Commodities.Packing " + " \r\n";
            queryString = queryString + "       FROM        Commodities " + "\r\n";
            queryString = queryString + "                   INNER JOIN EntireCommodityCategories ON Commodities.NMVNTaskID = @NMVNTaskID AND Commodities.CommodityCategoryID = EntireCommodityCategories.CommodityCategoryID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN (SELECT CommodityBoms.CommodityID, CommodityBoms.BomID, Boms.Code, Boms.Name, CommodityBoms.BlockUnit, CommodityBoms.BlockQuantity FROM CommodityBoms INNER JOIN Boms ON CommodityBoms.IsDefault = 1 AND CommodityBoms.BomID = Boms.BomID) CommodityBoms ON Commodities.CommodityID = CommodityBoms.CommodityID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN (SELECT CommodityMolds.CommodityID, CommodityMolds.MoldID, Molds.Code, Molds.Name, CommodityMolds.Quantity FROM CommodityMolds INNER JOIN Molds ON CommodityMolds.IsDefault = 1 AND CommodityMolds.MoldID = Molds.MoldID) CommodityMolds ON Commodities.CommodityID = CommodityMolds.CommodityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetCommodityIndexes", queryString);
        }


        private void CommoditySaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            //Boms: SHOULD CHECK AND MODIFY TO MEET REQUIRELEMENT
            queryString = queryString + "   IF (SELECT COUNT(*) FROM Commodities WHERE CommodityID = @EntityID AND CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Items + ") = 1 " + "\r\n";
            queryString = queryString + "       BEGIN " + "\r\n";
            queryString = queryString + "           IF (SELECT COUNT(*) FROM Boms WHERE MaterialID = @EntityID) = 0  " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   INSERT INTO     Boms (EntryDate, EffectiveDate, Reference, Code, Name, OfficialCode, CommodityID, MaterialID, CommodityTypeID, CommodityCategoryID, CommodityClassID, CommodityLineID, CustomerID, LayerCount, TotalQuantity, Remarks, InActive) " + "\r\n";
            queryString = queryString + "                   SELECT          GetDate() AS EntryDate, GetDate() AS EffectiveDate, '####000', Code, Name, OfficialCode, NULL AS CommodityID, CommodityID AS MaterialID, " + (int)GlobalEnums.CommodityTypeID.Products + ", CommodityCategoryID, CommodityClassID, CommodityLineID, NULL AS CustomerID, 1 AS LayerCount, 1 AS TotalQuantity, Remarks, 0 AS InActive " + "\r\n";
            queryString = queryString + "                   FROM            Commodities WHERE CommodityID = @EntityID " + "\r\n";

            queryString = queryString + "                   INSERT INTO     BomDetails (BomID, MaterialID, LayerCode, LayerQuantity, BlockUnit, BlockQuantity, Quantity, Remarks, InActive) " + "\r\n";
            queryString = queryString + "                   SELECT          BomID, MaterialID, 'A' AS LayerCode, 1 AS LayerQuantity, 1 AS BlockUnit, 1 AS BlockQuantity, 1 AS Quantity, Remarks, InActive " + "\r\n";
            queryString = queryString + "                   FROM            Boms WHERE BomID = SCOPE_IDENTITY() " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "           ELSE " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   UPDATE          Boms " + "\r\n"; //Boms.BomID = 1: DEFAULT NULL Boms: FOR INIT SOME WHERE ONLY
            queryString = queryString + "                   SET             Boms.Code = Commodities.Code, Boms.OfficialCode = Commodities.OfficialCode, Boms.Name = Commodities.Name, Boms.CommodityCategoryID = Commodities.CommodityCategoryID, Boms.CommodityClassID = Commodities.CommodityClassID, Boms.CommodityLineID = Commodities.CommodityLineID " + "\r\n";
            queryString = queryString + "                   FROM            Boms INNER JOIN Commodities ON Boms.BomID <> 1 AND Boms.MaterialID = @EntityID AND Boms.MaterialID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";


            queryString = queryString + "           IF (SELECT COUNT(*) FROM CommodityMolds WHERE CommodityID = @EntityID) = 0  " + "\r\n";
            queryString = queryString + "               INSERT INTO     CommodityMolds (CommodityID, MoldID, EntryDate, Quantity, Remarks, IsDefault, InActive) " + "\r\n";
            queryString = queryString + "               VALUES                         (@EntityID, 0, GETDATE(), 1, NULL, 1, 0) " + "\r\n";
            queryString = queryString + "       END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("CommoditySaveRelative", queryString);
        }

        private void CommodityEditable()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = CommodityID FROM Commodities WHERE @EntityID = 1"; //AT TUE VIET ONLY: Don't allow edit default mold, because it is related to Customers

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = CommodityID FROM Commodities WHERE CommodityID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            //queryArray[1] = " SELECT TOP 1 @FoundEntity = CommodityID FROM GoodsIssueDetails WHERE CommodityID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("CommodityEditable", queryArray);
        }

        private void CommodityDeletable()
        {
            string[] queryArray;

            if (!GlobalEnums.CBPP)
            {
                queryArray = new string[1];
                queryArray[0] = " SELECT TOP 1 @FoundEntity = CommodityID FROM Commodities WHERE CommodityID = @EntityID "; //DON'T ALLOW TO DELETE 
            }
            else
            {
                queryArray = new string[35];

                queryArray[0] = " SELECT TOP 1 @FoundEntity = CommodityID FROM PurchaseOrderDetails WHERE CommodityID = @EntityID ";
                queryArray[1] = " SELECT TOP 1 @FoundEntity = CommodityID FROM PurchaseRequisitionDetails WHERE CommodityID = @EntityID ";
                queryArray[2] = " SELECT TOP 1 @FoundEntity = CommodityID FROM GoodsReceiptDetails WHERE CommodityID = @EntityID ";
                queryArray[3] = " SELECT TOP 1 @FoundEntity = CommodityID FROM GoodsIssueDetails WHERE CommodityID = @EntityID ";
                queryArray[4] = " SELECT TOP 1 @FoundEntity = CommodityID FROM WarehouseAdjustmentDetails WHERE CommodityID = @EntityID ";
                queryArray[5] = " SELECT TOP 1 @FoundEntity = CommodityID FROM SalesOrderDetails WHERE CommodityID = @EntityID ";
                queryArray[6] = " SELECT TOP 1 @FoundEntity = CommodityID FROM QuotationDetails WHERE CommodityID = @EntityID ";
                queryArray[7] = " SELECT TOP 1 @FoundEntity = CommodityID FROM SalesReturnDetails WHERE CommodityID = @EntityID ";
                queryArray[8] = " SELECT TOP 1 @FoundEntity = CommodityID FROM PlannedOrderDetails WHERE CommodityID = @EntityID ";
                queryArray[9] = " SELECT TOP 1 @FoundEntity = CommodityID FROM BlendingInstructionDetails WHERE CommodityID = @EntityID ";
                queryArray[10] = " SELECT TOP 1 @FoundEntity = CommodityID FROM BlendingInstructions WHERE CommodityID = @EntityID ";
                queryArray[11] = " SELECT TOP 1 @FoundEntity = CommodityID FROM TransferOrderDetails WHERE CommodityID = @EntityID ";
                queryArray[12] = " SELECT TOP 1 @FoundEntity = CommodityID FROM MaterialIssueDetails WHERE CommodityID = @EntityID ";
                queryArray[13] = " SELECT TOP 1 @FoundEntity = CommodityID FROM PackageIssueDetails WHERE CommodityID = @EntityID ";
                queryArray[14] = " SELECT TOP 1 @FoundEntity = CommodityID FROM PackageIssues WHERE CommodityID = @EntityID ";
                queryArray[15] = " SELECT TOP 1 @FoundEntity = CommodityID FROM WarehouseTransferDetails WHERE CommodityID = @EntityID ";
                queryArray[16] = " SELECT TOP 1 @FoundEntity = CommodityID FROM CreditNoteDetails WHERE CommodityID = @EntityID ";
                queryArray[17] = " SELECT TOP 1 @FoundEntity = CommodityID FROM DeliveryAdviceDetails WHERE CommodityID = @EntityID ";
                queryArray[18] = " SELECT TOP 1 @FoundEntity = CommodityID FROM FinishedHandoverDetails WHERE CommodityID = @EntityID ";
                queryArray[19] = " SELECT TOP 1 @FoundEntity = CommodityID FROM FinishedProductDetails WHERE CommodityID = @EntityID ";
                queryArray[20] = " SELECT TOP 1 @FoundEntity = CommodityID FROM FinishedProductPackages WHERE CommodityID = @EntityID ";
                queryArray[21] = " SELECT TOP 1 @FoundEntity = CommodityID FROM FirmOrderDetails WHERE CommodityID = @EntityID ";
                queryArray[22] = " SELECT TOP 1 @FoundEntity = MaterialID FROM FirmOrderMaterials WHERE MaterialID = @EntityID ";
                queryArray[23] = " SELECT TOP 1 @FoundEntity = CommodityID FROM GoodsArrivalDetails WHERE CommodityID = @EntityID ";
                queryArray[24] = " SELECT TOP 1 @FoundEntity = CommodityID FROM GoodsArrivalPackages WHERE CommodityID = @EntityID ";
                queryArray[25] = " SELECT TOP 1 @FoundEntity = CommodityID FROM SemifinishedItemDetails WHERE CommodityID = @EntityID ";
                queryArray[26] = " SELECT TOP 1 @FoundEntity = MaterialID FROM SemifinishedItemMaterials WHERE MaterialID = @EntityID ";
                queryArray[27] = " SELECT TOP 1 @FoundEntity = CommodityID FROM SemifinishedProductDetails WHERE CommodityID = @EntityID ";
                queryArray[28] = " SELECT TOP 1 @FoundEntity = CommodityID FROM AccountInvoiceDetails WHERE CommodityID = @EntityID ";
                queryArray[29] = " SELECT TOP 1 @FoundEntity = MaterialID FROM BomDetails WHERE MaterialID = @EntityID ";
                queryArray[30] = " SELECT TOP 1 @FoundEntity = CommodityID FROM Boms WHERE CommodityID = @EntityID OR MaterialID = @EntityID ";
                queryArray[31] = " SELECT TOP 1 @FoundEntity = CommodityID FROM Batches WHERE CommodityID = @EntityID ";
                queryArray[32] = " SELECT TOP 1 @FoundEntity = CommodityID FROM PromotionCommodities WHERE CommodityID = @EntityID ";
                queryArray[33] = " SELECT TOP 1 @FoundEntity = CommodityID FROM CommodityBoms WHERE CommodityID = @EntityID ";
                queryArray[34] = " SELECT TOP 1 @FoundEntity = CommodityID FROM CommodityMolds WHERE CommodityID = @EntityID  ";
            }

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("CommodityDeletable", queryArray);
        }

        private void GetCommodityBases()
        {
            string queryString;
            string querySELECT = "                              Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Commodities.PiecePerPack, Commodities.ListedPrice, Commodities.GrossPrice, 0.0 AS DiscountPercent, 0.0 AS TradeDiscountRate, CommodityCategories.VATPercent " + " \r\n";
            string queryFROM = "                                @Commodities Commodities INNER JOIN CommodityCategories ON Commodities.CommodityCategoryID = CommodityCategories.CommodityCategoryID " + " \r\n";

            queryString = " @CommodityTypeIDList varchar(200), @NmvnTaskID int, @WarehouseID int, @SearchText nvarchar(60) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @Commodities TABLE (CommodityID int NOT NULL, Code nvarchar(50) NOT NULL, Name nvarchar(200) NOT NULL, PiecePerPack int NOT NULL, ListedPrice decimal(18, 2) NOT NULL, GrossPrice decimal(18, 2) NOT NULL, DiscountPercent decimal(18, 2) NOT NULL, TradeDiscountRate decimal(18, 2) NOT NULL, CommodityTypeID int NOT NULL, CommodityCategoryID int NOT NULL)" + "\r\n";
            queryString = queryString + "       INSERT INTO     @Commodities SELECT TOP 30 CommodityID, Code, Name, PiecePerPack, ListedPrice, GrossPrice, 0.0 AS DiscountPercent, 0.0 AS TradeDiscountRate, CommodityTypeID, CommodityCategoryID FROM Commodities WHERE InActive = 0 AND (@SearchText = '' OR Code = @SearchText OR Code LIKE '%' + @SearchText + '%' OR OfficialCode LIKE '%' + @SearchText + '%' OR Name LIKE '%' + @SearchText + '%') AND CommodityTypeID IN (SELECT Id FROM dbo.SplitToIntList (@CommodityTypeIDList)) " + "\r\n";

            queryString = queryString + "       IF (@NmvnTaskID IN (" + (int)GlobalEnums.NmvnTaskID.PlannedProduct + "," + (int)GlobalEnums.NmvnTaskID.PlannedItem + ")) " + " \r\n";
            queryString = queryString + "           SELECT      " + querySELECT + ", CommodityBoms.BomID, CommodityBoms.Code AS BomCode, CommodityBoms.Name AS BomName, CommodityBoms.BlockUnit, CommodityBoms.BlockQuantity, CommodityMolds.MoldID, CommodityMolds.Code AS MoldCode, CommodityMolds.Name AS MoldName, CommodityMolds.Quantity AS MoldQuantity, 0.0 AS QuantityAvailables " + " \r\n";
            queryString = queryString + "           FROM        " + queryFROM + "\r\n";
            queryString = queryString + "                       LEFT JOIN (SELECT CommodityBoms.CommodityID, CommodityBoms.BomID, Boms.Code, Boms.Name, CommodityBoms.BlockUnit, CommodityBoms.BlockQuantity FROM CommodityBoms INNER JOIN Boms ON CommodityBoms.CommodityID IN (SELECT CommodityID FROM @Commodities) AND CommodityBoms.IsDefault = 1 AND CommodityBoms.BomID = Boms.BomID) CommodityBoms ON Commodities.CommodityID = CommodityBoms.CommodityID " + "\r\n";
            queryString = queryString + "                       LEFT JOIN (SELECT CommodityMolds.CommodityID, CommodityMolds.MoldID, Molds.Code, Molds.Name, CommodityMolds.Quantity FROM CommodityMolds INNER JOIN Molds ON CommodityMolds.CommodityID IN (SELECT CommodityID FROM @Commodities) AND CommodityMolds.IsDefault = 1 AND CommodityMolds.MoldID = Molds.MoldID) CommodityMolds ON Commodities.CommodityID = CommodityMolds.CommodityID " + "\r\n";
            queryString = queryString + "       ELSE " + " \r\n";
            queryString = queryString + "           BEGIN " + "\r\n";

            querySELECT = querySELECT + "               " + ", NULL AS BomID, NULL AS BomCode, NULL AS BomName, NULL AS BlockUnit, NULL AS BlockQuantity, NULL AS MoldID, NULL AS MoldCode, NULL AS MoldName, NULL AS MoldQuantity " + "\r\n";

            queryString = queryString + "               IF (@WarehouseID > 0) " + "\r\n";
            queryString = queryString + "                   SELECT      " + querySELECT + ", ISNULL(CommoditiesAvailables.QuantityAvailables, 0.0) AS QuantityAvailables " + " \r\n";
            queryString = queryString + "                   FROM        " + queryFROM + "\r\n";
            queryString = queryString + "                               LEFT JOIN (SELECT CommodityID, SUM(Quantity - QuantityIssued) AS QuantityAvailables FROM GoodsReceiptDetails WHERE ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 AND WarehouseID = @WarehouseID AND CommodityID IN (SELECT DISTINCT CommodityID FROM @Commodities) GROUP BY CommodityID) CommoditiesAvailables ON Commodities.CommodityID = CommoditiesAvailables.CommodityID " + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n";
            queryString = queryString + "                   SELECT      " + querySELECT + ", 0.0 AS QuantityAvailables " + " \r\n";
            queryString = queryString + "                   FROM        " + queryFROM + "\r\n";

            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetCommodityBases", queryString);
        }

    }
}

