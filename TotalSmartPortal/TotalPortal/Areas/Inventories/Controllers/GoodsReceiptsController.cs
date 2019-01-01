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
using TotalPortal.Areas.Commons.Controllers.Sessions;
using TotalPortal.Areas.Inventories.Controllers.Sessions;
using TotalPortal.APIs.Sessions;

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

        public virtual ActionResult GetPendingGoodsArrivalPackages()
        {
            this.AddRequireJsOptions();
            return View("../GoodsReceipts/GetPendingGoodsArrivalPackages", this.InitViewModel(new TViewDetailViewModel()));
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


        protected override TViewDetailViewModel InitViewModelByDefault(TViewDetailViewModel simpleViewModel)
        {
            simpleViewModel = base.InitViewModelByDefault(simpleViewModel);

            if (((IGoodsReceiptPrimitiveDTO)simpleViewModel).ShiftID == 0)
            {
                string shiftSession = ShiftSession.GetShift(this.HttpContext);
                if (HomeSession.TryParseID(shiftSession) > 0) ((IGoodsReceiptPrimitiveDTO)simpleViewModel).ShiftID = (int)HomeSession.TryParseID(shiftSession);
            }

            if (simpleViewModel.Storekeeper == null)
            {
                string storekeeperSession = GoodsReceiptSession.GetStorekeeper(this.HttpContext);

                if (HomeSession.TryParseID(storekeeperSession) > 0)
                {
                    simpleViewModel.Storekeeper = new TotalDTO.Commons.EmployeeBaseDTO();
                    simpleViewModel.Storekeeper.EmployeeID = (int)HomeSession.TryParseID(storekeeperSession);
                    simpleViewModel.Storekeeper.Name = HomeSession.TryParseName(storekeeperSession);
                }
            }

            return simpleViewModel;
        }

        protected override void BackupViewModelToSession(TViewDetailViewModel simpleViewModel)
        {
            base.BackupViewModelToSession(simpleViewModel);
            ShiftSession.SetShift(this.HttpContext, ((IGoodsReceiptPrimitiveDTO)simpleViewModel).ShiftID);
            if (simpleViewModel.Storekeeper != null) GoodsReceiptSession.SetStorekeeper(this.HttpContext, simpleViewModel.Storekeeper.EmployeeID, simpleViewModel.Storekeeper.Name);
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