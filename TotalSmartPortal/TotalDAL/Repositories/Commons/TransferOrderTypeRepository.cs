using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{   
    public class TransferOrderTypeRepository : ITransferOrderTypeRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public TransferOrderTypeRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public IList<TransferOrderType> GetAllTransferOrderTypes()
        {
            return this.totalSmartPortalEntities.TransferOrderTypes.ToList();
        }

    }
}
