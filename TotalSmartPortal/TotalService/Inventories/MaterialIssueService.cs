using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Inventories;
using TotalCore.Repositories.Inventories;
using TotalCore.Services.Inventories;

namespace TotalService.Inventories
{
    public class MaterialIssueService : GenericWithViewDetailService<MaterialIssue, MaterialIssueDetail, MaterialIssueViewDetail, MaterialIssueDTO, MaterialIssuePrimitiveDTO, MaterialIssueDetailDTO>, IMaterialIssueService
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

        public override bool Save(MaterialIssueDTO materialIssueDTO)
        {
            materialIssueDTO.MaterialIssueViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(materialIssueDTO);
        }
    }
}