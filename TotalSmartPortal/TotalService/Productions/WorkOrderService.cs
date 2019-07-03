using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel;
using TotalDTO;
using TotalModel.Models;
using TotalDTO.Productions;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;

namespace TotalService.Productions
{
    public class WorkOrderService<TDto, TPrimitiveDto, TDtoDetail> : GenericWithViewDetailService<WorkOrder, WorkOrderDetail, WorkOrderViewDetail, TDto, TPrimitiveDto, TDtoDetail>, IWorkOrderService<TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>, IWorkOrderDTO
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
        public WorkOrderService(IWorkOrderRepository workOrderRepository)
            : base(workOrderRepository, "WorkOrderPostSaveValidate", "WorkOrderSaveRelative", "WorkOrderToggleApproved", null, null, null, "GetWorkOrderViewDetails")
        {
        }

        public override ICollection<WorkOrderViewDetail> GetViewDetails(int workOrderID)
        {
            throw new System.ArgumentException("Invalid call GetViewDetails(id). Use GetWorkOrderViewDetails instead.", "Service");
        }

        public ICollection<WorkOrderViewDetail> GetWorkOrderViewDetails(int workOrderID, int firmOrderID, int? warehouseID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("WorkOrderID", workOrderID), new ObjectParameter("FirmOrderID", firmOrderID), new ObjectParameter("WarehouseID", warehouseID) };
            return this.GetViewDetails(parameters);
        }
    }



    public class MaterialWorkOrderService : WorkOrderService<WorkOrderDTO<WOOptionMaterial>, WorkOrderPrimitiveDTO<WOOptionMaterial>, WorkOrderDetailDTO>, IMaterialWorkOrderService
    {
        public MaterialWorkOrderService(IWorkOrderRepository workOrderRepository)
            : base(workOrderRepository) { }
    }
    public class ItemWorkOrderService : WorkOrderService<WorkOrderDTO<WOOptionItem>, WorkOrderPrimitiveDTO<WOOptionItem>, WorkOrderDetailDTO>, IItemWorkOrderService
    {
        public ItemWorkOrderService(IWorkOrderRepository workOrderRepository)
            : base(workOrderRepository) { }
    }
    public class ProductWorkOrderService : WorkOrderService<WorkOrderDTO<WOOptionProduct>, WorkOrderPrimitiveDTO<WOOptionProduct>, WorkOrderDetailDTO>, IProductWorkOrderService
    {
        public ProductWorkOrderService(IWorkOrderRepository workOrderRepository)
            : base(workOrderRepository) { }
    }
}