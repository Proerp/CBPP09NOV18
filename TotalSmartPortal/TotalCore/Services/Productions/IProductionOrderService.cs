using TotalModel;
using TotalDTO;
using TotalModel.Models;
using TotalDTO.Productions;

namespace TotalCore.Services.Productions
{
    public interface IProductionOrderService<TDto, TPrimitiveDto, TDtoDetail> : IGenericWithViewDetailService<ProductionOrder, ProductionOrderDetail, ProductionOrderViewDetail, TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
    }

    public interface IItemOrderService : IProductionOrderService<ProductionOrderDTO<ProrderOptionItem>, ProductionOrderPrimitiveDTO<ProrderOptionItem>, ProductionOrderDetailDTO>
    { }
    public interface IProductOrderService : IProductionOrderService<ProductionOrderDTO<ProrderOptionProduct>, ProductionOrderPrimitiveDTO<ProrderOptionProduct>, ProductionOrderDetailDTO>
    { }   
}
