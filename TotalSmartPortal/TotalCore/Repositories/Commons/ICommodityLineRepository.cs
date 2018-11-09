using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface ICommodityLineRepository
    {
        IList<CommodityLine> GetAllCommodityLines();
    }
}
