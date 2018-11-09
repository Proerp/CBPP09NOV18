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
        IEnumerable<ProductionOrderPendingCustomer> GetCustomers(int? locationID);
        IEnumerable<ProductionOrderPendingPlannedOrder> GetPlannedOrders(int? locationID);

        IEnumerable<ProductionOrderPendingFirmOrder> GetPendingFirmOrders(int? locationID, int? productionOrderID, int? plannedOrderID, int? customerID, string firmOrderIDs, bool isReadonly);
    }
}