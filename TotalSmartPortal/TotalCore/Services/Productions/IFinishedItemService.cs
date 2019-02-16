using TotalModel.Models;
using TotalDTO.Productions;
using System.Collections.Generic;

namespace TotalCore.Services.Productions
{
    public interface IFinishedItemService : IGenericWithViewDetailService<FinishedItem, FinishedItemDetail, FinishedItemViewDetail, FinishedItemDTO, FinishedItemPrimitiveDTO, FinishedItemDetailDTO>
    {
        ICollection<FinishedItemViewDetail> GetFinishedItemViewDetails(int finishedItemID, int locationID, int firmOrderID, bool isReadonly);
    }
}
