using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Commons.ViewModels;

namespace TotalPortal.Areas.Commons.Builders
{
    public interface IMoldSelectListBuilder : IViewModelSelectListBuilder<MoldViewModel>
    {
    }

    public class MoldSelectListBuilder : IMoldSelectListBuilder
    {
        public virtual void BuildSelectLists(MoldViewModel moldViewModel)
        {
        }
    }

}