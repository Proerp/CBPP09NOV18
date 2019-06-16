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
    public class BarcodeAPIsController : Controller
    {
        private readonly IBarcodeAPIRepository barcodeAPIRepository;

        public BarcodeAPIsController(IBarcodeAPIRepository barcodeAPIRepository)
        {
            this.barcodeAPIRepository = barcodeAPIRepository;
        }


        public JsonResult GetBarcodeBasics(string searchText)
        {
            var result = this.barcodeAPIRepository.GetBarcodeBasics(searchText);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBarcodeJournals([DataSourceRequest] DataSourceRequest dataSourceRequest, string barcode)
        {
            var result = barcodeAPIRepository.GetBarcodeJournals(barcode);

            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }
    }
}