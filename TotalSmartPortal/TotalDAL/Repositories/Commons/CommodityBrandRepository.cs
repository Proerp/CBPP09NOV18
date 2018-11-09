using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class CommodityBrandRepository : ICommodityBrandRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public CommodityBrandRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public IList<CommodityBrand> GetAllCommodityBrands()
        {
            return this.totalSmartPortalEntities.CommodityBrands.ToList();
        }

    }
}
