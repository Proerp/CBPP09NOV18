using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class AspNetUserRepository : IAspNetUserRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public AspNetUserRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public IList<AspNetUser> GetAllAspNetUsers()
        {
            return this.totalSmartPortalEntities.AspNetUsers.ToList();
        }
    }
}
