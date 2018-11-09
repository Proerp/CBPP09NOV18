using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Productions.ViewModels;

namespace TotalPortal.Areas.Productions.Builders
{
    public interface IFinishedHandoverViewModelSelectListBuilder : IViewModelSelectListBuilder<FinishedHandoverViewModel>
    {
    }

    public class FinishedHandoverViewModelSelectListBuilder : A01ViewModelSelectListBuilder<FinishedHandoverViewModel>, IFinishedHandoverViewModelSelectListBuilder
    {

        public FinishedHandoverViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {          
        }
    }
}