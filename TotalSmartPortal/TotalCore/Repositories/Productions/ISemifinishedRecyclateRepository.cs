using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{

    public interface ISemifinishedRecyclateRepository : IGenericWithDetailRepository<SemifinishedRecyclate, SemifinishedRecyclateDetail>
    {
        List<SemifinishedRecyclateViewPackage> GetSemifinishedRecyclateViewPackages(int? semifinishedRecyclateID);
    }

    public interface ISemifinishedRecyclateAPIRepository : IGenericAPIRepository
    {
        IEnumerable<SemifinishedRecyclatePendingWorkshift> GetPendingWorkshifts(int? locationID);
    }
}
