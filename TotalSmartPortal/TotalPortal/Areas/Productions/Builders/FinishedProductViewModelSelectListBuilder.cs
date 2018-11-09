using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Productions.ViewModels;

namespace TotalPortal.Areas.Productions.Builders
{   
    public interface IFinishedProductViewModelSelectListBuilder : IViewModelSelectListBuilder<FinishedProductViewModel>
    {
    }

    public class FinishedProductViewModelSelectListBuilder : A01ViewModelSelectListBuilder<FinishedProductViewModel>, IFinishedProductViewModelSelectListBuilder
    {
        public FinishedProductViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
        }
    }
}