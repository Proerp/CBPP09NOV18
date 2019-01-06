using System.Net;
using System.Web.Mvc;
using System.Text;

using AutoMapper;
using RequireJsNet;

using TotalBase.Enums;

using TotalModel.Models;

using TotalCore.Services.Purchases;

using TotalDTO.Purchases;

using TotalPortal.Controllers;
using TotalPortal.Areas.Purchases.ViewModels;
using TotalPortal.Areas.Purchases.Builders;

using TotalPortal.APIs.Sessions;

namespace TotalPortal.Areas.Purchases.Controllers
{
    public class GoodsArrivalsController : GenericViewDetailController<GoodsArrival, GoodsArrivalDetail, GoodsArrivalViewDetail, GoodsArrivalDTO, GoodsArrivalPrimitiveDTO, GoodsArrivalDetailDTO, GoodsArrivalViewModel>
    {
        public GoodsArrivalsController(IGoodsArrivalService goodsArrivalService, IGoodsArrivalViewModelSelectListBuilder goodsArrivalViewModelSelectListBuilder)
            : base(goodsArrivalService, goodsArrivalViewModelSelectListBuilder, true)
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

        public virtual ActionResult GetPendingPurchaseOrderDetails()
        {
            this.AddRequireJsOptions();
            return View();
        }
    }
}