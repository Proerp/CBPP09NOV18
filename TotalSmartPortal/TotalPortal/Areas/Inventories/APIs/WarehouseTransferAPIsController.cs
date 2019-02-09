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
    public class WarehouseTransferAPIsController : Controller
    {
        private readonly IWarehouseTransferAPIRepository warehouseTransferAPIRepository;

        public WarehouseTransferAPIsController(IWarehouseTransferAPIRepository warehouseTransferAPIRepository)
        {
            this.warehouseTransferAPIRepository = warehouseTransferAPIRepository;
        }


        public JsonResult GetWarehouseTransferIndexes([DataSourceRequest] DataSourceRequest request, string nmvnTaskID)
        {
            this.warehouseTransferAPIRepository.RepositoryBag["NMVNTaskID"] = nmvnTaskID;
            ICollection<WarehouseTransferIndex> transferOrderIndexes = this.warehouseTransferAPIRepository.GetEntityIndexes<WarehouseTransferIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = transferOrderIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAvailableWarehouses([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? nmvnTaskID)
        {
            var result = this.warehouseTransferAPIRepository.GetAvailableWarehouses(locationID, nmvnTaskID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPendingWarehouses([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? nmvnTaskID)
        {
            var result = this.warehouseTransferAPIRepository.GetPendingWarehouses(locationID, nmvnTaskID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransferOrders([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? nmvnTaskID)
        {
            var result = this.warehouseTransferAPIRepository.GetTransferOrders(locationID, nmvnTaskID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransferOrderDetails([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? nmvnTaskID, int? warehouseTransferID, int? transferOrderID, int? warehouseID, int? warehouseReceiptID, string goodsReceiptDetailIDs)
        {
            var result = this.warehouseTransferAPIRepository.GetTransferOrderDetails(false, locationID, nmvnTaskID, warehouseTransferID, transferOrderID, warehouseID, warehouseReceiptID, goodsReceiptDetailIDs);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }
    }
}