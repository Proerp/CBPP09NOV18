using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using Microsoft.AspNet.Identity;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using TotalDTO.Commons;
using TotalModel.Models;
using TotalCore.Repositories.Commons;
using TotalPortal.APIs.Sessions;


namespace TotalPortal.Areas.Commons.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class BomAPIsController : Controller
    {
        private readonly IBomAPIRepository bomAPIRepository;

        public BomAPIsController(IBomAPIRepository bomAPIRepository)
        {
            this.bomAPIRepository = bomAPIRepository;
        }

        public JsonResult GetBomIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<BomIndex> bomIndexes = this.bomAPIRepository.GetEntityIndexes<BomIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = bomIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetBomBases(string searchText, int commodityID, int commodityTypeID, int commodityCategoryID, int commodityClassID, int commodityLineID)
        {
            var result = this.bomAPIRepository.GetBomBases(searchText, commodityID, commodityTypeID, commodityCategoryID, commodityClassID, commodityLineID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBomValues([DataSourceRequest] DataSourceRequest dataSourceRequest, int bomID, decimal quantity, bool overStockOnly)
        {
            var result = bomAPIRepository.GetBomValues(bomID, quantity, overStockOnly);

            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetCommodityBoms([DataSourceRequest] DataSourceRequest dataSourceRequest, int? bomID, int? commodityID)
        {
            if (bomID == null && commodityID == null) return Json(null);

            var result = bomAPIRepository.GetCommodityBoms(bomID, commodityID); 
            
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddCommodityBom(int? bomID, int? commodityID)
        {
            try
            {
                this.bomAPIRepository.AddCommodityBom(bomID, commodityID);
                return Json(new { AddResult = "Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { AddResult = "Trùng dữ liệu, hoặc " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult RemoveCommodityBom(int? commodityBomID)
        {
            try
            {
                this.bomAPIRepository.RemoveCommodityBom(commodityBomID);
                return Json(new { RemoveResult = "Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { RemoveResult = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateCommodityBom(int? commodityBomID, int commodityID, decimal blockUnit, decimal blockQuantity, string remarks, bool? isDefault)
        {
            try
            {
                this.bomAPIRepository.UpdateCommodityBom(commodityBomID, commodityID, blockUnit, blockQuantity, remarks, isDefault);
                return Json(new { SetResult = "Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { SetResult = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}