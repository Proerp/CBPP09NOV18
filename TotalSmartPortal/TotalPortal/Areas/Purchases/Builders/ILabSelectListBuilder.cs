using TotalPortal.Builders;
using TotalPortal.Areas.Purchases.ViewModels;

namespace TotalPortal.Areas.Purchases.Builders
{
    public interface ILabSelectListBuilder : IViewModelSelectListBuilder<LabViewModel>
    {
    }

    public class LabSelectListBuilder : ILabSelectListBuilder
    {
        public virtual void BuildSelectLists(LabViewModel labViewModel)
        {
        }
    }
}