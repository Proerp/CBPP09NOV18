using TotalModel.Models;
using TotalCore.Repositories.Purchases;

namespace TotalDAL.Repositories.Purchases
{
    public class PurchaseRequisitionRepository : GenericWithDetailRepository<PurchaseRequisition, PurchaseRequisitionDetail>, IPurchaseRequisitionRepository
    {
        public PurchaseRequisitionRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "PurchaseRequisitionEditable", "PurchaseRequisitionApproved", null, "PurchaseRequisitionVoidable")
        {
        }
    }








    public class PurchaseRequisitionAPIRepository : GenericAPIRepository, IPurchaseRequisitionAPIRepository
    {
        public PurchaseRequisitionAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetPurchaseRequisitionIndexes")
        {
        }
    }


}