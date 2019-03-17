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
        IEnumerable<FinishedHandoverPendingWorkshift> GetWorkshifts(int? nmvnTaskID, int? locationID);
        IEnumerable<FinishedHandoverPendingCustomer> GetCustomers(int? nmvnTaskID, int? locationID);
        IEnumerable<FinishedHandoverPendingPlannedOrder> GetPlannedOrders(int? nmvnTaskID, int? locationID);

        IEnumerable<FinishedHandoverPendingDetail> GetPendingDetails(int? nmvnTaskID, int? finishedHandoverID, int? workshiftID, int? plannedOrderID, int? customerID, string finishedItemPackageIDs, string finishedProductPackageIDs);

    }
}
