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
    public class FinishedHandoverRepository : GenericWithDetailRepository<FinishedHandover, FinishedHandoverDetail>, IFinishedHandoverRepository
    {
        public FinishedHandoverRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "FinishedHandoverEditable", "FinishedHandoverApproved")
        {
        }
    }








    public class FinishedHandoverAPIRepository : GenericAPIRepository, IFinishedHandoverAPIRepository
    {
        public FinishedHandoverAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetFinishedHandoverIndexes")
        {
        }

        protected override ObjectParameter[] GetEntityIndexParameters(string aspUserID, DateTime fromDate, DateTime toDate)
        {
            ObjectParameter[] baseParameters = base.GetEntityIndexParameters(aspUserID, fromDate, toDate);
            ObjectParameter[] objectParameters = new ObjectParameter[] { new ObjectParameter("NMVNTaskID", this.RepositoryBag.ContainsKey("NMVNTaskID") && this.RepositoryBag["NMVNTaskID"] != null ? this.RepositoryBag["NMVNTaskID"] : 0), baseParameters[0], baseParameters[1], baseParameters[2] };

            this.RepositoryBag.Remove("NMVNTaskID");

            return objectParameters;
        }

        public IEnumerable<FinishedHandoverPendingWorkshift> GetWorkshifts(int? nmvnTaskID, int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<FinishedHandoverPendingWorkshift> pendingPlannedOrderWorkshifts = base.TotalSmartPortalEntities.GetFinishedHandoverPendingWorkshifts(nmvnTaskID, locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPlannedOrderWorkshifts;
        }

        public IEnumerable<FinishedHandoverPendingCustomer> GetCustomers(int? nmvnTaskID, int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<FinishedHandoverPendingCustomer> pendingPlannedOrderCustomers = base.TotalSmartPortalEntities.GetFinishedHandoverPendingCustomers(nmvnTaskID, locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPlannedOrderCustomers;
        }

        public IEnumerable<FinishedHandoverPendingPlannedOrder> GetPlannedOrders(int? nmvnTaskID, int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<FinishedHandoverPendingPlannedOrder> pendingPlannedOrders = base.TotalSmartPortalEntities.GetFinishedHandoverPendingPlannedOrders(nmvnTaskID, locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPlannedOrders;
        }

        public IEnumerable<FinishedHandoverPendingDetail> GetPendingDetails(int? nmvnTaskID, int? finishedHandoverID, int? workshiftID, int? plannedOrderID, int? customerID, string finishedItemPackageIDs, string finishedProductPackageIDs)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<FinishedHandoverPendingDetail> finishedHandoverPendingDetails = base.TotalSmartPortalEntities.GetFinishedHandoverPendingDetails(nmvnTaskID, finishedHandoverID, workshiftID, plannedOrderID, customerID, finishedItemPackageIDs, finishedProductPackageIDs).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return finishedHandoverPendingDetails;
        }

    }
}
