using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class PackingMaterialRepository : IPackingMaterialRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public PackingMaterialRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public IList<PackingMaterial> GetAllPackingMaterials()
        {
            return this.totalSmartPortalEntities.PackingMaterials.ToList();
        }
    }
}

