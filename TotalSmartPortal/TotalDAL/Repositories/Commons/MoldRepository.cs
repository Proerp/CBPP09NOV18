using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

using TotalModel.Models;
using TotalCore.Repositories.Commons;
using System.Data.Entity.Core.Objects;
using System;

namespace TotalDAL.Repositories.Commons
{
    public class MoldRepository : GenericRepository<Mold>, IMoldRepository
    {
        public MoldRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "MoldEditable", null, "MoldDeletable")
        {
        }
    }



    public class MoldAPIRepository : GenericAPIRepository, IMoldAPIRepository
    {
        public MoldAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetMoldIndexes")
        {
        }

        public IList<MoldBase> GetMoldBases(string searchText, int commodityID)
        {
            List<MoldBase> moldBases = this.TotalSmartPortalEntities.GetMoldBases(searchText, commodityID).ToList();

            return moldBases;
        }

        public IList<CommodityMold> GetCommodityMolds(int commodityID)
        {
            return this.TotalSmartPortalEntities.GetCommodityMolds(commodityID).ToList();
        }

        public void AddCommodityMold(int? moldID, int? commodityID, decimal quantity)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("MoldID", moldID), new ObjectParameter("CommodityID", commodityID), new ObjectParameter("Quantity", quantity) };
            this.ExecuteFunction("AddCommodityMold", parameters);
        }

        public void RemoveCommodityMold(int? commodityMoldID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("CommodityMoldID", commodityMoldID) };
            this.ExecuteFunction("RemoveCommodityMold", parameters);
        }

        public void UpdateCommodityMold(int? commodityMoldID, int commodityID, decimal quantity, string remarks, bool? isDefault)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("CommodityMoldID", commodityMoldID), new ObjectParameter("CommodityID", commodityID), new ObjectParameter("Quantity", quantity), new ObjectParameter("Remarks", remarks != null ? remarks : ""), new ObjectParameter("IsDefault", isDefault) };
            this.ExecuteFunction("UpdateCommodityMold", parameters);
        }
    }

}