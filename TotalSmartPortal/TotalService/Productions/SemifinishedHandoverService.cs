using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Productions;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;

namespace TotalService.Productions
{ 
    public class SemifinishedHandoverService : GenericWithViewDetailService<SemifinishedHandover, SemifinishedHandoverDetail, SemifinishedHandoverViewDetail, SemifinishedHandoverDTO, SemifinishedHandoverPrimitiveDTO, SemifinishedHandoverDetailDTO>, ISemifinishedHandoverService
    {
        public SemifinishedHandoverService(ISemifinishedHandoverRepository semifinishedHandoverRepository)
            : base(semifinishedHandoverRepository, "SemifinishedHandoverPostSaveValidate", "SemifinishedHandoverSaveRelative", "SemifinishedHandoverToggleApproved", null, null, "GetSemifinishedHandoverViewDetails")
        {
        }

        public override ICollection<SemifinishedHandoverViewDetail> GetViewDetails(int semifinishedHandoverID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("SemifinishedHandoverID", semifinishedHandoverID) };
            return this.GetViewDetails(parameters);
        }

    }
}
