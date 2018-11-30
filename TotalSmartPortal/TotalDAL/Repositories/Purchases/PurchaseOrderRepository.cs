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
    }


}