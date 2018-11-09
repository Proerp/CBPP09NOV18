using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Commons
{
    public class Workshift
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public Workshift(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            //this.GetWorkshiftIndexes();

            //this.WorkshiftEditable();

            this.GetWorkshiftBases();
        }


        private void GetWorkshiftIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      WorkshiftID, Code, Name, Title, Birthday, Telephone, Address, Remarks " + "\r\n";
            queryString = queryString + "       FROM        Workshifts " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetWorkshiftIndexes", queryString);
        }

        private void WorkshiftEditable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = WorkshiftID FROM Workshifts WHERE @EntityID = 1"; //AT TUE VIET ONLY: Don't allow edit default workshift, because it is related to Customers

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = WorkshiftID FROM Workshifts WHERE WorkshiftID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            //queryArray[1] = " SELECT TOP 1 @FoundEntity = WorkshiftID FROM GoodsIssueDetails WHERE WorkshiftID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("WorkshiftEditable", queryArray);
        }

        private void GetWorkshiftBases()
        {
            string queryString;

            queryString = " @SearchText nvarchar(60) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      TOP 30 WorkshiftID, Code AS WorkshiftCode, Name AS WorkshiftName " + " \r\n";
            queryString = queryString + "       FROM        Workshifts " + "\r\n";
            queryString = queryString + "       WHERE       @SearchText = '' OR Code LIKE '%' + @SearchText + '%' OR Name LIKE '%' + @SearchText + '%' " + "\r\n";
            queryString = queryString + "       ORDER BY    EntryDate DESC " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetWorkshiftBases", queryString);
        }

    }
}

