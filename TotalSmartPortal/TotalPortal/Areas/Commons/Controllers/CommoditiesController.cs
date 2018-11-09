using System.Net;
using System.Linq;
using System.Web.Mvc;

using TotalModel;
using TotalModel.Models;

using TotalBase.Enums;

using TotalDTO;
using TotalDTO.Commons;

using TotalCore.Repositories.Commons;
using TotalCore.Services.Commons;

using TotalPortal.Controllers;
using TotalPortal.Areas.Commons.ViewModels;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.ViewModels.Helpers;


namespace TotalPortal.Areas.Commons.Controllers
{
    public class CommoditiesController<TDto, TPrimitiveDto, TSimpleViewModel> : GenericSimpleController<Commodity, TDto, TPrimitiveDto, TSimpleViewModel>
        where TDto : class, TPrimitiveDto
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TSimpleViewModel : TDto, ISimpleViewModel, ICommodityViewModel, new() //Note: constraints [TSimpleViewModel : ICommodityViewModel] -> IS REQUIRED FOR ICommoditySelectListBuilder ONLY. IF NEEDED: WE CAN MODIFY ICommoditySelectListBuilder. SEE: ICommoditySelectListBuilder (THIS ICommoditySelectListBuilder JUST IS REQUIRED SOME SelectList ONLY)
    {
        public CommoditiesController(ICommodityService<TDto, TPrimitiveDto> commodityService, ICommoditySelectListBuilder<TSimpleViewModel> commodityViewModelSelectListBuilder)
            : base(commodityService, commodityViewModelSelectListBuilder)
        {
        }

        public virtual ActionResult Boms(int id)
        {
            TSimpleViewModel simpleViewModel = this.GetViewModel(id, GlobalEnums.AccessLevel.Readable);
            if (simpleViewModel == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return View(simpleViewModel);
        }

        public virtual ActionResult Molds(int id)
        {           
            TSimpleViewModel simpleViewModel = this.GetViewModel(id, GlobalEnums.AccessLevel.Readable);
            if (simpleViewModel == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return View(simpleViewModel);
        }
    }

    public class MaterialsController : CommoditiesController<CommodityDTO<CMDMaterial>, CommodityPrimitiveDTO<CMDMaterial>, MaterialViewModel>
    {
        public MaterialsController(IMaterialService materialService, IMaterialSelectListBuilder materialSelectListBuilder)
            : base(materialService, materialSelectListBuilder)
        {
        }
    }

    public class ItemsController : CommoditiesController<CommodityDTO<CMDItem>, CommodityPrimitiveDTO<CMDItem>, ItemViewModel>
    {
        public ItemsController(IItemService itemService, IItemSelectListBuilder itemSelectListBuilder)
            : base(itemService, itemSelectListBuilder)
        {
        }
    }
    public class ProductsController : CommoditiesController<CommodityDTO<CMDProduct>, CommodityPrimitiveDTO<CMDProduct>, ProductViewModel>
    {
        public ProductsController(IProductService productService, IProductSelectListBuilder productSelectListBuilder)
            : base(productService, productSelectListBuilder)
        {
        }
    }
}