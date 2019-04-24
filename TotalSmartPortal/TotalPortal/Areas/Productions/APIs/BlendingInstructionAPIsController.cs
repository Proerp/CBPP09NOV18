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
    public class BlendingInstructionAPIsController : Controller
    {
        private readonly IBlendingInstructionAPIRepository blendingInstructionAPIRepository;

        public BlendingInstructionAPIsController(IBlendingInstructionAPIRepository blendingInstructionAPIRepository)
        {
            this.blendingInstructionAPIRepository = blendingInstructionAPIRepository;
        }


        public JsonResult GetBlendingInstructionIndexes([DataSourceRequest] DataSourceRequest request, bool withExtendedSearch, DateTime extendedFromDate, DateTime extendedToDate, int filterOptionID, int labOptionID)
        {
            this.blendingInstructionAPIRepository.RepositoryBag["LabOptionID"] = labOptionID;
            this.blendingInstructionAPIRepository.RepositoryBag["FilterOptionID"] = filterOptionID;            
            ICollection<BlendingInstructionIndex> blendingInstructionIndexes = this.blendingInstructionAPIRepository.GetEntityIndexes<BlendingInstructionIndex>(User.Identity.GetUserId(), (withExtendedSearch ? extendedFromDate : HomeSession.GetGlobalFromDate(this.HttpContext)), (withExtendedSearch ? extendedToDate : HomeSession.GetGlobalToDate(this.HttpContext)));

            DataSourceResult response = blendingInstructionIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetRunnings([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.blendingInstructionAPIRepository.GetRunnings(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBlendingInstructionLogs([DataSourceRequest] DataSourceRequest dataSourceRequest, int? blendingInstructionID)
        {
            var result = this.blendingInstructionAPIRepository.GetBlendingInstructionLogs(blendingInstructionID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }
    }
}