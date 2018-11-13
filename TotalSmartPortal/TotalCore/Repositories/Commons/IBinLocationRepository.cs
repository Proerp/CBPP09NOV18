using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface IBinLocationRepository : IGenericRepository<BinLocation>
    {
    }

    public interface IBinLocationAPIRepository : IGenericAPIRepository
    {
        IList<BinLocationBase> GetBinLocationBases(int? warehouseID, string searchText);
    }
}