using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Productions;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;

namespace TotalService.Productions
{
    public class FinishedItemService : GenericWithViewDetailService<FinishedItem, FinishedItemDetail, FinishedItemViewDetail, FinishedItemDTO, FinishedItemPrimitiveDTO, FinishedItemDetailDTO>, IFinishedItemService
    {
        public FinishedItemService(IFinishedItemRepository finishedItemRepository)
            : base(finishedItemRepository, "FinishedItemPostSaveValidate", "FinishedItemSaveRelative", "FinishedItemToggleApproved", null, null, "GetFinishedItemViewDetails")
        {
        }

        public override ICollection<FinishedItemViewDetail> GetViewDetails(int finishedItemID)
        {
            throw new System.ArgumentException("Invalid call GetViewDetails(id). Use GetFinishedItemViewDetails instead.", "FinishItem Service");
        }

        public ICollection<FinishedItemViewDetail> GetFinishedItemViewDetails(int finishedItemID, int locationID, int firmOrderID, bool isReadonly)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("FinishedItemID", finishedItemID), new ObjectParameter("LocationID", locationID), new ObjectParameter("FirmOrderID", firmOrderID), new ObjectParameter("IsReadonly", isReadonly) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(FinishedItemDTO finishedItemDTO)
        {
            finishedItemDTO.FinishedItemViewDetails.RemoveAll(x => (x.Quantity == 0 && x.QuantityFailure == 0 && x.QuantityExcess == 0 && x.QuantityShortage == 0 && x.Swarfs == 0));
            return base.Save(finishedItemDTO);
        }
    }
}
