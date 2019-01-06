using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Purchases
{
    public class Lab
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public Lab(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetLabIndexes();
            this.LabInitReference();
        }

        private void GetLabIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      Labs.LabID, Labs.EntryDate, Labs.Reference, Labs.Code, Labs.GoodsArrivalID, GoodsArrivals.Reference AS GoodsArrivalReference, Labs.CommodityCodes, Labs.CommodityNames, Labs.SealCodes, Labs.BatchCodes, Labs.Description, Labs.TotalQuantity, Labs.Approved " + "\r\n";
            queryString = queryString + "       FROM        Labs INNER JOIN GoodsArrivals ON Labs.GoodsArrivalID = GoodsArrivals.GoodsArrivalID " + "\r\n";
            queryString = queryString + "       WHERE       Labs.EntryDate >= @FromDate AND Labs.EntryDate <= @ToDate " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetLabIndexes", queryString);
        }

        private void LabInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("Labs", "LabID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.Lab));
            this.totalSmartPortalEntities.CreateTrigger("LabInitReference", simpleInitReference.CreateQuery());
        }
    }
}
