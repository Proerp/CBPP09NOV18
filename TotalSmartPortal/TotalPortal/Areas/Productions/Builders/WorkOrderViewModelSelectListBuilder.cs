using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Productions.ViewModels;

namespace TotalPortal.Areas.Productions.Builders
{
    public interface IWorkOrderViewModelSelectListBuilder<TWorkOrderViewModel> : IViewModelSelectListBuilder<TWorkOrderViewModel>
        where TWorkOrderViewModel : IWorkOrderViewModel
    {
    }

    public class WorkOrderViewModelSelectListBuilder<TWorkOrderViewModel> : A01ViewModelSelectListBuilder<TWorkOrderViewModel>, IWorkOrderViewModelSelectListBuilder<TWorkOrderViewModel>
        where TWorkOrderViewModel : IWorkOrderViewModel
    {
        public WorkOrderViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
        }
    }









    public interface IMaterialWorkOrderViewModelSelectListBuilder : IWorkOrderViewModelSelectListBuilder<MaterialWorkOrderViewModel>
    {
    }
    public class MaterialWorkOrderViewModelSelectListBuilder : WorkOrderViewModelSelectListBuilder<MaterialWorkOrderViewModel>, IMaterialWorkOrderViewModelSelectListBuilder
    {
        public MaterialWorkOrderViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }


    public interface IItemWorkOrderViewModelSelectListBuilder : IWorkOrderViewModelSelectListBuilder<ItemWorkOrderViewModel>
    {
    }
    public class ItemWorkOrderViewModelSelectListBuilder : WorkOrderViewModelSelectListBuilder<ItemWorkOrderViewModel>, IItemWorkOrderViewModelSelectListBuilder
    {
        public ItemWorkOrderViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }


    public interface IProductWorkOrderViewModelSelectListBuilder : IWorkOrderViewModelSelectListBuilder<ProductWorkOrderViewModel>
    {
    }
    public class ProductWorkOrderViewModelSelectListBuilder : WorkOrderViewModelSelectListBuilder<ProductWorkOrderViewModel>, IProductWorkOrderViewModelSelectListBuilder
    {
        public ProductWorkOrderViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }
}