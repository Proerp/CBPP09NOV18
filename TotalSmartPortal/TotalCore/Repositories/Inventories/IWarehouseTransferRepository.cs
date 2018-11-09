using TotalModel.Models;
using System.Collections.Generic;

namespace TotalCore.Repositories.Inventories
{

    public interface IWarehouseTransferRepository : IGenericWithDetailRepository<WarehouseTransfer, WarehouseTransferDetail>
    {
    }

    public interface IWarehouseTransferAPIRepository : IGenericAPIRepository
    {
        IEnumerable<WarehouseTransferPendingWarehouse> GetWarehouses(int? locationID, int? nmvnTaskID);
        IEnumerable<WarehouseTransferPendingTransferOrder> GetTransferOrders(int? locationID, int? nmvnTaskID);

        IEnumerable<WarehouseTransferPendingTransferOrderDetail> GetTransferOrderDetails(int? locationID, int? nmvnTaskID, int? warehouseTransferID, int? transferOrderID, int? warehouseID, int? warehouseReceiptID, string goodsReceiptDetailIDs, bool isReadonly);
    }    
}
