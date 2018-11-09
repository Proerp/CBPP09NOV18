using TotalModel.Models;

namespace TotalCore.Repositories.Purchases
{
    public interface IPurchaseRequisitionRepository : IGenericWithDetailRepository<PurchaseRequisition, PurchaseRequisitionDetail>
    {
    }

    public interface IPurchaseRequisitionAPIRepository : IGenericAPIRepository
    {
    }
}
