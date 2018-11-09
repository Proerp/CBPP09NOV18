using TotalModel;
using TotalDTO;
using TotalModel.Models;
using TotalDTO.Inventories;

namespace TotalCore.Services.Inventories
{   
    public interface ITransferOrderService<TDto, TPrimitiveDto, TDtoDetail> : IGenericWithViewDetailService<TransferOrder, TransferOrderDetail, TransferOrderViewDetail, TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
    }

    public interface IMaterialTransferOrderService : ITransferOrderService<TransferOrderDTO<TOOptionMaterial>, TransferOrderPrimitiveDTO<TOOptionMaterial>, TransferOrderDetailDTO>
    { }
    public interface IItemTransferOrderService : ITransferOrderService<TransferOrderDTO<TOOptionItem>, TransferOrderPrimitiveDTO<TOOptionItem>, TransferOrderDetailDTO>
    { }
    public interface IProductTransferOrderService : ITransferOrderService<TransferOrderDTO<TOOptionProduct>, TransferOrderPrimitiveDTO<TOOptionProduct>, TransferOrderDetailDTO>
    { }   
}
