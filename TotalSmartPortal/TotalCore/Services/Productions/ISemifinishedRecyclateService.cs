using TotalModel.Models;
using TotalDTO.Productions;
using System.Collections.Generic;

namespace TotalCore.Services.Productions
{
    public interface ISemifinishedRecyclateService : IGenericWithViewDetailService<SemifinishedRecyclate, SemifinishedRecyclateDetail, SemifinishedRecyclateViewDetail, SemifinishedRecyclateDTO, SemifinishedRecyclatePrimitiveDTO, SemifinishedRecyclateDetailDTO>
    {
        ICollection<SemifinishedRecyclateViewDetail> GetSemifinishedRecyclateViewDetails(int semifinishedRecyclateID, int locationID, int workshiftID, bool isReadonly);
    }
}
