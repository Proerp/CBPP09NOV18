using TotalModel.Models;
using TotalDTO.Productions;

namespace TotalCore.Services.Productions
{
    public interface IPlannedOrderService : IGenericWithViewDetailService<PlannedOrder, PlannedOrderDetail, PlannedOrderViewDetail, PlannedOrderDTO, PlannedOrderPrimitiveDTO, PlannedOrderDetailDTO>
    {
    }
}
