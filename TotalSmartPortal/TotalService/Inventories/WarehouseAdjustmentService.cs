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
    public class WarehouseAdjustmentService<TDto, TPrimitiveDto, TDtoDetail> : GenericWithViewDetailService<WarehouseAdjustment, WarehouseAdjustmentDetail, WarehouseAdjustmentViewDetail, TDto, TPrimitiveDto, TDtoDetail>, IWarehouseAdjustmentService<TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>, IWarehouseAdjustmentDTO
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
        public WarehouseAdjustmentService(IWarehouseAdjustmentRepository warehouseAdjustmentRepository)
            : base(warehouseAdjustmentRepository, "WarehouseAdjustmentPostSaveValidate", "WarehouseAdjustmentSaveRelative", "WarehouseAdjustmentToggleApproved", null, null, "GetWarehouseAdjustmentViewDetails")
        {
        }

        public override ICollection<WarehouseAdjustmentViewDetail> GetViewDetails(int warehouseAdjustmentID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("WarehouseAdjustmentID", warehouseAdjustmentID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(TDto dto)
        {
            dto.WarehouseAdjustmentViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(dto);
        }

        protected override void SaveRelative(WarehouseAdjustment warehouseAdjustment, SaveRelativeOption saveRelativeOption)
        {
            base.SaveRelative(warehouseAdjustment, saveRelativeOption);

            if (warehouseAdjustment.HasPositiveLine)
            {
                GRHelperService grHelperService = new GRHelperService(this.GetGROption(warehouseAdjustment.NMVNTaskID), this.GenericWithDetailRepository.TotalSmartPortalEntities, this.UserID);

                IGoodsReceiptAPIRepository goodsReceiptAPIRepository = new GoodsReceiptAPIRepository(this.GenericWithDetailRepository.TotalSmartPortalEntities);
                if (saveRelativeOption == SaveRelativeOption.Update)
                {
                    IGoodsReceiptDTO goodsReceiptDTO = grHelperService.NewGoodsReceiptDTO();

                    goodsReceiptDTO.EntryDate = warehouseAdjustment.EntryDate;
                    goodsReceiptDTO.Warehouse = new TotalDTO.Commons.WarehouseBaseDTO() { WarehouseID = warehouseAdjustment.WarehouseReceiptID };

                    goodsReceiptDTO.WarehouseAdjustmentID = warehouseAdjustment.WarehouseAdjustmentID;

                    goodsReceiptDTO.GoodsReceiptTypeID = (int)GlobalEnums.GoodsReceiptTypeID.WarehouseAdjustments;

                    goodsReceiptDTO.StorekeeperID = warehouseAdjustment.StorekeeperID;
                    goodsReceiptDTO.PreparedPersonID = warehouseAdjustment.PreparedPersonID;
                    goodsReceiptDTO.ApproverID = warehouseAdjustment.PreparedPersonID;

                    goodsReceiptDTO.Customer = new TotalDTO.Commons.CustomerBaseDTO() { CustomerID = warehouseAdjustment.CustomerID };

                    goodsReceiptDTO.Purposes = warehouseAdjustment.AdjustmentJobs;
                    goodsReceiptDTO.Description = warehouseAdjustment.Description;
                    goodsReceiptDTO.Remarks = warehouseAdjustment.Remarks;

                    goodsReceiptDTO.Approved = warehouseAdjustment.Approved;
                    goodsReceiptDTO.ApprovedDate = warehouseAdjustment.ApprovedDate;

                    List<PendingWarehouseAdjustmentDetail> pendingWarehouseAdjustmentDetails = goodsReceiptAPIRepository.GetPendingWarehouseAdjustmentDetails(warehouseAdjustment.LocationID, null, warehouseAdjustment.WarehouseAdjustmentID, warehouseAdjustment.WarehouseReceiptID, null, false);
                    foreach (PendingWarehouseAdjustmentDetail pendingWarehouseAdjustmentDetail in pendingWarehouseAdjustmentDetails)
                    {
                        GoodsReceiptDetailDTO goodsReceiptDetailDTO = new GoodsReceiptDetailDTO()
                        {
                            GoodsReceiptID = goodsReceiptDTO.GoodsReceiptID,

                            WarehouseAdjustmentID = pendingWarehouseAdjustmentDetail.WarehouseAdjustmentID,
                            WarehouseAdjustmentDetailID = pendingWarehouseAdjustmentDetail.WarehouseAdjustmentDetailID,
                            WarehouseAdjustmentReference = pendingWarehouseAdjustmentDetail.PrimaryReference,
                            WarehouseAdjustmentEntryDate = pendingWarehouseAdjustmentDetail.PrimaryEntryDate,

                            WarehouseAdjustmentTypeID = pendingWarehouseAdjustmentDetail.WarehouseAdjustmentTypeID,

                            BatchID = pendingWarehouseAdjustmentDetail.BatchID,
                            BatchEntryDate = pendingWarehouseAdjustmentDetail.BatchEntryDate,

                            CommodityID = pendingWarehouseAdjustmentDetail.CommodityID,
                            CommodityCode = pendingWarehouseAdjustmentDetail.CommodityCode,
                            CommodityName = pendingWarehouseAdjustmentDetail.CommodityName,

                            QuantityRemains = (decimal)pendingWarehouseAdjustmentDetail.QuantityRemains,
                            Quantity = (decimal)pendingWarehouseAdjustmentDetail.QuantityRemains,
                        };
                        goodsReceiptDTO.ViewDetails.Add(goodsReceiptDetailDTO);
                    }

                    goodsReceiptDTO.TotalQuantity = goodsReceiptDTO.GetTotalQuantity();

                    grHelperService.Save(goodsReceiptDTO);
                }

                if (saveRelativeOption == SaveRelativeOption.Undo)
                {//NOTES: THIS UNDO REQUIRE: JUST SAVE ONLY ONE GoodsReceipt FOR AN WarehouseAdjustment
                    int? goodsReceiptID = goodsReceiptAPIRepository.GetGoodsReceiptIDofWarehouseAdjustment(warehouseAdjustment.WarehouseAdjustmentID);
                    if (goodsReceiptID != null)
                        grHelperService.Delete(goodsReceiptID);
                    else
                        throw new Exception("Lỗi không tìm thấy phiếu nhập kho cũ của phiếu điều chỉnh kho này!" + "\r\n" + "\r\n" + "Vui lòng kiểm tra lại dữ liệu trước khi tiếp tục.");
                }
            }
        }


        #region Helper for save or delete GoodsReceipt
        private GlobalEnums.GROption GetGROption(int nmvnTaskID)
        {
            if (nmvnTaskID == (int)GlobalEnums.NmvnTaskID.OtherMaterialIssue || nmvnTaskID == (int)GlobalEnums.NmvnTaskID.OtherMaterialReceipt || nmvnTaskID == (int)GlobalEnums.NmvnTaskID.MaterialAdjustment)
                return GlobalEnums.GROption.IsMaterial;
            else
                if (nmvnTaskID == (int)GlobalEnums.NmvnTaskID.OtherItemIssue || nmvnTaskID == (int)GlobalEnums.NmvnTaskID.OtherItemReceipt || nmvnTaskID == (int)GlobalEnums.NmvnTaskID.ItemAdjustment)
                    return GlobalEnums.GROption.IsItem;
                else
                    if (nmvnTaskID == (int)GlobalEnums.NmvnTaskID.OtherProductIssue || nmvnTaskID == (int)GlobalEnums.NmvnTaskID.OtherProductReceipt || nmvnTaskID == (int)GlobalEnums.NmvnTaskID.ProductAdjustment)
                        return GlobalEnums.GROption.IsProduct;
                    else
                        return GlobalEnums.GROption.Unknown;
        }
        #endregion Helper for save or delete GoodsReceipt

    }







    public class OtherMaterialReceiptService : WarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionMtlRct>, WarehouseAdjustmentPrimitiveDTO<WAOptionMtlRct>, WarehouseAdjustmentDetailDTO>, IOtherMaterialReceiptService
    {
        public OtherMaterialReceiptService(IWarehouseAdjustmentRepository warehouseAdjustmentRepository)
            : base(warehouseAdjustmentRepository) { }
    }
    public class OtherMaterialIssueService : WarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionMtlIss>, WarehouseAdjustmentPrimitiveDTO<WAOptionMtlIss>, WarehouseAdjustmentDetailDTO>, IOtherMaterialIssueService
    {
        public OtherMaterialIssueService(IWarehouseAdjustmentRepository warehouseAdjustmentRepository)
            : base(warehouseAdjustmentRepository) { }
    }
    public class MaterialAdjustmentService : WarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionMtlAdj>, WarehouseAdjustmentPrimitiveDTO<WAOptionMtlAdj>, WarehouseAdjustmentDetailDTO>, IMaterialAdjustmentService
    {
        public MaterialAdjustmentService(IWarehouseAdjustmentRepository warehouseAdjustmentRepository)
            : base(warehouseAdjustmentRepository) { }
    }

    public class OtherItemReceiptService : WarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionItmRct>, WarehouseAdjustmentPrimitiveDTO<WAOptionItmRct>, WarehouseAdjustmentDetailDTO>, IOtherItemReceiptService
    {
        public OtherItemReceiptService(IWarehouseAdjustmentRepository warehouseAdjustmentRepository)
            : base(warehouseAdjustmentRepository) { }
    }
    public class OtherItemIssueService : WarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionItmIss>, WarehouseAdjustmentPrimitiveDTO<WAOptionItmIss>, WarehouseAdjustmentDetailDTO>, IOtherItemIssueService
    {
        public OtherItemIssueService(IWarehouseAdjustmentRepository warehouseAdjustmentRepository)
            : base(warehouseAdjustmentRepository) { }
    }
    public class ItemAdjustmentService : WarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionItmAdj>, WarehouseAdjustmentPrimitiveDTO<WAOptionItmAdj>, WarehouseAdjustmentDetailDTO>, IItemAdjustmentService
    {
        public ItemAdjustmentService(IWarehouseAdjustmentRepository warehouseAdjustmentRepository)
            : base(warehouseAdjustmentRepository) { }
    }

    public class OtherProductReceiptService : WarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionPrdRct>, WarehouseAdjustmentPrimitiveDTO<WAOptionPrdRct>, WarehouseAdjustmentDetailDTO>, IOtherProductReceiptService
    {
        public OtherProductReceiptService(IWarehouseAdjustmentRepository warehouseAdjustmentRepository)
            : base(warehouseAdjustmentRepository) { }
    }
    public class OtherProductIssueService : WarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionPrdIss>, WarehouseAdjustmentPrimitiveDTO<WAOptionPrdIss>, WarehouseAdjustmentDetailDTO>, IOtherProductIssueService
    {
        public OtherProductIssueService(IWarehouseAdjustmentRepository warehouseAdjustmentRepository)
            : base(warehouseAdjustmentRepository) { }
    }
    public class ProductAdjustmentService : WarehouseAdjustmentService<WarehouseAdjustmentDTO<WAOptionPrdAdj>, WarehouseAdjustmentPrimitiveDTO<WAOptionPrdAdj>, WarehouseAdjustmentDetailDTO>, IProductAdjustmentService
    {
        public ProductAdjustmentService(IWarehouseAdjustmentRepository warehouseAdjustmentRepository)
            : base(warehouseAdjustmentRepository) { }
    }
}
