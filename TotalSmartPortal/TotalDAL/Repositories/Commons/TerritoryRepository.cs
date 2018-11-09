using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;


namespace TotalDAL.Repositories.Commons
{
    public class TerritoryRepository : ITerritoryRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public TerritoryRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public IList<Territory> GetAllTerritories()
        {
            return this.totalSmartPortalEntities.Territories.ToList();
        }
    }
}

