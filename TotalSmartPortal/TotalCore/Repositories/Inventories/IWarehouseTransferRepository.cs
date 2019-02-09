using TotalModel.Models;
using System.Collections.Generic;

namespace TotalCore.Repositories.Inventories
{

    public interface IWarehouseTransferRepository : IGenericWithDetailRepository<WarehouseTransfer, WarehouseTransferDetail>
    {
    }

    public interface IWarehouseTransferAPIRepository : IGenericAPIRepository
    {
        string GetReference(int warehouseTransferID);

        IEnumerable<WarehouseTransferAvailableWarehouse> GetAvailableWarehouses(int? locationID, int? nmvnTaskID);

        IEnumerable<WarehouseTransferPendingWarehouse> GetPendingWarehouses(int? locationID, int? nmvnTaskID);
        IEnumerable<WarehouseTransferPendingTransferOrder> GetTransferOrders(int? locationID, int? nmvnTaskID);

        IEnumerable<WarehouseTransferPendingTransferOrderDetail> GetTransferOrderDetails(bool webAPI, int? locationID, int? nmvnTaskID, int? warehouseTransferID, int? transferOrderID, int? warehouseID, int? warehouseReceiptID, string goodsReceiptDetailIDs);
    }    
}
