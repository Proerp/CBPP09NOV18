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
    public class WorkOrderRepository : GenericWithDetailRepository<WorkOrder, WorkOrderDetail>, IWorkOrderRepository
    {
        public WorkOrderRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "WorkOrderEditable", "WorkOrderApproved")
        {
        }
    }








    public class WorkOrderAPIRepository : GenericAPIRepository, IWorkOrderAPIRepository
    {
        public WorkOrderAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetWorkOrderIndexes")
        {
        }

        protected override ObjectParameter[] GetEntityIndexParameters(string aspUserID, DateTime fromDate, DateTime toDate)
        {
            ObjectParameter[] baseParameters = base.GetEntityIndexParameters(aspUserID, fromDate, toDate);
            ObjectParameter[] objectParameters = new ObjectParameter[] { new ObjectParameter("NMVNTaskID", this.RepositoryBag.ContainsKey("NMVNTaskID") && this.RepositoryBag["NMVNTaskID"] != null ? this.RepositoryBag["NMVNTaskID"] : 0), baseParameters[0], baseParameters[1], baseParameters[2] };

            this.RepositoryBag.Remove("NMVNTaskID");

            return objectParameters;
        }

        public IEnumerable<WorkOrderPendingFirmOrder> GetFirmOrders(int? locationID, int? nmvnTaskID, int? firmOrderID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<WorkOrderPendingFirmOrder> pendingFirmOrders = base.TotalSmartPortalEntities.GetWorkOrderPendingFirmOrders(locationID, nmvnTaskID, firmOrderID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingFirmOrders;
        }
    }


}