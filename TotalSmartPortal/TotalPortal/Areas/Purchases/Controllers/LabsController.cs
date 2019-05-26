using System;
using System.Net;
using System.Web.Mvc;

using TotalBase.Enums;
using TotalModel.Models;

using TotalDTO.Purchases;
using TotalCore.Services.Purchases;

using TotalPortal.Controllers;
using TotalPortal.Areas.Purchases.ViewModels;
using TotalPortal.Areas.Purchases.Builders;


namespace TotalPortal.Areas.Purchases.Controllers
{
    public class LabsController : GenericSimpleController<Lab, LabDTO, LabPrimitiveDTO, LabViewModel>
    {
        private ILabService labService;
        public LabsController(ILabService labService, ILabSelectListBuilder labViewModelSelectListBuilder)
            : base(labService, labViewModelSelectListBuilder)
        {
            this.labService = labService;
        }



        #region Hold/ Release

        [AccessLevelAuthorize(GlobalEnums.AccessLevel.Readable), ImportModelStateFromTempData]
        [OnResultExecutingFilterAttribute]
        public virtual ActionResult Hold(int? id)
        {
            LabViewModel labViewModel = this.GetViewModel(id, GlobalEnums.AccessLevel.Readable, true);
            if (labViewModel == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (!labViewModel.Hold)
                if (this.GenericService.GetApprovalPermitted(labViewModel.OrganizationalUnitID))
                    labViewModel.Holdable = this.labService.Holdable(labViewModel);
                else //USER DON'T HAVE PERMISSION TO DO
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            if (labViewModel.Hold)
                if (this.GenericService.GetUnApprovalPermitted(labViewModel.OrganizationalUnitID))
                    labViewModel.Releasable = this.labService.Releasable(labViewModel);
                else //USER DON'T HAVE PERMISSION TO DO
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            return View(labViewModel);
        }

        [HttpPost, ActionName("Hold")]
        [ValidateAntiForgeryToken, ExportModelStateToTempData]
        public virtual ActionResult HoldConfirmed(LabViewModel labViewModel)
        {
            try
            {
                if (this.labService.ToggleHold(labViewModel))
                    return RedirectToAction("Index");
                else
                    throw new System.ArgumentException("Lỗi hold hoặc release", "Dữ liệu này không thể hold hoặc release.");
            }
            catch (Exception exception)
            {
                ModelState.AddValidationErrors(exception);
                return RedirectToAction("Hold", labViewModel.GetID());
            }
        }
        #endregion Hold/ Release

    }
}