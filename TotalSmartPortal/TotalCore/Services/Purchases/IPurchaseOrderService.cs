using TotalModel.Models;
using TotalDTO.Purchases;

namespace TotalCore.Services.Purchases
{
    public interface IPurchaseOrderService : IGenericWithViewDetailService<PurchaseOrder, PurchaseOrderDetail, PurchaseOrderViewDetail, PurchaseOrderDTO, PurchaseOrderPrimitiveDTO, PurchaseOrderDetailDTO>
    {
    }
}
