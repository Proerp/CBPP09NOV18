using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Productions;

namespace TotalDAL.Repositories.Productions
{
    public class ProductionOrderRepository : GenericWithDetailRepository<ProductionOrder, ProductionOrderDetail>, IProductionOrderRepository
    {
        public ProductionOrderRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "ProductionOrderEditable", "ProductionOrderApproved", null, "ProductionOrderVoidable")
        {
        }
    }








    public class ProductionOrderAPIRepository : GenericAPIRepository, IProductionOrderAPIRepository
    {
        public ProductionOrderAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetProductionOrderIndexes")
        {
        }

        protected override ObjectParameter[] GetEntityIndexParameters(string aspUserID, DateTime fromDate, DateTime toDate)
        {
            ObjectParameter[] baseParameters = base.GetEntityIndexParameters(aspUserID, fromDate, toDate);
            ObjectParameter[] objectParameters = new ObjectParameter[] { new ObjectParameter("NMVNTaskID", this.RepositoryBag.ContainsKey("NMVNTaskID") && this.RepositoryBag["NMVNTaskID"] != null ? this.RepositoryBag["NMVNTaskID"] : 0), baseParameters[0], baseParameters[1], baseParameters[2] };

            this.RepositoryBag.Remove("NMVNTaskID");

            return objectParameters;
        }

        public IEnumerable<ProductionOrderPendingCustomer> GetCustomers(int? locationID, int? nmvnTaskID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<ProductionOrderPendingCustomer> pendingPlannedOrderCustomers = base.TotalSmartPortalEntities.GetProductionOrderPendingCustomers(locationID, nmvnTaskID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPlannedOrderCustomers;
        }

        public IEnumerable<ProductionOrderPendingPlannedOrder> GetPlannedOrders(int? locationID, int? nmvnTaskID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<ProductionOrderPendingPlannedOrder> pendingPlannedOrders = base.TotalSmartPortalEntities.GetProductionOrderPendingPlannedOrders(locationID, nmvnTaskID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPlannedOrders;
        }

        public IEnumerable<ProductionOrderPendingFirmOrder> GetPendingFirmOrders(int? locationID, int? nmvnTaskID, int? productionOrderID, int? plannedOrderID, int? customerID, string firmOrderIDs, bool isReadonly)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<ProductionOrderPendingFirmOrder> pendingPlannedOrderDetails = base.TotalSmartPortalEntities.GetProductionOrderPendingFirmOrders(locationID, nmvnTaskID, productionOrderID, plannedOrderID, customerID, firmOrderIDs, isReadonly).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPlannedOrderDetails;
        }
    }


}