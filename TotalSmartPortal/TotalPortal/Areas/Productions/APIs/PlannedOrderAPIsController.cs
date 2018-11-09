using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;

using Microsoft.AspNet.Identity;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using TotalModel.Models;
using TotalCore.Repositories.Productions;

using TotalPortal.APIs.Sessions;
using System;

namespace TotalPortal.Areas.Productions.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class PlannedOrderAPIsController : Controller
    {
        private readonly IPlannedOrderAPIRepository plannedOrderAPIRepository;

        public PlannedOrderAPIsController(IPlannedOrderAPIRepository plannedOrderAPIRepository)
        {
            this.plannedOrderAPIRepository = plannedOrderAPIRepository;
        }


        public JsonResult GetPlannedOrderIndexes([DataSourceRequest] DataSourceRequest request, bool withExtendedSearch, DateTime extendedFromDate, DateTime extendedToDate, int dateOptionID, int filterOptionID)
        {
            this.plannedOrderAPIRepository.RepositoryBag["DateOptionID"] = dateOptionID;
            this.plannedOrderAPIRepository.RepositoryBag["FilterOptionID"] = filterOptionID;
            ICollection<PlannedOrderIndex> plannedOrderIndexes = this.plannedOrderAPIRepository.GetEntityIndexes<PlannedOrderIndex>(User.Identity.GetUserId(), (withExtendedSearch ? extendedFromDate : HomeSession.GetGlobalFromDate(this.HttpContext)), (withExtendedSearch ? extendedToDate : HomeSession.GetGlobalToDate(this.HttpContext)));

            DataSourceResult response = plannedOrderIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetPlannedOrderLogs([DataSourceRequest] DataSourceRequest dataSourceRequest, int? plannedOrderID, int? firmOrderID)
        {
            var result = this.plannedOrderAPIRepository.GetPlannedOrderLogs(plannedOrderID, firmOrderID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }
    }
}