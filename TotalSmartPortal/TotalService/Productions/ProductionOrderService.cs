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
    public class ProductionOrderService<TDto, TPrimitiveDto, TDtoDetail> : GenericWithViewDetailService<ProductionOrder, ProductionOrderDetail, ProductionOrderViewDetail, TDto, TPrimitiveDto, TDtoDetail>, IProductionOrderService<TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>, IProductionOrderDTO
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
        public ProductionOrderService(IProductionOrderRepository productionOrderRepository)
            : base(productionOrderRepository, "ProductionOrderPostSaveValidate", "ProductionOrderSaveRelative", "ProductionOrderToggleApproved", "ProductionOrderToggleVoid", "ProductionOrderToggleVoidDetail", null, "GetProductionOrderViewDetails")
        {
        }

        public override ICollection<ProductionOrderViewDetail> GetViewDetails(int productionOrderID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("ProductionOrderID", productionOrderID) };
            return this.GetViewDetails(parameters);
        }

    }


    public class ItemOrderService : ProductionOrderService<ProductionOrderDTO<ProrderOptionItem>, ProductionOrderPrimitiveDTO<ProrderOptionItem>, ProductionOrderDetailDTO>, IItemOrderService
    {
        public ItemOrderService(IProductionOrderRepository productionOrderRepository)
            : base(productionOrderRepository) { }
    }
    public class ProductOrderService : ProductionOrderService<ProductionOrderDTO<ProrderOptionProduct>, ProductionOrderPrimitiveDTO<ProrderOptionProduct>, ProductionOrderDetailDTO>, IProductOrderService
    {
        public ProductOrderService(IProductionOrderRepository productionOrderRepository)
            : base(productionOrderRepository) { }
    }
}