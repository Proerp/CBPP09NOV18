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
    public class SemifinishedRecyclateAPIsController : Controller
    {
        private readonly ISemifinishedRecyclateAPIRepository semisemifinishedRecyclateAPIRepository;

        public SemifinishedRecyclateAPIsController(ISemifinishedRecyclateAPIRepository semisemifinishedRecyclateAPIRepository)
        {
            this.semisemifinishedRecyclateAPIRepository = semisemifinishedRecyclateAPIRepository;
        }


        public JsonResult GetSemifinishedRecyclateIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<SemifinishedRecyclateIndex> semisemifinishedRecyclateIndexes = this.semisemifinishedRecyclateAPIRepository.GetEntityIndexes<SemifinishedRecyclateIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = semisemifinishedRecyclateIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }




        public JsonResult GetPendingWorkshifts([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.semisemifinishedRecyclateAPIRepository.GetPendingWorkshifts(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

    }
}