using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Inventories
{
    public interface IMaterialIssueRepository : IGenericWithDetailRepository<MaterialIssue, MaterialIssueDetail>
    {
    }

    public interface IMaterialIssueAPIRepository : IGenericAPIRepository
    {
        IEnumerable<MaterialIssuePendingFirmOrder> GetFirmOrders(int? locationID, int? firmOrderID);

        IEnumerable<MaterialIssuePendingFirmOrderMaterial> GetPendingFirmOrderMaterials(int? locationID, int? materialIssueID, int? firmOrderID, int? warehouseID, string goodsReceiptDetailIDs, bool isReadonly);
    }

}