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
        private readonly IShiftSelectListBuilder shiftSelectListBuilder;
        private readonly IShiftRepository shiftRepository;

        public FinishedProductViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IShiftSelectListBuilder shiftSelectListBuilder, IShiftRepository shiftRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
            this.shiftSelectListBuilder = shiftSelectListBuilder;
            this.shiftRepository = shiftRepository;
        }

        public override void BuildSelectLists(FinishedProductViewModel finishedProductViewModel)
        {
            base.BuildSelectLists(finishedProductViewModel);
            finishedProductViewModel.ShiftSelectList = this.shiftSelectListBuilder.BuildSelectListItemsForShifts(this.shiftRepository.GetAllShifts());
        }
    }
}