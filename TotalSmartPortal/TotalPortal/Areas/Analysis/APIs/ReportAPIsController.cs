using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using TotalCore.Repositories.Analysis;
using TotalModel.Models;
using TotalPortal.APIs.Sessions;

using Microsoft.AspNet.Identity;


namespace TotalPortal.Areas.Analysis.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class ReportAPIsController : Controller
    {
        private readonly IReportAPIRepository reportAPIRepository;

        public ReportAPIsController(IReportAPIRepository reportAPIRepository)
        {
            this.reportAPIRepository = reportAPIRepository;
        }

        public JsonResult GetReportIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<ReportIndex> reportIndexes = this.reportAPIRepository.GetEntityIndexes<ReportIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = reportIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}

