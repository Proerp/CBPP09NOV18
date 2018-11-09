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
        IEnumerable<SemifinishedHandoverPendingCustomer> GetCustomers(int? locationID);
        IEnumerable<SemifinishedHandoverPendingWorkshift> GetWorkshifts(int? locationID);

        IEnumerable<SemifinishedHandoverPendingDetail> GetPendingDetails(int? semifinishedHandoverID, int? workshiftID, int? customerID, string semifinishedProductIDs, bool? isReadonly);
     
    }
}
