using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;


namespace TotalDAL.Repositories.Commons
{
    public class MonetaryAccountRepository : IMonetaryAccountRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public MonetaryAccountRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public IList<MonetaryAccount> GetAllMonetaryAccounts()
        {
            return this.totalSmartPortalEntities.MonetaryAccounts.ToList();
        }
    }
}

