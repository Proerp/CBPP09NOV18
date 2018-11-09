using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Productions;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;

namespace TotalService.Productions
{
    public class SemifinishedProductService : GenericWithViewDetailService<SemifinishedProduct, SemifinishedProductDetail, SemifinishedProductViewDetail, SemifinishedProductDTO, SemifinishedProductPrimitiveDTO, SemifinishedProductDetailDTO>, ISemifinishedProductService
    {
        ISemifinishedProductRepository semifinishedProductRepository;
        public SemifinishedProductService(ISemifinishedProductRepository semifinishedProductRepository)
            : base(semifinishedProductRepository, "SemifinishedProductPostSaveValidate", "SemifinishedProductSaveRelative", "SemifinishedProductToggleApproved", null, null, "GetSemifinishedProductViewDetails")
        {
            this.semifinishedProductRepository = semifinishedProductRepository;
        }

        public override ICollection<SemifinishedProductViewDetail> GetViewDetails(int semifinishedProductID)
        {
            throw new System.ArgumentException("Invalid call GetViewDetails(id). Use GetSemifinishedProductViewDetails instead.", "SemifinishProduct Service");
        }

        public ICollection<SemifinishedProductViewDetail> GetSemifinishedProductViewDetails(int semifinishedProductID, int firmOrderID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("SemifinishedProductID", semifinishedProductID), new ObjectParameter("FirmOrderID", firmOrderID) };
            return this.GetViewDetails(parameters);
        }


        public decimal GetMaterialQuantityRemains(int? materialIssueDetailID) { return this.semifinishedProductRepository.GetMaterialQuantityRemains(materialIssueDetailID); }
    }
}
