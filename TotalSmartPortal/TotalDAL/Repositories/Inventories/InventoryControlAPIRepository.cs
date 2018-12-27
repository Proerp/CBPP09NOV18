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

        public List<InventoryControl> GetInventoryControls(string aspUserID, bool? summaryOnly, int? labOptionID, int? filterOptionID, int? pendingOptionID, int? expiryDay)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            List<InventoryControl> inventoryControls = base.TotalSmartPortalEntities.GetInventoryControls(aspUserID, summaryOnly, labOptionID, filterOptionID, pendingOptionID, expiryDay).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return inventoryControls;
        }
    }


}
