using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Productions;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;

namespace TotalService.Productions
{
    public class SemifinishedRecyclateService : GenericWithViewDetailService<SemifinishedRecyclate, SemifinishedRecyclateDetail, SemifinishedRecyclateViewDetail, SemifinishedRecyclateDTO, SemifinishedRecyclatePrimitiveDTO, SemifinishedRecyclateDetailDTO>, ISemifinishedRecyclateService
    {
        public SemifinishedRecyclateService(ISemifinishedRecyclateRepository semifinishedRecyclateRepository)
            : base(semifinishedRecyclateRepository, "SemifinishedRecyclatePostSaveValidate", "SemifinishedRecyclateSaveRelative", "SemifinishedRecyclateToggleApproved", null, null, "GetSemifinishedRecyclateViewDetails")
        {
        }

        public override ICollection<SemifinishedRecyclateViewDetail> GetViewDetails(int semifinishedRecyclateID)
        {
            throw new System.ArgumentException("Invalid call GetViewDetails(id). Use GetSemifinishedRecyclateViewDetails instead.", "FinishProduct Service");
        }

        public ICollection<SemifinishedRecyclateViewDetail> GetSemifinishedRecyclateViewDetails(int semifinishedRecyclateID, int locationID, int workshiftID, bool isReadonly)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("SemifinishedRecyclateID", semifinishedRecyclateID), new ObjectParameter("LocationID", locationID), new ObjectParameter("WorkshiftID", workshiftID), new ObjectParameter("IsReadonly", isReadonly) };
            return this.GetViewDetails(parameters);
        }
    }
}
