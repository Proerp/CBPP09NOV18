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

            queryString = queryString + "       SELECT      Commodities.CommodityID, EntireCommodityCategories.Name1 AS CommodityCategoryName1, EntireCommodityCategories.Name2 AS CommodityCategoryName2, Commodities.Code, Commodities.OfficialCode, Commodities.CodePartA, Commodities.CodePartB, Commodities.CodePartC, Commodities.CodePartD, Commodities.CodePartE, Commodities.CodePartF, Commodities.Name, Commodities.SalesUnit, Commodities.Remarks, Commodities.Discontinue, Commodities.InActive, " + "\r\n";
            queryString = queryString + "                   CommodityBoms.BomID, CommodityBoms.Code AS BomCode, CommodityBoms.Name AS BomName, CommodityBoms.BlockUnit, CommodityBoms.BlockQuantity, CommodityMolds.MoldID, CommodityMolds.Code AS MoldCode, CommodityMolds.Name AS MoldName, CommodityMolds.Quantity AS MoldQuantity " + " \r\n";
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
            queryString = queryString + "           IF (SELECT COUNT(*) FROM Boms WHERE CustomerID = @EntityID) = 0  " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   INSERT INTO     Boms (EntryDate, Reference, Code, Name, OfficialCode, CommodityCategoryID, CommodityClassID, CommodityLineID, CustomerID, Remarks, InActive) " + "\r\n";
            queryString = queryString + "                   SELECT          GetDate() AS  EntryDate, '####000', Code, Name, OfficialCode, CommodityCategoryID, CommodityClassID, CommodityLineID, CommodityID AS CustomerID, Remarks, 0 AS InActive " + "\r\n";
            queryString = queryString + "                   FROM            Commodities WHERE CommodityID = @EntityID " + "\r\n";

            queryString = queryString + "                   INSERT INTO     BomDetails (BomID, MaterialID, BlockUnit, BlockQuantity, Remarks, InActive) " + "\r\n";
            queryString = queryString + "                   SELECT          BomID, CustomerID AS MaterialID, 1 AS BlockUnit, 1 AS BlockQuantity, Remarks, InActive " + "\r\n";
            queryString = queryString + "                   FROM            Boms WHERE BomID = SCOPE_IDENTITY() " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "           ELSE " + "\r\n";
            queryString = queryString + "                   UPDATE          Boms " + "\r\n"; //Boms.BomID = 1: DEFAULT NULL Boms: FOR INIT SOME WHERE ONLY
            queryString = queryString + "                   SET             Boms.Code = Commodities.Code, Boms.OfficialCode = Commodities.OfficialCode, Boms.Name = Commodities.Name, Boms.CommodityCategoryID = Commodities.CommodityCategoryID, Boms.CommodityClassID = Commodities.CommodityClassID, Boms.CommodityLineID = Commodities.CommodityLineID " + "\r\n";
            queryString = queryString + "                   FROM            Boms INNER JOIN Commodities ON Boms.BomID <> 1 AND Boms.CustomerID = @EntityID AND Boms.CustomerID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "       END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("CommoditySaveRelative", queryString);
        }

        private void CommodityEditable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = CommodityID FROM Commodities WHERE @EntityID = 1"; //AT TUE VIET ONLY: Don't allow edit default mold, because it is related to Customers

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = CommodityID FROM Commodities WHERE CommodityID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            //queryArray[1] = " SELECT TOP 1 @FoundEntity = CommodityID FROM GoodsIssueDetails WHERE CommodityID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("CommodityEditable", queryArray);
        }

        private void CommodityDeletable()
        {
            string[] queryArray = new string[1];
            queryArray[0] = " SELECT TOP 1 @FoundEntity = CommodityID FROM Commodities WHERE CommodityID = @EntityID "; //DON'T ALLOW TO DELETE 

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

            queryString = queryString + "       IF (@NmvnTaskID = " + (int)GlobalEnums.NmvnTaskID.PlannedOrder + ") " + " \r\n";
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

