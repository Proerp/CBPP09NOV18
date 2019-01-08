using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Commons.ViewModels;

namespace TotalPortal.Areas.Commons.Builders
{
    public interface IBinLocationSelectListBuilder : IViewModelSelectListBuilder<BinLocationViewModel>
    {
    }

    public class BinLocationSelectListBuilder : IBinLocationSelectListBuilder
    {
        public virtual void BuildSelectLists(BinLocationViewModel binLocationViewModel)
        {
        }
    }

}