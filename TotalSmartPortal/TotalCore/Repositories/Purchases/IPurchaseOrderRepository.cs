using TotalModel.Models;

namespace TotalCore.Repositories.Purchases
{
    public interface IPurchaseOrderRepository : IGenericWithDetailRepository<PurchaseOrder, PurchaseOrderDetail>
    {
    }

    public interface IPurchaseOrderAPIRepository : IGenericAPIRepository
    {
    }
}
