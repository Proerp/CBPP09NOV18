using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using TotalCore.Repositories.Commons;
using TotalModel.Models;
using TotalDTO.Commons;
using TotalPortal.APIs.Sessions;

using Microsoft.AspNet.Identity;


namespace TotalPortal.Areas.Commons.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class BinLocationAPIsController : Controller
    {
        private readonly IBinLocationAPIRepository binLocationAPIRepository;

        public BinLocationAPIsController(IBinLocationAPIRepository binLocationAPIRepository)
        {
            this.binLocationAPIRepository = binLocationAPIRepository;
        }

        public JsonResult GetBinLocationIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<BinLocationIndex> binLocationIndexes = this.binLocationAPIRepository.GetEntityIndexes<BinLocationIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = binLocationIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBinLocationBases(int? warehouseID, string searchText)
        {
            var result = this.binLocationAPIRepository.GetBinLocationBases(warehouseID, searchText);
            return Json(result, JsonRequestBehavior.AllowGet);
        }        
    }
}

