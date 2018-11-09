using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class CommodityCategoryRepository : ICommodityCategoryRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public CommodityCategoryRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public IList<CommodityCategory> GetAllCommodityCategories(string commodityTypeIDs)
        {
            List<int> listCommodityTypeID = commodityTypeIDs.Split(',').Select(n => int.Parse(n)).ToList();
            return this.totalSmartPortalEntities.CommodityCategories.Where(w => w.AncestorID != null && listCommodityTypeID.Contains(w.CommodityTypeID)).ToList();
        }

    }
}
