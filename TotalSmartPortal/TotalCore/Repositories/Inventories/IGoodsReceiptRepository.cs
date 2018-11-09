using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Inventories
{
    public interface IGoodsReceiptRepository : IGenericWithDetailRepository<GoodsReceipt, GoodsReceiptDetail>
    {
    }

    public interface IGoodsReceiptAPIRepository : IGenericAPIRepository
    {
        IEnumerable<GoodsReceiptPendingCustomer> GetCustomers(int? locationID);
        IEnumerable<GoodsReceiptPendingPurchaseRequisition> GetPurchaseRequisitions(int? locationID);
        IEnumerable<GoodsReceiptPendingPurchaseRequisitionDetail> GetPendingPurchaseRequisitionDetails(int? locationID, int? goodsReceiptID, int? purchaseRequisitionID, int? customerID, string purchaseRequisitionDetailIDs, bool isReadonly);


        IEnumerable<GoodsReceiptPendingWarehouse> GetWarehouses(int? locationID, int? nmvnTaskID);
        IEnumerable<GoodsReceiptPendingWarehouseTransfer> GetWarehouseTransfers(int? locationID, int? nmvnTaskID);
        IEnumerable<GoodsReceiptPendingWarehouseTransferDetail> GetPendingWarehouseTransferDetails(int? nmvnTaskID, int? goodsReceiptID, int? warehouseTransferID, int? warehouseID, int? warehouseIssueID, string warehouseTransferDetailIDs, bool isReadonly);



        IEnumerable<GoodsReceiptPendingPlannedOrderCustomer> GetPlannedOrderCustomers(int? locationID);
        IEnumerable<GoodsReceiptPendingPlannedOrder> GetPlannedOrders(int? locationID);
        IEnumerable<GoodsReceiptPendingPlannedOrderDetail> GetPendingPlannedOrderDetails(int? locationID, int? goodsReceiptID, int? plannedOrderID, int? customerID, string finishedProductPackageIDs, bool isReadonly);

        IEnumerable<GoodsReceiptPendingMaterialIssueDetail> GetPendingMaterialIssueDetails(int? locationID, int? goodsReceiptID, string materialIssueDetailIDs, bool isReadonly);



        List<PendingWarehouseAdjustmentDetail> GetPendingWarehouseAdjustmentDetails(int? locationID, int? goodsReceiptID, int? warehouseAdjustmentID, int? warehouseID, string warehouseAdjustmentDetailIDs, bool isReadonly);
        int? GetGoodsReceiptIDofWarehouseAdjustment(int? warehouseAdjustmentID);



        IEnumerable<GoodsReceiptDetailAvailable> GetGoodsReceiptDetailAvailables(int? locationID, int? warehouseID, int? commodityID, string commodityIDs, int? batchID, string goodsReceiptDetailIDs, bool onlyApproved, bool onlyIssuable);
    }

}
