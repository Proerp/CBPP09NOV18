using TotalCore.Repositories.Commons;

using TotalPortal.Builders;
using TotalPortal.Areas.Analysis.Builders;
using TotalPortal.Areas.Analysis.ViewModels;

namespace TotalPortal.Areas.Analysis.Builders
{
    public interface IReportSelectListBuilder : IViewModelSelectListBuilder<ReportViewModel>
    {
    }

    public class ReportSelectListBuilder : IReportSelectListBuilder
    {
        public ReportSelectListBuilder()
        {
        }

        public virtual void BuildSelectLists(ReportViewModel reportViewModel)
        {
        }
    }
}