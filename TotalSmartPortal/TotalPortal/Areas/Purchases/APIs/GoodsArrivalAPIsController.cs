using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using System.Web.UI;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;


using TotalBase.Enums;
using TotalModel.Models;

using TotalDTO.Purchases;

using TotalCore.Repositories.Purchases;
using TotalPortal.Areas.Purchases.ViewModels;
using TotalPortal.APIs.Sessions;

using Microsoft.AspNet.Identity;

namespace TotalPortal.Areas.Purchases.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class GoodsArrivalAPIsController : Controller
    {
        private readonly IGoodsArrivalAPIRepository goodsArrivalAPIRepository;

        public GoodsArrivalAPIsController(IGoodsArrivalAPIRepository goodsArrivalAPIRepository)
        {
            this.goodsArrivalAPIRepository = goodsArrivalAPIRepository;
        }


        public JsonResult GetGoodsArrivalIndexes([DataSourceRequest] DataSourceRequest request, bool withExtendedSearch, DateTime extendedFromDate, DateTime extendedToDate, bool pendingOnly)
        {
            this.goodsArrivalAPIRepository.RepositoryBag["PendingOnly"] = pendingOnly;
            ICollection<GoodsArrivalIndex> goodsArrivalIndexes = this.goodsArrivalAPIRepository.GetEntityIndexes<GoodsArrivalIndex>(User.Identity.GetUserId(), (withExtendedSearch ? extendedFromDate : HomeSession.GetGlobalFromDate(this.HttpContext)), (withExtendedSearch ? extendedToDate : HomeSession.GetGlobalToDate(this.HttpContext)));

            DataSourceResult response = goodsArrivalIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }





        public JsonResult GetCustomers([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.goodsArrivalAPIRepository.GetCustomers(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPurchaseOrders([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.goodsArrivalAPIRepository.GetPurchaseOrders(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPendingPurchaseOrderDetails([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? goodsArrivalID, int? purchaseOrderID, int? customerID, int? transporterID, string purchaseOrderDetailIDs)
        {
            var result = this.goodsArrivalAPIRepository.GetPendingPurchaseOrderDetails(locationID, goodsArrivalID, purchaseOrderID, customerID, transporterID, purchaseOrderDetailIDs);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }
    }
}
