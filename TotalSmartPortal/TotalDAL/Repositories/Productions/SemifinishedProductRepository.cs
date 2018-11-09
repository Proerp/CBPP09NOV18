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
    public class SemifinishedProductRepository : GenericWithDetailRepository<SemifinishedProduct, SemifinishedProductDetail>, ISemifinishedProductRepository
    {
        public SemifinishedProductRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "SemifinishedProductEditable", "SemifinishedProductApproved")
        {
        }

        public decimal GetMaterialQuantityRemains(int? materialIssueDetailID)
        {
            decimal? materialQuantityRemains = base.TotalSmartPortalEntities.GetSemifinishedProductPendingMaterialQuantityRemains(materialIssueDetailID).FirstOrDefault();
            return materialQuantityRemains == null ? 0 : (decimal)materialQuantityRemains;
        }
    }

    public class SemifinishedProductAPIRepository : GenericAPIRepository, ISemifinishedProductAPIRepository
    {
        public SemifinishedProductAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetSemifinishedProductIndexes")
        {
        }

        public IEnumerable<SemifinishedProductPendingMaterialIssueDetail> GetMaterialIssueDetails(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<SemifinishedProductPendingMaterialIssueDetail> pendingMaterialIssueDetail = base.TotalSmartPortalEntities.GetSemifinishedProductPendingMaterialIssueDetails(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingMaterialIssueDetail;
        }

    }
}
