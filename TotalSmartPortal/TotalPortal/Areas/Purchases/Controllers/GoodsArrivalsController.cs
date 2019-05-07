using System.Net;
using System.Web.Mvc;
using System.Text;

using AutoMapper;
using RequireJsNet;

using TotalBase.Enums;

using TotalModel;
using TotalDTO;
using TotalModel.Models;

using TotalCore.Services.Purchases;

using TotalDTO.Purchases;

using TotalPortal.Controllers;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Areas.Purchases.ViewModels;
using TotalPortal.Areas.Purchases.Builders;

using TotalPortal.APIs.Sessions;

namespace TotalPortal.Areas.Purchases.Controllers
{
    public class GoodsArrivalsController<TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel> : GenericViewDetailController<GoodsArrival, GoodsArrivalDetail, GoodsArrivalViewDetail, TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
        where TViewDetailViewModel : TDto, IViewDetailViewModel<TDtoDetail>, IGoodsArrivalViewModel, new()
    {
        public GoodsArrivalsController(IGoodsArrivalService<TDto, TPrimitiveDto, TDtoDetail> goodsArrivalService, IGoodsArrivalViewModelSelectListBuilder<TViewDetailViewModel> goodsArrivalViewModelSelectListBuilder)
            : base(goodsArrivalService, goodsArrivalViewModelSelectListBuilder, true)
        {
        }

        public virtual ActionResult GetPendingPurchaseOrderDetails()
        {
            this.AddRequireJsOptions();
            return View();
        }
    }


    public class MaterialArrivalsController : GoodsArrivalsController<GoodsArrivalDTO<GoodsArrivalOptionMaterial>, GoodsArrivalPrimitiveDTO<GoodsArrivalOptionMaterial>, GoodsArrivalDetailDTO, MaterialArrivalViewModel>
    {
        public MaterialArrivalsController(IMaterialArrivalService materialArrivalService, IMaterialArrivalViewModelSelectListBuilder materialArrivalViewModelSelectListBuilder)
            : base(materialArrivalService, materialArrivalViewModelSelectListBuilder)
        {
        }
    }


    public class ItemArrivalsController : GoodsArrivalsController<GoodsArrivalDTO<GoodsArrivalOptionItem>, GoodsArrivalPrimitiveDTO<GoodsArrivalOptionItem>, GoodsArrivalDetailDTO, ItemArrivalViewModel>
    {
        public ItemArrivalsController(IItemArrivalService itemArrivalService, IItemArrivalViewModelSelectListBuilder itemArrivalViewModelSelectListBuilder)
            : base(itemArrivalService, itemArrivalViewModelSelectListBuilder)
        {
        }
    }


    public class ProductArrivalsController : GoodsArrivalsController<GoodsArrivalDTO<GoodsArrivalOptionProduct>, GoodsArrivalPrimitiveDTO<GoodsArrivalOptionProduct>, GoodsArrivalDetailDTO, ProductArrivalViewModel>
    {
        public ProductArrivalsController(IProductArrivalService productArrivalService, IProductArrivalViewModelSelectListBuilder productArrivalViewModelSelectListBuilder)
            : base(productArrivalService, productArrivalViewModelSelectListBuilder)
        {
        }
    }
}