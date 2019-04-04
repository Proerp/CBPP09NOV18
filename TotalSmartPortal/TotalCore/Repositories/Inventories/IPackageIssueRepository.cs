using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Inventories
{
    public interface IPackageIssueRepository : IGenericWithDetailRepository<PackageIssue, PackageIssueDetail>
    {        
        int? SavePackageIssueImage(string base64Image);
    }

    public interface IPackageIssueAPIRepository : IGenericAPIRepository
    {
        string GetReference(int packageIssueID);
        string GetPackageIssueImage(int packageIssueImageID);

        IEnumerable<PackageIssuePendingBlendingInstruction> GetBlendingInstructions(int? locationID, int? blendingInstructionID);

        IEnumerable<PackageIssuePendingBlendingInstructionDetail> GetPendingBlendingInstructionDetails(bool webAPI, int? locationID, int? materialIssueID, int? blendingInstructionID, int? warehouseID, string barcode, string goodsReceiptDetailIDs);
    }

}