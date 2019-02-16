using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Productions.ViewModels;

namespace TotalPortal.Areas.Productions.Builders
{
    public interface IFinishedItemViewModelSelectListBuilder : IViewModelSelectListBuilder<FinishedItemViewModel>
    {
    }

    public class FinishedItemViewModelSelectListBuilder : A01ViewModelSelectListBuilder<FinishedItemViewModel>, IFinishedItemViewModelSelectListBuilder
    {
        private readonly IShiftSelectListBuilder shiftSelectListBuilder;
        private readonly IShiftRepository shiftRepository;

        public FinishedItemViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IShiftSelectListBuilder shiftSelectListBuilder, IShiftRepository shiftRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
            this.shiftSelectListBuilder = shiftSelectListBuilder;
            this.shiftRepository = shiftRepository;
        }

        public override void BuildSelectLists(FinishedItemViewModel finishedItemViewModel)
        {
            base.BuildSelectLists(finishedItemViewModel);
            finishedItemViewModel.ShiftSelectList = this.shiftSelectListBuilder.BuildSelectListItemsForShifts(this.shiftRepository.GetAllShifts());
        }
    }
}