using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalCore.Repositories.Inventories;

namespace TotalDAL.Repositories.Inventories
{  
    public class TransferOrderRepository : GenericWithDetailRepository<TransferOrder, TransferOrderDetail>, ITransferOrderRepository
    {
        public TransferOrderRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "TransferOrderEditable", "TransferOrderApproved", null, "TransferOrderVoidable")
        {
        }
    }








    public class TransferOrderAPIRepository : GenericAPIRepository, ITransferOrderAPIRepository
    {
        public TransferOrderAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetTransferOrderIndexes")
        {
        }

        protected override ObjectParameter[] GetEntityIndexParameters(string aspUserID, DateTime fromDate, DateTime toDate)
        {

            ObjectParameter[] baseParameters = base.GetEntityIndexParameters(aspUserID, fromDate, toDate);
            ObjectParameter[] objectParameters = new ObjectParameter[] { new ObjectParameter("NMVNTaskID", this.RepositoryBag.ContainsKey("NMVNTaskID") && this.RepositoryBag["NMVNTaskID"] != null ? this.RepositoryBag["NMVNTaskID"] : 0), baseParameters[0], baseParameters[1], baseParameters[2] };

            this.RepositoryBag.Remove("NMVNTaskID");

            return objectParameters;
        }

        public IEnumerable<TransferOrderAvailableWarehouse> GetAvailableWarehouses(int? locationID, int? nmvnTaskID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<TransferOrderAvailableWarehouse> availableWarehouses = base.TotalSmartPortalEntities.GetTransferOrderAvailableWarehouses(locationID, nmvnTaskID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return availableWarehouses;
        }

        public IEnumerable<TransferOrderPendingWorkOrder> GetTransferOrderPendingWorkOrders(int? locationID, int? transferOrderID, string commodityIDs)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<TransferOrderPendingWorkOrder> transferOrderPendingWorkOrders = base.TotalSmartPortalEntities.GetTransferOrderPendingWorkOrders(locationID, transferOrderID, commodityIDs).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return transferOrderPendingWorkOrders;
        }

        public IEnumerable<TransferOrderPendingBlendingInstruction> GetTransferOrderPendingBlendingInstructions(int? locationID, int? transferOrderID, string commodityIDs)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<TransferOrderPendingBlendingInstruction> transferOrderPendingBlendingInstructions = base.TotalSmartPortalEntities.GetTransferOrderPendingBlendingInstructions(locationID, transferOrderID, commodityIDs).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return transferOrderPendingBlendingInstructions;
        }
    }
}
