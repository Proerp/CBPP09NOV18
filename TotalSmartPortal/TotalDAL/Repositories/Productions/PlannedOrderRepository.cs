using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalCore.Repositories.Productions;

namespace TotalDAL.Repositories.Productions
{
    public class PlannedOrderRepository : GenericWithDetailRepository<PlannedOrder, PlannedOrderDetail>, IPlannedOrderRepository
    {
        public PlannedOrderRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "PlannedOrderEditable", "PlannedOrderApproved", null, "PlannedOrderVoidable")
        {
        }
    }








    public class PlannedOrderAPIRepository : GenericAPIRepository, IPlannedOrderAPIRepository
    {
        public PlannedOrderAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetPlannedOrderIndexes")
        {
        }

        protected override ObjectParameter[] GetEntityIndexParameters(string aspUserID, System.DateTime fromDate, System.DateTime toDate)
        {
            ObjectParameter[] baseParameters = base.GetEntityIndexParameters(aspUserID, fromDate, toDate);
            ObjectParameter[] objectParameters = new ObjectParameter[] { baseParameters[0], baseParameters[1], baseParameters[2], new ObjectParameter("DateOptionID", this.RepositoryBag.ContainsKey("DateOptionID") && this.RepositoryBag["DateOptionID"] != null ? this.RepositoryBag["DateOptionID"] : 0), new ObjectParameter("FilterOptionID", this.RepositoryBag.ContainsKey("FilterOptionID") && this.RepositoryBag["FilterOptionID"] != null ? this.RepositoryBag["FilterOptionID"] : 0) };

            this.RepositoryBag.Remove("DateOptionID");
            this.RepositoryBag.Remove("FilterOptionID");

            return objectParameters;
        }

        public IEnumerable<PlannedOrderLog> GetPlannedOrderLogs(int? plannedOrderID, int? firmOrderID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PlannedOrderLog> plannedOrderLogs = base.TotalSmartPortalEntities.GetPlannedOrderLogs(plannedOrderID, firmOrderID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return plannedOrderLogs;
        }
    }


}