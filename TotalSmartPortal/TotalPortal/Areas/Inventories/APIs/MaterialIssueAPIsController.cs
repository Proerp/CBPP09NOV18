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
    public class MaterialIssueAPIsController : Controller
    {
        private readonly IMaterialIssueAPIRepository materialIssueAPIRepository;

        public MaterialIssueAPIsController(IMaterialIssueAPIRepository materialIssueAPIRepository)
        {
            this.materialIssueAPIRepository = materialIssueAPIRepository;
        }


        public JsonResult GetMaterialIssueIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<MaterialIssueIndex> materialIssueIndexes = this.materialIssueAPIRepository.GetEntityIndexes<MaterialIssueIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = materialIssueIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }





        public JsonResult GetFirmOrders([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? firmOrderID)
        {
            var result = this.materialIssueAPIRepository.GetFirmOrders(locationID, firmOrderID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPendingFirmOrderMaterials([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? materialIssueID, int? firmOrderID, int? warehouseID, string goodsReceiptDetailIDs, bool isReadonly)
        {
            var result = this.materialIssueAPIRepository.GetPendingFirmOrderMaterials(locationID, materialIssueID, firmOrderID, warehouseID, goodsReceiptDetailIDs, isReadonly);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }



    }
}