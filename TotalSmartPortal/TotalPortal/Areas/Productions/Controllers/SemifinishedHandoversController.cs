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
    public class SemifinishedHandoversController<TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel> : GenericViewDetailController<SemifinishedHandover, SemifinishedHandoverDetail, SemifinishedHandoverViewDetail, TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
        where TViewDetailViewModel : TDto, IViewDetailViewModel<TDtoDetail>, ISemifinishedHandoverViewModel, new()
    {
        public SemifinishedHandoversController(ISemifinishedHandoverService<TDto, TPrimitiveDto, TDtoDetail> semifinishedHandoverService, ISemifinishedHandoverViewModelSelectListBuilder<TViewDetailViewModel> semifinishedHandoverViewModelSelectListBuilder)
            : base(semifinishedHandoverService, semifinishedHandoverViewModelSelectListBuilder, true)
        {
        }

        public virtual ActionResult GetPendingDetails()
        {
            this.AddRequireJsOptions();
            return View();
        }
    }



    public class SemifinishedItemHandoversController : SemifinishedHandoversController<SemifinishedHandoverDTO<SemifinishedItemHandoverOption>, SemifinishedHandoverPrimitiveDTO<SemifinishedItemHandoverOption>, SemifinishedHandoverDetailDTO, SemifinishedItemHandoverViewModel>
    {
        public SemifinishedItemHandoversController(ISemifinishedItemHandoverService semifinishedItemHandoverService, ISemifinishedItemHandoverViewModelSelectListBuilder semifinishedItemHandoverViewModelSelectListBuilder)
            : base(semifinishedItemHandoverService, semifinishedItemHandoverViewModelSelectListBuilder)
        {
        }
    }


    public class SemifinishedProductHandoversController : SemifinishedHandoversController<SemifinishedHandoverDTO<SemifinishedProductHandoverOption>, SemifinishedHandoverPrimitiveDTO<SemifinishedProductHandoverOption>, SemifinishedHandoverDetailDTO, SemifinishedProductHandoverViewModel>
    {
        public SemifinishedProductHandoversController(ISemifinishedProductHandoverService semifinishedProductHandoverService, ISemifinishedProductHandoverViewModelSelectListBuilder semifinishedProductHandoverViewModelSelectListBuilder)
            : base(semifinishedProductHandoverService, semifinishedProductHandoverViewModelSelectListBuilder)
        {
        }
    }

}