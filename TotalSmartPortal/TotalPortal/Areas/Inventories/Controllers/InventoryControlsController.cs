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
using TotalCore.Repositories.Inventories;

namespace TotalPortal.Areas.Inventories.Controllers
{
    public class InventoryControlsController : CoreController
    {
        private IInventoryControlAPIRepository inventoryControlAPIRepository;
        
        public InventoryControlsController(IInventoryControlAPIRepository inventoryControlAPIRepository)
        {
            this.inventoryControlAPIRepository = inventoryControlAPIRepository;
        }

        public ActionResult Summaries()
        {
            return View();
        }

        public ActionResult Details()
        {
            return View();
        }
    }
}