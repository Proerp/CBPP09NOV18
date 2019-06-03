using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{
    public interface IRecyclateRepository : IGenericWithDetailRepository<Recyclate, RecyclateDetail>
    {
        List<RecyclateViewPackage> GetRecyclateViewPackages(int? nmvnTaskID, int? recyclateID);
    }

    public interface IRecyclateAPIRepository : IGenericAPIRepository
    {
        IEnumerable<RecyclatePendingWorkshift> GetPendingWorkshifts(int? nmvnTaskID, int? locationID);
    }
}
