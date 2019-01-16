using TotalModel;
using TotalDTO;
using TotalModel.Models;
using TotalDTO.Inventories;

namespace TotalCore.Services.Inventories
{
    public interface IMaterialIssueService<TDto, TPrimitiveDto, TDtoDetail> : IGenericWithViewDetailService<MaterialIssue, MaterialIssueDetail, MaterialIssueViewDetail, TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
    }

    public interface IMaterialStagingService : IMaterialIssueService<MaterialIssueDTO<MIOptionMaterial>, MaterialIssuePrimitiveDTO<MIOptionMaterial>, MaterialIssueDetailDTO>
    { }
    public interface IItemStagingService : IMaterialIssueService<MaterialIssueDTO<MIOptionItem>, MaterialIssuePrimitiveDTO<MIOptionItem>, MaterialIssueDetailDTO>
    { }
    public interface IProductStagingService : IMaterialIssueService<MaterialIssueDTO<MIOptionProduct>, MaterialIssuePrimitiveDTO<MIOptionProduct>, MaterialIssueDetailDTO>
    { } 
}
