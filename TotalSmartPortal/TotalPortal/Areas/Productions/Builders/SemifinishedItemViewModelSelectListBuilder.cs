using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Productions.ViewModels;

namespace TotalPortal.Areas.Productions.Builders
{
    public interface ISemifinishedItemViewModelSelectListBuilder : IViewModelSelectListBuilder<SemifinishedItemViewModel>
    {
    }

    public class SemifinishedItemViewModelSelectListBuilder : A01ViewModelSelectListBuilder<SemifinishedItemViewModel>, ISemifinishedItemViewModelSelectListBuilder
    {
        private readonly IShiftSelectListBuilder shiftSelectListBuilder;
        private readonly IShiftRepository shiftRepository;

        public SemifinishedItemViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IShiftSelectListBuilder shiftSelectListBuilder, IShiftRepository shiftRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
            this.shiftSelectListBuilder = shiftSelectListBuilder;
            this.shiftRepository = shiftRepository;
        }

        public override void BuildSelectLists(SemifinishedItemViewModel semifinishedItemViewModel)
        {
            base.BuildSelectLists(semifinishedItemViewModel);
            semifinishedItemViewModel.ShiftSelectList = this.shiftSelectListBuilder.BuildSelectListItemsForShifts(this.shiftRepository.GetAllShifts());
        }
    }
}