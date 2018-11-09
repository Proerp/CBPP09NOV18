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
    public class SemifinishedProductAPIsController : Controller
    {
        private readonly ISemifinishedProductAPIRepository semifinishedProductAPIRepository;

        public SemifinishedProductAPIsController(ISemifinishedProductAPIRepository semifinishedProductAPIRepository)
        {
            this.semifinishedProductAPIRepository = semifinishedProductAPIRepository;
        }


        public JsonResult GetSemifinishedProductIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<SemifinishedProductIndex> semifinishedProductIndexes = this.semifinishedProductAPIRepository.GetEntityIndexes<SemifinishedProductIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = semifinishedProductIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }




        public JsonResult GetMaterialIssueDetails([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.semifinishedProductAPIRepository.GetMaterialIssueDetails(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }      

    }
}