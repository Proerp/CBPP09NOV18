using System;
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

        [HttpPost]
        public JsonResult SetReportParameters(DateTime fromDate, DateTime toDate)
        {
            try
            {
                HomeSession.SetReportFromDate(this.HttpContext, fromDate);
                HomeSession.SetReportToDate(this.HttpContext, toDate.AddHours(23).AddMinutes(59).AddSeconds(59));

                return Json(new { AddResult = "Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { AddResult = "Lỗi cài ngày xem báo cáo, hoặc " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}

