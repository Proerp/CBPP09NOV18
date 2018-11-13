using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class BinLocationRepository : GenericRepository<BinLocation>, IBinLocationRepository
    {
        public BinLocationRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities)
        {
        }
    }



    public class BinLocationAPIRepository : GenericAPIRepository, IBinLocationAPIRepository
    {
        public BinLocationAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetBinLocationIndexes")
        {
        }

        public IList<BinLocationBase> GetBinLocationBases(int? warehouseID, string searchText)
        {
            List<BinLocationBase> binLocationBases = this.TotalSmartPortalEntities.GetBinLocationBases(warehouseID, searchText).ToList();

            return binLocationBases;
        }
    }

}