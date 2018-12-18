﻿using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Inventories
{
    public interface ITransferOrderRepository : IGenericWithDetailRepository<TransferOrder, TransferOrderDetail>
    {
    }

    public interface ITransferOrderAPIRepository : IGenericAPIRepository
    {
        IEnumerable<TransferOrderAvailableWarehouse> GetAvailableWarehouses(int? locationID, int? nmvnTaskID);
        IEnumerable<TransferOrderPendingBlendingInstruction> GetTransferOrderPendingBlendingInstructions(int? locationID, int? transferOrderID, string commodityIDs);
    }    
}
