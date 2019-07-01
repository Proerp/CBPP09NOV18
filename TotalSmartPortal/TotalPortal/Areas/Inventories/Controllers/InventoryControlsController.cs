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
using TotalCore.Services.Commons;
using TotalDTO.Commons;
using TotalPortal.Areas.Commons.ViewModels;
using TotalPortal.Areas.Commons.Builders;

namespace TotalPortal.Areas.Inventories.Controllers
{
    [Authorize]
    public class InventoryControlsController : GenericSimpleController<BinLocation, BinLocationDTO, BinLocationPrimitiveDTO, BinLocationViewModel>
    {
        private IBinLocationService binLocationService; //Temporary use BinLocationService to get LocationID
        private IInventoryControlAPIRepository inventoryControlAPIRepository;

        public InventoryControlsController(IBinLocationService binLocationService, IInventoryControlAPIRepository inventoryControlAPIRepository, IBinLocationSelectListBuilder binLocationViewModelSelectListBuilder)
            : base(binLocationService, binLocationViewModelSelectListBuilder)
        {
            this.binLocationService = binLocationService;
            this.inventoryControlAPIRepository = inventoryControlAPIRepository;
        }

        public ActionResult Summaries()
        {
            this.AddRequireJsOptions(6668805);

            InventoryControlViewModel inventoryControlViewModel = new InventoryControlViewModel() { LocationID = this.binLocationService.LocationID, SummaryOptionID = this.binLocationService.LocationID == 2 ? 0 : 20 };

            return View(inventoryControlViewModel);
        }

        public ActionResult Details(int? id, int? detailID)
        {
            this.AddRequireJsOptions(6668809);

            InventoryControlViewModel inventoryControlViewModel = new InventoryControlViewModel() { LocationID = (detailID != null ? (int)detailID : this.binLocationService.LocationID), SummaryOptionID = (detailID != null ? (int)detailID : this.binLocationService.LocationID) == 2 ? 0 : 20 };

            if (id != null && id > 0)
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