using TotalModel;
using TotalDTO;
using TotalModel.Models;
using TotalDTO.Purchases;

namespace TotalCore.Services.Purchases
{
    public interface IPurchaseOrderService<TDto, TPrimitiveDto, TDtoDetail> : IGenericWithViewDetailService<PurchaseOrder, PurchaseOrderDetail, PurchaseOrderViewDetail, TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
    }

    public interface IPurchaseMaterialService : IPurchaseOrderService<PurchaseOrderDTO<PurchaseOptionMaterial>, PurchaseOrderPrimitiveDTO<PurchaseOptionMaterial>, PurchaseOrderDetailDTO>
    { }
    public interface IPurchaseItemService : IPurchaseOrderService<PurchaseOrderDTO<PurchaseOptionItem>, PurchaseOrderPrimitiveDTO<PurchaseOptionItem>, PurchaseOrderDetailDTO>
    { }
    public interface IPurchaseProductService : IPurchaseOrderService<PurchaseOrderDTO<PurchaseOptionProduct>, PurchaseOrderPrimitiveDTO<PurchaseOptionProduct>, PurchaseOrderDetailDTO>
    { }
}
