using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Commons
{
    public class ProductionLine
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public ProductionLine(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            //this.GetProductionLineIndexes();

            //this.ProductionLineEditable();

            this.GetProductionLineBases();
        }


        private void GetProductionLineIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      ProductionLineID, Code, Name, Title, Birthday, Telephone, Address, Remarks " + "\r\n";
            queryString = queryString + "       FROM        ProductionLines " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetProductionLineIndexes", queryString);
        }

        private void ProductionLineEditable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = ProductionLineID FROM ProductionLines WHERE @EntityID = 1"; //AT TUE VIET ONLY: Don't allow edit default productionLine, because it is related to Customers

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = ProductionLineID FROM ProductionLines WHERE ProductionLineID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            //queryArray[1] = " SELECT TOP 1 @FoundEntity = ProductionLineID FROM GoodsIssueDetails WHERE ProductionLineID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("ProductionLineEditable", queryArray);
        }

        private void GetProductionLineBases()
        {
            string queryString;

            queryString = " @SearchText nvarchar(60) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      TOP 30 ProductionLineID, Code, Name " + " \r\n";
            queryString = queryString + "       FROM        ProductionLines " + "\r\n";
            queryString = queryString + "       WHERE       InActive = 0 AND (@SearchText = '' OR Code LIKE '%' + @SearchText + '%' OR Name LIKE '%' + @SearchText + '%') " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetProductionLineBases", queryString);
        }

    }
}

