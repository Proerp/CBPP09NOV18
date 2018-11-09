using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Productions;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;

namespace TotalService.Productions
{
    public class PlannedOrderService : GenericWithViewDetailService<PlannedOrder, PlannedOrderDetail, PlannedOrderViewDetail, PlannedOrderDTO, PlannedOrderPrimitiveDTO, PlannedOrderDetailDTO>, IPlannedOrderService
    {
        public PlannedOrderService(IPlannedOrderRepository plannedOrderRepository)
            : base(plannedOrderRepository, "PlannedOrderPostSaveValidate", "PlannedOrderSaveRelative", "PlannedOrderToggleApproved", "PlannedOrderToggleVoid", "PlannedOrderToggleVoidDetail", "GetPlannedOrderViewDetails")
        {
        }

        public override ICollection<PlannedOrderViewDetail> GetViewDetails(int plannedOrderID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("PlannedOrderID", plannedOrderID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(PlannedOrderDTO plannedOrderDTO)
        {
            plannedOrderDTO.PlannedOrderViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(plannedOrderDTO);
        }
    }
}