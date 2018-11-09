using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class ProductionLineRepository : GenericRepository<ProductionLine>, IProductionLineRepository
    {
        public ProductionLineRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities)
        {
        }
    }



    public class ProductionLineAPIRepository : GenericAPIRepository, IProductionLineAPIRepository
    {
        public ProductionLineAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetProductionLineIndexes")
        {
        }

        public IList<ProductionLineBase> GetProductionLineBases(string searchText)
        {
            List<ProductionLineBase> productionLineBases = this.TotalSmartPortalEntities.GetProductionLineBases(searchText).ToList();

            return productionLineBases;
        }
    }

}