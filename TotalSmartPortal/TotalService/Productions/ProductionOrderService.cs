using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Productions;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;

namespace TotalService.Productions
{
    public class ProductionOrderService : GenericWithViewDetailService<ProductionOrder, ProductionOrderDetail, ProductionOrderViewDetail, ProductionOrderDTO, ProductionOrderPrimitiveDTO, ProductionOrderDetailDTO>, IProductionOrderService
    {
        public ProductionOrderService(IProductionOrderRepository productionOrderRepository)
            : base(productionOrderRepository, "ProductionOrderPostSaveValidate", "ProductionOrderSaveRelative", "ProductionOrderToggleApproved", "ProductionOrderToggleVoid", "ProductionOrderToggleVoidDetail", "GetProductionOrderViewDetails")
        {
        }

        public override ICollection<ProductionOrderViewDetail> GetViewDetails(int productionOrderID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("ProductionOrderID", productionOrderID) };
            return this.GetViewDetails(parameters);
        }

    }
}