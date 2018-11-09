using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface ICommodityClassRepository
    {
        IList<CommodityClass> GetAllCommodityClasses();
    }
}
