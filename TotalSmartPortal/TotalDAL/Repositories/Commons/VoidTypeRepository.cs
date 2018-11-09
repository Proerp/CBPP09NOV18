using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class VoidTypeRepository : GenericRepository<VoidType>, IVoidTypeRepository
    {
        public VoidTypeRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities)
        {
        }

        public IList<VoidType> SearchVoidTypes(string searchText)
        {
            
                this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
                List<VoidType> goodsIssueTypes = this.TotalSmartPortalEntities.VoidTypes.Where(w => (w.Code.Contains(searchText) || w.Name.Contains(searchText))).OrderByDescending(or => or.Name).Take(3).ToList();
                this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

                return goodsIssueTypes;
            
        }
    }
}