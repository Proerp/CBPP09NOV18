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
    public class SemifinishedHandoverAPIsController : Controller
    {
        private readonly ISemifinishedHandoverAPIRepository semifinishedHandoverAPIRepository;

        public SemifinishedHandoverAPIsController(ISemifinishedHandoverAPIRepository semifinishedHandoverAPIRepository)
        {
            this.semifinishedHandoverAPIRepository = semifinishedHandoverAPIRepository;
        }


        public JsonResult GetSemifinishedHandoverIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<SemifinishedHandoverIndex> semifinishedHandoverIndexes = this.semifinishedHandoverAPIRepository.GetEntityIndexes<SemifinishedHandoverIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = semifinishedHandoverIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCustomers([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.semifinishedHandoverAPIRepository.GetCustomers(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWorkshifts([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.semifinishedHandoverAPIRepository.GetWorkshifts(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPendingDetails([DataSourceRequest] DataSourceRequest dataSourceRequest, int? semifinishedHandoverID, int? workshiftID, int? customerID, string semifinishedProductIDs, bool? isReadonly)
        {
            var result = this.semifinishedHandoverAPIRepository.GetPendingDetails(semifinishedHandoverID, workshiftID, customerID, semifinishedProductIDs, false);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }
    }
}