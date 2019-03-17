using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Productions.ViewModels;

namespace TotalPortal.Areas.Productions.Builders
{
    public interface IFinishedHandoverViewModelSelectListBuilder<TFinishedHandoverViewModel> : IViewModelSelectListBuilder<TFinishedHandoverViewModel>
        where TFinishedHandoverViewModel : IFinishedHandoverViewModel
    {
    }

    public class FinishedHandoverViewModelSelectListBuilder<TFinishedHandoverViewModel> : A01ViewModelSelectListBuilder<TFinishedHandoverViewModel>, IFinishedHandoverViewModelSelectListBuilder<TFinishedHandoverViewModel>
        where TFinishedHandoverViewModel : IFinishedHandoverViewModel
    {
        public FinishedHandoverViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
        }
    }



    public interface IFinishedItemHandoverViewModelSelectListBuilder : IFinishedHandoverViewModelSelectListBuilder<FinishedItemHandoverViewModel>
    {
    }
    public class FinishedItemHandoverViewModelSelectListBuilder : FinishedHandoverViewModelSelectListBuilder<FinishedItemHandoverViewModel>, IFinishedItemHandoverViewModelSelectListBuilder
    {
        public FinishedItemHandoverViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }


    public interface IFinishedProductHandoverViewModelSelectListBuilder : IFinishedHandoverViewModelSelectListBuilder<FinishedProductHandoverViewModel>
    {
    }
    public class FinishedProductHandoverViewModelSelectListBuilder : FinishedHandoverViewModelSelectListBuilder<FinishedProductHandoverViewModel>, IFinishedProductHandoverViewModelSelectListBuilder
    {
        public FinishedProductHandoverViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }
}