using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;


namespace TotalDAL.Repositories.Commons
{
    public class PriceCategoryRepository : IPriceCategoryRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public PriceCategoryRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public IList<PriceCategory> GetAllPriceCategories()
        {
            return this.totalSmartPortalEntities.PriceCategories.ToList();
        }
    }
}

