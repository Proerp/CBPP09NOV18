using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Purchases.ViewModels;

namespace TotalPortal.Areas.Purchases.Builders
{
    public interface IGoodsArrivalViewModelSelectListBuilder<TGoodsArrivalViewModel> : IViewModelSelectListBuilder<TGoodsArrivalViewModel>
        where TGoodsArrivalViewModel : IGoodsArrivalViewModel
    {
    }

    public class GoodsArrivalViewModelSelectListBuilder<TGoodsArrivalViewModel> : A01ViewModelSelectListBuilder<TGoodsArrivalViewModel>, IGoodsArrivalViewModelSelectListBuilder<TGoodsArrivalViewModel>
        where TGoodsArrivalViewModel : IGoodsArrivalViewModel
    {
        public GoodsArrivalViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
        }
    }



    public interface IMaterialArrivalViewModelSelectListBuilder : IGoodsArrivalViewModelSelectListBuilder<MaterialArrivalViewModel>
    {
    }
    public class MaterialArrivalViewModelSelectListBuilder : GoodsArrivalViewModelSelectListBuilder<MaterialArrivalViewModel>, IMaterialArrivalViewModelSelectListBuilder
    {
        public MaterialArrivalViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }

    public interface IItemArrivalViewModelSelectListBuilder : IGoodsArrivalViewModelSelectListBuilder<ItemArrivalViewModel>
    {
    }
    public class ItemArrivalViewModelSelectListBuilder : GoodsArrivalViewModelSelectListBuilder<ItemArrivalViewModel>, IItemArrivalViewModelSelectListBuilder
    {
        public ItemArrivalViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }

    public interface IProductArrivalViewModelSelectListBuilder : IGoodsArrivalViewModelSelectListBuilder<ProductArrivalViewModel>
    {
    }
    public class ProductArrivalViewModelSelectListBuilder : GoodsArrivalViewModelSelectListBuilder<ProductArrivalViewModel>, IProductArrivalViewModelSelectListBuilder
    {
        public ProductArrivalViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }
}