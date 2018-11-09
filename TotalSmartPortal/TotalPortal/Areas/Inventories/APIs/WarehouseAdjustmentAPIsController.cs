using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;

using Microsoft.AspNet.Identity;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using TotalModel.Models;
using TotalCore.Repositories.Inventories;

using TotalPortal.APIs.Sessions;

namespace TotalPortal.Areas.Inventories.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class WarehouseAdjustmentAPIsController : Controller
    {
        private readonly IWarehouseAdjustmentAPIRepository warehouseAdjustmentAPIRepository;

        public WarehouseAdjustmentAPIsController(IWarehouseAdjustmentAPIRepository warehouseAdjustmentAPIRepository)
        {
            this.warehouseAdjustmentAPIRepository = warehouseAdjustmentAPIRepository;
        }


        public JsonResult GetWarehouseAdjustmentIndexes([DataSourceRequest] DataSourceRequest request, string nmvnTaskID)
        {
            this.warehouseAdjustmentAPIRepository.RepositoryBag["NMVNTaskID"] = nmvnTaskID;
            ICollection<WarehouseAdjustmentIndex> warehouseAdjustmentIndexes = this.warehouseAdjustmentAPIRepository.GetEntityIndexes<WarehouseAdjustmentIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = warehouseAdjustmentIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}