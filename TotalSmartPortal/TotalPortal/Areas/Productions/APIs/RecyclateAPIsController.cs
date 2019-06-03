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
    public class RecyclateAPIsController : Controller
    {
        private readonly IRecyclateAPIRepository recyclateAPIRepository;

        public RecyclateAPIsController(IRecyclateAPIRepository recyclateAPIRepository)
        {
            this.recyclateAPIRepository = recyclateAPIRepository;
        }


        public JsonResult GetRecyclateIndexes([DataSourceRequest] DataSourceRequest request, int nmvnTaskID)
        {
            this.recyclateAPIRepository.RepositoryBag["NMVNTaskID"] = nmvnTaskID;
            ICollection<RecyclateIndex> recyclateIndexes = this.recyclateAPIRepository.GetEntityIndexes<RecyclateIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = recyclateIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }




        public JsonResult GetPendingWorkshifts([DataSourceRequest] DataSourceRequest dataSourceRequest, int? nmvnTaskID, int? locationID)
        {
            var result = this.recyclateAPIRepository.GetPendingWorkshifts(nmvnTaskID, locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

    }
}