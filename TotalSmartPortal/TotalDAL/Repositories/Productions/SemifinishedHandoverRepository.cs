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

        protected override ObjectParameter[] GetEntityIndexParameters(string aspUserID, DateTime fromDate, DateTime toDate)
        {
            ObjectParameter[] baseParameters = base.GetEntityIndexParameters(aspUserID, fromDate, toDate);
            ObjectParameter[] objectParameters = new ObjectParameter[] { new ObjectParameter("NMVNTaskID", this.RepositoryBag.ContainsKey("NMVNTaskID") && this.RepositoryBag["NMVNTaskID"] != null ? this.RepositoryBag["NMVNTaskID"] : 0), baseParameters[0], baseParameters[1], baseParameters[2] };

            this.RepositoryBag.Remove("NMVNTaskID");

            return objectParameters;
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

        public IEnumerable<SemifinishedHandoverPendingDetail> GetPendingDetails(int? nmvnTaskID, int? semifinishedHandoverID, int? workshiftID, int? customerID, string semifinishedItemIDs, string semifinishedProductIDs)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<SemifinishedHandoverPendingDetail> semifinishedHandoverPendingDetails = base.TotalSmartPortalEntities.GetSemifinishedHandoverPendingDetails(nmvnTaskID, semifinishedHandoverID, workshiftID, customerID, semifinishedItemIDs, semifinishedProductIDs).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return semifinishedHandoverPendingDetails;
        }
    
    }
}
