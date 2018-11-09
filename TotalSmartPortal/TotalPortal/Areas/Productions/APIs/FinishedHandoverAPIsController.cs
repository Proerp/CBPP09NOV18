using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;

using Microsoft.AspNet.Identity;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using TotalModel.Models;
using TotalCore.Repositories.Productions;

using TotalPortal.APIs.Sessions;
namespace TotalPortal.Areas.Productions.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class FinishedHandoverAPIsController : Controller
    {
        private readonly IFinishedHandoverAPIRepository finishedHandoverAPIRepository;

        public FinishedHandoverAPIsController(IFinishedHandoverAPIRepository finishedHandoverAPIRepository)
        {
            this.finishedHandoverAPIRepository = finishedHandoverAPIRepository;
        }


        public JsonResult GetFinishedHandoverIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<FinishedHandoverIndex> finishedHandoverIndexes = this.finishedHandoverAPIRepository.GetEntityIndexes<FinishedHandoverIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = finishedHandoverIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCustomers([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.finishedHandoverAPIRepository.GetCustomers(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPlannedOrders([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.finishedHandoverAPIRepository.GetPlannedOrders(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPendingDetails([DataSourceRequest] DataSourceRequest dataSourceRequest, int? finishedHandoverID, int? plannedOrderID, int? customerID, string finishedProductPackageIDs, bool? isReadonly)
        {
            var result = this.finishedHandoverAPIRepository.GetPendingDetails(finishedHandoverID, plannedOrderID, customerID, finishedProductPackageIDs, false);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }
    }
}