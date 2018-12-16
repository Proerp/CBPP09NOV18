using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalDTO;
using TotalModel;
using TotalModel.Models;
using TotalBase.Enums;
using TotalDTO.Inventories;
using TotalCore.Repositories.Inventories;
using TotalCore.Services.Inventories;
using TotalDAL.Repositories.Inventories;

namespace TotalService.Inventories
{
    public class GoodsReceiptService<TDto, TPrimitiveDto, TDtoDetail> : GoodsReceiptBaseService<TDto, TPrimitiveDto, TDtoDetail>, IGoodsReceiptService<TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>, IGoodsReceiptDTO
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
        public GoodsReceiptService(IGoodsReceiptRepository goodsReceiptRepository)
            : base(goodsReceiptRepository)
        {
        }

        public override bool Save(TDto dto)
        {
            dto.GoodsReceiptViewDetails.RemoveAll(x => x.Quantity == 0 && dto.GoodsReceiptTypeID != (int)GlobalEnums.GoodsReceiptTypeID.MaterialIssue);
            return base.Save(dto);
        }

        public override bool Approvable(TDto dto)
        {
            return !(dto.WarehouseAdjustmentID != null || (dto.WarehouseTransferID != null && dto.OneStep)) && base.Approvable(dto);
        }

        public override bool UnApprovable(TDto dto)
        {
            return !(dto.WarehouseAdjustmentID != null || (dto.WarehouseTransferID != null && dto.OneStep)) && base.UnApprovable(dto);
        }

        public override bool Editable(TDto dto)
        {
            return !(dto.WarehouseAdjustmentID != null || (dto.WarehouseTransferID != null && dto.OneStep)) && base.Editable(dto);
        }
    }

    public class GoodsReceiptBaseService<TDto, TPrimitiveDto, TDtoDetail> : GenericWithViewDetailService<GoodsReceipt, GoodsReceiptDetail, GoodsReceiptViewDetail, TDto, TPrimitiveDto, TDtoDetail>, IGoodsReceiptBaseService<TDto, TPrimitiveDto, TDtoDetail>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>, IGoodsReceiptDTO
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
    {
        public GoodsReceiptBaseService(IGoodsReceiptRepository goodsReceiptRepository)
            : base(goodsReceiptRepository, "GoodsReceiptPostSaveValidate", "GoodsReceiptSaveRelative", "GoodsReceiptToggleApproved", null, null, "GetGoodsReceiptViewDetails")
        {
        }

        public new bool Save(TDto goodsReceiptDTO, bool useExistingTransaction)
        {
            goodsReceiptDTO.GoodsReceiptViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(goodsReceiptDTO, useExistingTransaction);
        }

        public new bool Delete(int id, bool useExistingTransaction)
        {
            return base.Delete(id, useExistingTransaction);
        }

        public override ICollection<GoodsReceiptViewDetail> GetViewDetails(int goodsReceiptID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("GoodsReceiptID", goodsReceiptID) };
            return this.GetViewDetails(parameters);
        }
    }







    public class MaterialReceiptBaseService : GoodsReceiptBaseService<GoodsReceiptDTO<GROptionMaterial>, GoodsReceiptPrimitiveDTO<GROptionMaterial>, GoodsReceiptDetailDTO>, IMaterialReceiptBaseService
    {
        public MaterialReceiptBaseService(IGoodsReceiptRepository warehouseReceiptRepository)
            : base(warehouseReceiptRepository) { }
    }
    public class ItemReceiptBaseService : GoodsReceiptBaseService<GoodsReceiptDTO<GROptionItem>, GoodsReceiptPrimitiveDTO<GROptionItem>, GoodsReceiptDetailDTO>, IItemReceiptBaseService
    {
        public ItemReceiptBaseService(IGoodsReceiptRepository warehouseReceiptRepository)
            : base(warehouseReceiptRepository) { }
    }
    public class ProductReceiptBaseService : GoodsReceiptBaseService<GoodsReceiptDTO<GROptionProduct>, GoodsReceiptPrimitiveDTO<GROptionProduct>, GoodsReceiptDetailDTO>, IProductReceiptBaseService
    {
        public ProductReceiptBaseService(IGoodsReceiptRepository warehouseReceiptRepository)
            : base(warehouseReceiptRepository) { }
    }





    public class MaterialReceiptService : GoodsReceiptService<GoodsReceiptDTO<GROptionMaterial>, GoodsReceiptPrimitiveDTO<GROptionMaterial>, GoodsReceiptDetailDTO>, IMaterialReceiptService
    {
        public MaterialReceiptService(IGoodsReceiptRepository warehouseReceiptRepository)
            : base(warehouseReceiptRepository) { }
    }
    public class ItemReceiptService : GoodsReceiptService<GoodsReceiptDTO<GROptionItem>, GoodsReceiptPrimitiveDTO<GROptionItem>, GoodsReceiptDetailDTO>, IItemReceiptService
    {
        public ItemReceiptService(IGoodsReceiptRepository warehouseReceiptRepository)
            : base(warehouseReceiptRepository) { }
    }
    public class ProductReceiptService : GoodsReceiptService<GoodsReceiptDTO<GROptionProduct>, GoodsReceiptPrimitiveDTO<GROptionProduct>, GoodsReceiptDetailDTO>, IProductReceiptService
    {
        public ProductReceiptService(IGoodsReceiptRepository warehouseReceiptRepository)
            : base(warehouseReceiptRepository) { }
    }



    public class GRHelperService
    {
        private GlobalEnums.GROption grOption;

        private IMaterialReceiptBaseService materialReceiptBaseService;
        private IItemReceiptBaseService itemReceiptBaseService;
        private IProductReceiptBaseService productReceiptBaseService;

        public GRHelperService(GlobalEnums.GROption grOption, TotalSmartPortalEntities totalSmartPortalEntities, int serviceUserID)
        {
            this.grOption = grOption;

            //***VERY IMPORTANT***: THE BaseService.UserID IS AUTOMATICALLY SET BY CustomControllerAttribute OF CONTROLLER, ONLY WHEN BaseService IS INITIALIZED BY CONTROLLER. BUT HERE, THE this.goodsReceiptBaseService IS INITIALIZED BY VehiclesInvoiceService => SO SHOULD SET goodsReceiptBaseService.UserID = serviceUserID
            if (this.grOption == GlobalEnums.GROption.IsMaterial)
            {
                this.materialReceiptBaseService = new MaterialReceiptBaseService(new GoodsReceiptRepository(totalSmartPortalEntities));
                materialReceiptBaseService.UserID = serviceUserID;
            }
            else
                if (this.grOption == GlobalEnums.GROption.IsItem)
                {
                    this.itemReceiptBaseService = new ItemReceiptBaseService(new GoodsReceiptRepository(totalSmartPortalEntities));
                    itemReceiptBaseService.UserID = serviceUserID;
                }
                else
                    if (this.grOption == GlobalEnums.GROption.IsProduct)
                    {
                        this.productReceiptBaseService = new ProductReceiptBaseService(new GoodsReceiptRepository(totalSmartPortalEntities));
                        productReceiptBaseService.UserID = serviceUserID;
                    }
        }

        public IGoodsReceiptDTO NewGoodsReceiptDTO()
        {
            if (this.grOption == GlobalEnums.GROption.IsMaterial)
                return new GoodsReceiptDTO<GROptionMaterial>();
            else
                if (this.grOption == GlobalEnums.GROption.IsItem)
                    return new GoodsReceiptDTO<GROptionItem>();
                else
                    if (this.grOption == GlobalEnums.GROption.IsProduct)
                        return new GoodsReceiptDTO<GROptionProduct>();
                    else
                        return null;
        }

        public void Save(IGoodsReceiptDTO goodsReceiptDTO)
        {
            if (this.grOption == GlobalEnums.GROption.IsMaterial)
                materialReceiptBaseService.Save((GoodsReceiptDTO<GROptionMaterial>)goodsReceiptDTO, true);
            else
                if (this.grOption == GlobalEnums.GROption.IsItem)
                    itemReceiptBaseService.Save((GoodsReceiptDTO<GROptionItem>)goodsReceiptDTO, true);
                else
                    if (this.grOption == GlobalEnums.GROption.IsProduct)
                        productReceiptBaseService.Save((GoodsReceiptDTO<GROptionProduct>)goodsReceiptDTO, true);
        }

        public void Delete(int? goodsReceiptID)
        {
            if (goodsReceiptID != null)
            {
                if (this.grOption == GlobalEnums.GROption.IsMaterial)
                    materialReceiptBaseService.Delete((int)goodsReceiptID, true);
                else
                    if (this.grOption == GlobalEnums.GROption.IsItem)
                        itemReceiptBaseService.Delete((int)goodsReceiptID, true);
                    else
                        if (this.grOption == GlobalEnums.GROption.IsProduct)
                            productReceiptBaseService.Delete((int)goodsReceiptID, true);
            }
            else
                throw new Exception("Lỗi không tìm thấy phiếu nhập kho cũ của phiếu này!" + "\r\n" + "\r\n" + "Vui lòng kiểm tra lại dữ liệu trước khi tiếp tục.");
        }
    }
}