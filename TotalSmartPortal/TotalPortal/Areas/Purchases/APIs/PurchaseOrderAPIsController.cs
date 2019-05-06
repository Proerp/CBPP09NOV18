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
    public class PurchaseOrderAPIsController : Controller
    {
        private readonly IPurchaseOrderAPIRepository purchaseOrderAPIRepository;

        public PurchaseOrderAPIsController(IPurchaseOrderAPIRepository purchaseOrderAPIRepository)
        {
            this.purchaseOrderAPIRepository = purchaseOrderAPIRepository;
        }


        public JsonResult GetPurchaseOrderIndexes([DataSourceRequest] DataSourceRequest request, string nmvnTaskID)
        {
            this.purchaseOrderAPIRepository.RepositoryBag["NMVNTaskID"] = nmvnTaskID;
            ICollection<PurchaseOrderIndex> purchaseOrderIndexes = this.purchaseOrderAPIRepository.GetEntityIndexes<PurchaseOrderIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = purchaseOrderIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
