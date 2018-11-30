using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Purchases;
using TotalCore.Repositories.Purchases;
using TotalCore.Services.Purchases;

namespace TotalService.Purchases
{
    public class PurchaseOrderService : GenericWithViewDetailService<PurchaseOrder, PurchaseOrderDetail, PurchaseOrderViewDetail, PurchaseOrderDTO, PurchaseOrderPrimitiveDTO, PurchaseOrderDetailDTO>, IPurchaseOrderService
    {
        public PurchaseOrderService(IPurchaseOrderRepository purchaseOrderRepository)
            : base(purchaseOrderRepository, "PurchaseOrderPostSaveValidate", "PurchaseOrderSaveRelative", "PurchaseOrderToggleApproved", "PurchaseOrderToggleVoid", "PurchaseOrderToggleVoidDetail", "GetPurchaseOrderViewDetails")
        {
        }

        public override ICollection<PurchaseOrderViewDetail> GetViewDetails(int purchaseOrderID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("PurchaseOrderID", purchaseOrderID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(PurchaseOrderDTO purchaseOrderDTO)
        {
            purchaseOrderDTO.PurchaseOrderViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(purchaseOrderDTO);
        }
    }
}
