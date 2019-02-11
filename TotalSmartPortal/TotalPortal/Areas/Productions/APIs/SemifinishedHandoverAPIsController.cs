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


        public JsonResult GetSemifinishedHandoverIndexes([DataSourceRequest] DataSourceRequest request, int nmvnTaskID)
        {
            this.semifinishedHandoverAPIRepository.RepositoryBag["NMVNTaskID"] = nmvnTaskID;
            ICollection<SemifinishedHandoverIndex> semifinishedHandoverIndexes = this.semifinishedHandoverAPIRepository.GetEntityIndexes<SemifinishedHandoverIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = semifinishedHandoverIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCustomers([DataSourceRequest] DataSourceRequest dataSourceRequest, int? nmvnTaskID, int? locationID)
        {
            var result = this.semifinishedHandoverAPIRepository.GetCustomers(nmvnTaskID, locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWorkshifts([DataSourceRequest] DataSourceRequest dataSourceRequest, int? nmvnTaskID, int? locationID)
        {
            var result = this.semifinishedHandoverAPIRepository.GetWorkshifts(nmvnTaskID, locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPendingDetails([DataSourceRequest] DataSourceRequest dataSourceRequest, int? nmvnTaskID, int? semifinishedHandoverID, int? workshiftID, int? customerID, string semifinishedItemIDs, string semifinishedProductIDs)
        {
            var result = this.semifinishedHandoverAPIRepository.GetPendingDetails(nmvnTaskID, semifinishedHandoverID, workshiftID, customerID, semifinishedItemIDs, semifinishedProductIDs);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }
    }
}