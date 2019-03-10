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
    public class FinishedItemsController : GenericViewDetailController<FinishedItem, FinishedItemDetail, FinishedItemViewDetail, FinishedItemDTO, FinishedItemPrimitiveDTO, FinishedItemDetailDTO, FinishedItemViewModel>
    {
        private readonly IFinishedItemService finishedItemService;

        public FinishedItemsController(IFinishedItemService finishedItemService, IFinishedItemViewModelSelectListBuilder finishedItemViewModelSelectListBuilder)
            : base(finishedItemService, finishedItemViewModelSelectListBuilder, true)
        {
            this.finishedItemService = finishedItemService;
        }

        protected override ICollection<FinishedItemViewDetail> GetEntityViewDetails(FinishedItemViewModel finishedItemViewModel)
        {
            IList<FinishedItemViewDetail> finishedItemViewDetails = this.finishedItemService.GetFinishedItemViewDetails(finishedItemViewModel.FinishedItemID, this.finishedItemService.LocationID, finishedItemViewModel.FirmOrderID, false).ToList();


            //1-TAO TABLE/ DTO COLLECTION / FUNCCTION GET COLLECTION
            //2-TAI CHO NAY: CHIA 2 T/H: NEW/ EDIT: EDIT: GET FROM DATAABASE, ELSE: GIONG NHU HIEN TAI (TU DONG INIT)
            //3-CHO PHEP INSERT ==> DE INSERT DUPLICATE ROW

            //4-JS: UPDATE SUMMARY COLLLECTION: UPDATE FOR ALL ROW
            //5-CHECK WHEN SAVE: KTRA TRA: TONG CHI TIET CUON === PHAI BANG SO NL TONG
            //6-T-SQL: GENERATE: finishedItemLots

            //CHÚ Ý: FinishedItemPackages: TRONG T-SQL: PHẢI XEM LẠI FinishedItemPackages, CHỈ KHI APRROVE MOI GENERATE FinishedItemPackages ĐƯỢC
            if (finishedItemViewModel.FinishedItemID > 0) //EDIT
            {
                List<FinishedItemViewLot> entityViewDetails = this.finishedItemService.GetFinishedItemViewLots(finishedItemViewModel.FinishedItemID);
                Mapper.Map<List<FinishedItemViewLot>, List<FinishedItemLotDTO>>(entityViewDetails, finishedItemViewModel.FinishedItemLots);
            }
            else
            { //NEW
                var finishedItemLots = finishedItemViewDetails
                                                .GroupBy(g => g.CommodityID)
                                                .Select(sl => new FinishedItemLotDTO
                                                {
                                                    CommodityID = sl.First().CommodityID,
                                                    CommodityCode = sl.First().CommodityCode,
                                                    CommodityName = sl.First().CommodityName,
                                                    CommodityTypeID = sl.First().CommodityTypeID,

                                                    PiecePerPack = sl.First().PiecePerPack,
                                                    PackageUnitWeights = sl.First().PackageUnitWeights,

                                                    QuantityRemains = sl.Sum(s => s.QuantityRemains),
                                                    Quantity = sl.Sum(s => (s.Quantity + s.QuantityExcess)),
                                                    QuantityFailure = sl.Sum(s => s.QuantityFailure),
                                                    QuantityExcess = sl.Sum(s => s.QuantityExcess),
                                                    QuantityShortage = sl.Sum(s => s.QuantityShortage),
                                                    Swarfs = sl.Sum(s => s.Swarfs),
                                                });

                finishedItemViewModel.FinishedItemLots = finishedItemLots.ToList();
            }

            return finishedItemViewDetails;
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

        protected override FinishedItemViewModel InitViewModelByDefault(FinishedItemViewModel simpleViewModel)
        {
            simpleViewModel = base.InitViewModelByDefault(simpleViewModel);

            if (simpleViewModel.ShiftID == 0)
            {
                string shiftSession = ShiftSession.GetShift(this.HttpContext);
                if (HomeSession.TryParseID(shiftSession) > 0) simpleViewModel.ShiftID = (int)HomeSession.TryParseID(shiftSession);
            }

            if (simpleViewModel.CrucialWorker == null)
            {
                string storekeeperSession = FinishedItemSession.GetCrucialWorker(this.HttpContext);

                if (HomeSession.TryParseID(storekeeperSession) > 0)
                {
                    simpleViewModel.CrucialWorker = new TotalDTO.Commons.EmployeeBaseDTO();
                    simpleViewModel.CrucialWorker.EmployeeID = (int)HomeSession.TryParseID(storekeeperSession);
                    simpleViewModel.CrucialWorker.Name = HomeSession.TryParseName(storekeeperSession);
                }
            }

            return simpleViewModel;
        }

        protected override void BackupViewModelToSession(FinishedItemViewModel simpleViewModel)
        {
            base.BackupViewModelToSession(simpleViewModel);
            ShiftSession.SetShift(this.HttpContext, simpleViewModel.ShiftID);
            FinishedItemSession.SetCrucialWorker(this.HttpContext, simpleViewModel.CrucialWorker.EmployeeID, simpleViewModel.CrucialWorker.Name);
        }

        public virtual ActionResult GetPendingFirmOrderMaterials()
        {
            this.AddRequireJsOptions();
            return View();
        }
    }
}