using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{
    public interface IWorkOrderRepository : IGenericWithDetailRepository<WorkOrder, WorkOrderDetail>
    {
    }

    public interface IWorkOrderAPIRepository : IGenericAPIRepository
    {
        IEnumerable<WorkOrderPendingFirmOrder> GetFirmOrders(int? locationID, int? nmvnTaskID, int? firmOrderID);
    }

}