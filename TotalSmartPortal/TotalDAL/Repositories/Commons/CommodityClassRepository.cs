using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{

    public class CommodityClassRepository : ICommodityClassRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public CommodityClassRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public IList<CommodityClass> GetAllCommodityClasses()
        {
            return this.totalSmartPortalEntities.CommodityClasses.OrderBy(o => o.Code).ToList();
        }

    }
}
