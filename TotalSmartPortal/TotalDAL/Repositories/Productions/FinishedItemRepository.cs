using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Productions;

namespace TotalDAL.Repositories.Productions
{
    public class FinishedItemRepository : GenericWithDetailRepository<FinishedItem, FinishedItemDetail>, IFinishedItemRepository
    {
        public FinishedItemRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "FinishedItemEditable", "FinishedItemApproved")
        {
        }

        public List<FinishedItemViewLot> GetFinishedItemViewLots(int? finishedItemID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            List<FinishedItemViewLot> finishedItemViewLots = base.TotalSmartPortalEntities.GetFinishedItemViewLots(finishedItemID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return finishedItemViewLots;
        }
    }

    public class FinishedItemAPIRepository : GenericAPIRepository, IFinishedItemAPIRepository
    {
        public FinishedItemAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetFinishedItemIndexes")
        {
        }

        public IEnumerable<FinishedItemPendingFirmOrder> GetFirmOrders(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<FinishedItemPendingFirmOrder> pendingFirmOrder = base.TotalSmartPortalEntities.GetFinishedItemPendingFirmOrders(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingFirmOrder;
        }

    }
}
