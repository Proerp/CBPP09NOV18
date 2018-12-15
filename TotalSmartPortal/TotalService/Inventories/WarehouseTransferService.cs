using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel;
using TotalDTO;
using TotalModel.Models;
using TotalDTO.Inventories;
using TotalCore.Repositories.Inventories;
using TotalCore.Services.Inventories;
using TotalDAL.Repositories.Inventories;
using TotalBase.Enums;

namespace TotalService.Inventories
{
    public class WarehouseTransferService<TDto, TPrimitiveDto, TDtoDetail> : GenericWithViewDetailService<WarehouseTransfer, WarehouseTransferDetail, WarehouseTransferViewDetail, TDto, TPrimitiveDto, TDtoDetail>, IWarehouseTransferService<TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>, IWarehouseTransferDTO
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
        public WarehouseTransferService(IWarehouseTransferRepository warehouseTransferRepository)
            : base(warehouseTransferRepository, "WarehouseTransferPostSaveValidate", "WarehouseTransferSaveRelative", "WarehouseTransferToggleApproved", "WarehouseTransferToggleVoid", "WarehouseTransferToggleVoidDetail", "GetWarehouseTransferViewDetails")
        {
        }

        public override ICollection<WarehouseTransferViewDetail> GetViewDetails(int warehouseTransferID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("WarehouseTransferID", warehouseTransferID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(TDto dto)
        {
            dto.WarehouseTransferViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(dto);
        }


        protected override void SaveRelative(WarehouseTransfer warehouseTransfer, SaveRelativeOption saveRelativeOption)
        {
            base.SaveRelative(warehouseTransfer, saveRelativeOption);

            if (warehouseTransfer.OneStep)
            {
                GRHelperService grHelperService = new GRHelperService(this.GetGROption(warehouseTransfer.NMVNTaskID), this.GenericWithDetailRepository.TotalSmartPortalEntities, this.UserID);

                IGoodsReceiptAPIRepository goodsReceiptAPIRepository = new GoodsReceiptAPIRepository(this.GenericWithDetailRepository.TotalSmartPortalEntities);
                if (saveRelativeOption == SaveRelativeOption.Update)
                {
                    IGoodsReceiptDTO goodsReceiptDTO = grHelperService.NewGoodsReceiptDTO();

                    goodsReceiptDTO.EntryDate = warehouseTransfer.EntryDate;

                    goodsReceiptDTO.GoodsReceiptTypeID = (int)GlobalEnums.GoodsReceiptTypeID.WarehouseTransfer;
                    goodsReceiptDTO.WarehouseTransferID = warehouseTransfer.WarehouseTransferID;
                    goodsReceiptDTO.OneStep = warehouseTransfer.OneStep;

                    goodsReceiptDTO.Warehouse = new TotalDTO.Commons.WarehouseBaseDTO() { WarehouseID = warehouseTransfer.WarehouseReceiptID };
                    goodsReceiptDTO.WarehouseIssue = new TotalDTO.Commons.WarehouseBaseDTO() { WarehouseID = warehouseTransfer.WarehouseID };

                    goodsReceiptDTO.StorekeeperID = warehouseTransfer.StorekeeperID;
                    goodsReceiptDTO.PreparedPersonID = warehouseTransfer.PreparedPersonID;
                    goodsReceiptDTO.ApproverID = warehouseTransfer.PreparedPersonID;

                    goodsReceiptDTO.Purposes = warehouseTransfer.Caption;
                    goodsReceiptDTO.Description = warehouseTransfer.Description;
                    goodsReceiptDTO.Remarks = warehouseTransfer.Remarks;

                    goodsReceiptDTO.Approved = warehouseTransfer.Approved;
                    goodsReceiptDTO.ApprovedDate = warehouseTransfer.ApprovedDate;

                    List<GoodsReceiptPendingWarehouseTransferDetail> pendingWarehouseTransferDetails = goodsReceiptAPIRepository.GetPendingWarehouseTransferDetails((int)goodsReceiptDTO.NMVNTaskID, null, goodsReceiptDTO.WarehouseTransferID, goodsReceiptDTO.WarehouseID, goodsReceiptDTO.WarehouseIssueID, null, goodsReceiptDTO.OneStep);
                    foreach (GoodsReceiptPendingWarehouseTransferDetail pendingWarehouseTransferDetail in pendingWarehouseTransferDetails)
                    {
                        GoodsReceiptDetailDTO goodsReceiptDetailDTO = new GoodsReceiptDetailDTO()
                        {
                            GoodsReceiptID = goodsReceiptDTO.GoodsReceiptID,

                            WarehouseTransferID = pendingWarehouseTransferDetail.WarehouseTransferID,
                            WarehouseTransferDetailID = pendingWarehouseTransferDetail.WarehouseTransferDetailID,
                            WarehouseTransferReference = pendingWarehouseTransferDetail.WarehouseTransferReference,
                            WarehouseTransferEntryDate = pendingWarehouseTransferDetail.WarehouseTransferEntryDate,
                            GoodsReceiptReference = pendingWarehouseTransferDetail.GoodsReceiptReference,
                            GoodsReceiptEntryDate = pendingWarehouseTransferDetail.GoodsReceiptEntryDate,
                            BatchEntryDate = pendingWarehouseTransferDetail.BatchEntryDate,


                            //BatchID = pendingWarehouseTransferDetail.BatchID,

                            CommodityID = pendingWarehouseTransferDetail.CommodityID,
                            CommodityCode = pendingWarehouseTransferDetail.CommodityCode,
                            CommodityName = pendingWarehouseTransferDetail.CommodityName,
                            CommodityTypeID = pendingWarehouseTransferDetail.CommodityTypeID,

                            Barcode = pendingWarehouseTransferDetail.Barcode,
                            BatchCode = pendingWarehouseTransferDetail.BatchCode,
                            SealCode = pendingWarehouseTransferDetail.SealCode,
                            LabCode = pendingWarehouseTransferDetail.LabCode,

                            BinLocationID = pendingWarehouseTransferDetail.BinLocationID,
                            BinLocationCode = pendingWarehouseTransferDetail.BinLocationCode,

                            QuantityRemains = (decimal)pendingWarehouseTransferDetail.QuantityRemains,
                            Quantity = (decimal)pendingWarehouseTransferDetail.QuantityRemains,
                        };
                        goodsReceiptDTO.ViewDetails.Add(goodsReceiptDetailDTO);
                    }

                    goodsReceiptDTO.TotalQuantity = goodsReceiptDTO.GetTotalQuantity();

                    grHelperService.Save(goodsReceiptDTO);
                }

                if (saveRelativeOption == SaveRelativeOption.Undo)
                {//NOTES: THIS UNDO REQUIRE: JUST SAVE ONLY ONE GoodsReceipt FOR AN WarehouseTransfer
                    int? goodsReceiptID = goodsReceiptAPIRepository.GetGoodsReceiptID(null, null, warehouseTransfer.WarehouseTransferID, null);
                    grHelperService.Delete(goodsReceiptID);
                }
            }
        }


        #region Helper for save or delete GoodsReceipt
        private GlobalEnums.GROption GetGROption(int nmvnTaskID)
        {
            if (nmvnTaskID == (int)GlobalEnums.NmvnTaskID.MaterialTransfer)
                return GlobalEnums.GROption.IsMaterial;
            else
                if (nmvnTaskID == (int)GlobalEnums.NmvnTaskID.ItemTransfer)
                    return GlobalEnums.GROption.IsItem;
                else
                    if (nmvnTaskID == (int)GlobalEnums.NmvnTaskID.ProductTransfer)
                        return GlobalEnums.GROption.IsProduct;
                    else
                        return GlobalEnums.GROption.Unknown;
        }
        #endregion Helper for save or delete GoodsReceipt

    }




    public class MaterialTransferService : WarehouseTransferService<WarehouseTransferDTO<WTOptionMaterial>, WarehouseTransferPrimitiveDTO<WTOptionMaterial>, WarehouseTransferDetailDTO>, IMaterialTransferService
    {
        public MaterialTransferService(IWarehouseTransferRepository warehouseTransferRepository)
            : base(warehouseTransferRepository) { }
    }
    public class ItemTransferService : WarehouseTransferService<WarehouseTransferDTO<WTOptionItem>, WarehouseTransferPrimitiveDTO<WTOptionItem>, WarehouseTransferDetailDTO>, IItemTransferService
    {
        public ItemTransferService(IWarehouseTransferRepository warehouseTransferRepository)
            : base(warehouseTransferRepository) { }
    }
    public class ProductTransferService : WarehouseTransferService<WarehouseTransferDTO<WTOptionProduct>, WarehouseTransferPrimitiveDTO<WTOptionProduct>, WarehouseTransferDetailDTO>, IProductTransferService
    {
        public ProductTransferService(IWarehouseTransferRepository warehouseTransferRepository)
            : base(warehouseTransferRepository) { }
    }
}
