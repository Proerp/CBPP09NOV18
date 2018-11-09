using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;


namespace TotalDAL.Repositories.Commons
{
    public class PaymentTermRepository : IPaymentTermRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public PaymentTermRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public IList<PaymentTerm> GetAllPaymentTerms()
        {
            return this.totalSmartPortalEntities.PaymentTerms.ToList();
        }
    }
}

