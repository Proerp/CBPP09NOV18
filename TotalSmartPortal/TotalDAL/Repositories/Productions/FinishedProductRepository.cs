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
    public class FinishedProductRepository : GenericWithDetailRepository<FinishedProduct, FinishedProductDetail>, IFinishedProductRepository
    {
        public FinishedProductRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "FinishedProductEditable", "FinishedProductApproved")
        {
        }
    }

    public class FinishedProductAPIRepository : GenericAPIRepository, IFinishedProductAPIRepository
    {
        public FinishedProductAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetFinishedProductIndexes")
        {
        }

        public IEnumerable<FinishedProductPendingFirmOrder> GetFirmOrders(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<FinishedProductPendingFirmOrder> pendingFirmOrder = base.TotalSmartPortalEntities.GetFinishedProductPendingFirmOrders(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingFirmOrder;
        }

    }
}
