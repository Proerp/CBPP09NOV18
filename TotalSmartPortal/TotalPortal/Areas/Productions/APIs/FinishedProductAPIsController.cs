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
    public class FinishedProductAPIsController : Controller
    {
        private readonly IFinishedProductAPIRepository semifinishedProductAPIRepository;

        public FinishedProductAPIsController(IFinishedProductAPIRepository semifinishedProductAPIRepository)
        {
            this.semifinishedProductAPIRepository = semifinishedProductAPIRepository;
        }


        public JsonResult GetFinishedProductIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<FinishedProductIndex> semifinishedProductIndexes = this.semifinishedProductAPIRepository.GetEntityIndexes<FinishedProductIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = semifinishedProductIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }




        public JsonResult GetFirmOrders([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.semifinishedProductAPIRepository.GetFirmOrders(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

    }
}