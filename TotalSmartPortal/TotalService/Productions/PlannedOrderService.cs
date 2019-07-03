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
    public class PlannedOrderService<TDto, TPrimitiveDto, TDtoDetail> : GenericWithViewDetailService<PlannedOrder, PlannedOrderDetail, PlannedOrderViewDetail, TDto, TPrimitiveDto, TDtoDetail>, IPlannedOrderService<TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>, IPlannedOrderDTO
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
        public PlannedOrderService(IPlannedOrderRepository plannedOrderRepository)
            : base(plannedOrderRepository, "PlannedOrderPostSaveValidate", "PlannedOrderSaveRelative", "PlannedOrderToggleApproved", "PlannedOrderToggleVoid", "PlannedOrderToggleVoidDetail", null, "GetPlannedOrderViewDetails")
        {
        }

        public override ICollection<PlannedOrderViewDetail> GetViewDetails(int plannedOrderID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("PlannedOrderID", plannedOrderID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(TDto dto)
        {
            dto.PlannedOrderViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(dto);
        }
    }


    public class PlannedItemService : PlannedOrderService<PlannedOrderDTO<PlannedOptionItem>, PlannedOrderPrimitiveDTO<PlannedOptionItem>, PlannedOrderDetailDTO>, IPlannedItemService
    {
        public PlannedItemService(IPlannedOrderRepository plannedOrderRepository)
            : base(plannedOrderRepository) { }
    }
    public class PlannedProductService : PlannedOrderService<PlannedOrderDTO<PlannedOptionProduct>, PlannedOrderPrimitiveDTO<PlannedOptionProduct>, PlannedOrderDetailDTO>, IPlannedProductService
    {
        public PlannedProductService(IPlannedOrderRepository plannedOrderRepository)
            : base(plannedOrderRepository) { }
    }

}