using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Productions.ViewModels;

namespace TotalPortal.Areas.Productions.Builders
{    
    public interface ISemifinishedHandoverViewModelSelectListBuilder<TSemifinishedHandoverViewModel> : IViewModelSelectListBuilder<TSemifinishedHandoverViewModel>
    where TSemifinishedHandoverViewModel : ISemifinishedHandoverViewModel
    {
    }

    public class SemifinishedHandoverViewModelSelectListBuilder<TSemifinishedHandoverViewModel> : A01ViewModelSelectListBuilder<TSemifinishedHandoverViewModel>, ISemifinishedHandoverViewModelSelectListBuilder<TSemifinishedHandoverViewModel>
        where TSemifinishedHandoverViewModel : ISemifinishedHandoverViewModel
    {
        public SemifinishedHandoverViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
        }
    }



    public interface ISemifinishedItemHandoverViewModelSelectListBuilder : ISemifinishedHandoverViewModelSelectListBuilder<SemifinishedItemHandoverViewModel>
    {
    }
    public class SemifinishedItemHandoverViewModelSelectListBuilder : SemifinishedHandoverViewModelSelectListBuilder<SemifinishedItemHandoverViewModel>, ISemifinishedItemHandoverViewModelSelectListBuilder
    {
        public SemifinishedItemHandoverViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }


    public interface ISemifinishedProductHandoverViewModelSelectListBuilder : ISemifinishedHandoverViewModelSelectListBuilder<SemifinishedProductHandoverViewModel>
    {
    }
    public class SemifinishedProductHandoverViewModelSelectListBuilder : SemifinishedHandoverViewModelSelectListBuilder<SemifinishedProductHandoverViewModel>, ISemifinishedProductHandoverViewModelSelectListBuilder
    {
        public SemifinishedProductHandoverViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }

}