using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Productions.ViewModels;

namespace TotalPortal.Areas.Productions.Builders
{
    public interface IPlannedOrderViewModelSelectListBuilder<TPlannedOrderViewModel> : IViewModelSelectListBuilder<TPlannedOrderViewModel>
        where TPlannedOrderViewModel : IPlannedOrderViewModel
    {
    }

    public class PlannedOrderViewModelSelectListBuilder<TPlannedOrderViewModel> : A01ViewModelSelectListBuilder<TPlannedOrderViewModel>, IPlannedOrderViewModelSelectListBuilder<TPlannedOrderViewModel>
        where TPlannedOrderViewModel : IPlannedOrderViewModel
    {
        public PlannedOrderViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
        }
    }



    public interface IPlannedItemViewModelSelectListBuilder : IPlannedOrderViewModelSelectListBuilder<PlannedItemViewModel>
    {
    }
    public class PlannedItemViewModelSelectListBuilder : PlannedOrderViewModelSelectListBuilder<PlannedItemViewModel>, IPlannedItemViewModelSelectListBuilder
    {
        public PlannedItemViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }


    public interface IPlannedProductViewModelSelectListBuilder : IPlannedOrderViewModelSelectListBuilder<PlannedProductViewModel>
    {
    }
    public class PlannedProductViewModelSelectListBuilder : PlannedOrderViewModelSelectListBuilder<PlannedProductViewModel>, IPlannedProductViewModelSelectListBuilder
    {
        public PlannedProductViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }

}