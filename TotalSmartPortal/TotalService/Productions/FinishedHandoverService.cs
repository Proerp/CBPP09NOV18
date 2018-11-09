using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Productions;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;

namespace TotalService.Productions
{
    public class FinishedHandoverService : GenericWithViewDetailService<FinishedHandover, FinishedHandoverDetail, FinishedHandoverViewDetail, FinishedHandoverDTO, FinishedHandoverPrimitiveDTO, FinishedHandoverDetailDTO>, IFinishedHandoverService
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
}
