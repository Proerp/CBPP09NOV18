using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class CommodityPriceRepository : GenericRepository<CommodityPrice>, ICommodityPriceRepository
    {
        public CommodityPriceRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities)
        {
        }
    }


    public class CommodityPriceAPIRepository : GenericAPIRepository, ICommodityPriceAPIRepository
    {
        public CommodityPriceAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetCommodityPriceIndexes")
        {
        }
    }
}