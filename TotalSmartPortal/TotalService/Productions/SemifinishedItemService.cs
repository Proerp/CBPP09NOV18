using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Productions;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;

namespace TotalService.Productions
{
    public class SemifinishedItemService : GenericWithViewDetailService<SemifinishedItem, SemifinishedItemDetail, SemifinishedItemViewDetail, SemifinishedItemDTO, SemifinishedItemPrimitiveDTO, SemifinishedItemDetailDTO>, ISemifinishedItemService
    {
        ISemifinishedItemRepository semifinishedItemRepository;
        public SemifinishedItemService(ISemifinishedItemRepository semifinishedItemRepository)
            : base(semifinishedItemRepository, "SemifinishedItemPostSaveValidate", "SemifinishedItemSaveRelative", "SemifinishedItemToggleApproved", null, null, null, "GetSemifinishedItemViewDetails")
        {
            this.semifinishedItemRepository = semifinishedItemRepository;
        }

        public override ICollection<SemifinishedItemViewDetail> GetViewDetails(int semifinishedItemID)
        {
            throw new System.ArgumentException("Invalid call GetViewDetails(id). Use GetSemifinishedItemViewDetails instead.", "SemifinishProduct Service");
        }

        public override bool Save(SemifinishedItemDTO semifinishedItemDTO)
        {
            semifinishedItemDTO.SemifinishedItemViewDetails.RemoveAll(x => x.Quantity == 0 && x.QuantityFailure == 0);
            return base.Save(semifinishedItemDTO);
        }
        public ICollection<SemifinishedItemViewDetail> GetSemifinishedItemViewDetails(int semifinishedItemID, int firmOrderID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("SemifinishedItemID", semifinishedItemID), new ObjectParameter("FirmOrderID", firmOrderID) };
            return this.GetViewDetails(parameters);
        }
    }
}
