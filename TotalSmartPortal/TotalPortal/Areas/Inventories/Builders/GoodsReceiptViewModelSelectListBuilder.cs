using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Builders
{
    public interface IGoodsReceiptViewModelSelectListBuilder<TGoodsReceiptViewModel> : IViewModelSelectListBuilder<TGoodsReceiptViewModel>
    where TGoodsReceiptViewModel : IGoodsReceiptViewModel
    {
    }

    public class GoodsReceiptViewModelSelectListBuilder<TGoodsReceiptViewModel> : A01ViewModelSelectListBuilder<TGoodsReceiptViewModel>, IGoodsReceiptViewModelSelectListBuilder<TGoodsReceiptViewModel>
        where TGoodsReceiptViewModel : IGoodsReceiptViewModel
    {
        public GoodsReceiptViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
        }
    }









    public interface IMaterialReceiptViewModelSelectListBuilder : IGoodsReceiptViewModelSelectListBuilder<MaterialReceiptViewModel>
    {
    }
    public class MaterialReceiptViewModelSelectListBuilder : GoodsReceiptViewModelSelectListBuilder<MaterialReceiptViewModel>, IMaterialReceiptViewModelSelectListBuilder
    {
        public MaterialReceiptViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }


    public interface IItemReceiptViewModelSelectListBuilder : IGoodsReceiptViewModelSelectListBuilder<ItemReceiptViewModel>
    {
    }
    public class ItemReceiptViewModelSelectListBuilder : GoodsReceiptViewModelSelectListBuilder<ItemReceiptViewModel>, IItemReceiptViewModelSelectListBuilder
    {
        public ItemReceiptViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }


    public interface IProductReceiptViewModelSelectListBuilder : IGoodsReceiptViewModelSelectListBuilder<ProductReceiptViewModel>
    {
    }
    public class ProductReceiptViewModelSelectListBuilder : GoodsReceiptViewModelSelectListBuilder<ProductReceiptViewModel>, IProductReceiptViewModelSelectListBuilder
    {
        public ProductReceiptViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }
}