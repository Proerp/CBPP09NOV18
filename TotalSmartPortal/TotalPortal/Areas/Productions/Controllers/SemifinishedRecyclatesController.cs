using System.Net;
using System.Web.Mvc;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using AutoMapper;
using RequireJsNet;

using TotalBase;
using TotalBase.Enums;

using TotalModel.Models;

using TotalCore.Services.Productions;
using TotalCore.Repositories.Commons;

using TotalDTO.Productions;

using TotalPortal.APIs.Sessions;

using TotalPortal.Controllers;
using TotalPortal.Areas.Commons.Controllers.Sessions;
using TotalPortal.Areas.Productions.ViewModels;
using TotalPortal.Areas.Productions.Builders;
using TotalPortal.Areas.Productions.Controllers.Sessions;

namespace TotalPortal.Areas.Productions.Controllers
{
    public class SemifinishedRecyclatesController : GenericViewDetailController<SemifinishedRecyclate, SemifinishedRecyclateDetail, SemifinishedRecyclateViewDetail, SemifinishedRecyclateDTO, SemifinishedRecyclatePrimitiveDTO, SemifinishedRecyclateDetailDTO, SemifinishedRecyclateViewModel>
    {
        private readonly ISemifinishedRecyclateService semifinishedRecyclateService;

        public SemifinishedRecyclatesController(ISemifinishedRecyclateService semifinishedRecyclateService, ISemifinishedRecyclateViewModelSelectListBuilder semifinishedRecyclateViewModelSelectListBuilder)
            : base(semifinishedRecyclateService, semifinishedRecyclateViewModelSelectListBuilder, true)
        {
            this.semifinishedRecyclateService = semifinishedRecyclateService;
        }

        protected override ICollection<SemifinishedRecyclateViewDetail> GetEntityViewDetails(SemifinishedRecyclateViewModel semifinishedRecyclateViewModel)
        {
            ICollection<SemifinishedRecyclateViewDetail> semifinishedRecyclateViewDetails = this.semifinishedRecyclateService.GetSemifinishedRecyclateViewDetails(semifinishedRecyclateViewModel.SemifinishedRecyclateID, this.semifinishedRecyclateService.LocationID, semifinishedRecyclateViewModel.WorkshiftID, false);

            return semifinishedRecyclateViewDetails;
        }

        protected override SemifinishedRecyclateViewModel TailorViewModel(SemifinishedRecyclateViewModel semifinishedRecyclateViewModel, bool forDelete, bool forAlter, bool forOpen)
        {
            if (semifinishedRecyclateViewModel.ViewDetails.Where(w => w.RecycleCommodityID == null).Count() > 0)
            {
                var semifinishedRecyclatePackages = semifinishedRecyclateViewModel.ViewDetails
                                                .GroupBy(g => g.RecycleCommodityID)
                                                .Select(sl => new SemifinishedRecyclatePackageDTO
                                                {
                                                    CommodityID = (int)sl.First().RecycleCommodityID,
                                                    CommodityCode = sl.First().RecycleCommodityCode,
                                                    CommodityName = sl.First().RecycleCommodityName,

                                                    RejectWeights = sl.First().RejectWeights,
                                                    FailureWeights = sl.First().FailureWeights,

                                                    QuantityRemains = sl.Sum(s => s.QuantityRemains),
                                                    Quantity = sl.Sum(s => s.Quantity)
                                                });

                semifinishedRecyclateViewModel.SemifinishedRecyclateSummaries = semifinishedRecyclatePackages.ToList();
            }
            return base.TailorViewModel(semifinishedRecyclateViewModel, forDelete, forAlter, forOpen);
        }

        protected override SemifinishedRecyclateViewModel InitViewModelByDefault(SemifinishedRecyclateViewModel simpleViewModel)
        {
            simpleViewModel = base.InitViewModelByDefault(simpleViewModel);

            if (simpleViewModel.ShiftID == 0)
            {
                string shiftSession = ShiftSession.GetShift(this.HttpContext);
                if (HomeSession.TryParseID(shiftSession) > 0) simpleViewModel.ShiftID = (int)HomeSession.TryParseID(shiftSession);
            }

            if (simpleViewModel.CrucialWorker == null)
            {
                string storekeeperSession = SemifinishedRecyclateSession.GetCrucialWorker(this.HttpContext);

                if (HomeSession.TryParseID(storekeeperSession) > 0)
                {
                    simpleViewModel.CrucialWorker = new TotalDTO.Commons.EmployeeBaseDTO();
                    simpleViewModel.CrucialWorker.EmployeeID = (int)HomeSession.TryParseID(storekeeperSession);
                    simpleViewModel.CrucialWorker.Name = HomeSession.TryParseName(storekeeperSession);
                }
            }

            if (simpleViewModel.Storekeeper == null)
            {
                string storekeeperSession = SemifinishedRecyclateSession.GetStorekeeper(this.HttpContext);

                if (HomeSession.TryParseID(storekeeperSession) > 0)
                {
                    simpleViewModel.Storekeeper = new TotalDTO.Commons.EmployeeBaseDTO();
                    simpleViewModel.Storekeeper.EmployeeID = (int)HomeSession.TryParseID(storekeeperSession);
                    simpleViewModel.Storekeeper.Name = HomeSession.TryParseName(storekeeperSession);
                }
            }

            return simpleViewModel;
        }

        protected override void BackupViewModelToSession(SemifinishedRecyclateViewModel simpleViewModel)
        {
            base.BackupViewModelToSession(simpleViewModel);
            ShiftSession.SetShift(this.HttpContext, simpleViewModel.ShiftID);
            SemifinishedRecyclateSession.SetCrucialWorker(this.HttpContext, simpleViewModel.CrucialWorker.EmployeeID, simpleViewModel.CrucialWorker.Name);
            SemifinishedRecyclateSession.SetStorekeeper(this.HttpContext, simpleViewModel.Storekeeper.EmployeeID, simpleViewModel.Storekeeper.Name);
        }

        public virtual ActionResult GetPendingFirmOrderMaterials()
        {
            this.AddRequireJsOptions();
            return View();
        }
    }
}