using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{
   
    public interface IFinishedHandoverRepository : IGenericWithDetailRepository<FinishedHandover, FinishedHandoverDetail>
    {
    }

    public interface IFinishedHandoverAPIRepository : IGenericAPIRepository
    {
        IEnumerable<FinishedHandoverPendingCustomer> GetCustomers(int? locationID);
        IEnumerable<FinishedHandoverPendingPlannedOrder> GetPlannedOrders(int? locationID);

        IEnumerable<FinishedHandoverPendingDetail> GetPendingDetails(int? finishedHandoverID, int? plannedOrderID, int? customerID, string finishedProductPackageIDs, bool? isReadonly);

    }
}
