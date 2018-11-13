using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Commons
{
    public class BinLocation
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public BinLocation(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            //this.GetBinLocationIndexes();

            //this.BinLocationEditable();

            this.GetBinLocationBases();
        }


        private void GetBinLocationIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      BinLocationID, Code, Name, Title, Birthday, Telephone, Address, Remarks " + "\r\n";
            queryString = queryString + "       FROM        BinLocations " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetBinLocationIndexes", queryString);
        }

        private void BinLocationEditable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = BinLocationID FROM BinLocations WHERE @EntityID = 1"; //AT TUE VIET ONLY: Don't allow edit default BinLocation, because it is related to Customers

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = BinLocationID FROM BinLocations WHERE BinLocationID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            //queryArray[1] = " SELECT TOP 1 @FoundEntity = BinLocationID FROM GoodsIssueDetails WHERE BinLocationID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("BinLocationEditable", queryArray);
        }

        private void GetBinLocationBases()
        {
            string queryString;

            queryString = " @WarehouseID int, @SearchText nvarchar(60) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      TOP 30 BinLocationID, Code, Name " + " \r\n";
            queryString = queryString + "       FROM        BinLocations " + "\r\n";
            queryString = queryString + "       WHERE       InActive = 0 AND WarehouseID = @WarehouseID AND (@SearchText = '' OR Code LIKE '%' + @SearchText + '%' OR Name LIKE '%' + @SearchText + '%') " + "\r\n";
            queryString = queryString + "       ORDER BY    Code " + " \r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetBinLocationBases", queryString);
        }

    }
}

