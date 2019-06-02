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

            //1-TAO TABLE/ DTO COLLECTION / FUNCCTION GET COLLECTION
            //2-TAI CHO NAY: CHIA 2 T/H: NEW/ EDIT: EDIT: GET FROM DATAABASE, ELSE: GIONG NHU HIEN TAI (TU DONG INIT)

            //5-CHECK WHEN SAVE: KTRA TRA, BẮT BUỘC: semifinishedRecyclateViewModel.SemifinishedRecyclatePackages.Count > 0, SAU ĐÓ: PHÂN BỔ SemifinishedRecyclatePackageDTO.Quantity VÔ: SemifinishedRecyclateViewDetail.Quantity THEO TỪNG RecycleCommodityID

            if (semifinishedRecyclateViewModel.SemifinishedRecyclateID > 0) //EDIT
            {
                List<SemifinishedRecyclateViewPackage> entityViewDetails = this.semifinishedRecyclateService.GetSemifinishedRecyclateViewPackages(semifinishedRecyclateViewModel.SemifinishedRecyclateID);
                Mapper.Map<List<SemifinishedRecyclateViewPackage>, List<SemifinishedRecyclatePackageDTO>>(entityViewDetails, semifinishedRecyclateViewModel.SemifinishedRecyclatePackages);
            }
            else
            { //NEW
                if (semifinishedRecyclateViewDetails.Where(w => w.RecycleCommodityID == null).Count() == 0)
                {
                    var semifinishedRecyclatePackages = semifinishedRecyclateViewDetails
                                                    .GroupBy(g => g.RecycleCommodityID)
                                                    .Select(sl => new SemifinishedRecyclatePackageDTO
                                                    {
                                                        CommodityID = (int)sl.First().RecycleCommodityID,
                                                        CommodityCode = sl.First().RecycleCommodityCode,
                                                        CommodityName = sl.First().RecycleCommodityName,
                                                        CommodityTypeID = (int)sl.First().RecycleCommodityTypeID,

                                                        RejectWeights = sl.Sum(s => s.RejectWeights),
                                                        FailureWeights = sl.Sum(s => s.FailureWeights),

                                                        QuantityRemains = sl.Sum(s => s.QuantityRemains),
                                                        Quantity = sl.Sum(s => s.Quantity)
                                                    });

                    semifinishedRecyclateViewModel.SemifinishedRecyclatePackages = semifinishedRecyclatePackages.ToList();
                }
            }

            return semifinishedRecyclateViewDetails;
        }


        protected override SemifinishedRecyclateViewModel InitViewModelByDefault(SemifinishedRecyclateViewModel simpleViewModel)
        {
            simpleViewModel = base.InitViewModelByDefault(simpleViewModel);

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