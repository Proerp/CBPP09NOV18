using System.Collections.Generic;

using TotalModel;
using TotalDTO;
using TotalModel.Models;
using TotalDTO.Productions;

namespace TotalCore.Services.Productions
{
    public interface IRecyclateService<TDto, TPrimitiveDto, TDtoDetail> : IGenericWithViewDetailService<Recyclate, RecyclateDetail, RecyclateViewDetail, TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
        ICollection<RecyclateViewDetail> GetRecyclateViewDetails(int? nmvnTaskID, int recyclateID, int locationID, int workshiftID, bool isReadonly);
        List<RecyclateViewPackage> GetRecyclateViewPackages(int? nmvnTaskID, int? recyclateID);
    }

    public interface ISemifinishedProductRecyclateService : IRecyclateService<RecyclateDTO<SemifinishedProductRecyclateOption>, RecyclatePrimitiveDTO<SemifinishedProductRecyclateOption>, RecyclateDetailDTO>
    { }
    public interface IFinishedProductRecyclateService : IRecyclateService<RecyclateDTO<FinishedProductRecyclateOption>, RecyclatePrimitiveDTO<FinishedProductRecyclateOption>, RecyclateDetailDTO>
    { }
    public interface IFinishedItemRecyclateService : IRecyclateService<RecyclateDTO<FinishedItemRecyclateOption>, RecyclatePrimitiveDTO<FinishedItemRecyclateOption>, RecyclateDetailDTO>
    { }
}
