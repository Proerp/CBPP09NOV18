using System.Net;
using System.Web.Mvc;
using System.Text;

using RequireJsNet;

using TotalBase.Enums;
using TotalModel;
using TotalDTO;
using TotalDTO.Inventories;
using TotalModel.Models;
using TotalPortal.ViewModels.Helpers;

using TotalCore.Services.Inventories;

using TotalPortal.Controllers;
using TotalPortal.Areas.Inventories.ViewModels;
using TotalPortal.Areas.Inventories.Builders;


namespace TotalPortal.Areas.Inventories.Controllers
{
    public class WarehouseAdjustmentsController<TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel> : GenericViewDetailController<WarehouseAdjustment, WarehouseAdjustmentDetail, WarehouseAdjustmentViewDetail, TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
        where TViewDetailViewModel : TDto, IViewDetailViewModel<TDtoDetail>, IWarehouseAdjustmentViewModel, new()
    {
        public WarehouseAdjustmentsController(IWarehouseAdjustmentService<TDto, TPrimitiveDto, TDtoDetail> warehouseAdjustmentService, IWarehouseAdjustmentViewModelSelectListBuilder<TViewDetailViewModel> warehouseAdjustmentViewModelSelectListBuilder)
            : base(warehouseAdjustmentService, warehouseAdjustmentViewModelSelectListBuilder, true)
        {
        }

        public override void AddRequireJsOptions()
        {
            base.AddRequireJsOptions();

            TViewDetailViewModel viewDetailViewModel = new TViewDetailViewModel();

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)(viewDetailViewModel.IsMaterial ? GlobalEnums.CommodityTypeID.Materials : (viewDetailViewModel.IsItem ? GlobalEnums.CommodityTypeID.Items : (viewDetailViewModel.IsProduct ? GlobalEnums.CommodityTypeID.Products : GlobalEnums.CommodityTypeID.Unknown))));

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);


            StringBuilder warehouseTaskIDList = new StringBuilder();
            warehouseTaskIDList.Append((int)(viewDetailViewModel.IsMaterial ? GlobalEnums.WarehouseTaskID.MaterialAdjustment : (viewDetailViewModel.IsItem ? GlobalEnums.WarehouseTaskID.ItemAdjustment : (viewDetailViewModel.IsProduct ? GlobalEnums.WarehouseTaskID.ProductAdjustment : GlobalEnums.WarehouseTaskID.Unknown))));

            ViewBag.WarehouseTaskID = (int)(viewDetailViewModel.IsMaterial ? GlobalEnums.WarehouseTaskID.MaterialAdjustment : (viewDetailViewModel.IsItem ? GlobalEnums.WarehouseTaskID.ItemAdjustment : (viewDetailViewModel.IsProduct ? GlobalEnums.WarehouseTaskID.ProductAdjustment : GlobalEnums.WarehouseTaskID.Unknown)));
            ViewBag.WarehouseTaskIDList = warehouseTaskIDList.ToString();
        }

        public virtual ActionResult GetGoodsReceiptDetailAvailables()
        {
            this.AddRequireJsOptions();
            TViewDetailViewModel viewDetailViewModel = new TViewDetailViewModel();
            return View(this.InitViewModel(viewDetailViewModel));
        }

        protected override PrintViewModel InitPrintViewModel(int? id, int? detailID)
        {
            PrintViewModel printViewModel = base.InitPrintViewModel(id, detailID);

            TViewDetailViewModel viewDetailViewModel = this.GetViewModel(id, GlobalEnums.AccessLevel.Readable, true); if (viewDetailViewModel == null) printViewModel.Id = 0;

            printViewModel.PrintOptionID = viewDetailViewModel.Approved ? 1 : 0;
            printViewModel.ReportPath = viewDetailViewModel.IsMaterial ? "MaterialAdjustmentSheet" : (viewDetailViewModel.IsItem ? "ItemAdjustmentSheet" : (viewDetailViewModel.IsProduct ? "ProductAdjustmentSheet" : ""));

            return printViewModel;
        }
    }





    public class OtherMaterialReceiptsController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO<WAOptionMtlRct>, WarehouseAdjustmentPrimitiveDTO<WAOptionMtlRct>, WarehouseAdjustmentDetailDTO, OtherMaterialReceiptViewModel>
    {
        public OtherMaterialReceiptsController(IOtherMaterialReceiptService otherMaterialReceiptService, IOtherMaterialReceiptViewModelSelectListBuilder otherMaterialReceiptViewModelSelectListBuilder)
            : base(otherMaterialReceiptService, otherMaterialReceiptViewModelSelectListBuilder)
        {
        }
    }

    public class OtherMaterialIssuesController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO<WAOptionMtlIss>, WarehouseAdjustmentPrimitiveDTO<WAOptionMtlIss>, WarehouseAdjustmentDetailDTO, OtherMaterialIssueViewModel>
    {
        public OtherMaterialIssuesController(IOtherMaterialIssueService otherMaterialIssueService, IOtherMaterialIssueViewModelSelectListBuilder otherMaterialIssueViewModelSelectListBuilder)
            : base(otherMaterialIssueService, otherMaterialIssueViewModelSelectListBuilder)
        {
        }
    }

    public class MaterialAdjustmentsController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO<WAOptionMtlAdj>, WarehouseAdjustmentPrimitiveDTO<WAOptionMtlAdj>, WarehouseAdjustmentDetailDTO, MaterialAdjustmentViewModel>
    {
        public MaterialAdjustmentsController(IMaterialAdjustmentService materialAdjustmentService, IMaterialAdjustmentViewModelSelectListBuilder materialAdjustmentViewModelSelectListBuilder)
            : base(materialAdjustmentService, materialAdjustmentViewModelSelectListBuilder)
        {
        }
    }



    public class OtherItemReceiptsController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO<WAOptionItmRct>, WarehouseAdjustmentPrimitiveDTO<WAOptionItmRct>, WarehouseAdjustmentDetailDTO, OtherItemReceiptViewModel>
    {
        public OtherItemReceiptsController(IOtherItemReceiptService otherItemReceiptService, IOtherItemReceiptViewModelSelectListBuilder otherItemReceiptViewModelSelectListBuilder)
            : base(otherItemReceiptService, otherItemReceiptViewModelSelectListBuilder)
        {
        }
    }

    public class OtherItemIssuesController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO<WAOptionItmIss>, WarehouseAdjustmentPrimitiveDTO<WAOptionItmIss>, WarehouseAdjustmentDetailDTO, OtherItemIssueViewModel>
    {
        public OtherItemIssuesController(IOtherItemIssueService otherItemIssueService, IOtherItemIssueViewModelSelectListBuilder otherItemIssueViewModelSelectListBuilder)
            : base(otherItemIssueService, otherItemIssueViewModelSelectListBuilder)
        {
        }
    }

    public class ItemAdjustmentsController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO<WAOptionItmAdj>, WarehouseAdjustmentPrimitiveDTO<WAOptionItmAdj>, WarehouseAdjustmentDetailDTO, ItemAdjustmentViewModel>
    {
        public ItemAdjustmentsController(IItemAdjustmentService itemAdjustmentService, IItemAdjustmentViewModelSelectListBuilder itemAdjustmentViewModelSelectListBuilder)
            : base(itemAdjustmentService, itemAdjustmentViewModelSelectListBuilder)
        {
        }
    }



    public class OtherProductReceiptsController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO<WAOptionPrdRct>, WarehouseAdjustmentPrimitiveDTO<WAOptionPrdRct>, WarehouseAdjustmentDetailDTO, OtherProductReceiptViewModel>
    {
        public OtherProductReceiptsController(IOtherProductReceiptService otherProductReceiptService, IOtherProductReceiptViewModelSelectListBuilder otherProductReceiptViewModelSelectListBuilder)
            : base(otherProductReceiptService, otherProductReceiptViewModelSelectListBuilder)
        {
        }
    }

    public class OtherProductIssuesController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO<WAOptionPrdIss>, WarehouseAdjustmentPrimitiveDTO<WAOptionPrdIss>, WarehouseAdjustmentDetailDTO, OtherProductIssueViewModel>
    {
        public OtherProductIssuesController(IOtherProductIssueService otherProductIssueService, IOtherProductIssueViewModelSelectListBuilder otherProductIssueViewModelSelectListBuilder)
            : base(otherProductIssueService, otherProductIssueViewModelSelectListBuilder)
        {
        }
    }

    public class ProductAdjustmentsController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO<WAOptionPrdAdj>, WarehouseAdjustmentPrimitiveDTO<WAOptionPrdAdj>, WarehouseAdjustmentDetailDTO, ProductAdjustmentViewModel>
    {
        public ProductAdjustmentsController(IProductAdjustmentService productAdjustmentService, IProductAdjustmentViewModelSelectListBuilder productAdjustmentViewModelSelectListBuilder)
            : base(productAdjustmentService, productAdjustmentViewModelSelectListBuilder)
        {
        }
    }
}