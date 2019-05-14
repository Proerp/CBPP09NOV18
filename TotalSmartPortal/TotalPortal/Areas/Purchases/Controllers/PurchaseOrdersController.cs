using System.Net;
using System.Web.Mvc;
using System.Text;

using RequireJsNet;

using TotalModel;
using TotalDTO;
using TotalBase.Enums;
using TotalDTO.Purchases;
using TotalModel.Models;

using TotalCore.Services.Purchases;

using TotalDTO.Commons;

using TotalPortal.Controllers;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Areas.Purchases.ViewModels;
using TotalPortal.Areas.Purchases.Builders;

namespace TotalPortal.Areas.Purchases.Controllers
{
    public class PurchaseOrdersController<TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel> : GenericViewDetailController<PurchaseOrder, PurchaseOrderDetail, PurchaseOrderViewDetail, TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
        where TViewDetailViewModel : TDto, IViewDetailViewModel<TDtoDetail>, IPurchaseOrderViewModel, new()
    {
        public PurchaseOrdersController(IPurchaseOrderService<TDto, TPrimitiveDto, TDtoDetail> purchaseOrderService, IPurchaseOrderViewModelSelectListBuilder<TViewDetailViewModel> purchaseOrderViewModelSelectListBuilder)
            : base(purchaseOrderService, purchaseOrderViewModelSelectListBuilder)
        {
        }

        public override void AddRequireJsOptions()
        {
            base.AddRequireJsOptions();

            TViewDetailViewModel viewDetailViewModel = new TViewDetailViewModel();

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)(viewDetailViewModel.IsItem ? GlobalEnums.CommodityTypeID.Items : (viewDetailViewModel.IsMaterial ? GlobalEnums.CommodityTypeID.Materials : GlobalEnums.CommodityTypeID.Unknown)));

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);
        }
    }



    public class PurchaseMaterialsController : PurchaseOrdersController<PurchaseOrderDTO<PurchaseOptionMaterial>, PurchaseOrderPrimitiveDTO<PurchaseOptionMaterial>, PurchaseOrderDetailDTO, PurchaseMaterialViewModel>
    {
        public PurchaseMaterialsController(IPurchaseMaterialService purchaseMaterialService, IPurchaseMaterialViewModelSelectListBuilder purchaseMaterialViewModelSelectListBuilder)
            : base(purchaseMaterialService, purchaseMaterialViewModelSelectListBuilder)
        {
        }
    }


    public class PurchaseItemsController : PurchaseOrdersController<PurchaseOrderDTO<PurchaseOptionItem>, PurchaseOrderPrimitiveDTO<PurchaseOptionItem>, PurchaseOrderDetailDTO, PurchaseItemViewModel>
    {
        public PurchaseItemsController(IPurchaseItemService purchaseItemService, IPurchaseItemViewModelSelectListBuilder purchaseItemViewModelSelectListBuilder)
            : base(purchaseItemService, purchaseItemViewModelSelectListBuilder)
        {
        }
    }


    public class PurchaseProductsController : PurchaseOrdersController<PurchaseOrderDTO<PurchaseOptionProduct>, PurchaseOrderPrimitiveDTO<PurchaseOptionProduct>, PurchaseOrderDetailDTO, PurchaseProductViewModel>
    {
        public PurchaseProductsController(IPurchaseProductService purchaseProductService, IPurchaseProductViewModelSelectListBuilder purchaseProductViewModelSelectListBuilder)
            : base(purchaseProductService, purchaseProductViewModelSelectListBuilder)
        {
        }
    }
}