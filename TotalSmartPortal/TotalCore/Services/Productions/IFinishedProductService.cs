using TotalModel.Models;
using TotalDTO.Productions;
using System.Collections.Generic;

namespace TotalCore.Services.Productions
{  
    public interface IFinishedProductService : IGenericWithViewDetailService<FinishedProduct, FinishedProductDetail, FinishedProductViewDetail, FinishedProductDTO, FinishedProductPrimitiveDTO, FinishedProductDetailDTO>
    {
        ICollection<FinishedProductViewDetail> GetFinishedProductViewDetails(int finishedProductID, int locationID, int firmOrderID, bool isReadonly);
    }
}
