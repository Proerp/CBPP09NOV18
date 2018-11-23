using System.Net;
using System.Web.Mvc;
using System.Text;

using AutoMapper;
using RequireJsNet;

using TotalBase.Enums;

using TotalModel.Models;

using TotalCore.Services.Inventories;
using TotalCore.Repositories.Commons;

using TotalDTO.Inventories;

using TotalPortal.Controllers;
using TotalPortal.APIs.Sessions;
using TotalPortal.Areas.Inventories.ViewModels;
using TotalPortal.Areas.Inventories.Builders;
using TotalPortal.Areas.Inventories.Controllers.Sessions;
using TotalPortal.Areas.Commons.Controllers.Sessions;

namespace TotalPortal.Areas.Inventories.Controllers
{
    public class PackageIssuesController : GenericViewDetailController<PackageIssue, PackageIssueDetail, PackageIssueViewDetail, PackageIssueDTO, PackageIssuePrimitiveDTO, PackageIssueDetailDTO, PackageIssueViewModel>
    {
        public PackageIssuesController(IPackageIssueService packageIssueService, IPackageIssueViewModelSelectListBuilder packageIssueViewModelSelectListBuilder)
            : base(packageIssueService, packageIssueViewModelSelectListBuilder, true)
        {
        }

        public override void AddRequireJsOptions()
        {
            base.AddRequireJsOptions();

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Items);
            commodityTypeIDList.Append(","); commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Consumables);

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);


            StringBuilder warehouseTaskIDList = new StringBuilder();
            warehouseTaskIDList.Append((int)GlobalEnums.WarehouseTaskID.DeliveryAdvice);

            ViewBag.WarehouseTaskID = (int)GlobalEnums.WarehouseTaskID.DeliveryAdvice;
            ViewBag.WarehouseTaskIDList = warehouseTaskIDList.ToString();
        }

        public virtual ActionResult GetPendingBlendingInstructionDetails()
        {
            this.AddRequireJsOptions();
            return View();
        }

        protected override PackageIssueViewModel InitViewModelByDefault(PackageIssueViewModel simpleViewModel)
        {
            simpleViewModel = base.InitViewModelByDefault(simpleViewModel);

            if (simpleViewModel.ShiftID == 0)
            {
                string shiftSession = ShiftSession.GetShift(this.HttpContext);
                if (HomeSession.TryParseID(shiftSession) > 0) simpleViewModel.ShiftID = (int)HomeSession.TryParseID(shiftSession);
            }

            if (simpleViewModel.Storekeeper == null)
            {
                string storekeeperSession = PackageIssueSession.GetStorekeeper(this.HttpContext);

                if (HomeSession.TryParseID(storekeeperSession) > 0)
                {
                    simpleViewModel.Storekeeper = new TotalDTO.Commons.EmployeeBaseDTO();
                    simpleViewModel.Storekeeper.EmployeeID = (int)HomeSession.TryParseID(storekeeperSession);
                    simpleViewModel.Storekeeper.Name = HomeSession.TryParseName(storekeeperSession);
                }
            }

            if (simpleViewModel.CrucialWorker == null)
            {
                string storekeeperSession = PackageIssueSession.GetCrucialWorker(this.HttpContext);

                if (HomeSession.TryParseID(storekeeperSession) > 0)
                {
                    simpleViewModel.CrucialWorker = new TotalDTO.Commons.EmployeeBaseDTO();
                    simpleViewModel.CrucialWorker.EmployeeID = (int)HomeSession.TryParseID(storekeeperSession);
                    simpleViewModel.CrucialWorker.Name = HomeSession.TryParseName(storekeeperSession);
                }
            }

            return simpleViewModel;
        }

        protected override void BackupViewModelToSession(PackageIssueViewModel simpleViewModel)
        {
            base.BackupViewModelToSession(simpleViewModel);
            ShiftSession.SetShift(this.HttpContext, simpleViewModel.ShiftID);
            PackageIssueSession.SetStorekeeper(this.HttpContext, simpleViewModel.Storekeeper.EmployeeID, simpleViewModel.Storekeeper.Name);
            PackageIssueSession.SetCrucialWorker(this.HttpContext, simpleViewModel.CrucialWorker.EmployeeID, simpleViewModel.CrucialWorker.Name);
        }
    }

}