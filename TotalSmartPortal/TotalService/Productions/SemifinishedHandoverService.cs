using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel;
using TotalDTO;
using TotalModel.Models;
using TotalDTO.Productions;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;

namespace TotalService.Productions
{
    public class SemifinishedHandoverService<TDto, TPrimitiveDto, TDtoDetail> : GenericWithViewDetailService<SemifinishedHandover, SemifinishedHandoverDetail, SemifinishedHandoverViewDetail, TDto, TPrimitiveDto, TDtoDetail>, ISemifinishedHandoverService<TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>, ISemifinishedHandoverDTO
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
        public SemifinishedHandoverService(ISemifinishedHandoverRepository semifinishedHandoverRepository)
            : base(semifinishedHandoverRepository, "SemifinishedHandoverPostSaveValidate", "SemifinishedHandoverSaveRelative", "SemifinishedHandoverToggleApproved", null, null, null, "GetSemifinishedHandoverViewDetails")
        {
        }

        public override ICollection<SemifinishedHandoverViewDetail> GetViewDetails(int semifinishedHandoverID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("SemifinishedHandoverID", semifinishedHandoverID) };
            return this.GetViewDetails(parameters);
        }

    }
    
    public class SemifinishedItemHandoverService : SemifinishedHandoverService<SemifinishedHandoverDTO<SemifinishedItemHandoverOption>, SemifinishedHandoverPrimitiveDTO<SemifinishedItemHandoverOption>, SemifinishedHandoverDetailDTO>, ISemifinishedItemHandoverService
    {
        public SemifinishedItemHandoverService(ISemifinishedHandoverRepository semifinishedHandoverRepository)
            : base(semifinishedHandoverRepository) { }
    }
    public class SemifinishedProductHandoverService : SemifinishedHandoverService<SemifinishedHandoverDTO<SemifinishedProductHandoverOption>, SemifinishedHandoverPrimitiveDTO<SemifinishedProductHandoverOption>, SemifinishedHandoverDetailDTO>, ISemifinishedProductHandoverService
    {
        public SemifinishedProductHandoverService(ISemifinishedHandoverRepository semifinishedHandoverRepository)
            : base(semifinishedHandoverRepository) { }
    }
}
