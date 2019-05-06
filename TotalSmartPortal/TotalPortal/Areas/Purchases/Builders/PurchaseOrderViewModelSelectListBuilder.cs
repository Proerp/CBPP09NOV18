using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Purchases.ViewModels;

namespace TotalPortal.Areas.Purchases.Builders
{
    public interface IPurchaseOrderViewModelSelectListBuilder<TPurchaseOrderViewModel> : IViewModelSelectListBuilder<TPurchaseOrderViewModel>
        where TPurchaseOrderViewModel : IPurchaseOrderViewModel
    {
    }

    public class PurchaseOrderViewModelSelectListBuilder<TPurchaseOrderViewModel> : A01ViewModelSelectListBuilder<TPurchaseOrderViewModel>, IPurchaseOrderViewModelSelectListBuilder<TPurchaseOrderViewModel>
        where TPurchaseOrderViewModel : IPurchaseOrderViewModel
    {
        public PurchaseOrderViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
        }
    }


    public interface IPurchaseMaterialViewModelSelectListBuilder : IPurchaseOrderViewModelSelectListBuilder<PurchaseMaterialViewModel>
    {
    }
    public class PurchaseMaterialViewModelSelectListBuilder : PurchaseOrderViewModelSelectListBuilder<PurchaseMaterialViewModel>, IPurchaseMaterialViewModelSelectListBuilder
    {
        public PurchaseMaterialViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }


    public interface IPurchaseItemViewModelSelectListBuilder : IPurchaseOrderViewModelSelectListBuilder<PurchaseItemViewModel>
    {
    }
    public class PurchaseItemViewModelSelectListBuilder : PurchaseOrderViewModelSelectListBuilder<PurchaseItemViewModel>, IPurchaseItemViewModelSelectListBuilder
    {
        public PurchaseItemViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }


    public interface IPurchaseProductViewModelSelectListBuilder : IPurchaseOrderViewModelSelectListBuilder<PurchaseProductViewModel>
    {
    }
    public class PurchaseProductViewModelSelectListBuilder : PurchaseOrderViewModelSelectListBuilder<PurchaseProductViewModel>, IPurchaseProductViewModelSelectListBuilder
    {
        public PurchaseProductViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        { }
    }
}