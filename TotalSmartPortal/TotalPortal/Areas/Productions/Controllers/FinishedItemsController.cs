﻿using System.Net;
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
            ICollection<FinishedItemViewDetail> finishedItemViewDetails = this.finishedItemService.GetFinishedItemViewDetails(finishedItemViewModel.FinishedItemID, this.finishedItemService.LocationID, finishedItemViewModel.FirmOrderID, false);

            return finishedItemViewDetails;
        }

        protected override FinishedItemViewModel TailorViewModel(FinishedItemViewModel finishedItemViewModel, bool forDelete, bool forAlter, bool forOpen)
        {
            if (finishedItemViewModel.ViewDetails != null && finishedItemViewModel.ViewDetails.Count > 0)
                for (int i = 0; i <= finishedItemViewModel.ViewDetails.Count - 1; i++)
                {
                    if (finishedItemViewModel.ViewDetails[i].FinishedItemDetailID > 0)
                        finishedItemViewModel.GetDetails().Each(detailDTO =>
                        {
                            if (detailDTO.CommodityID == finishedItemViewModel.ViewDetails[i].CommodityID)
                            {
                                detailDTO.PiecePerPack = finishedItemViewModel.ViewDetails[i].PiecePerPack;
                                detailDTO.PackageUnitWeights = finishedItemViewModel.ViewDetails[i].PackageUnitWeights;
                            }
                        });
                }

            var finishedItemPackages = finishedItemViewModel.ViewDetails
                                            .GroupBy(g => g.CommodityID)
                                            .Select(sl => new FinishedItemPackageDTO
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

            finishedItemViewModel.FinishedItemPackages = finishedItemPackages.ToList();

            return base.TailorViewModel(finishedItemViewModel, forDelete, forAlter, forOpen);
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