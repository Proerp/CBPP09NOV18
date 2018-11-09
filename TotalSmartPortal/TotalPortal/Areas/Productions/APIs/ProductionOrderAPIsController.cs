using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;

using Microsoft.AspNet.Identity;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using TotalModel.Models;
using TotalCore.Repositories.Productions;

using TotalPortal.APIs.Sessions;

namespace TotalPortal.Areas.Productions.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class ProductionOrderAPIsController : Controller
    {
        private readonly IProductionOrderAPIRepository productionOrderAPIRepository;

        public ProductionOrderAPIsController(IProductionOrderAPIRepository productionOrderAPIRepository)
        {
            this.productionOrderAPIRepository = productionOrderAPIRepository;
        }


        public JsonResult GetProductionOrderIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<ProductionOrderIndex> productionOrderIndexes = this.productionOrderAPIRepository.GetEntityIndexes<ProductionOrderIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = productionOrderIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCustomers([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.productionOrderAPIRepository.GetCustomers(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPlannedOrders([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.productionOrderAPIRepository.GetPlannedOrders(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPendingFirmOrders([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? productionOrderID, int? plannedOrderID, int? customerID, string plannedOrderDetailIDs, bool isReadonly)
        {
            var result = this.productionOrderAPIRepository.GetPendingFirmOrders(locationID, productionOrderID, plannedOrderID, customerID, plannedOrderDetailIDs, false);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

    }
}