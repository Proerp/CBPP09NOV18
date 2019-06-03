using TotalModel.Models;
using TotalDTO.Productions;
using System.Collections.Generic;

namespace TotalCore.Services.Productions
{
    public interface IRecyclateService : IGenericWithViewDetailService<Recyclate, RecyclateDetail, RecyclateViewDetail, RecyclateDTO, RecyclatePrimitiveDTO, RecyclateDetailDTO>
    {
        ICollection<RecyclateViewDetail> GetRecyclateViewDetails(int recyclateID, int locationID, int workshiftID, bool isReadonly);
        List<RecyclateViewPackage> GetRecyclateViewPackages(int? recyclateID);
    }
}
