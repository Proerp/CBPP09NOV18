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
using TotalPortal.Areas.Productions.ViewModels;
using TotalPortal.Areas.Productions.Builders;
using TotalPortal.Areas.Productions.Controllers.Sessions;

namespace TotalPortal.Areas.Productions.Controllers
{
    public class FinishedProductsController : GenericViewDetailController<FinishedProduct, FinishedProductDetail, FinishedProductViewDetail, FinishedProductDTO, FinishedProductPrimitiveDTO, FinishedProductDetailDTO, FinishedProductViewModel>
    {
        private readonly IFinishedProductService finishedProductService;

        public FinishedProductsController(IFinishedProductService finishedProductService, IFinishedProductViewModelSelectListBuilder finishedProductViewModelSelectListBuilder)
            : base(finishedProductService, finishedProductViewModelSelectListBuilder, true)
        {
            this.finishedProductService = finishedProductService;
        }

        protected override ICollection<FinishedProductViewDetail> GetEntityViewDetails(FinishedProductViewModel finishedProductViewModel)
        {
            ICollection<FinishedProductViewDetail> finishedProductViewDetails = this.finishedProductService.GetFinishedProductViewDetails(finishedProductViewModel.FinishedProductID, this.finishedProductService.LocationID, finishedProductViewModel.FirmOrderID, false);

            return finishedProductViewDetails;
        }

        protected override FinishedProductViewModel TailorViewModel(FinishedProductViewModel finishedProductViewModel, bool forDelete, bool forAlter, bool forOpen)
        {
            if (finishedProductViewModel.ViewDetails != null && finishedProductViewModel.ViewDetails.Count > 0)
                for (int i = 0; i <= finishedProductViewModel.ViewDetails.Count - 1; i++)
                {
                    if (finishedProductViewModel.ViewDetails[i].FinishedProductDetailID > 0)
                        finishedProductViewModel.GetDetails().Each(detailDTO =>
                        {
                            if (detailDTO.CommodityID == finishedProductViewModel.ViewDetails[i].CommodityID)
                                detailDTO.PiecePerPack = finishedProductViewModel.ViewDetails[i].PiecePerPack;
                        });
                }

            var finishedProductSummaries = finishedProductViewModel.ViewDetails
                                            .GroupBy(g => g.CommodityID)
                                            .Select(sl => new FinishedProductSummaryDTO
                                            {
                                                CommodityID = sl.First().CommodityID,
                                                CommodityCode = sl.First().CommodityCode,
                                                CommodityName = sl.First().CommodityName,
                                                PiecePerPack = sl.First().PiecePerPack,

                                                FoilUnitCounts = 1,
                                                FoilUnitWeights = 1,

                                                QuantityRemains = sl.Sum(s => s.QuantityRemains),
                                                Quantity = sl.Sum(s => (s.Quantity + s.QuantityExcess)),
                                                QuantityFailure = sl.Sum(s => s.QuantityFailure),
                                                Swarfs = sl.Sum(s => s.Swarfs),
                                            });

            finishedProductViewModel.FinishedProductSummaries = finishedProductSummaries.ToList();

            return base.TailorViewModel(finishedProductViewModel, forDelete, forAlter, forOpen);
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

        protected override FinishedProductViewModel InitViewModelByDefault(FinishedProductViewModel simpleViewModel)
        {
            simpleViewModel = base.InitViewModelByDefault(simpleViewModel);

            if (simpleViewModel.CrucialWorker == null)
            {
                string storekeeperSession = FinishedProductSession.GetCrucialWorker(this.HttpContext);

                if (HomeSession.TryParseID(storekeeperSession) > 0)
                {
                    simpleViewModel.CrucialWorker = new TotalDTO.Commons.EmployeeBaseDTO();
                    simpleViewModel.CrucialWorker.EmployeeID = (int)HomeSession.TryParseID(storekeeperSession);
                    simpleViewModel.CrucialWorker.Name = HomeSession.TryParseName(storekeeperSession);
                }
            }

            return simpleViewModel;
        }

        protected override void BackupViewModelToSession(FinishedProductViewModel simpleViewModel)
        {
            base.BackupViewModelToSession(simpleViewModel);
            FinishedProductSession.SetCrucialWorker(this.HttpContext, simpleViewModel.CrucialWorker.EmployeeID, simpleViewModel.CrucialWorker.Name);
        }

        public virtual ActionResult GetPendingFirmOrderMaterials()
        {
            this.AddRequireJsOptions();
            return View();
        }
    }
}