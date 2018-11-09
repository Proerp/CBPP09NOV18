using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalCore.Repositories.Accounts;
using TotalDAL.Helpers;

namespace TotalDAL.Repositories.Accounts
{
    public class ReceiptRepository : GenericWithDetailRepository<Receipt, ReceiptDetail>, IReceiptRepository
    {
        public ReceiptRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "ReceiptEditable")
        {
        }

        
    }


    public class ReceiptAPIRepository : GenericAPIRepository, IReceiptAPIRepository
    {
        public ReceiptAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetReceiptIndexes")
        {
        }

        public ICollection<GoodsIssueReceivable> GetGoodsIssueReceivables(int locationID)
        {
            return this.TotalSmartPortalEntities.GetGoodsIssueReceivables(locationID).ToList();
        }

        public ICollection<CustomerReceivable> GetCustomerReceivables(int locationID)
        {
            return this.TotalSmartPortalEntities.GetCustomerReceivables(locationID).ToList();
        }

        public ICollection<PendingCustomerCredit> GetPendingCustomerCredits(int locationID, int customerID)
        {
            return this.TotalSmartPortalEntities.GetPendingCustomerCredits(locationID, customerID).ToList();
        }        
    }
}
