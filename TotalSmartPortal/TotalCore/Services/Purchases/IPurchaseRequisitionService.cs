using TotalModel.Models;
using TotalDTO.Purchases;

namespace TotalCore.Services.Purchases
{
    public interface IPurchaseRequisitionService : IGenericWithViewDetailService<PurchaseRequisition, PurchaseRequisitionDetail, PurchaseRequisitionViewDetail, PurchaseRequisitionDTO, PurchaseRequisitionPrimitiveDTO, PurchaseRequisitionDetailDTO>
    {
    }
}
