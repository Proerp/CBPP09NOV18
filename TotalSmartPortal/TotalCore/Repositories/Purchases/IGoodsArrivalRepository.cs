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
        IEnumerable<GoodsArrivalPendingCustomer> GetCustomers(int? locationID);
        IEnumerable<GoodsArrivalPendingPurchaseOrder> GetPurchaseOrders(int? locationID);

        IEnumerable<GoodsArrivalPendingPurchaseOrderDetail> GetPendingPurchaseOrderDetails(int? locationID, int? goodsArrivalID, int? purchaseOrderID, int? customerID, int? transporterID, string purchaseOrderDetailIDs);
    }

}
