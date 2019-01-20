using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{
    public interface ISemifinishedItemRepository : IGenericWithDetailRepository<SemifinishedItem, SemifinishedItemDetail>
    {
    }

    public interface ISemifinishedItemAPIRepository : IGenericAPIRepository
    {
        IEnumerable<SemifinishedItemPendingMaterialIssue> GetMaterialIssues(int? locationID);
    }
}
