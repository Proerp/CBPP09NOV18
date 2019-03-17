using System.Net;
using System.Web.Mvc;
using System.Text;

using RequireJsNet;

using TotalModel;
using TotalDTO;
using TotalBase.Enums;
using TotalDTO.Productions;
using TotalModel.Models;

using TotalCore.Services.Productions;

using TotalPortal.Controllers;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Areas.Productions.ViewModels;
using TotalPortal.Areas.Productions.Builders;

namespace TotalPortal.Areas.Productions.Controllers
{
    public class FinishedHandoversController<TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel> : GenericViewDetailController<FinishedHandover, FinishedHandoverDetail, FinishedHandoverViewDetail, TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
        where TViewDetailViewModel : TDto, IViewDetailViewModel<TDtoDetail>, IFinishedHandoverViewModel, new()
    {
        public FinishedHandoversController(IFinishedHandoverService<TDto, TPrimitiveDto, TDtoDetail> finishedHandoverService, IFinishedHandoverViewModelSelectListBuilder<TViewDetailViewModel> finishedHandoverViewModelSelectListBuilder)
            : base(finishedHandoverService, finishedHandoverViewModelSelectListBuilder, true)
        {
        }

        public virtual ActionResult GetPendingDetails()
        {
            this.AddRequireJsOptions();
            return View();
        }
    }



    public class FinishedItemHandoversController : FinishedHandoversController<FinishedHandoverDTO<FinishedItemHandoverOption>, FinishedHandoverPrimitiveDTO<FinishedItemHandoverOption>, FinishedHandoverDetailDTO, FinishedItemHandoverViewModel>
    {
        public FinishedItemHandoversController(IFinishedItemHandoverService finishedItemHandoverService, IFinishedItemHandoverViewModelSelectListBuilder finishedItemHandoverViewModelSelectListBuilder)
            : base(finishedItemHandoverService, finishedItemHandoverViewModelSelectListBuilder)
        {
        }
    }


    public class FinishedProductHandoversController : FinishedHandoversController<FinishedHandoverDTO<FinishedProductHandoverOption>, FinishedHandoverPrimitiveDTO<FinishedProductHandoverOption>, FinishedHandoverDetailDTO, FinishedProductHandoverViewModel>
    {
        public FinishedProductHandoversController(IFinishedProductHandoverService finishedProductHandoverService, IFinishedProductHandoverViewModelSelectListBuilder finishedProductHandoverViewModelSelectListBuilder)
            : base(finishedProductHandoverService, finishedProductHandoverViewModelSelectListBuilder)
        {
        }
    }
}