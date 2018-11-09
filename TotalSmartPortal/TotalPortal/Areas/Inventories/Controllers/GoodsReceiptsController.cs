using System.Net;
using System.Web.Mvc;
using System.Text;

using AutoMapper;
using RequireJsNet;

using TotalBase.Enums;
using TotalDTO;
using TotalModel;
using TotalModel.Models;

using TotalCore.Services.Inventories;
using TotalCore.Repositories.Commons;

using TotalDTO.Inventories;

using TotalPortal.Controllers;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Areas.Inventories.ViewModels;
using TotalPortal.Areas.Inventories.Builders;

namespace TotalPortal.Areas.Inventories.Controllers
{
    public class GoodsReceiptsController<TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel> : GenericViewDetailController<GoodsReceipt, GoodsReceiptDetail, GoodsReceiptViewDetail, TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
        where TViewDetailViewModel : TDto, IViewDetailViewModel<TDtoDetail>, IGoodsReceiptViewModel, new()
    {
        public GoodsReceiptsController(IGoodsReceiptService<TDto, TPrimitiveDto, TDtoDetail> goodsReceiptService, IGoodsReceiptViewModelSelectListBuilder<TViewDetailViewModel> goodsReceiptViewModelSelectListBuilder)
            : base(goodsReceiptService, goodsReceiptViewModelSelectListBuilder, true)
        {
        }

        public override void AddRequireJsOptions()
        {
            base.AddRequireJsOptions();

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Items);
            commodityTypeIDList.Append(","); commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Consumables);

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);


            StringBuilder warehouseTaskIDList = new StringBuilder();
            warehouseTaskIDList.Append((int)GlobalEnums.WarehouseTaskID.DeliveryAdvice);

            ViewBag.WarehouseTaskID = (int)GlobalEnums.WarehouseTaskID.DeliveryAdvice;
            ViewBag.WarehouseTaskIDList = warehouseTaskIDList.ToString();
        }

        public virtual ActionResult GetPendingPurchaseRequisitionDetails()
        {
            this.AddRequireJsOptions();
            return View("../GoodsReceipts/GetPendingPurchaseRequisitionDetails", this.InitViewModel(new TViewDetailViewModel()));
        }

        public virtual ActionResult GetPendingWarehouseTransferDetails()
        {
            this.AddRequireJsOptions();
            return View("../GoodsReceipts/GetPendingWarehouseTransferDetails", this.InitViewModel(new TViewDetailViewModel()));
        }

        public virtual ActionResult GetPendingMaterialIssueDetails()
        {
            this.AddRequireJsOptions();
            return View("../GoodsReceipts/GetPendingMaterialIssueDetails", this.InitViewModel(new TViewDetailViewModel()));
        }

        public virtual ActionResult GetPendingPlannedOrderDetails()
        {
            this.AddRequireJsOptions();
            return View("../GoodsReceipts/GetPendingPlannedOrderDetails", this.InitViewModel(new TViewDetailViewModel()));
        }
    }






    public class MaterialReceiptsController : GoodsReceiptsController<GoodsReceiptDTO<GROptionMaterial>, GoodsReceiptPrimitiveDTO<GROptionMaterial>, GoodsReceiptDetailDTO, MaterialReceiptViewModel>
    {
        public MaterialReceiptsController(IMaterialReceiptService materialReceiptService, IMaterialReceiptViewModelSelectListBuilder materialReceiptViewModelSelectListBuilder)
            : base(materialReceiptService, materialReceiptViewModelSelectListBuilder)
        {
        }
    }

    public class ItemReceiptsController : GoodsReceiptsController<GoodsReceiptDTO<GROptionItem>, GoodsReceiptPrimitiveDTO<GROptionItem>, GoodsReceiptDetailDTO, ItemReceiptViewModel>
    {
        public ItemReceiptsController(IItemReceiptService itemReceiptService, IItemReceiptViewModelSelectListBuilder itemReceiptViewModelSelectListBuilder)
            : base(itemReceiptService, itemReceiptViewModelSelectListBuilder)
        {
        }
    }


    public class ProductReceiptsController : GoodsReceiptsController<GoodsReceiptDTO<GROptionProduct>, GoodsReceiptPrimitiveDTO<GROptionProduct>, GoodsReceiptDetailDTO, ProductReceiptViewModel>
    {
        public ProductReceiptsController(IProductReceiptService productReceiptService, IProductReceiptViewModelSelectListBuilder productReceiptViewModelSelectListBuilder)
            : base(productReceiptService, productReceiptViewModelSelectListBuilder)
        {
        }
    }
}