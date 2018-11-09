using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Purchases;
using TotalCore.Repositories.Purchases;
using TotalCore.Services.Purchases;

namespace TotalService.Purchases
{
    public class PurchaseRequisitionService : GenericWithViewDetailService<PurchaseRequisition, PurchaseRequisitionDetail, PurchaseRequisitionViewDetail, PurchaseRequisitionDTO, PurchaseRequisitionPrimitiveDTO, PurchaseRequisitionDetailDTO>, IPurchaseRequisitionService
    {
        public PurchaseRequisitionService(IPurchaseRequisitionRepository purchaseRequisitionRepository)
            : base(purchaseRequisitionRepository, "PurchaseRequisitionPostSaveValidate", "PurchaseRequisitionSaveRelative", "PurchaseRequisitionToggleApproved", "PurchaseRequisitionToggleVoid", "PurchaseRequisitionToggleVoidDetail", "GetPurchaseRequisitionViewDetails")
        {
        }

        public override ICollection<PurchaseRequisitionViewDetail> GetViewDetails(int purchaseRequisitionID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("PurchaseRequisitionID", purchaseRequisitionID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(PurchaseRequisitionDTO purchaseRequisitionDTO)
        {
            purchaseRequisitionDTO.PurchaseRequisitionViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(purchaseRequisitionDTO);
        }
    }
}
