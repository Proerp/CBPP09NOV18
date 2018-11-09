using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;

using Microsoft.AspNet.Identity;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using TotalModel.Models;
using TotalCore.Repositories.Purchases;

using TotalPortal.APIs.Sessions;


namespace TotalPortal.Areas.Purchases.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class PurchaseRequisitionAPIsController : Controller
    {
        private readonly IPurchaseRequisitionAPIRepository purchaseRequisitionAPIRepository;

        public PurchaseRequisitionAPIsController(IPurchaseRequisitionAPIRepository purchaseRequisitionAPIRepository)
        {
            this.purchaseRequisitionAPIRepository = purchaseRequisitionAPIRepository;
        }


        public JsonResult GetPurchaseRequisitionIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<PurchaseRequisitionIndex> purchaseRequisitionIndexes = this.purchaseRequisitionAPIRepository.GetEntityIndexes<PurchaseRequisitionIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = purchaseRequisitionIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
