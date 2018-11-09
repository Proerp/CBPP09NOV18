using TotalModel.Models;
using TotalDTO.Productions;

namespace TotalCore.Services.Productions
{
    public interface IProductionOrderService : IGenericWithViewDetailService<ProductionOrder, ProductionOrderDetail, ProductionOrderViewDetail, ProductionOrderDTO, ProductionOrderPrimitiveDTO, ProductionOrderDetailDTO>
    {
    }
}
