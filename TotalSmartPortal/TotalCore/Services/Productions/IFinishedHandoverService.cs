using TotalModel.Models;
using TotalDTO.Productions;

namespace TotalCore.Services.Productions
{   
    public interface IFinishedHandoverService : IGenericWithViewDetailService<FinishedHandover, FinishedHandoverDetail, FinishedHandoverViewDetail, FinishedHandoverDTO, FinishedHandoverPrimitiveDTO, FinishedHandoverDetailDTO>
    {
    }
}
