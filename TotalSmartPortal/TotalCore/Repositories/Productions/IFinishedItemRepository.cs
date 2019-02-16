using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{

    public interface IFinishedItemRepository : IGenericWithDetailRepository<FinishedItem, FinishedItemDetail>
    {
    }

    public interface IFinishedItemAPIRepository : IGenericAPIRepository
    {
        IEnumerable<FinishedItemPendingFirmOrder> GetFirmOrders(int? locationID);

    }
}
