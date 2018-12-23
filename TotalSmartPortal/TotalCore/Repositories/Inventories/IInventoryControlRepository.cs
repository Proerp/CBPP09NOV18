using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Inventories
{
    public interface IInventoryControlAPIRepository : IGenericAPIRepository
    {
        List<InventoryControl> GetInventoryControls(string aspUserID, bool? summaryOnly, int? labOptionID, int? filterOptionID, int? expiryDay);
    }

}