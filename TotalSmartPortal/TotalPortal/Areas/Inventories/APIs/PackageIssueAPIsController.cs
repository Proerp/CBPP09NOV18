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

using TotalDTO.Inventories;

using TotalCore.Repositories.Inventories;
using TotalPortal.Areas.Inventories.ViewModels;
using TotalPortal.APIs.Sessions;

using Microsoft.AspNet.Identity;

namespace TotalPortal.Areas.Inventories.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class PackageIssueAPIsController : Controller
    {
        private readonly IPackageIssueAPIRepository packageIssueAPIRepository;

        public PackageIssueAPIsController(IPackageIssueAPIRepository packageIssueAPIRepository)
        {
            this.packageIssueAPIRepository = packageIssueAPIRepository;
        }


        public JsonResult GetPackageIssueIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<PackageIssueIndex> packageIssueIndexes = this.packageIssueAPIRepository.GetEntityIndexes<PackageIssueIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = packageIssueIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }





        public JsonResult GetBlendingInstructions([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? blendingInstructionID)
        {
            var result = this.packageIssueAPIRepository.GetBlendingInstructions(locationID, blendingInstructionID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPendingBlendingInstructionDetails([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? packageIssueID, int? blendingInstructionID, int? warehouseID, string barcode, string goodsReceiptDetailIDs)
        {
            var result = this.packageIssueAPIRepository.GetPendingBlendingInstructionDetails(false, locationID, packageIssueID, blendingInstructionID, warehouseID, barcode, goodsReceiptDetailIDs);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }



    }
}