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

        public string GetReference(int packageIssueID)
        {
            return base.TotalSmartPortalEntities.PackageIssueGetReference(packageIssueID).FirstOrDefault();
        }

        public IEnumerable<PackageIssuePendingBlendingInstruction> GetBlendingInstructions(int? locationID, int? blendingInstructionID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PackageIssuePendingBlendingInstruction> pendingBlendingInstructions = base.TotalSmartPortalEntities.GetPackageIssuePendingBlendingInstructions(locationID, blendingInstructionID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingBlendingInstructions;
        }

        public IEnumerable<PackageIssuePendingBlendingInstructionDetail> GetPendingBlendingInstructionDetails(bool webAPI, int? locationID, int? packageIssueID, int? blendingInstructionID, int? warehouseID, string barcode, string goodsReceiptDetailIDs)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PackageIssuePendingBlendingInstructionDetail> pendingBlendingInstructionDetails = base.TotalSmartPortalEntities.GetPackageIssuePendingBlendingInstructionDetails(webAPI, locationID, packageIssueID, blendingInstructionID, warehouseID, barcode, goodsReceiptDetailIDs).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingBlendingInstructionDetails;
        }

    }


}
