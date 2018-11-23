using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Inventories
{
    public interface IPackageIssueRepository : IGenericWithDetailRepository<PackageIssue, PackageIssueDetail>
    {
    }

    public interface IPackageIssueAPIRepository : IGenericAPIRepository
    {
        IEnumerable<PackageIssuePendingBlendingInstruction> GetBlendingInstructions(int? locationID, int? blendingInstructionID);

        IEnumerable<PackageIssuePendingBlendingInstructionDetail> GetPendingBlendingInstructionDetails(int? locationID, int? materialIssueID, int? blendingInstructionID, int? warehouseID, string goodsReceiptDetailIDs);
    }

}