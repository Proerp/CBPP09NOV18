using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Purchases.ViewModels;

namespace TotalPortal.Areas.Purchases.Builders
{
    public interface IPurchaseOrderViewModelSelectListBuilder : IViewModelSelectListBuilder<PurchaseOrderViewModel>
    {
    }

    public class PurchaseOrderViewModelSelectListBuilder : A01ViewModelSelectListBuilder<PurchaseOrderViewModel>, IPurchaseOrderViewModelSelectListBuilder
    {
        public PurchaseOrderViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
        }
    }

}