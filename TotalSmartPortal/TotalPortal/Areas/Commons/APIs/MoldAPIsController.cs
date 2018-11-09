using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using Microsoft.AspNet.Identity;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using TotalCore.Repositories.Commons;
using TotalModel.Models;
using TotalDTO.Commons;
using TotalPortal.APIs.Sessions;
using System;

namespace TotalPortal.Areas.Commons.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class MoldAPIsController : Controller
    {
        private readonly IMoldAPIRepository moldAPIRepository;

        public MoldAPIsController(IMoldAPIRepository moldAPIRepository)
        {
            this.moldAPIRepository = moldAPIRepository;
        }

        public JsonResult GetMoldIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<MoldIndex> moldIndexes = this.moldAPIRepository.GetEntityIndexes<MoldIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = moldIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMoldBases(string searchText, int commodityID)
        {
            var result = this.moldAPIRepository.GetMoldBases(searchText, commodityID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetCommodityMolds([DataSourceRequest] DataSourceRequest dataSourceRequest, int? commodityID)
        {
            if (commodityID == null) return Json(null);

            var result = moldAPIRepository.GetCommodityMolds((int)commodityID);

            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddCommodityMold(int? moldID, int? commodityID, decimal quantity)
        {
            try
            {
                this.moldAPIRepository.AddCommodityMold(moldID, commodityID, quantity);
                return Json(new { AddResult = "Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { AddResult = "Trùng dữ liệu, hoặc " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult RemoveCommodityMold(int? commodityMoldID)
        {
            try
            {
                this.moldAPIRepository.RemoveCommodityMold(commodityMoldID);
                return Json(new { RemoveResult = "Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { RemoveResult = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateCommodityMold(int? commodityMoldID, int commodityID, decimal quantity, string remarks, bool? isDefault)
        {
            try
            {
                this.moldAPIRepository.UpdateCommodityMold(commodityMoldID, commodityID, quantity, remarks, isDefault);
                return Json(new { SetResult = "Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { SetResult = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}