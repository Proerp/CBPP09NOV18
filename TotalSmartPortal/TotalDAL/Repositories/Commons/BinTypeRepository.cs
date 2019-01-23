using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;
namespace TotalDAL.Repositories.Commons
{
    public class BinTypeRepository : IBinTypeRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public BinTypeRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public IList<BinType> GetAllBinTypes()
        {
            return this.totalSmartPortalEntities.BinTypes.ToList();
        }
    }
}
