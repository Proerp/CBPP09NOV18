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
    public class TransferOrderAPIsController : Controller
    {
        private readonly ITransferOrderAPIRepository transferOrderAPIRepository;

        public TransferOrderAPIsController(ITransferOrderAPIRepository transferOrderAPIRepository)
        {
            this.transferOrderAPIRepository = transferOrderAPIRepository;
        }


        public JsonResult GetTransferOrderIndexes([DataSourceRequest] DataSourceRequest request, string nmvnTaskID)
        {
            this.transferOrderAPIRepository.RepositoryBag["NMVNTaskID"] = nmvnTaskID;
            ICollection<TransferOrderIndex> transferOrderIndexes = this.transferOrderAPIRepository.GetEntityIndexes<TransferOrderIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = transferOrderIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}