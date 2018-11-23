using TotalModel.Models;
using TotalDTO.Inventories;

namespace TotalCore.Services.Inventories
{
    public interface IPackageIssueService : IGenericWithViewDetailService<PackageIssue, PackageIssueDetail, PackageIssueViewDetail, PackageIssueDTO, PackageIssuePrimitiveDTO, PackageIssueDetailDTO>
    {
    }
}
