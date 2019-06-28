using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Inventories;

namespace TotalDAL.Repositories.Inventories
{
    public class GoodsReceiptRepository : GenericWithDetailRepository<GoodsReceipt, GoodsReceiptDetail>, IGoodsReceiptRepository
    {
        public GoodsReceiptRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GoodsReceiptEditable", "GoodsReceiptApproved")
        {
        }
    }








    public class GoodsReceiptAPIRepository : GenericAPIRepository, IGoodsReceiptAPIRepository
    {
        public GoodsReceiptAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetGoodsReceiptIndexes")
        {
        }

        protected override ObjectParameter[] GetEntityIndexParameters(string aspUserID, DateTime fromDate, DateTime toDate)
        {

            ObjectParameter[] baseParameters = base.GetEntityIndexParameters(aspUserID, fromDate, toDate);
            ObjectParameter[] objectParameters = new ObjectParameter[] { new ObjectParameter("NMVNTaskID", this.RepositoryBag.ContainsKey("NMVNTaskID") && this.RepositoryBag["NMVNTaskID"] != null ? this.RepositoryBag["NMVNTaskID"] : 0), baseParameters[0], baseParameters[1], baseParameters[2] };

            this.RepositoryBag.Remove("NMVNTaskID");

            return objectParameters;
        }

        public string GetReference(int goodsReceiptID)
        {
            return base.TotalSmartPortalEntities.GoodsReceiptGetReference(goodsReceiptID).FirstOrDefault();
        }

        public IEnumerable<GoodsReceiptPendingCustomer> GetCustomers(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptPendingCustomer> pendingPurchaseRequisitionCustomers = base.TotalSmartPortalEntities.GetGoodsReceiptPendingCustomers(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPurchaseRequisitionCustomers;
        }

        public IEnumerable<GoodsReceiptPendingPurchaseRequisition> GetPurchaseRequisitions(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptPendingPurchaseRequisition> pendingPurchaseRequisitions = base.TotalSmartPortalEntities.GetGoodsReceiptPendingPurchaseRequisitions(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPurchaseRequisitions;
        }

        public IEnumerable<GoodsReceiptPendingPurchaseRequisitionDetail> GetPendingPurchaseRequisitionDetails(int? locationID, int? goodsReceiptID, int? purchaseRequisitionID, int? customerID, string purchaseRequisitionDetailIDs, bool isReadonly)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptPendingPurchaseRequisitionDetail> pendingPurchaseRequisitionDetails = base.TotalSmartPortalEntities.GetGoodsReceiptPendingPurchaseRequisitionDetails(locationID, goodsReceiptID, purchaseRequisitionID, customerID, purchaseRequisitionDetailIDs, isReadonly).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPurchaseRequisitionDetails;
        }



        public IEnumerable<GoodsReceiptPendingPurchasing> GetPurchasings(int? locationID, int? nmvnTaskID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptPendingPurchasing> pendingPurchasings = base.TotalSmartPortalEntities.GetGoodsReceiptPendingPurchasings(locationID, nmvnTaskID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPurchasings;
        }

        public IEnumerable<GoodsReceiptPendingGoodsArrival> GetGoodsArrivals(int? locationID, int? nmvnTaskID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptPendingGoodsArrival> pendingGoodsArrivals = base.TotalSmartPortalEntities.GetGoodsReceiptPendingGoodsArrivals(locationID, nmvnTaskID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsArrivals;
        }

        public IEnumerable<GoodsReceiptPendingGoodsArrivalPackage> GetPendingGoodsArrivalPackages(bool webAPI, int? locationID, int? nmvnTaskID, int? goodsReceiptID, int? goodsArrivalID, string barcode, string goodsArrivalPackageIDs)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptPendingGoodsArrivalPackage> pendingGoodsArrivalPackages = base.TotalSmartPortalEntities.GetGoodsReceiptPendingGoodsArrivalPackages(webAPI, locationID, nmvnTaskID, goodsReceiptID, goodsArrivalID, barcode, goodsArrivalPackageIDs).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsArrivalPackages;
        }



        public IEnumerable<GoodsReceiptPendingWarehouse> GetWarehouses(int? locationID, int? nmvnTaskID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptPendingWarehouse> pendingWarehouseTransferWarehouses = base.TotalSmartPortalEntities.GetGoodsReceiptPendingWarehouses(locationID, nmvnTaskID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingWarehouseTransferWarehouses;
        }

        public IEnumerable<GoodsReceiptPendingWarehouseTransfer> GetWarehouseTransfers(int? locationID, int? nmvnTaskID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptPendingWarehouseTransfer> pendingWarehouseTransfers = base.TotalSmartPortalEntities.GetGoodsReceiptPendingWarehouseTransfers(locationID, nmvnTaskID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingWarehouseTransfers;
        }

        public List<GoodsReceiptPendingWarehouseTransferDetail> GetPendingWarehouseTransferDetails(int? nmvnTaskID, int? goodsReceiptID, int? warehouseTransferID, int? warehouseID, int? warehouseIssueID, string warehouseTransferDetailIDs, bool oneStep)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            List<GoodsReceiptPendingWarehouseTransferDetail> pendingWarehouseTransferDetails = base.TotalSmartPortalEntities.GetGoodsReceiptPendingWarehouseTransferDetails(nmvnTaskID, goodsReceiptID, warehouseTransferID, warehouseID, warehouseIssueID, warehouseTransferDetailIDs, oneStep).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingWarehouseTransferDetails;
        }





        public IEnumerable<GoodsReceiptPendingPlannedOrderCustomer> GetPlannedOrderCustomers(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptPendingPlannedOrderCustomer> pendingPlannedOrderCustomers = base.TotalSmartPortalEntities.GetGoodsReceiptPendingPlannedOrderCustomers(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPlannedOrderCustomers;
        }

        public IEnumerable<GoodsReceiptPendingPlannedOrder> GetPlannedOrders(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptPendingPlannedOrder> pendingPlannedOrders = base.TotalSmartPortalEntities.GetGoodsReceiptPendingPlannedOrders(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPlannedOrders;
        }

        public IEnumerable<GoodsReceiptPendingPlannedOrderDetail> GetPendingPlannedOrderDetails(int? locationID, int? goodsReceiptID, int? plannedOrderID, int? customerID, string finishedProductPackageIDs, bool isReadonly)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptPendingPlannedOrderDetail> pendingPlannedOrderDetails = base.TotalSmartPortalEntities.GetGoodsReceiptPendingPlannedOrderDetails(locationID, goodsReceiptID, plannedOrderID, customerID, finishedProductPackageIDs, isReadonly).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPlannedOrderDetails;
        }



        public IEnumerable<GoodsReceiptPendingPlannedItemCustomer> GetPlannedItemCustomers(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptPendingPlannedItemCustomer> pendingPlannedItemCustomers = base.TotalSmartPortalEntities.GetGoodsReceiptPendingPlannedItemCustomers(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPlannedItemCustomers;
        }

        public IEnumerable<GoodsReceiptPendingPlannedItem> GetPlannedItems(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptPendingPlannedItem> pendingPlannedItems = base.TotalSmartPortalEntities.GetGoodsReceiptPendingPlannedItems(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPlannedItems;
        }

        public IEnumerable<GoodsReceiptPendingPlannedItemDetail> GetPendingPlannedItemDetails(int? locationID, int? goodsReceiptID, int? plannedOrderID, int? customerID, string finishedItemPackageIDs)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptPendingPlannedItemDetail> pendingPlannedItemDetails = base.TotalSmartPortalEntities.GetGoodsReceiptPendingPlannedItemDetails(locationID, goodsReceiptID, plannedOrderID, customerID, finishedItemPackageIDs).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPlannedItemDetails;
        }





        public IEnumerable<GoodsReceiptPendingRecyclate> GetRecyclates(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptPendingRecyclate> pendingRecyclates = base.TotalSmartPortalEntities.GetGoodsReceiptPendingRecyclates(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingRecyclates;
        }

        public IEnumerable<GoodsReceiptPendingRecyclateDetail> GetPendingRecyclateDetails(int? locationID, int? goodsReceiptID, int? recyclateID, string recyclatePackageIDs)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptPendingRecyclateDetail> pendingRecyclateDetails = base.TotalSmartPortalEntities.GetGoodsReceiptPendingRecyclateDetails(locationID, goodsReceiptID, recyclateID, recyclatePackageIDs).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingRecyclateDetails;
        }




        public IEnumerable<GoodsReceiptPendingMaterialIssueDetail> GetPendingMaterialIssueDetails(int? locationID, int? goodsReceiptID, string materialIssueDetailIDs, bool isReadonly)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptPendingMaterialIssueDetail> pendingMaterialIssueDetails = base.TotalSmartPortalEntities.GetGoodsReceiptPendingMaterialIssueDetails(locationID, goodsReceiptID, materialIssueDetailIDs, isReadonly).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingMaterialIssueDetails;
        }



        public List<PendingWarehouseAdjustmentDetail> GetPendingWarehouseAdjustmentDetails(int? locationID, int? goodsReceiptID, int? warehouseAdjustmentID, int? warehouseID, string warehouseAdjustmentDetailIDs, bool isReadonly)
        {
            return base.TotalSmartPortalEntities.GetPendingWarehouseAdjustmentDetails(locationID, goodsReceiptID, warehouseAdjustmentID, warehouseID, warehouseAdjustmentDetailIDs, isReadonly).ToList();
        }




        public int? GetGoodsReceiptID(int? goodsArrivalID, int? plannedOrderID, int? warehouseTransferID, int? warehouseAdjustmentID)
        {
            return base.TotalSmartPortalEntities.GetGoodsReceiptID(goodsArrivalID, plannedOrderID, warehouseTransferID, warehouseAdjustmentID).FirstOrDefault();
        }

        public IEnumerable<GoodsReceiptDetailAvailable> GetGoodsReceiptDetailAvailables(int? locationID, int? warehouseID, int? warehouseReceiptID, int? commodityID, string commodityIDs, int? batchID, int? blendingInstructionID, string barcode, string goodsReceiptDetailIDs, bool onlyApproved, bool onlyIssuable)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptDetailAvailable> goodsReceiptDetailAvailables = base.TotalSmartPortalEntities.GetGoodsReceiptDetailAvailables(locationID, warehouseID, warehouseReceiptID, commodityID, commodityIDs, batchID, blendingInstructionID, barcode, goodsReceiptDetailIDs, onlyApproved, onlyIssuable).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return goodsReceiptDetailAvailables;
        }

        public IEnumerable<TransferOrderPendingBlendingInstructionCompact> GetTransferOrderPendingBlendingInstructionCompacts(int? warehouseReceiptID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<TransferOrderPendingBlendingInstructionCompact> transferOrderPendingBlendingInstructionCompacts = base.TotalSmartPortalEntities.GetTransferOrderPendingBlendingInstructionCompacts(warehouseReceiptID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return transferOrderPendingBlendingInstructionCompacts;
        }
    }


}
