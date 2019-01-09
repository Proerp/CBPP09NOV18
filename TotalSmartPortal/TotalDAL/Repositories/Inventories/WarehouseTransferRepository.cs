using System;
using System.Linq;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Inventories;

namespace TotalDAL.Repositories.Inventories
{  
    public class WarehouseTransferRepository : GenericWithDetailRepository<WarehouseTransfer, WarehouseTransferDetail>, IWarehouseTransferRepository
    {
        public WarehouseTransferRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "WarehouseTransferEditable", "WarehouseTransferApproved", null, "WarehouseTransferVoidable")
        {
        }
    }








    public class WarehouseTransferAPIRepository : GenericAPIRepository, IWarehouseTransferAPIRepository
    {
        public WarehouseTransferAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetWarehouseTransferIndexes")
        {
        }

        protected override ObjectParameter[] GetEntityIndexParameters(string aspUserID, DateTime fromDate, DateTime toDate)
        {

            ObjectParameter[] baseParameters = base.GetEntityIndexParameters(aspUserID, fromDate, toDate);
            ObjectParameter[] objectParameters = new ObjectParameter[] { new ObjectParameter("NMVNTaskID", this.RepositoryBag.ContainsKey("NMVNTaskID") && this.RepositoryBag["NMVNTaskID"] != null ? this.RepositoryBag["NMVNTaskID"] : 0), baseParameters[0], baseParameters[1], baseParameters[2] };

            this.RepositoryBag.Remove("NMVNTaskID");

            return objectParameters;
        }


        public string GetReference(int warehouseTransferID)
        {
            return base.TotalSmartPortalEntities.WarehouseTransferGetReference(warehouseTransferID).FirstOrDefault();
        }

        public IEnumerable<WarehouseTransferAvailableWarehouse> GetAvailableWarehouses(int? locationID, int? nmvnTaskID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<WarehouseTransferAvailableWarehouse> availableWarehouses = base.TotalSmartPortalEntities.GetWarehouseTransferAvailableWarehouses(locationID, nmvnTaskID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return availableWarehouses;
        }

        public IEnumerable<WarehouseTransferPendingWarehouse> GetPendingWarehouses(int? locationID, int? nmvnTaskID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<WarehouseTransferPendingWarehouse> pendingWarehouses = base.TotalSmartPortalEntities.GetWarehouseTransferPendingWarehouses(locationID, nmvnTaskID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingWarehouses;
        }

        public IEnumerable<WarehouseTransferPendingTransferOrder> GetTransferOrders(int? locationID, int? nmvnTaskID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<WarehouseTransferPendingTransferOrder> pendingTransferOrders = base.TotalSmartPortalEntities.GetWarehouseTransferPendingTransferOrders(locationID, nmvnTaskID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingTransferOrders;
        }

        public IEnumerable<WarehouseTransferPendingTransferOrderDetail> GetTransferOrderDetails(int? locationID, int? nmvnTaskID, int? warehouseTransferID, int? transferOrderID, int? warehouseID, int? warehouseReceiptID, string goodsReceiptDetailIDs)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<WarehouseTransferPendingTransferOrderDetail> pendingTransferOrderDetails = base.TotalSmartPortalEntities.GetWarehouseTransferPendingTransferOrderDetails(locationID, nmvnTaskID, warehouseTransferID, transferOrderID, warehouseID, warehouseReceiptID, goodsReceiptDetailIDs).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingTransferOrderDetails;
        }
    }
}
