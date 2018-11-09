using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;
namespace TotalDAL.Repositories.Commons
{
    public class CommodityTypeRepository : ICommodityTypeRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public CommodityTypeRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public IList<CommodityType> GetAllCommodityTypes()
        {
            return this.totalSmartPortalEntities.CommodityTypes.ToList();
        }

    }
}
