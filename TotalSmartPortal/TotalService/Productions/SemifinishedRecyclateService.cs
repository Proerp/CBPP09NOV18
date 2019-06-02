using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using AutoMapper;

using TotalBase;
using TotalModel.Models;
using TotalDTO.Productions;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;

namespace TotalService.Productions
{
    public class SemifinishedRecyclateService : GenericWithViewDetailService<SemifinishedRecyclate, SemifinishedRecyclateDetail, SemifinishedRecyclateViewDetail, SemifinishedRecyclateDTO, SemifinishedRecyclatePrimitiveDTO, SemifinishedRecyclateDetailDTO>, ISemifinishedRecyclateService
    {
        private ISemifinishedRecyclateRepository semifinishedRecyclateRepository;
        public SemifinishedRecyclateService(ISemifinishedRecyclateRepository semifinishedRecyclateRepository)
            : base(semifinishedRecyclateRepository, "SemifinishedRecyclatePostSaveValidate", "SemifinishedRecyclateSaveRelative", "SemifinishedRecyclateToggleApproved", null, null, "GetSemifinishedRecyclateViewDetails")
        {
            this.semifinishedRecyclateRepository = semifinishedRecyclateRepository;
        }

        public override ICollection<SemifinishedRecyclateViewDetail> GetViewDetails(int semifinishedRecyclateID)
        {
            throw new System.ArgumentException("Invalid call GetViewDetails(id). Use GetSemifinishedRecyclateViewDetails instead.", "FinishProduct Service");
        }

        public ICollection<SemifinishedRecyclateViewDetail> GetSemifinishedRecyclateViewDetails(int semifinishedRecyclateID, int locationID, int workshiftID, bool isReadonly)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("SemifinishedRecyclateID", semifinishedRecyclateID), new ObjectParameter("LocationID", locationID), new ObjectParameter("WorkshiftID", workshiftID), new ObjectParameter("IsReadonly", isReadonly) };
            return this.GetViewDetails(parameters);
        }

        public List<SemifinishedRecyclateViewPackage> GetSemifinishedRecyclateViewPackages(int? semifinishedRecyclateID)
        {
            return this.semifinishedRecyclateRepository.GetSemifinishedRecyclateViewPackages(semifinishedRecyclateID);
        }

        protected override void UpdateDetail(SemifinishedRecyclateDTO dto, SemifinishedRecyclate entity)
        {
            base.UpdateDetail(dto, entity);

            if (dto.SemifinishedRecyclatePackages != null && dto.SemifinishedRecyclatePackages.Count > 0)
                dto.SemifinishedRecyclatePackages.Each(detailDTO =>
                {
                    SemifinishedRecyclatePackage semifinishedRecyclatePackage;

                    if (detailDTO.SemifinishedRecyclatePackageID <= 0 || (semifinishedRecyclatePackage = entity.SemifinishedRecyclatePackages.First(detailModel => detailModel.SemifinishedRecyclatePackageID == detailDTO.SemifinishedRecyclatePackageID)) == null)
                    {
                        semifinishedRecyclatePackage = new SemifinishedRecyclatePackage();
                        entity.SemifinishedRecyclatePackages.Add(semifinishedRecyclatePackage);
                    }

                    Mapper.Map<SemifinishedRecyclatePackageDTO, SemifinishedRecyclatePackage>(detailDTO, semifinishedRecyclatePackage);
                });
        }

        protected override void UndoDetail(SemifinishedRecyclateDTO dto, SemifinishedRecyclate entity, bool isDelete)
        {
            base.UndoDetail(dto, entity, isDelete);

            if (entity.GetID() > 0 && entity.SemifinishedRecyclatePackages.Count > 0)
                if (isDelete || dto.SemifinishedRecyclatePackages == null || dto.SemifinishedRecyclatePackages.Count == 0)
                    this.semifinishedRecyclateRepository.TotalSmartPortalEntities.SemifinishedRecyclatePackages.RemoveRange(entity.SemifinishedRecyclatePackages);
                else
                    entity.SemifinishedRecyclatePackages.ToList()//Have to use .ToList(): to convert enumerable to List before do remove. To correct this error: Collection was modified; enumeration operation may not execute. 
                            .Where(detailModel => !dto.SemifinishedRecyclatePackages.Any(detailDTO => detailDTO.SemifinishedRecyclatePackageID == detailModel.SemifinishedRecyclatePackageID))
                            .Each(deleted => this.semifinishedRecyclateRepository.TotalSmartPortalEntities.SemifinishedRecyclatePackages.Remove(deleted)); //remove deleted details
        }
    }
}
