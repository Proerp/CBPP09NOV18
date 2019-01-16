using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel;
using TotalDTO;
using TotalModel.Models;
using TotalDTO.Inventories;
using TotalCore.Repositories.Inventories;
using TotalCore.Services.Inventories;

namespace TotalService.Inventories
{
    public class MaterialIssueService<TDto, TPrimitiveDto, TDtoDetail> : GenericWithViewDetailService<MaterialIssue, MaterialIssueDetail, MaterialIssueViewDetail, TDto, TPrimitiveDto, TDtoDetail>, IMaterialIssueService<TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>, IMaterialIssueDTO
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
        public MaterialIssueService(IMaterialIssueRepository materialIssueRepository)
            : base(materialIssueRepository, "MaterialIssuePostSaveValidate", "MaterialIssueSaveRelative", "MaterialIssueToggleApproved", null, null, "GetMaterialIssueViewDetails")
        {
        }

        public override ICollection<MaterialIssueViewDetail> GetViewDetails(int materialIssueID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("MaterialIssueID", materialIssueID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(TDto dto)
        {
            dto.MaterialIssueViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(dto);
        }
    }



    public class MaterialStagingService : MaterialIssueService<MaterialIssueDTO<MIOptionMaterial>, MaterialIssuePrimitiveDTO<MIOptionMaterial>, MaterialIssueDetailDTO>, IMaterialStagingService
    {
        public MaterialStagingService(IMaterialIssueRepository materialIssueRepository)
            : base(materialIssueRepository) { }
    }
    public class ItemStagingService : MaterialIssueService<MaterialIssueDTO<MIOptionItem>, MaterialIssuePrimitiveDTO<MIOptionItem>, MaterialIssueDetailDTO>, IItemStagingService
    {
        public ItemStagingService(IMaterialIssueRepository materialIssueRepository)
            : base(materialIssueRepository) { }
    }
    public class ProductStagingService : MaterialIssueService<MaterialIssueDTO<MIOptionProduct>, MaterialIssuePrimitiveDTO<MIOptionProduct>, MaterialIssueDetailDTO>, IProductStagingService
    {
        public ProductStagingService(IMaterialIssueRepository materialIssueRepository)
            : base(materialIssueRepository) { }
    }
}