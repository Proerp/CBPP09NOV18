using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Productions;

namespace TotalDAL.Repositories.Productions
{
    public class RecyclateRepository : GenericWithDetailRepository<Recyclate, RecyclateDetail>, IRecyclateRepository
    {
        public RecyclateRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "RecyclateEditable", "RecyclateApproved")
        {
        }

        public List<RecyclateViewPackage> GetRecyclateViewPackages(int? nmvnTaskID, int? recyclateID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            List<RecyclateViewPackage> recyclateViewLots = base.TotalSmartPortalEntities.GetRecyclateViewPackages(nmvnTaskID, recyclateID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return recyclateViewLots;
        }
    }

    public class RecyclateAPIRepository : GenericAPIRepository, IRecyclateAPIRepository
    {
        public RecyclateAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetRecyclateIndexes")
        {
        }

        public IEnumerable<RecyclatePendingWorkshift> GetPendingWorkshifts(int? nmvnTaskID, int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<RecyclatePendingWorkshift> pendingWorkshifts = base.TotalSmartPortalEntities.GetRecyclatePendingWorkshifts(nmvnTaskID, locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingWorkshifts;
        }
    }
}
