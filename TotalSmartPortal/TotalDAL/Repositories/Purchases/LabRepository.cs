using TotalModel.Models;
using TotalCore.Repositories.Purchases;

namespace TotalDAL.Repositories.Purchases
{
    public class LabRepository : GenericRepository<Lab>, ILabRepository
    {
        public LabRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "LabEditable", null, "LabDeletable")
        {
        }
    }


    public class LabAPIRepository : GenericAPIRepository, ILabAPIRepository
    {
        public LabAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetLabIndexes")
        {
        }
    }
}