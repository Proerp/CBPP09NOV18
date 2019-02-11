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
    public class SemifinishedHandoverRepository : GenericWithDetailRepository<SemifinishedHandover, SemifinishedHandoverDetail>, ISemifinishedHandoverRepository
    {
        public SemifinishedHandoverRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "SemifinishedHandoverEditable", "SemifinishedHandoverApproved")
        {
        }
    }








    public class SemifinishedHandoverAPIRepository : GenericAPIRepository, ISemifinishedHandoverAPIRepository
    {
        public SemifinishedHandoverAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetSemifinishedHandoverIndexes")
        {
        }

        public IEnumerable<SemifinishedHandoverPendingCustomer> GetCustomers(int? nmvnTaskID, int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<SemifinishedHandoverPendingCustomer> pendingPlannedOrderCustomers = base.TotalSmartPortalEntities.GetSemifinishedHandoverPendingCustomers(nmvnTaskID, locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPlannedOrderCustomers;
        }

        public IEnumerable<SemifinishedHandoverPendingWorkshift> GetWorkshifts(int? nmvnTaskID, int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<SemifinishedHandoverPendingWorkshift> pendingWorkshifts = base.TotalSmartPortalEntities.GetSemifinishedHandoverPendingWorkshifts(nmvnTaskID, locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingWorkshifts;
        }

        public IEnumerable<SemifinishedHandoverPendingDetail> GetPendingDetails(int? nmvnTaskID, int? semifinishedHandoverID, int? workshiftID, int? customerID, string semifinishedProductIDs, string semifinishedItemIDs)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<SemifinishedHandoverPendingDetail> semifinishedHandoverPendingDetails = base.TotalSmartPortalEntities.GetSemifinishedHandoverPendingDetails(nmvnTaskID, semifinishedHandoverID, workshiftID, customerID, semifinishedProductIDs, semifinishedItemIDs).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return semifinishedHandoverPendingDetails;
        }
    
    }
}
