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

using TotalDTO.Productions;

using TotalCore.Repositories.Productions;
using TotalPortal.Areas.Productions.ViewModels;
using TotalPortal.APIs.Sessions;

using Microsoft.AspNet.Identity;

namespace TotalPortal.Areas.Productions.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class WorkOrderAPIsController : Controller
    {
        private readonly IWorkOrderAPIRepository workOrderAPIRepository;

        public WorkOrderAPIsController(IWorkOrderAPIRepository workOrderAPIRepository)
        {
            this.workOrderAPIRepository = workOrderAPIRepository;
        }


        public JsonResult GetWorkOrderIndexes([DataSourceRequest] DataSourceRequest request, string nmvnTaskID)
        {
            this.workOrderAPIRepository.RepositoryBag["NMVNTaskID"] = nmvnTaskID;
            ICollection<WorkOrderIndex> workOrderIndexes = this.workOrderAPIRepository.GetEntityIndexes<WorkOrderIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = workOrderIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }





        public JsonResult GetFirmOrders([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? nmvnTaskID, int? firmOrderID)
        {
            var result = this.workOrderAPIRepository.GetFirmOrders(locationID, nmvnTaskID, firmOrderID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }
    }
}