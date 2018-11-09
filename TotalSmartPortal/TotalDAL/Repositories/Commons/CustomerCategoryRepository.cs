using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;


namespace TotalDAL.Repositories.Commons
{
    public class CustomerCategoryRepository : ICustomerCategoryRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public CustomerCategoryRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public IList<CustomerCategory> GetAllCustomerCategories()
        {
            return this.totalSmartPortalEntities.CustomerCategories.ToList();
        }
    }
}

