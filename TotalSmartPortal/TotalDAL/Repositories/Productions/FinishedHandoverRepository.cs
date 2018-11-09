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

        public IEnumerable<FinishedHandoverPendingCustomer> GetCustomers(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<FinishedHandoverPendingCustomer> pendingPlannedOrderCustomers = base.TotalSmartPortalEntities.GetFinishedHandoverPendingCustomers(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPlannedOrderCustomers;
        }

        public IEnumerable<FinishedHandoverPendingPlannedOrder> GetPlannedOrders(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<FinishedHandoverPendingPlannedOrder> pendingPlannedOrders = base.TotalSmartPortalEntities.GetFinishedHandoverPendingPlannedOrders(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPlannedOrders;
        }

        public IEnumerable<FinishedHandoverPendingDetail> GetPendingDetails(int? finishedHandoverID, int? plannedOrderID, int? customerID, string finishedProductPackageIDs, bool? isReadonly)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<FinishedHandoverPendingDetail> finishedHandoverPendingDetails = base.TotalSmartPortalEntities.GetFinishedHandoverPendingDetails(finishedHandoverID, plannedOrderID, customerID, finishedProductPackageIDs, isReadonly).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return finishedHandoverPendingDetails;
        }

    }
}
