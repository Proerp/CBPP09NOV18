using System.Net;
using System.Web.Mvc;
using System.Text;

using RequireJsNet;

using TotalModel;
using TotalDTO;
using TotalBase.Enums;
using TotalDTO.Productions;
using TotalModel.Models;

using TotalCore.Services.Productions;

using TotalPortal.Controllers;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Areas.Productions.ViewModels;
using TotalPortal.Areas.Productions.Builders;

namespace TotalPortal.Areas.Productions.Controllers
{
    public class ProductionOrdersController<TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel> : GenericViewDetailController<ProductionOrder, ProductionOrderDetail, ProductionOrderViewDetail, TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
        where TViewDetailViewModel : TDto, IViewDetailViewModel<TDtoDetail>, IProductionOrderViewModel, new()
    {
        public ProductionOrdersController(IProductionOrderService<TDto, TPrimitiveDto, TDtoDetail> productionOrderService, IProductionOrderViewModelSelectListBuilder<TViewDetailViewModel> productionOrderViewModelSelectListBuilder)
            : base(productionOrderService, productionOrderViewModelSelectListBuilder, true)
        {
        }

        public override void AddRequireJsOptions()
        {
            base.AddRequireJsOptions();

            TViewDetailViewModel viewDetailViewModel = new TViewDetailViewModel();

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)(viewDetailViewModel.IsItem ? GlobalEnums.CommodityTypeID.Items : (viewDetailViewModel.IsProduct ? GlobalEnums.CommodityTypeID.Products : GlobalEnums.CommodityTypeID.Unknown)));

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);
        }

        public virtual ActionResult GetPendingFirmOrders()
        {
            this.AddRequireJsOptions();
            return View();
        }
    }



    public class ItemOrdersController : ProductionOrdersController<ProductionOrderDTO<ProrderOptionItem>, ProductionOrderPrimitiveDTO<ProrderOptionItem>, ProductionOrderDetailDTO, ItemOrderViewModel>
    {
        public ItemOrdersController(IItemOrderService itemOrderService, IItemOrderViewModelSelectListBuilder itemOrderViewModelSelectListBuilder)
            : base(itemOrderService, itemOrderViewModelSelectListBuilder)
        {
        }
    }


    public class ProductOrdersController : ProductionOrdersController<ProductionOrderDTO<ProrderOptionProduct>, ProductionOrderPrimitiveDTO<ProrderOptionProduct>, ProductionOrderDetailDTO, ProductOrderViewModel>
    {
        public ProductOrdersController(IProductOrderService productOrderService, IProductOrderViewModelSelectListBuilder productOrderViewModelSelectListBuilder)
            : base(productOrderService, productOrderViewModelSelectListBuilder)
        {
        }
    }
}