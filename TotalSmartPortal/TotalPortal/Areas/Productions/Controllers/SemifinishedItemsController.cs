using System.Net;
using System.Web.Mvc;
using System.Text;
using System.Collections.Generic;

using AutoMapper;
using RequireJsNet;

using TotalBase.Enums;

using TotalModel.Models;

using TotalCore.Services.Productions;
using TotalCore.Repositories.Commons;

using TotalDTO.Productions;

using TotalPortal.Controllers;
using TotalPortal.APIs.Sessions;
using TotalPortal.Areas.Productions.ViewModels;
using TotalPortal.Areas.Productions.Builders;
using TotalPortal.Areas.Commons.Controllers.Sessions;

namespace TotalPortal.Areas.Productions.Controllers
{
    public class SemifinishedItemsController : GenericViewDetailController<SemifinishedItem, SemifinishedItemDetail, SemifinishedItemViewDetail, SemifinishedItemDTO, SemifinishedItemPrimitiveDTO, SemifinishedItemDetailDTO, SemifinishedItemViewModel>
    {
        private readonly ISemifinishedItemService semifinishedItemService;

        public SemifinishedItemsController(ISemifinishedItemService semifinishedItemService, ISemifinishedItemViewModelSelectListBuilder semifinishedItemViewModelSelectListBuilder)
            : base(semifinishedItemService, semifinishedItemViewModelSelectListBuilder, true)
        {
            this.semifinishedItemService = semifinishedItemService;
        }

        protected override ICollection<SemifinishedItemViewDetail> GetEntityViewDetails(SemifinishedItemViewModel semifinishedItemViewModel)
        {
            ICollection<SemifinishedItemViewDetail> semifinishedItemViewDetails = this.semifinishedItemService.GetSemifinishedItemViewDetails(semifinishedItemViewModel.SemifinishedItemID, semifinishedItemViewModel.FirmOrderID);

            return semifinishedItemViewDetails;
        }

        protected override SemifinishedItemViewModel InitViewModelByDefault(SemifinishedItemViewModel simpleViewModel)
        {
            simpleViewModel = base.InitViewModelByDefault(simpleViewModel);

            if (simpleViewModel.ShiftID == 0)
            {
                string shiftSession = ShiftSession.GetShift(this.HttpContext);
                if (HomeSession.TryParseID(shiftSession) > 0) simpleViewModel.ShiftID = (int)HomeSession.TryParseID(shiftSession);
            }

            return simpleViewModel;
        }

        protected override void BackupViewModelToSession(SemifinishedItemViewModel simpleViewModel)
        {
            base.BackupViewModelToSession(simpleViewModel);
            ShiftSession.SetShift(this.HttpContext, simpleViewModel.ShiftID);
        }

        public virtual ActionResult GetPendingFirmOrderMaterials()
        {
            this.AddRequireJsOptions();
            return View();
        }
    }
}