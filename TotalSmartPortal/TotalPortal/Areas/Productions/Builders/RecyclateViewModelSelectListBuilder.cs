using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Productions.ViewModels;

namespace TotalPortal.Areas.Productions.Builders
{
    public interface IRecyclateViewModelSelectListBuilder : IViewModelSelectListBuilder<RecyclateViewModel>
    {
    }

    public class RecyclateViewModelSelectListBuilder : A01ViewModelSelectListBuilder<RecyclateViewModel>, IRecyclateViewModelSelectListBuilder
    {
        public RecyclateViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IShiftSelectListBuilder shiftSelectListBuilder, IShiftRepository shiftRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
        }
    }
}