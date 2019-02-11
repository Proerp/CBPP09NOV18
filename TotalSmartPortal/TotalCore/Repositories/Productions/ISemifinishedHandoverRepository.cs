using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{  
    public interface ISemifinishedHandoverRepository : IGenericWithDetailRepository<SemifinishedHandover, SemifinishedHandoverDetail>
    {
    }

    public interface ISemifinishedHandoverAPIRepository : IGenericAPIRepository
    {
        IEnumerable<SemifinishedHandoverPendingCustomer> GetCustomers(int? nmvnTaskID, int? locationID);
        IEnumerable<SemifinishedHandoverPendingWorkshift> GetWorkshifts(int? nmvnTaskID, int? locationID);

        IEnumerable<SemifinishedHandoverPendingDetail> GetPendingDetails(int? nmvnTaskID, int? semifinishedHandoverID, int? workshiftID, int? customerID, string semifinishedItemIDs, string semifinishedProductIDs);
     
    }
}
