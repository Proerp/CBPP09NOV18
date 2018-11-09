using System.Net;
using System.Web.Mvc;
using System.Text;

using RequireJsNet;

using TotalBase.Enums;
using TotalDTO.Productions;
using TotalModel.Models;

using TotalCore.Services.Productions;

using TotalPortal.Controllers;
using TotalPortal.Areas.Productions.ViewModels;
using TotalPortal.Areas.Productions.Builders;

namespace TotalPortal.Areas.Productions.Controllers
{
    public class ProductionOrdersController : GenericViewDetailController<ProductionOrder, ProductionOrderDetail, ProductionOrderViewDetail, ProductionOrderDTO, ProductionOrderPrimitiveDTO, ProductionOrderDetailDTO, ProductionOrderViewModel>
    {
        public ProductionOrdersController(IProductionOrderService productionOrderService, IProductionOrderViewModelSelectListBuilder productionOrderViewModelSelectListBuilder)
            : base(productionOrderService, productionOrderViewModelSelectListBuilder, true)
        {
        }

        public override void AddRequireJsOptions()
        {
            base.AddRequireJsOptions();

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Items);
            commodityTypeIDList.Append(","); commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Materials);

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);
        }

        public virtual ActionResult GetPendingFirmOrders()
        {
            this.AddRequireJsOptions();
            return View();
        }

    }

}