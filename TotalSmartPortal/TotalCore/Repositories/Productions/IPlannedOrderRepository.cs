using System.Collections.Generic;
using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{
    public interface IPlannedOrderRepository : IGenericWithDetailRepository<PlannedOrder, PlannedOrderDetail>
    {
    }

    public interface IPlannedOrderAPIRepository : IGenericAPIRepository
    {
        IEnumerable<PlannedOrderLog> GetPlannedOrderLogs(int? plannedOrderID, int? firmOrderID);
    }
}