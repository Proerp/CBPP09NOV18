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
    public class RecyclatesController : GenericViewDetailController<Recyclate, RecyclateDetail, RecyclateViewDetail, RecyclateDTO, RecyclatePrimitiveDTO, RecyclateDetailDTO, RecyclateViewModel>
    {
        private readonly IRecyclateService recyclateService;

        public RecyclatesController(IRecyclateService recyclateService, IRecyclateViewModelSelectListBuilder recyclateViewModelSelectListBuilder)
            : base(recyclateService, recyclateViewModelSelectListBuilder, true)
        {
            this.recyclateService = recyclateService;
        }

        protected override ICollection<RecyclateViewDetail> GetEntityViewDetails(RecyclateViewModel recyclateViewModel)
        {
            ICollection<RecyclateViewDetail> recyclateViewDetails = this.recyclateService.GetRecyclateViewDetails((int)recyclateViewModel.NMVNTaskID, recyclateViewModel.RecyclateID, this.recyclateService.LocationID, recyclateViewModel.WorkshiftID, false);

            //1-TAO TABLE/ DTO COLLECTION / FUNCCTION GET COLLECTION
            //2-TAI CHO NAY: CHIA 2 T/H: NEW/ EDIT: EDIT: GET FROM DATAABASE, ELSE: GIONG NHU HIEN TAI (TU DONG INIT)

            //5-CHECK WHEN SAVE: KTRA TRA, BẮT BUỘC: recyclateViewModel.RecyclatePackages.Count > 0, SAU ĐÓ: PHÂN BỔ RecyclatePackageDTO.Quantity VÔ: RecyclateViewDetail.Quantity THEO TỪNG RecycleCommodityID

            if (recyclateViewModel.RecyclateID > 0) //EDIT
            {
                List<RecyclateViewPackage> entityViewDetails = this.recyclateService.GetRecyclateViewPackages((int)recyclateViewModel.NMVNTaskID, recyclateViewModel.RecyclateID);
                Mapper.Map<List<RecyclateViewPackage>, List<RecyclatePackageDTO>>(entityViewDetails, recyclateViewModel.RecyclatePackages);
            }
            else
            { //NEW
                if (recyclateViewDetails.Where(w => w.RecycleCommodityID == null).Count() == 0)
                {
                    var recyclatePackages = recyclateViewDetails
                                                    .GroupBy(g => g.RecycleCommodityID)
                                                    .Select(sl => new RecyclatePackageDTO
                                                    {
                                                        CommodityID = (int)sl.First().RecycleCommodityID,
                                                        CommodityCode = sl.First().RecycleCommodityCode,
                                                        CommodityName = sl.First().RecycleCommodityName,
                                                        CommodityTypeID = (int)sl.First().RecycleCommodityTypeID,

                                                        QuantityFailures = sl.Sum(s => s.QuantityFailures),
                                                        QuantitySwarfs = sl.Sum(s => s.QuantitySwarfs),

                                                        QuantityRemains = sl.Sum(s => s.QuantityRemains),
                                                        Quantity = sl.Sum(s => s.Quantity)
                                                    });

                    recyclateViewModel.RecyclatePackages = recyclatePackages.ToList();
                }
            }

            return recyclateViewDetails;
        }


        protected override RecyclateViewModel InitViewModelByDefault(RecyclateViewModel simpleViewModel)
        {
            simpleViewModel = base.InitViewModelByDefault(simpleViewModel);

            if (simpleViewModel.CrucialWorker == null)
            {
                string storekeeperSession = RecyclateSession.GetCrucialWorker(this.HttpContext);

                if (HomeSession.TryParseID(storekeeperSession) > 0)
                {
                    simpleViewModel.CrucialWorker = new TotalDTO.Commons.EmployeeBaseDTO();
                    simpleViewModel.CrucialWorker.EmployeeID = (int)HomeSession.TryParseID(storekeeperSession);
                    simpleViewModel.CrucialWorker.Name = HomeSession.TryParseName(storekeeperSession);
                }
            }

            if (simpleViewModel.Storekeeper == null)
            {
                string storekeeperSession = RecyclateSession.GetStorekeeper(this.HttpContext);

                if (HomeSession.TryParseID(storekeeperSession) > 0)
                {
                    simpleViewModel.Storekeeper = new TotalDTO.Commons.EmployeeBaseDTO();
                    simpleViewModel.Storekeeper.EmployeeID = (int)HomeSession.TryParseID(storekeeperSession);
                    simpleViewModel.Storekeeper.Name = HomeSession.TryParseName(storekeeperSession);
                }
            }

            return simpleViewModel;
        }

        protected override void BackupViewModelToSession(RecyclateViewModel simpleViewModel)
        {
            base.BackupViewModelToSession(simpleViewModel);
            RecyclateSession.SetCrucialWorker(this.HttpContext, simpleViewModel.CrucialWorker.EmployeeID, simpleViewModel.CrucialWorker.Name);
            RecyclateSession.SetStorekeeper(this.HttpContext, simpleViewModel.Storekeeper.EmployeeID, simpleViewModel.Storekeeper.Name);
        }

        public virtual ActionResult GetPendingFirmOrderMaterials()
        {
            this.AddRequireJsOptions();
            return View();
        }
    }
}