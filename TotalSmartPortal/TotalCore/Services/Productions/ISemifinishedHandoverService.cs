using TotalModel;
using TotalDTO;
using TotalModel.Models;
using TotalDTO.Productions;

namespace TotalCore.Services.Productions
{
    public interface ISemifinishedHandoverService<TDto, TPrimitiveDto, TDtoDetail> : IGenericWithViewDetailService<SemifinishedHandover, SemifinishedHandoverDetail, SemifinishedHandoverViewDetail, TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
    }

    public interface ISemifinishedItemHandoverService : ISemifinishedHandoverService<SemifinishedHandoverDTO<SemifinishedItemHandoverOption>, SemifinishedHandoverPrimitiveDTO<SemifinishedItemHandoverOption>, SemifinishedHandoverDetailDTO>
    { }
    public interface ISemifinishedProductHandoverService : ISemifinishedHandoverService<SemifinishedHandoverDTO<SemifinishedProductHandoverOption>, SemifinishedHandoverPrimitiveDTO<SemifinishedProductHandoverOption>, SemifinishedHandoverDetailDTO>
    { }   
}
