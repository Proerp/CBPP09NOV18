using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;


namespace TotalDAL.Repositories.Commons
{
    public class CustomerTypeRepository : ICustomerTypeRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public CustomerTypeRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public IList<CustomerType> GetAllCustomerTypes()
        {
            return this.totalSmartPortalEntities.CustomerTypes.ToList();
        }
    }
}

