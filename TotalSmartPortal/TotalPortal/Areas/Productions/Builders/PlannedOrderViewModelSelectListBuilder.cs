using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Productions.ViewModels;

namespace TotalPortal.Areas.Productions.Builders
{
    public interface IPlannedOrderViewModelSelectListBuilder : IViewModelSelectListBuilder<PlannedOrderViewModel>
    {
    }

    public class PlannedOrderViewModelSelectListBuilder : A01ViewModelSelectListBuilder<PlannedOrderViewModel>, IPlannedOrderViewModelSelectListBuilder
    {
        public PlannedOrderViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
        }
    }

}