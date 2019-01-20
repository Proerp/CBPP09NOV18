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
    public class SemifinishedItemAPIsController : Controller
    {
        private readonly ISemifinishedItemAPIRepository semifinishedItemAPIRepository;

        public SemifinishedItemAPIsController(ISemifinishedItemAPIRepository semifinishedItemAPIRepository)
        {
            this.semifinishedItemAPIRepository = semifinishedItemAPIRepository;
        }


        public JsonResult GetSemifinishedItemIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<SemifinishedItemIndex> semifinishedItemIndexes = this.semifinishedItemAPIRepository.GetEntityIndexes<SemifinishedItemIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = semifinishedItemIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }




        public JsonResult GetMaterialIssues([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.semifinishedItemAPIRepository.GetMaterialIssues(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

    }
}