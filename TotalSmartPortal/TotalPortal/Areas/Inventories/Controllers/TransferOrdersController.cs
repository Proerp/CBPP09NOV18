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
    public class TransferOrdersController<TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel> : GenericViewDetailController<TransferOrder, TransferOrderDetail, TransferOrderViewDetail, TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
        where TViewDetailViewModel : TDto, IViewDetailViewModel<TDtoDetail>, ITransferOrderViewModel, new()
    {
        public TransferOrdersController(ITransferOrderService<TDto, TPrimitiveDto, TDtoDetail> transferOrderService, ITransferOrderViewModelSelectListBuilder<TViewDetailViewModel> transferOrderViewModelSelectListBuilder)
            : base(transferOrderService, transferOrderViewModelSelectListBuilder, true)
        {
        }

        protected override TViewDetailViewModel InitViewModelByDefault(TViewDetailViewModel simpleViewModel)
        {
            simpleViewModel = base.InitViewModelByDefault(simpleViewModel);

            simpleViewModel.Warehouse = new TotalDTO.Commons.WarehouseBaseDTO() { WarehouseID = 2, Code = "KM片", Name = "KM", LocationID = 1 };
            simpleViewModel.WarehouseReceipt = new TotalDTO.Commons.WarehouseBaseDTO() { WarehouseID = 6, Code = "KDH", Name = "KDH", LocationID = 2 };

            return simpleViewModel;
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

        protected override PrintViewModel InitPrintViewModel(int? id, int? detailID)
        {
            PrintViewModel printViewModel = base.InitPrintViewModel(id, detailID);

            TViewDetailViewModel viewDetailViewModel = this.GetViewModel(id, GlobalEnums.AccessLevel.Readable, true); if (viewDetailViewModel == null) printViewModel.Id = 0;

            printViewModel.PrintOptionID = viewDetailViewModel.Approved ? 1 : 0;
            printViewModel.ReportPath = viewDetailViewModel.IsMaterial ? "MaterialAdjustmentSheet" : (viewDetailViewModel.IsItem ? "ItemAdjustmentSheet" : (viewDetailViewModel.IsProduct ? "ProductAdjustmentSheet" : ""));

            return printViewModel;
        }
    }





    public class MaterialTransferOrdersController : TransferOrdersController<TransferOrderDTO<TOOptionMaterial>, TransferOrderPrimitiveDTO<TOOptionMaterial>, TransferOrderDetailDTO, MaterialTransferOrderViewModel>
    {
        public MaterialTransferOrdersController(IMaterialTransferOrderService materialTransferOrderService, IMaterialTransferOrderViewModelSelectListBuilder materialTransferOrderViewModelSelectListBuilder)
            : base(materialTransferOrderService, materialTransferOrderViewModelSelectListBuilder)
        {
        }
    }  

    public class ItemTransferOrdersController : TransferOrdersController<TransferOrderDTO<TOOptionItem>, TransferOrderPrimitiveDTO<TOOptionItem>, TransferOrderDetailDTO, ItemTransferOrderViewModel>
    {
        public ItemTransferOrdersController(IItemTransferOrderService itemTransferOrderService, IItemTransferOrderViewModelSelectListBuilder itemTransferOrderViewModelSelectListBuilder)
            : base(itemTransferOrderService, itemTransferOrderViewModelSelectListBuilder)
        {
        }
    }

   
    public class ProductTransferOrdersController : TransferOrdersController<TransferOrderDTO<TOOptionProduct>, TransferOrderPrimitiveDTO<TOOptionProduct>, TransferOrderDetailDTO, ProductTransferOrderViewModel>
    {
        public ProductTransferOrdersController(IProductTransferOrderService productTransferOrderService, IProductTransferOrderViewModelSelectListBuilder productTransferOrderViewModelSelectListBuilder)
            : base(productTransferOrderService, productTransferOrderViewModelSelectListBuilder)
        {
        }
    }
   
}