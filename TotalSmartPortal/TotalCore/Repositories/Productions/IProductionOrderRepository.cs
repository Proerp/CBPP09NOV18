using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{
    public interface IProductionOrderRepository : IGenericWithDetailRepository<ProductionOrder, ProductionOrderDetail>
    {
    }

    public interface IProductionOrderAPIRepository : IGenericAPIRepository
    {
        IEnumerable<ProductionOrderPendingCustomer> GetCustomers(int? locationID, int? nmvnTaskID);
        IEnumerable<ProductionOrderPendingPlannedOrder> GetPlannedOrders(int? locationID, int? nmvnTaskID);

        IEnumerable<ProductionOrderPendingFirmOrder> GetPendingFirmOrders(int? locationID, int? nmvnTaskID, int? productionOrderID, int? plannedOrderID, int? customerID, string firmOrderIDs, bool isReadonly);
    }
}