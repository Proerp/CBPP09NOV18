using TotalModel.Models;
using TotalCore.Repositories.Analysis;

namespace TotalDAL.Repositories.Analysis
{
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        public ReportRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "ReportEditable", null, "ReportDeletable")
        {
        }
    }





    public class ReportAPIRepository : GenericAPIRepository, IReportAPIRepository
    {
        public ReportAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetReportIndexes")
        {
        }
    }
}
