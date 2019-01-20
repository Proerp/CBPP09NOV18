using TotalModel.Models;
using TotalDTO.Productions;
using System.Collections.Generic;


namespace TotalCore.Services.Productions
{
    public interface ISemifinishedItemService : IGenericWithViewDetailService<SemifinishedItem, SemifinishedItemDetail, SemifinishedItemViewDetail, SemifinishedItemDTO, SemifinishedItemPrimitiveDTO, SemifinishedItemDetailDTO>
    {
        ICollection<SemifinishedItemViewDetail> GetSemifinishedItemViewDetails(int semifinishedItemID, int firmOrderID);
    }
}
