using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using System.Web.UI;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;


using TotalBase.Enums;
using TotalModel.Models;

using TotalDTO.Inventories;

using TotalCore.Repositories.Inventories;
using TotalPortal.Areas.Inventories.ViewModels;
using TotalPortal.APIs.Sessions;

using Microsoft.AspNet.Identity;

namespace TotalPortal.Areas.Inventories.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class InventoryControlAPIsController : Controller
    {
        private readonly IInventoryControlAPIRepository inventoryControlAPIRepository;

        public InventoryControlAPIsController(IInventoryControlAPIRepository inventoryControlAPIRepository)
        {
            this.inventoryControlAPIRepository = inventoryControlAPIRepository;
        }



        public JsonResult GetBlendingInstructions([DataSourceRequest] DataSourceRequest dataSourceRequest, bool? summaryOnly, int? labOptionID, int? filterOptionID, int? pendingOptionID, int? shelfLife)
        {
            var result = this.inventoryControlAPIRepository.GetInventoryControls(User.Identity.GetUserId(), summaryOnly, labOptionID, filterOptionID, pendingOptionID, shelfLife);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

    }
}