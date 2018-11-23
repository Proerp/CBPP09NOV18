using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Builders
{
    public interface IPackageIssueViewModelSelectListBuilder : IViewModelSelectListBuilder<PackageIssueViewModel>
    {
    }

    public class PackageIssueViewModelSelectListBuilder : A01ViewModelSelectListBuilder<PackageIssueViewModel>, IPackageIssueViewModelSelectListBuilder
    {
        private readonly IShiftSelectListBuilder shiftSelectListBuilder;
        private readonly IShiftRepository shiftRepository;

        public PackageIssueViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IShiftSelectListBuilder shiftSelectListBuilder, IShiftRepository shiftRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
            this.shiftSelectListBuilder = shiftSelectListBuilder;
            this.shiftRepository = shiftRepository;
        }

        public override void BuildSelectLists(PackageIssueViewModel packageIssueViewModel)
        {
            base.BuildSelectLists(packageIssueViewModel);
            packageIssueViewModel.ShiftSelectList = this.shiftSelectListBuilder.BuildSelectListItemsForShifts(this.shiftRepository.GetAllShifts());
        }
    }

}