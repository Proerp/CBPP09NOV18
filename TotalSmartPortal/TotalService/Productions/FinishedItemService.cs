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
    public class FinishedItemService : GenericWithViewDetailService<FinishedItem, FinishedItemDetail, FinishedItemViewDetail, FinishedItemDTO, FinishedItemPrimitiveDTO, FinishedItemDetailDTO>, IFinishedItemService
    {
        private IFinishedItemRepository finishedItemRepository;
        public FinishedItemService(IFinishedItemRepository finishedItemRepository)
            : base(finishedItemRepository, "FinishedItemPostSaveValidate", "FinishedItemSaveRelative", "FinishedItemToggleApproved", null, null, null, "GetFinishedItemViewDetails")
        {
            this.finishedItemRepository = finishedItemRepository;
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

        public List<FinishedItemViewLot> GetFinishedItemViewLots(int? finishedItemID)
        {
            return this.finishedItemRepository.GetFinishedItemViewLots(finishedItemID);
        }

        public override bool Save(FinishedItemDTO finishedItemDTO)
        {
            finishedItemDTO.FinishedItemViewDetails.RemoveAll(x => (x.Quantity == 0 && x.QuantityFailure == 0 && x.QuantityExcess == 0 && x.QuantityShortage == 0 && x.Swarfs == 0));
            return base.Save(finishedItemDTO);
        }

        protected override void UpdateDetail(FinishedItemDTO dto, FinishedItem entity)
        {
            base.UpdateDetail(dto, entity);

            if (dto.FinishedItemLots != null && dto.FinishedItemLots.Count > 0)
                dto.FinishedItemLots.Each(detailDTO =>
                {
                    FinishedItemLot finishedItemLot;

                    if (detailDTO.FinishedItemLotID <= 0 || (finishedItemLot = entity.FinishedItemLots.First(detailModel => detailModel.FinishedItemLotID == detailDTO.FinishedItemLotID)) == null)
                    {
                        finishedItemLot = new FinishedItemLot();
                        entity.FinishedItemLots.Add(finishedItemLot);
                    }

                    Mapper.Map<FinishedItemLotDTO, FinishedItemLot>(detailDTO, finishedItemLot);
                });
        }

        protected override void UndoDetail(FinishedItemDTO dto, FinishedItem entity, bool isDelete)
        {
            base.UndoDetail(dto, entity, isDelete);

            if (entity.GetID() > 0 && entity.FinishedItemLots.Count > 0)
                if (isDelete || dto.FinishedItemLots == null || dto.FinishedItemLots.Count == 0)
                    this.finishedItemRepository.TotalSmartPortalEntities.FinishedItemLots.RemoveRange(entity.FinishedItemLots);
                else
                    entity.FinishedItemLots.ToList()//Have to use .ToList(): to convert enumerable to List before do remove. To correct this error: Collection was modified; enumeration operation may not execute. 
                            .Where(detailModel => !dto.FinishedItemLots.Any(detailDTO => detailDTO.FinishedItemLotID == detailModel.FinishedItemLotID))
                            .Each(deleted => this.finishedItemRepository.TotalSmartPortalEntities.FinishedItemLots.Remove(deleted)); //remove deleted details
        }
    }
}
