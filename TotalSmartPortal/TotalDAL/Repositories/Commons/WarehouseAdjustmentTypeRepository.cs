using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class WarehouseAdjustmentTypeRepository : IWarehouseAdjustmentTypeRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public WarehouseAdjustmentTypeRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public IList<WarehouseAdjustmentType> GetAllWarehouseAdjustmentTypes()
        {
            return this.totalSmartPortalEntities.WarehouseAdjustmentTypes.ToList();
        }

    }
}
