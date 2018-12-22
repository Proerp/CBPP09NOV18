using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Inventories;

namespace TotalDAL.Repositories.Inventories
{
    public class PackageIssueRepository : GenericWithDetailRepository<PackageIssue, PackageIssueDetail>, IPackageIssueRepository
    {
        public PackageIssueRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "PackageIssueEditable", "PackageIssueApproved")
        {
        }
    }








    public class PackageIssueAPIRepository : GenericAPIRepository, IPackageIssueAPIRepository
    {
        public PackageIssueAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetPackageIssueIndexes")
        {
        }

        public IEnumerable<PackageIssuePendingBlendingInstruction> GetBlendingInstructions(int? locationID, int? blendingInstructionID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PackageIssuePendingBlendingInstruction> pendingBlendingInstructions = base.TotalSmartPortalEntities.GetPackageIssuePendingBlendingInstructions(locationID, blendingInstructionID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingBlendingInstructions;
        }

        public IEnumerable<PackageIssuePendingBlendingInstructionDetail> GetPendingBlendingInstructionDetails(int? locationID, int? packageIssueID, int? blendingInstructionID, int? warehouseID, string goodsReceiptDetailIDs, bool webAPI)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PackageIssuePendingBlendingInstructionDetail> pendingBlendingInstructionDetails = base.TotalSmartPortalEntities.GetPackageIssuePendingBlendingInstructionDetails(locationID, packageIssueID, blendingInstructionID, warehouseID, goodsReceiptDetailIDs, webAPI).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingBlendingInstructionDetails;
        }

    }


}
