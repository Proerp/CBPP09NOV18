using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Productions.ViewModels;

namespace TotalPortal.Areas.Productions.Builders
{
    public interface ISemifinishedProductViewModelSelectListBuilder : IViewModelSelectListBuilder<SemifinishedProductViewModel>
    {
    }

    public class SemifinishedProductViewModelSelectListBuilder : A01ViewModelSelectListBuilder<SemifinishedProductViewModel>, ISemifinishedProductViewModelSelectListBuilder
    {
        private readonly IShiftSelectListBuilder shiftSelectListBuilder;
        private readonly IShiftRepository shiftRepository;

        public SemifinishedProductViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IShiftSelectListBuilder shiftSelectListBuilder, IShiftRepository shiftRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
            this.shiftSelectListBuilder = shiftSelectListBuilder;
            this.shiftRepository = shiftRepository;
        }

        public override void BuildSelectLists(SemifinishedProductViewModel semifinishedProductViewModel)
        {
            base.BuildSelectLists(semifinishedProductViewModel);
            semifinishedProductViewModel.ShiftSelectList = this.shiftSelectListBuilder.BuildSelectListItemsForShifts(this.shiftRepository.GetAllShifts());
        }
    }
}