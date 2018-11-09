using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{  

    public interface IFinishedProductRepository : IGenericWithDetailRepository<FinishedProduct, FinishedProductDetail>
    {
    }

    public interface IFinishedProductAPIRepository : IGenericAPIRepository
    {
        IEnumerable<FinishedProductPendingFirmOrder> GetFirmOrders(int? locationID);

    }
}
