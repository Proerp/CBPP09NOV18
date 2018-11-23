using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Inventories;
using TotalCore.Repositories.Inventories;
using TotalCore.Services.Inventories;

namespace TotalService.Inventories
{
    public class PackageIssueService : GenericWithViewDetailService<PackageIssue, PackageIssueDetail, PackageIssueViewDetail, PackageIssueDTO, PackageIssuePrimitiveDTO, PackageIssueDetailDTO>, IPackageIssueService
    {
        public PackageIssueService(IPackageIssueRepository packageIssueRepository)
            : base(packageIssueRepository, "PackageIssuePostSaveValidate", "PackageIssueSaveRelative", "PackageIssueToggleApproved", null, null, "GetPackageIssueViewDetails")
        {
        }

        public override ICollection<PackageIssueViewDetail> GetViewDetails(int packageIssueID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("PackageIssueID", packageIssueID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(PackageIssueDTO packageIssueDTO)
        {
            packageIssueDTO.PackageIssueViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(packageIssueDTO);
        }
    }
}