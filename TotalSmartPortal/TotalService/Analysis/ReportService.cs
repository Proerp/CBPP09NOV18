using TotalModel.Models;
using TotalDTO.Analysis;
using TotalCore.Repositories.Analysis;
using TotalCore.Services.Analysis;

namespace TotalService.Analysis
{
    public class ReportService : GenericService<Report, ReportDTO, ReportPrimitiveDTO>, IReportService
    {
        public ReportService(IReportRepository reportRepository)
            : base(reportRepository, "ReportPostSaveValidate", "ReportSaveRelative")
        {
        }
    }
}