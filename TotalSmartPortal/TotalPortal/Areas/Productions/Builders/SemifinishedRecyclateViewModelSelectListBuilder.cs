using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Productions.ViewModels;

namespace TotalPortal.Areas.Productions.Builders
{
    public interface ISemifinishedRecyclateViewModelSelectListBuilder : IViewModelSelectListBuilder<SemifinishedRecyclateViewModel>
    {
    }

    public class SemifinishedRecyclateViewModelSelectListBuilder : A01ViewModelSelectListBuilder<SemifinishedRecyclateViewModel>, ISemifinishedRecyclateViewModelSelectListBuilder
    {
        public SemifinishedRecyclateViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IShiftSelectListBuilder shiftSelectListBuilder, IShiftRepository shiftRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
        }
    }
}