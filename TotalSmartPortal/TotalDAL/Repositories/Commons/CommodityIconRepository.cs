using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class CommodityIconRepository : ICommodityIconRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public CommodityIconRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public IList<CommodityIconBase> GetCommodityIconBases()
        {
            return this.totalSmartPortalEntities.GetCommodityIconBases().ToList();
        }
    }
}
