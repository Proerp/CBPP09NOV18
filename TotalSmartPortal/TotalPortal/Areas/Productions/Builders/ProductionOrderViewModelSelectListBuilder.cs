using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Productions.ViewModels;

namespace TotalPortal.Areas.Productions.Builders
{
    public interface IProductionOrderViewModelSelectListBuilder<TProductionOrderViewModel> : IViewModelSelectListBuilder<TProductionOrderViewModel>
        where TProductionOrderViewModel : IProductionOrderViewModel
    {
    }

    public class ProductionOrderViewModelSelectListBuilder<TProductionOrderViewModel> : A01ViewModelSelectListBuilder<TProductionOrderViewModel>, IProductionOrderViewModelSelectListBuilder<TProductionOrderViewModel>
        where TProductionOrderViewModel : IProductionOrderViewModel
    {
        public ProductionOrderViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
        }
    }



    public interface IItemOrderViewModelSelectListBuilder : IProductionOrderViewModelSelectListBuilder<ItemOrderViewModel>
    {
    }
    public class ItemOrderViewModelSelectListBuilder : ProductionOrderViewModelSelectListBuilder<ItemOrderViewModel>, IItemOrderViewModelSelectListBuilder
    {
        public ItemOrderViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }


    public interface IProductOrderViewModelSelectListBuilder : IProductionOrderViewModelSelectListBuilder<ProductOrderViewModel>
    {
    }
    public class ProductOrderViewModelSelectListBuilder : ProductionOrderViewModelSelectListBuilder<ProductOrderViewModel>, IProductOrderViewModelSelectListBuilder
    {
        public ProductOrderViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }

}