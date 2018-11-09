using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface ITransferOrderTypeRepository
    {
        IList<TransferOrderType> GetAllTransferOrderTypes();
    }    
}
