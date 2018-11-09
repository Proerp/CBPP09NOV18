using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Commons
{
    public class Mold
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public Mold(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetMoldIndexes();

            this.MoldEditable();
            this.MoldDeletable();


            this.GetCommodityMolds();

            this.AddCommodityMold();
            this.RemoveCommodityMold();

            this.UpdateCommodityMold();

            this.GetMoldBases();
        }


        private void GetMoldIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      MoldID, Code, Name, Quantity, MoldWidth, MoldLength, ItemWidth, ItemLength, ItemHigh, Remarks, InActive " + "\r\n";
            queryString = queryString + "       FROM        Molds " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetMoldIndexes", queryString);
        }

        private void MoldEditable()
        {
            string[] queryArray = new string[0];

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("MoldEditable", queryArray);
        }

        private void MoldDeletable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = MoldID FROM CommodityMolds WHERE @EntityID = @EntityID ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = MoldID FROM PlannedOrderDetails WHERE @EntityID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("MoldDeletable", queryArray);
        }





        private void GetCommodityMolds()
        {
            string queryString;

            queryString = " @CommodityID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      CommodityMolds.CommodityMoldID, CommodityMolds.MoldID, Molds.Code AS MoldCode, Molds.Name AS MoldName, CommodityMolds.EntryDate, CommodityMolds.Remarks, CommodityMolds.Quantity, CommodityMolds.IsDefault, CommodityMolds.InActive " + "\r\n";
            queryString = queryString + "       FROM        CommodityMolds INNER JOIN Molds ON CommodityMolds.CommodityID = @CommodityID AND CommodityMolds.MoldID = Molds.MoldID " + "\r\n";
            queryString = queryString + "       ORDER BY    CommodityMolds.EntryDate, CommodityMolds.CommodityMoldID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetCommodityMolds", queryString);
        }



        private void AddCommodityMold()
        {
            string queryString;

            queryString = " @CommodityID int, @MoldID int, @Quantity decimal(18, 2)" + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";
            queryString = queryString + "       DECLARE         @COUNTCommodityMoldID int = (SELECT COUNT(CommodityMoldID) FROM CommodityMolds WHERE CommodityID = @CommodityID) " + "\r\n";

            queryString = queryString + "       INSERT INTO     CommodityMolds  (CommodityID, MoldID, EntryDate, Quantity, Remarks, IsDefault, InActive) " + "\r\n";
            queryString = queryString + "       VALUES                          (@CommodityID, @MoldID, GETDATE(), @Quantity, NULL, IIF(@COUNTCommodityMoldID = 0, 1, 0), 0) " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("AddCommodityMold", queryString);
        }

        private void RemoveCommodityMold()
        {
            string queryString;

            queryString = " @CommodityMoldID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";
            queryString = queryString + "       DECLARE         @CommodityID int, @MINCommodityMoldID int, @COUNTCommodityMoldID int, @IsDefault bit " + "\r\n";
            queryString = queryString + "       SELECT          @CommodityID = CommodityID, @IsDefault = IsDefault FROM CommodityMolds WHERE CommodityMoldID = @CommodityMoldID " + "\r\n";

            queryString = queryString + "       DELETE FROM     CommodityMolds WHERE CommodityMoldID = @CommodityMoldID" + "\r\n";

            queryString = queryString + "       SELECT          @MINCommodityMoldID = MIN(CommodityMoldID), @COUNTCommodityMoldID = COUNT(CommodityMoldID) FROM CommodityMolds WHERE CommodityID = @CommodityID " + "\r\n";
            queryString = queryString + "       IF (@IsDefault = 1 OR @COUNTCommodityMoldID = 1) UPDATE CommodityMolds SET IsDefault = 1 WHERE CommodityMoldID = @MINCommodityMoldID" + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("RemoveCommodityMold", queryString);
        }

        private void UpdateCommodityMold()
        {
            string queryString = " @CommodityMoldID int, @CommodityID int, @Quantity decimal(18, 2), @Remarks nvarchar(50), @IsDefault bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       BEGIN " + "\r\n";

            queryString = queryString + "           UPDATE          CommodityMolds " + "\r\n";
            queryString = queryString + "           SET             Quantity = ROUND(IIF(@Quantity > 0, @Quantity, Quantity), " + (int)GlobalEnums.rndN0 + "), Remarks = @Remarks " + "\r\n";
            queryString = queryString + "           WHERE           CommodityMoldID = @CommodityMoldID " + "\r\n";

            queryString = queryString + "           IF (@IsDefault = 1) " + "\r\n"; //ONLY CHANGE WHEN @IsDefault = true: THIS WILL KEEP AT LEAST 1 ROW IS DEFAULT
            queryString = queryString + "               UPDATE      CommodityMolds " + "\r\n";
            queryString = queryString + "               SET         IsDefault = IIF(CommodityMoldID = @CommodityMoldID, 1, 0) " + "\r\n"; //IIF(CommodityMoldID = @CommodityMoldID, @IsDefault, 0)
            queryString = queryString + "               WHERE       CommodityID = @CommodityID " + "\r\n";

            queryString = queryString + "       END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("UpdateCommodityMold", queryString);
        }

        private void GetMoldBases()
        {
            string queryString;

            string querySELECT = "              SELECT      Molds.MoldID, Molds.Code, Molds.Name, Molds.Reference " + " \r\n";
            string queryFROM = "                FROM        Molds " + "\r\n";
            string queryWHERE = "               WHERE       Molds.InActive = 0 AND (@SearchText = '' OR Molds.Code LIKE '%' + @SearchText + '%' OR Molds.OfficialCode LIKE '%' + @SearchText + '%' OR Molds.Name LIKE '%' + @SearchText + '%' OR Molds.Reference LIKE '%' + @SearchText + '%')  " + "\r\n";

            queryString = " @SearchText nvarchar(60), @CommodityID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF (@CommodityID > 0) " + "\r\n"; //GET ALL MOLDS BY @CommodityID
            queryString = queryString + "           " + querySELECT + ", CommodityMolds.Quantity " + "\r\n";
            queryString = queryString + "           " + queryFROM + " INNER JOIN CommodityMolds ON CommodityMolds.CommodityID = @CommodityID AND Molds.MoldID = CommodityMolds.MoldID " + "\r\n";
            queryString = queryString + "           " + queryWHERE + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n"; //GET ALL MOLDS
            queryString = queryString + "           " + querySELECT + ", Molds.Quantity " + "\r\n";
            queryString = queryString + "           " + queryFROM + "\r\n";
            queryString = queryString + "           " + queryWHERE + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetMoldBases", queryString);
        }

    }
}

