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
    public class FinishedHandoverService<TDto, TPrimitiveDto, TDtoDetail> : GenericWithViewDetailService<FinishedHandover, FinishedHandoverDetail, FinishedHandoverViewDetail, TDto, TPrimitiveDto, TDtoDetail>, IFinishedHandoverService<TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>, IFinishedHandoverDTO
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
        public FinishedHandoverService(IFinishedHandoverRepository finishedHandoverRepository)
            : base(finishedHandoverRepository, "FinishedHandoverPostSaveValidate", "FinishedHandoverSaveRelative", "FinishedHandoverToggleApproved", null, null, "GetFinishedHandoverViewDetails")
        {
        }

        public override ICollection<FinishedHandoverViewDetail> GetViewDetails(int finishedHandoverID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("FinishedHandoverID", finishedHandoverID) };
            return this.GetViewDetails(parameters);
        }

    }

    public class FinishedItemHandoverService : FinishedHandoverService<FinishedHandoverDTO<FinishedItemHandoverOption>, FinishedHandoverPrimitiveDTO<FinishedItemHandoverOption>, FinishedHandoverDetailDTO>, IFinishedItemHandoverService
    {
        public FinishedItemHandoverService(IFinishedHandoverRepository finishedHandoverRepository)
            : base(finishedHandoverRepository) { }
    }
    public class FinishedProductHandoverService : FinishedHandoverService<FinishedHandoverDTO<FinishedProductHandoverOption>, FinishedHandoverPrimitiveDTO<FinishedProductHandoverOption>, FinishedHandoverDetailDTO>, IFinishedProductHandoverService
    {
        public FinishedProductHandoverService(IFinishedHandoverRepository finishedHandoverRepository)
            : base(finishedHandoverRepository) { }
    }
}
