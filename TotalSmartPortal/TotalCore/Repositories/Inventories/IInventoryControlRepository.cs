using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Inventories
{
    public interface IInventoryControlAPIRepository : IGenericAPIRepository
    {
        List<InventoryControl> GetInventoryControls(string aspUserID, int? locationID, int? summaryOptionID, int? labOptionID, int? filterOptionID, int? pendingOptionID, int? shelfLife);
    }

}