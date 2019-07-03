using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase;
using TotalModel.Models;
using TotalDTO.Inventories;
using TotalCore.Repositories.Inventories;
using TotalCore.Services.Inventories;

namespace TotalService.Inventories
{
    public class PackageIssueService : GenericWithViewDetailService<PackageIssue, PackageIssueDetail, PackageIssueViewDetail, PackageIssueDTO, PackageIssuePrimitiveDTO, PackageIssueDetailDTO>, IPackageIssueService
    {
        private IPackageIssueRepository packageIssueRepository;
        public PackageIssueService(IPackageIssueRepository packageIssueRepository)
            : base(packageIssueRepository, "PackageIssuePostSaveValidate", "PackageIssueSaveRelative", "PackageIssueToggleApproved", null, null, null, "GetPackageIssueViewDetails")
        {
            this.packageIssueRepository = packageIssueRepository;
        }

        public override ICollection<PackageIssueViewDetail> GetViewDetails(int packageIssueID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("PackageIssueID", packageIssueID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(PackageIssueDTO packageIssueDTO)
        {
            packageIssueDTO.ViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(packageIssueDTO);
        }

        protected override void UpdateDetail(PackageIssueDTO dto, PackageIssue entity)
        {
            if (dto.GetDetails() != null && dto.GetDetails().Count > 0)
                dto.GetDetails().Each(detailDTO =>
                {
                    if (detailDTO.Base64Image1 != null)
                        detailDTO.PackageIssueImage1ID = this.packageIssueRepository.SavePackageIssueImage(detailDTO.Base64Image1);
                    if (detailDTO.Base64Image2 != null)
                        detailDTO.PackageIssueImage2ID = this.packageIssueRepository.SavePackageIssueImage(detailDTO.Base64Image2);
                });

            base.UpdateDetail(dto, entity);
        }
    }
}