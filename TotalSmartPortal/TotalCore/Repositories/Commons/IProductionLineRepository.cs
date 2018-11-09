using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface IProductionLineRepository : IGenericRepository<ProductionLine>
    {
    }

    public interface IProductionLineAPIRepository : IGenericAPIRepository
    {
        IList<ProductionLineBase> GetProductionLineBases(string searchText);
    }
}