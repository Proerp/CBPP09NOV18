using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Purchases.ViewModels;

namespace TotalPortal.Areas.Purchases.Builders
{
    public interface IGoodsArrivalViewModelSelectListBuilder : IViewModelSelectListBuilder<GoodsArrivalViewModel>
    {
    }

    public class GoodsArrivalViewModelSelectListBuilder : A01ViewModelSelectListBuilder<GoodsArrivalViewModel>, IGoodsArrivalViewModelSelectListBuilder
    {
        public GoodsArrivalViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
        }
    }
}