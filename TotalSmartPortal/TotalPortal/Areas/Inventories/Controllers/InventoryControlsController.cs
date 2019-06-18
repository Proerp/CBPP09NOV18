using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using TotalCore.Repositories.Analysis;
using TotalModel.Models;
using RequireJsNet;
using TotalPortal.APIs.Sessions;
using System.Net;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Models;
using TotalCore.Helpers;
using TotalCore.Repositories.Sessions;
using TotalPortal.Controllers;
using TotalPortal.Areas.Inventories.ViewModels;
using TotalCore.Repositories.Inventories;

namespace TotalPortal.Areas.Inventories.Controllers
{
    [Authorize]
    public class InventoryControlsController : CoreController
    {
        private IInventoryControlAPIRepository inventoryControlAPIRepository;
        
        public InventoryControlsController(IInventoryControlAPIRepository inventoryControlAPIRepository)
        {
            this.inventoryControlAPIRepository = inventoryControlAPIRepository;
        }

        public ActionResult Summaries()
        {
            this.AddRequireJsOptions(6668805);

            InventoryControlViewModel inventoryControlViewModel = new InventoryControlViewModel() { LocationID = 1 };

            return View(inventoryControlViewModel);
        }

        public ActionResult Details(int? id)
        {
            this.AddRequireJsOptions(6668809);

            InventoryControlViewModel inventoryControlViewModel = new InventoryControlViewModel() { LocationID = 1 };

            if (id != null)
            {
                Commodity commodity = this.inventoryControlAPIRepository.TotalSmartPortalEntities.Commodities.Where(w => w.CommodityID == id).FirstOrDefault();
                if (commodity != null) { inventoryControlViewModel.CommodityID = commodity.CommodityID; inventoryControlViewModel.CommodityCode = commodity.Code; }
            }

            return View(inventoryControlViewModel);
        }

        //FOR SIMPLICITY, AT NOW (JUST FOR HIGHTLIGHT MENUONLY): JUST CALL THIS. BUT LATER, WE CAN INHERIT FROM BaseController
        public virtual void AddRequireJsOptions(int nmvnTaskID)
        {
            MenuSession.SetModuleID(this.HttpContext, 3);
            MenuSession.SetModuleDetailID(this.HttpContext, nmvnTaskID);

            RequireJsOptions.Add("ModuleID", 3, RequireJsOptionsScope.Page);
            RequireJsOptions.Add("ModuleDetailID", nmvnTaskID, RequireJsOptionsScope.Page);
            RequireJsOptions.Add("NmvnTaskID", nmvnTaskID, RequireJsOptionsScope.Page);
        }
    }
}