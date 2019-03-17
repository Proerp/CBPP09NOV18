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


        public JsonResult GetFinishedHandoverIndexes([DataSourceRequest] DataSourceRequest request, int nmvnTaskID)
        {
            this.finishedHandoverAPIRepository.RepositoryBag["NMVNTaskID"] = nmvnTaskID;
            ICollection<FinishedHandoverIndex> finishedHandoverIndexes = this.finishedHandoverAPIRepository.GetEntityIndexes<FinishedHandoverIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = finishedHandoverIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWorkshifts([DataSourceRequest] DataSourceRequest dataSourceRequest, int nmvnTaskID, int? locationID)
        {
            var result = this.finishedHandoverAPIRepository.GetWorkshifts(nmvnTaskID, locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomers([DataSourceRequest] DataSourceRequest dataSourceRequest, int nmvnTaskID, int? locationID)
        {
            var result = this.finishedHandoverAPIRepository.GetCustomers(nmvnTaskID, locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPlannedOrders([DataSourceRequest] DataSourceRequest dataSourceRequest, int nmvnTaskID, int? locationID)
        {
            var result = this.finishedHandoverAPIRepository.GetPlannedOrders(nmvnTaskID, locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPendingDetails([DataSourceRequest] DataSourceRequest dataSourceRequest, int nmvnTaskID, int? finishedHandoverID, int? workshiftID, int? plannedOrderID, int? customerID, string finishedItemPackageIDs, string finishedProductPackageIDs)
        {
            var result = this.finishedHandoverAPIRepository.GetPendingDetails(nmvnTaskID, finishedHandoverID, workshiftID, plannedOrderID, customerID, finishedItemPackageIDs, finishedProductPackageIDs);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }
    }
}