using TotalModel.Models;
using TotalCore.Repositories.Purchases;

namespace TotalDAL.Repositories.Purchases
{
    public class LabRepository : GenericRepository<Lab>, ILabRepository
    {
        public LabRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "LabEditable", "LabApproved", "LabDeletable", "LabVoidable")
        {
        }

        public bool ToggleHold(int entityID, bool hold)
        {
            return this.TotalSmartPortalEntities.LabToggleHold(entityID, hold) == 1;
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