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

using TotalDTO;
using TotalModel;
using TotalModel.Models;

using TotalCore.Services.Productions;
using TotalCore.Repositories.Commons;

using TotalDTO.Productions;

using TotalPortal.APIs.Sessions;

using TotalPortal.Controllers;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Areas.Commons.Controllers.Sessions;
using TotalPortal.Areas.Productions.ViewModels;
using TotalPortal.Areas.Productions.Builders;
using TotalPortal.Areas.Productions.Controllers.Sessions;

namespace TotalPortal.Areas.Productions.Controllers
{
    public class RecyclatesController<TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel> : GenericViewDetailController<Recyclate, RecyclateDetail, RecyclateViewDetail, TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
        where TViewDetailViewModel : TDto, IViewDetailViewModel<TDtoDetail>, IRecyclateViewModel, new()
    {
        private readonly IRecyclateService<TDto, TPrimitiveDto, TDtoDetail> recyclateService;

        public RecyclatesController(IRecyclateService<TDto, TPrimitiveDto, TDtoDetail> recyclateService, IRecyclateViewModelSelectListBuilder<TViewDetailViewModel> recyclateViewModelSelectListBuilder)
            : base(recyclateService, recyclateViewModelSelectListBuilder, true)
        {
            this.recyclateService = recyclateService;
        }

        protected override ICollection<RecyclateViewDetail> GetEntityViewDetails(TViewDetailViewModel recyclateViewModel)
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


        protected override TViewDetailViewModel InitViewModelByDefault(TViewDetailViewModel simpleViewModel)
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

        protected override void BackupViewModelToSession(TViewDetailViewModel simpleViewModel)
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



    public class SemifinishedProductRecyclatesController : RecyclatesController<RecyclateDTO<SemifinishedProductRecyclateOption>, RecyclatePrimitiveDTO<SemifinishedProductRecyclateOption>, RecyclateDetailDTO, SemifinishedProductRecyclateViewModel>
    {
        public SemifinishedProductRecyclatesController(ISemifinishedProductRecyclateService semifinishedProductRecyclateService, ISemifinishedProductRecyclateViewModelSelectListBuilder semifinishedProductRecyclateViewModelSelectListBuilder)
            : base(semifinishedProductRecyclateService, semifinishedProductRecyclateViewModelSelectListBuilder)
        {
        }
    }

    public class FinishedProductRecyclatesController : RecyclatesController<RecyclateDTO<FinishedProductRecyclateOption>, RecyclatePrimitiveDTO<FinishedProductRecyclateOption>, RecyclateDetailDTO, FinishedProductRecyclateViewModel>
    {
        public FinishedProductRecyclatesController(IFinishedProductRecyclateService finishedProductRecyclateService, IFinishedProductRecyclateViewModelSelectListBuilder finishedProductRecyclateViewModelSelectListBuilder)
            : base(finishedProductRecyclateService, finishedProductRecyclateViewModelSelectListBuilder)
        {
        }
    }

    public class FinishedItemRecyclatesController : RecyclatesController<RecyclateDTO<FinishedItemRecyclateOption>, RecyclatePrimitiveDTO<FinishedItemRecyclateOption>, RecyclateDetailDTO, FinishedItemRecyclateViewModel>
    {
        public FinishedItemRecyclatesController(IFinishedItemRecyclateService finishedItemRecyclateService, IFinishedItemRecyclateViewModelSelectListBuilder finishedItemRecyclateViewModelSelectListBuilder)
            : base(finishedItemRecyclateService, finishedItemRecyclateViewModelSelectListBuilder)
        {
        }
    }
}