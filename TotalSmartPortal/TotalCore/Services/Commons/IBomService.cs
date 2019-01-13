using TotalModel.Models;
using TotalDTO.Commons;

namespace TotalCore.Services.Commons
{
    public interface IBomService : IGenericWithViewDetailService<Bom, BomDetail, BomViewDetail, BomDTO, BomPrimitiveDTO, BomDetailDTO>
    {
    }
}
