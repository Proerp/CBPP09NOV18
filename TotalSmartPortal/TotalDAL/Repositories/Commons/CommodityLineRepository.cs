using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{

    public class CommodityLineRepository : ICommodityLineRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public CommodityLineRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public IList<CommodityLine> GetAllCommodityLines()
        {
            return this.totalSmartPortalEntities.CommodityLines.ToList();
        }

    }
}
