using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{

    public interface ICommodityCategoryRepository 
    {
        IList<CommodityCategory> GetAllCommodityCategories(string commodityTypeIDs);
    }
  
}
