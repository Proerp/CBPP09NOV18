using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Commons.ViewModels;

namespace TotalPortal.Areas.Commons.Builders
{
    public interface IBomViewModelSelectListBuilder : IViewModelSelectListBuilder<BomViewModel>
    {
    }

    public class BomViewModelSelectListBuilder : IBomViewModelSelectListBuilder
    {
        public virtual void BuildSelectLists(BinLocationViewModel binLocationViewModel)
        {
        }
    }

}