using TotalModel;
using TotalDTO;
using TotalModel.Models;
using TotalDTO.Productions;

namespace TotalCore.Services.Productions
{
    public interface IPlannedOrderService<TDto, TPrimitiveDto, TDtoDetail> : IGenericWithViewDetailService<PlannedOrder, PlannedOrderDetail, PlannedOrderViewDetail, TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
    }

    public interface IPlannedItemService : IPlannedOrderService<PlannedOrderDTO<PlannedOptionItem>, PlannedOrderPrimitiveDTO<PlannedOptionItem>, PlannedOrderDetailDTO>
    { }
    public interface IPlannedProductService : IPlannedOrderService<PlannedOrderDTO<PlannedOptionProduct>, PlannedOrderPrimitiveDTO<PlannedOptionProduct>, PlannedOrderDetailDTO>
    { }   
}
