using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Commons
{
    public class Bom
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public Bom(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetCommodityBoms();

            this.AddCommodityBom();
            this.RemoveCommodityBom();

            this.UpdateCommodityBom();

            this.GetBomBases();
        }


        private void GetCommodityBoms()
        {
            string queryString;

            queryString = " @CommodityID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      CommodityBoms.CommodityBomID, CommodityBoms.BomID, Boms.Code AS BomCode, Boms.Name AS BomName, CommodityBoms.EntryDate, CommodityBoms.Remarks, CommodityBoms.BlockUnit, CommodityBoms.BlockQuantity, CommodityBoms.IsDefault, CommodityBoms.InActive " + "\r\n";
            queryString = queryString + "       FROM        CommodityBoms INNER JOIN Boms ON CommodityBoms.CommodityID = @CommodityID AND CommodityBoms.BomID = Boms.BomID " + "\r\n";
            queryString = queryString + "       ORDER BY    CommodityBoms.EntryDate, CommodityBoms.CommodityBomID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetCommodityBoms", queryString);
        }



        private void AddCommodityBom()
        {
            string queryString;

            queryString = " @CommodityID int, @BomID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";
            queryString = queryString + "       DECLARE         @COUNTCommodityBomID int = (SELECT COUNT(CommodityBomID) FROM CommodityBoms WHERE CommodityID = @CommodityID) " + "\r\n";

            queryString = queryString + "       INSERT INTO     CommodityBoms   (CommodityID, BomID, EntryDate, BlockUnit, BlockQuantity, Remarks, IsDefault, InActive) " + "\r\n";
            queryString = queryString + "       VALUES                          (@CommodityID, @BomID, GETDATE(), 100, 1, NULL, IIF(@COUNTCommodityBomID = 0, 1, 0), 0) " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("AddCommodityBom", queryString);
        }

        private void RemoveCommodityBom()
        {
            string queryString;

            queryString = " @CommodityBomID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";
            queryString = queryString + "       DECLARE         @CommodityID int, @MINCommodityBomID int, @COUNTCommodityBomID int, @IsDefault bit " + "\r\n";
            queryString = queryString + "       SELECT          @CommodityID = CommodityID, @IsDefault = IsDefault FROM CommodityBoms WHERE CommodityBomID = @CommodityBomID " + "\r\n";

            queryString = queryString + "       DELETE FROM     CommodityBoms WHERE CommodityBomID = @CommodityBomID" + "\r\n";

            queryString = queryString + "       SELECT          @MINCommodityBomID = MIN(CommodityBomID), @COUNTCommodityBomID = COUNT(CommodityBomID) FROM CommodityBoms WHERE CommodityID = @CommodityID " + "\r\n";
            queryString = queryString + "       IF (@IsDefault = 1 OR @COUNTCommodityBomID = 1) UPDATE CommodityBoms SET IsDefault = 1 WHERE CommodityBomID = @MINCommodityBomID" + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("RemoveCommodityBom", queryString);

        }

        private void UpdateCommodityBom()
        {
            string queryString = " @CommodityBomID int, @CommodityID int, @BlockUnit decimal(18, 2), @BlockQuantity decimal(18, 2), @Remarks nvarchar(50), @IsDefault bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       BEGIN " + "\r\n";

            queryString = queryString + "           UPDATE          CommodityBoms " + "\r\n";
            queryString = queryString + "           SET             BlockUnit = ROUND(IIF(@BlockUnit > 0, @BlockUnit, BlockUnit), " + (int)GlobalEnums.rndN0 + "), BlockQuantity = ROUND(IIF(@BlockQuantity > 0, @BlockQuantity, BlockQuantity), " + (int)GlobalEnums.rndQuantity + "), Remarks = @Remarks " + "\r\n";
            queryString = queryString + "           WHERE           CommodityBomID = @CommodityBomID " + "\r\n";

            queryString = queryString + "           IF (@IsDefault = 1) " + "\r\n"; //ONLY CHANGE WHEN @IsDefault = true: THIS WILL KEEP AT LEAST 1 ROW IS DEFAULT
            queryString = queryString + "               UPDATE      CommodityBoms " + "\r\n";
            queryString = queryString + "               SET         IsDefault = IIF(CommodityBomID = @CommodityBomID, 1, 0) " + "\r\n"; //IIF(CommodityBomID = @CommodityBomID, @IsDefault, 0)
            queryString = queryString + "               WHERE       CommodityID = @CommodityID " + "\r\n";

            queryString = queryString + "       END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("UpdateCommodityBom", queryString);
        }

        private void GetBomBases()
        {
            string queryString;

            string querySELECT = "              SELECT      Boms.BomID, Boms.Code, Boms.Name, Boms.Reference " + " \r\n";
            string queryFROM = "                FROM        Boms " + "\r\n";
            string queryWHERE = "               WHERE       Boms.InActive = 0 AND (@SearchText = '' OR Boms.Code LIKE '%' + @SearchText + '%' OR Boms.OfficialCode LIKE '%' + @SearchText + '%' OR Boms.Name LIKE '%' + @SearchText + '%' OR Boms.Reference LIKE '%' + @SearchText + '%') " + "\r\n";

            queryString = " @SearchText nvarchar(60), @CommodityID int, @CommodityCategoryID int, @CommodityClassID int, @CommodityLineID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF (@CommodityID > 0) " + "\r\n"; //GET ALL BOMS BY @CommodityID
            queryString = queryString + "           " + querySELECT + ", CommodityBoms.BlockUnit, CommodityBoms.BlockQuantity " + "\r\n";
            queryString = queryString + "           " + queryFROM + " INNER JOIN CommodityBoms ON CommodityBoms.CommodityID = @CommodityID AND Boms.BomID = CommodityBoms.BomID " + "\r\n";
            queryString = queryString + "           " + queryWHERE + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n"; //GET ALL BOMS BY @CommodityCategoryID AND @CommodityClassID AND @CommodityLineID
            queryString = queryString + "           " + querySELECT + ", 0.0 AS BlockUnit, 0.0 AS BlockQuantity " + "\r\n";
            queryString = queryString + "           " + queryFROM + "\r\n";
            queryString = queryString + "           " + queryWHERE + " AND CommodityLineID = @CommodityLineID " + "\r\n"; //AND CommodityCategoryID = @CommodityCategoryID AND CommodityClassID = @CommodityClassID 
            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetBomBases", queryString);
        }
    }
}
