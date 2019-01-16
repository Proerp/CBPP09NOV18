using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Builders
{
    public interface IMaterialIssueViewModelSelectListBuilder<TMaterialIssueViewModel> : IViewModelSelectListBuilder<TMaterialIssueViewModel>
        where TMaterialIssueViewModel : IMaterialIssueViewModel
    {
    }

    public class MaterialIssueViewModelSelectListBuilder<TMaterialIssueViewModel> : A01ViewModelSelectListBuilder<TMaterialIssueViewModel>, IMaterialIssueViewModelSelectListBuilder<TMaterialIssueViewModel>
        where TMaterialIssueViewModel : IMaterialIssueViewModel
    {
        private readonly IShiftSelectListBuilder shiftSelectListBuilder;
        private readonly IShiftRepository shiftRepository;

        public MaterialIssueViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IShiftSelectListBuilder shiftSelectListBuilder, IShiftRepository shiftRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
            this.shiftSelectListBuilder = shiftSelectListBuilder;
            this.shiftRepository = shiftRepository;
        }

        public override void BuildSelectLists(TMaterialIssueViewModel materialIssueViewModel)
        {
            base.BuildSelectLists(materialIssueViewModel);
            materialIssueViewModel.ShiftSelectList = this.shiftSelectListBuilder.BuildSelectListItemsForShifts(this.shiftRepository.GetAllShifts());
        }
    }









    public interface IMaterialStagingViewModelSelectListBuilder : IMaterialIssueViewModelSelectListBuilder<MaterialStagingViewModel>
    {
    }
    public class MaterialStagingViewModelSelectListBuilder : MaterialIssueViewModelSelectListBuilder<MaterialStagingViewModel>, IMaterialStagingViewModelSelectListBuilder
    {
        public MaterialStagingViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IShiftSelectListBuilder shiftSelectListBuilder, IShiftRepository shiftRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, shiftSelectListBuilder, shiftRepository)
        { }
    }


    public interface IItemStagingViewModelSelectListBuilder : IMaterialIssueViewModelSelectListBuilder<ItemStagingViewModel>
    {
    }
    public class ItemStagingViewModelSelectListBuilder : MaterialIssueViewModelSelectListBuilder<ItemStagingViewModel>, IItemStagingViewModelSelectListBuilder
    {
        public ItemStagingViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IShiftSelectListBuilder shiftSelectListBuilder, IShiftRepository shiftRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, shiftSelectListBuilder, shiftRepository)
        { }
    }


    public interface IProductStagingViewModelSelectListBuilder : IMaterialIssueViewModelSelectListBuilder<ProductStagingViewModel>
    {
    }
    public class ProductStagingViewModelSelectListBuilder : MaterialIssueViewModelSelectListBuilder<ProductStagingViewModel>, IProductStagingViewModelSelectListBuilder
    {
        public ProductStagingViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IShiftSelectListBuilder shiftSelectListBuilder, IShiftRepository shiftRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, shiftSelectListBuilder, shiftRepository)
        { }
    }
}