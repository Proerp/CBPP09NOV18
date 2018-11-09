using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Productions.ViewModels;

namespace TotalPortal.Areas.Productions.Builders
{
    public interface IProductionOrderViewModelSelectListBuilder : IViewModelSelectListBuilder<ProductionOrderViewModel>
    {
    }

    public class ProductionOrderViewModelSelectListBuilder : A01ViewModelSelectListBuilder<ProductionOrderViewModel>, IProductionOrderViewModelSelectListBuilder
    {
        public ProductionOrderViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
        }
    }

}