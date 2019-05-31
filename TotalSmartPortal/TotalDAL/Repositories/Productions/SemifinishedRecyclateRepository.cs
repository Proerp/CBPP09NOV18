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
    public class SemifinishedRecyclateRepository : GenericWithDetailRepository<SemifinishedRecyclate, SemifinishedRecyclateDetail>, ISemifinishedRecyclateRepository
    {
        public SemifinishedRecyclateRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "SemifinishedRecyclateEditable", "SemifinishedRecyclateApproved")
        {
        }
    }

    public class SemifinishedRecyclateAPIRepository : GenericAPIRepository, ISemifinishedRecyclateAPIRepository
    {
        public SemifinishedRecyclateAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetSemifinishedRecyclateIndexes")
        {
        }

        public IEnumerable<SemifinishedRecyclatePendingWorkshift> GetPendingWorkshifts(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<SemifinishedRecyclatePendingWorkshift> pendingWorkshifts = base.TotalSmartPortalEntities.GetSemifinishedRecyclatePendingWorkshifts(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingWorkshifts;
        }
    }
}
