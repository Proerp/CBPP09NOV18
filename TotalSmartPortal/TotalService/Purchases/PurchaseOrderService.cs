using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel;
using TotalDTO;
using TotalModel.Models;
using TotalDTO.Purchases;
using TotalCore.Repositories.Purchases;
using TotalCore.Services.Purchases;

namespace TotalService.Purchases
{
    public class PurchaseOrderService<TDto, TPrimitiveDto, TDtoDetail> : GenericWithViewDetailService<PurchaseOrder, PurchaseOrderDetail, PurchaseOrderViewDetail, TDto, TPrimitiveDto, TDtoDetail>, IPurchaseOrderService<TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>, IPurchaseOrderDTO
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
        public PurchaseOrderService(IPurchaseOrderRepository purchaseOrderRepository)
            : base(purchaseOrderRepository, "PurchaseOrderPostSaveValidate", "PurchaseOrderSaveRelative", "PurchaseOrderToggleApproved", "PurchaseOrderToggleVoid", "PurchaseOrderToggleVoidDetail", null, "GetPurchaseOrderViewDetails")
        {
        }

        public override ICollection<PurchaseOrderViewDetail> GetViewDetails(int purchaseOrderID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("PurchaseOrderID", purchaseOrderID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(TDto dto)
        {
            dto.PurchaseOrderViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(dto);
        }
    }


    public class PurchaseMaterialService : PurchaseOrderService<PurchaseOrderDTO<PurchaseOptionMaterial>, PurchaseOrderPrimitiveDTO<PurchaseOptionMaterial>, PurchaseOrderDetailDTO>, IPurchaseMaterialService
    {
        public PurchaseMaterialService(IPurchaseOrderRepository purchaseOrderRepository)
            : base(purchaseOrderRepository) { }
    }
    public class PurchaseItemService : PurchaseOrderService<PurchaseOrderDTO<PurchaseOptionItem>, PurchaseOrderPrimitiveDTO<PurchaseOptionItem>, PurchaseOrderDetailDTO>, IPurchaseItemService
    {
        public PurchaseItemService(IPurchaseOrderRepository purchaseOrderRepository)
            : base(purchaseOrderRepository) { }
    }
    public class PurchaseProductService : PurchaseOrderService<PurchaseOrderDTO<PurchaseOptionProduct>, PurchaseOrderPrimitiveDTO<PurchaseOptionProduct>, PurchaseOrderDetailDTO>, IPurchaseProductService
    {
        public PurchaseProductService(IPurchaseOrderRepository purchaseOrderRepository)
            : base(purchaseOrderRepository) { }
    }
}
