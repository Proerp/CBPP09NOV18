using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalCore.Repositories.Purchases;

namespace TotalDAL.Repositories.Purchases
{
    public class PurchaseOrderRepository : GenericWithDetailRepository<PurchaseOrder, PurchaseOrderDetail>, IPurchaseOrderRepository
    {
        public PurchaseOrderRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "PurchaseOrderEditable", "PurchaseOrderApproved", null, "PurchaseOrderVoidable")
        {
        }
    }








    public class PurchaseOrderAPIRepository : GenericAPIRepository, IPurchaseOrderAPIRepository
    {
        public PurchaseOrderAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetPurchaseOrderIndexes")
        {
        }

        protected override ObjectParameter[] GetEntityIndexParameters(string aspUserID, System.DateTime fromDate, System.DateTime toDate)
        {
            ObjectParameter[] baseParameters = base.GetEntityIndexParameters(aspUserID, fromDate, toDate);
            ObjectParameter[] objectParameters = new ObjectParameter[] { new ObjectParameter("NMVNTaskID", this.RepositoryBag.ContainsKey("NMVNTaskID") && this.RepositoryBag["NMVNTaskID"] != null ? this.RepositoryBag["NMVNTaskID"] : 0), baseParameters[0], baseParameters[1], baseParameters[2] };

            this.RepositoryBag.Remove("NMVNTaskID");

            return objectParameters;
        }
    }


}