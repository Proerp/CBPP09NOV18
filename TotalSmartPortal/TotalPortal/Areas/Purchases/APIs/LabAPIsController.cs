using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using System.Web.UI;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using Microsoft.AspNet.Identity;

using TotalModel.Models;
using TotalDTO.Purchases;

using TotalCore.Repositories.Purchases;
using TotalPortal.Areas.Purchases.ViewModels;
using TotalPortal.APIs.Sessions;


namespace TotalPortal.Areas.Purchases.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class LabAPIsController : Controller
    {
        private readonly ILabRepository labRepository;
        private readonly ILabAPIRepository labAPIRepository;

        public LabAPIsController(ILabRepository labRepository, ILabAPIRepository labAPIRepository)
        {
            this.labRepository = labRepository;
            this.labAPIRepository = labAPIRepository;
        }

        public JsonResult GetLabIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<LabIndex> labIndexes = this.labAPIRepository.GetEntityIndexes<LabIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = labIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}