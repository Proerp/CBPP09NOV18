using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Inventories
{
    public interface ITransferOrderRepository : IGenericWithDetailRepository<TransferOrder, TransferOrderDetail>
    {
    }

    public interface ITransferOrderAPIRepository : IGenericAPIRepository
    {
        IEnumerable<TransferOrderAvailableWarehouse> GetAvailableWarehouses(int? locationID, int? nmvnTaskID);
        IEnumerable<TransferOrderPendingWorkOrder> GetTransferOrderPendingWorkOrders(int? locationID, int? transferOrderID, int? warehouseID, int? warehouseReceiptID, string commodityIDs);
        IEnumerable<TransferOrderPendingBlendingInstruction> GetTransferOrderPendingBlendingInstructions(int? locationID, int? transferOrderID, int? warehouseID, int? warehouseReceiptID, string commodityIDs);
    }    
}
