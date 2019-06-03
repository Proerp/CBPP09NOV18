using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Productions.ViewModels;

namespace TotalPortal.Areas.Productions.Builders
{
    public interface IRecyclateViewModelSelectListBuilder<TRecyclateViewModel> : IViewModelSelectListBuilder<TRecyclateViewModel>
        where TRecyclateViewModel : IRecyclateViewModel
    {
    }

    public class RecyclateViewModelSelectListBuilder<TRecyclateViewModel> : A01ViewModelSelectListBuilder<TRecyclateViewModel>, IRecyclateViewModelSelectListBuilder<TRecyclateViewModel>
        where TRecyclateViewModel : IRecyclateViewModel
    {
        public RecyclateViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
        }
    }




    public interface ISemifinishedProductRecyclateViewModelSelectListBuilder : IRecyclateViewModelSelectListBuilder<SemifinishedProductRecyclateViewModel>
    {
    }
    public class SemifinishedProductRecyclateViewModelSelectListBuilder : RecyclateViewModelSelectListBuilder<SemifinishedProductRecyclateViewModel>, ISemifinishedProductRecyclateViewModelSelectListBuilder
    {
        public SemifinishedProductRecyclateViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }

    public interface IFinishedProductRecyclateViewModelSelectListBuilder : IRecyclateViewModelSelectListBuilder<FinishedProductRecyclateViewModel>
    {
    }
    public class FinishedProductRecyclateViewModelSelectListBuilder : RecyclateViewModelSelectListBuilder<FinishedProductRecyclateViewModel>, IFinishedProductRecyclateViewModelSelectListBuilder
    {
        public FinishedProductRecyclateViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }

    public interface IFinishedItemRecyclateViewModelSelectListBuilder : IRecyclateViewModelSelectListBuilder<FinishedItemRecyclateViewModel>
    {
    }
    public class FinishedItemRecyclateViewModelSelectListBuilder : RecyclateViewModelSelectListBuilder<FinishedItemRecyclateViewModel>, IFinishedItemRecyclateViewModelSelectListBuilder
    {
        public FinishedItemRecyclateViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }
}