using System.Collections.Generic;

using TotalModel;
using TotalDTO;
using TotalModel.Models;
using TotalDTO.Productions;

namespace TotalCore.Services.Productions
{
    public interface IWorkOrderService<TDto, TPrimitiveDto, TDtoDetail> : IGenericWithViewDetailService<WorkOrder, WorkOrderDetail, WorkOrderViewDetail, TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
        ICollection<WorkOrderViewDetail> GetWorkOrderViewDetails(int workOrderID, int firmOrderID, int? warehouseID);
    }

    public interface IMaterialWorkOrderService : IWorkOrderService<WorkOrderDTO<WOOptionMaterial>, WorkOrderPrimitiveDTO<WOOptionMaterial>, WorkOrderDetailDTO>
    { }
    public interface IItemWorkOrderService : IWorkOrderService<WorkOrderDTO<WOOptionItem>, WorkOrderPrimitiveDTO<WOOptionItem>, WorkOrderDetailDTO>
    { }
    public interface IProductWorkOrderService : IWorkOrderService<WorkOrderDTO<WOOptionProduct>, WorkOrderPrimitiveDTO<WOOptionProduct>, WorkOrderDetailDTO>
    { }
}
