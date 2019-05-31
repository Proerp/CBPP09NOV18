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
        private readonly IShiftSelectListBuilder shiftSelectListBuilder;
        private readonly IShiftRepository shiftRepository;

        public SemifinishedRecyclateViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IShiftSelectListBuilder shiftSelectListBuilder, IShiftRepository shiftRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
            this.shiftSelectListBuilder = shiftSelectListBuilder;
            this.shiftRepository = shiftRepository;
        }

        public override void BuildSelectLists(SemifinishedRecyclateViewModel semifinishedRecyclateViewModel)
        {
            base.BuildSelectLists(semifinishedRecyclateViewModel);
            semifinishedRecyclateViewModel.ShiftSelectList = this.shiftSelectListBuilder.BuildSelectListItemsForShifts(this.shiftRepository.GetAllShifts());
        }
    }
}