using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface IMoldRepository : IGenericRepository<Mold>
    {
    }

    public interface IMoldAPIRepository : IGenericAPIRepository
    {
        IList<MoldBase> GetMoldBases(string searchText, int commodityID);

        IList<CommodityMold> GetCommodityMolds(int commodityID);

        void AddCommodityMold(int? moldID, int? commodityID, decimal quantity);

        void RemoveCommodityMold(int? commodityMoldID);

        void UpdateCommodityMold(int? commodityMoldID, int commodityID, decimal Quantity, string remarks, bool? isDefault);
    }
}