using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Purchases.ViewModels;

namespace TotalPortal.Areas.Purchases.Builders
{
    public interface IPurchaseRequisitionViewModelSelectListBuilder : IViewModelSelectListBuilder<PurchaseRequisitionViewModel>
    {
    }

    public class PurchaseRequisitionViewModelSelectListBuilder : A01ViewModelSelectListBuilder<PurchaseRequisitionViewModel>, IPurchaseRequisitionViewModelSelectListBuilder
    {
        public PurchaseRequisitionViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
        }
    }

}