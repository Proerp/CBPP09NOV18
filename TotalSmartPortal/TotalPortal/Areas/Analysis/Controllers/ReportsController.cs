using TotalModel.Models;

using TotalDTO.Analysis;
using TotalCore.Services.Analysis;

using TotalPortal.Controllers;
using TotalPortal.Areas.Analysis.ViewModels;
using TotalPortal.Areas.Analysis.Builders;

namespace TotalPortal.Areas.Analysis.Controllers
{
    public class ReportsController : GenericSimpleController<Report, ReportDTO, ReportPrimitiveDTO, ReportViewModel>
    {
        public ReportsController(IReportService reportService, IReportSelectListBuilder reportViewModelSelectListBuilder)
            : base(reportService, reportViewModelSelectListBuilder)
        {
        }
    }
}