using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Inventories;

namespace TotalDAL.Repositories.Inventories
{
    public class InventoryControlAPIRepository : GenericAPIRepository, IInventoryControlAPIRepository
    {
        public InventoryControlAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetInventoryControlIndexes")
        {
        }

        public List<InventoryControl> GetInventoryControls(string aspUserID, int? summaryOptionID, int? labOptionID, int? filterOptionID, int? pendingOptionID, int? shelfLife)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            List<InventoryControl> inventoryControls = base.TotalSmartPortalEntities.GetInventoryControls(aspUserID, summaryOptionID, labOptionID, filterOptionID, pendingOptionID, shelfLife).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return inventoryControls;
        }
    }


}
