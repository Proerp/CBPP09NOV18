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
    public class PlannedOrdersController : GenericViewDetailController<PlannedOrder, PlannedOrderDetail, PlannedOrderViewDetail, PlannedOrderDTO, PlannedOrderPrimitiveDTO, PlannedOrderDetailDTO, PlannedOrderViewModel>
    {
        public PlannedOrdersController(IPlannedOrderService plannedOrderService, IPlannedOrderViewModelSelectListBuilder plannedOrderViewModelSelectListBuilder)
            : base(plannedOrderService, plannedOrderViewModelSelectListBuilder, true)
        {
        }

        public override void AddRequireJsOptions()
        {
            base.AddRequireJsOptions();

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Products);

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);
        }
    }

}