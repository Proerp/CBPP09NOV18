using TotalModel;
using TotalDTO;
using TotalModel.Models;
using TotalDTO.Productions;

namespace TotalCore.Services.Productions
{       
    public interface IFinishedHandoverService<TDto, TPrimitiveDto, TDtoDetail> : IGenericWithViewDetailService<FinishedHandover, FinishedHandoverDetail, FinishedHandoverViewDetail, TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
    }

    public interface IFinishedItemHandoverService : IFinishedHandoverService<FinishedHandoverDTO<FinishedItemHandoverOption>, FinishedHandoverPrimitiveDTO<FinishedItemHandoverOption>, FinishedHandoverDetailDTO>
    { }
    public interface IFinishedProductHandoverService : IFinishedHandoverService<FinishedHandoverDTO<FinishedProductHandoverOption>, FinishedHandoverPrimitiveDTO<FinishedProductHandoverOption>, FinishedHandoverDetailDTO>
    { }
}
