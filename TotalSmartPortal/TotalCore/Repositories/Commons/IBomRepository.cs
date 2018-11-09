using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface IBomRepository : IGenericRepository<Bom>
    {
    }

    public interface IBomAPIRepository : IGenericAPIRepository
    {
        IList<BomBase> GetBomBases(string searchText, int commodityID, int commodityCategoryID, int commodityClassID, int commodityLineID);

        IList<CommodityBom> GetCommodityBoms(int commodityID);

        void AddCommodityBom(int? bomID, int? commodityID);

        void RemoveCommodityBom(int? commodityBomID);

        void UpdateCommodityBom(int? commodityBomID, int commodityID, decimal blockUnit, decimal blockQuantity, string remarks, bool? isDefault);
    }
}