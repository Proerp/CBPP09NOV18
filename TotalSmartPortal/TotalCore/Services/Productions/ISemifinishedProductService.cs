using TotalModel.Models;
using TotalDTO.Productions;
using System.Collections.Generic;


namespace TotalCore.Services.Productions
{
    public interface ISemifinishedProductService : IGenericWithViewDetailService<SemifinishedProduct, SemifinishedProductDetail, SemifinishedProductViewDetail, SemifinishedProductDTO, SemifinishedProductPrimitiveDTO, SemifinishedProductDetailDTO>
    {
        ICollection<SemifinishedProductViewDetail> GetSemifinishedProductViewDetails(int semifinishedProductID, int firmOrderID);

        decimal GetMaterialQuantityRemains(int? materialIssueDetailID);
    }
}
