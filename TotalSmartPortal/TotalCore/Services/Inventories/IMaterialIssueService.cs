using TotalModel.Models;
using TotalDTO.Inventories;

namespace TotalCore.Services.Inventories
{
    public interface IMaterialIssueService : IGenericWithViewDetailService<MaterialIssue, MaterialIssueDetail, MaterialIssueViewDetail, MaterialIssueDTO, MaterialIssuePrimitiveDTO, MaterialIssueDetailDTO>
    {
    }
}
