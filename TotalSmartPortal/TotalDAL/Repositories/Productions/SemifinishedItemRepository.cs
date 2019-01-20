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
    public class SemifinishedItemRepository : GenericWithDetailRepository<SemifinishedItem, SemifinishedItemDetail>, ISemifinishedItemRepository
    {
        public SemifinishedItemRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "SemifinishedItemEditable", "SemifinishedItemApproved")
        {
        }
    }

    public class SemifinishedItemAPIRepository : GenericAPIRepository, ISemifinishedItemAPIRepository
    {
        public SemifinishedItemAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetSemifinishedItemIndexes")
        {
        }

        public IEnumerable<SemifinishedItemPendingMaterialIssue> GetMaterialIssues(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<SemifinishedItemPendingMaterialIssue> pendingMaterialIssue = base.TotalSmartPortalEntities.GetSemifinishedItemPendingMaterialIssues(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingMaterialIssue;
        }

    }
}
