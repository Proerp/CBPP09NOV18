using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface IBinTypeRepository
    {
        IList<BinType> GetAllBinTypes();
    }
}
