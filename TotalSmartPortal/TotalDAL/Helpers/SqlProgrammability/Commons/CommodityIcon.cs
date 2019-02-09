using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Commons
{
    public class CommodityIcon
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public CommodityIcon(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetCommodityIconBases();
        }


        private void GetCommodityIconBases()
        {
            string queryString;

            queryString = " " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";
            queryString = queryString + "       SELECT      CommodityIconID, Code, Name " + "\r\n";
            queryString = queryString + "       FROM        CommodityIcons " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetCommodityIconBases", queryString);
        }
    }
}