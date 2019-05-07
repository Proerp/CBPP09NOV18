using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Purchases
{
    public interface IGoodsArrivalRepository : IGenericWithDetailRepository<GoodsArrival, GoodsArrivalDetail>
    {
        List<BarcodeBase> GetBarcodeBases(int? goodsArrivalID);        
        void SetBarcodeSymbologies(int? barcodeID, string symbologies);
    }

    public interface IGoodsArrivalAPIRepository : IGenericAPIRepository
    {
        string GetBarcodeSymbologies(int barcodeID);

        IEnumerable<GoodsArrivalPendingCustomer> GetCustomers(int? locationID, int? nmvnTaskID);
        IEnumerable<GoodsArrivalPendingPurchaseOrder> GetPurchaseOrders(int? locationID, int? nmvnTaskID);

        IEnumerable<GoodsArrivalPendingPurchaseOrderDetail> GetPendingPurchaseOrderDetails(int? locationID, int? nmvnTaskID, int? goodsArrivalID, int? purchaseOrderID, int? customerID, int? transporterID, string purchaseOrderDetailIDs);
    }

}
