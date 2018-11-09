using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalCore.Repositories.Inventories;
using TotalDAL.Helpers;

namespace TotalDAL.Repositories.Inventories
{
    public class GoodsDeliveryRepository : GenericWithDetailRepository<GoodsDelivery, GoodsDeliveryDetail>, IGoodsDeliveryRepository
    {
        public GoodsDeliveryRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities) { }
    }


    public class GoodsDeliveryAPIRepository : GenericAPIRepository, IGoodsDeliveryAPIRepository
    {
        public GoodsDeliveryAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetGoodsDeliveryIndexes")
        {
        }





        public IEnumerable<PendingHandlingUnitReceiver> GetReceivers(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingHandlingUnitReceiver> pendingHandlingUnitReceivers = base.TotalSmartPortalEntities.GetPendingHandlingUnitReceivers(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingHandlingUnitReceivers;
        }

        public IEnumerable<PendingHandlingUnit> GetPendingHandlingUnits(int? goodsDeliveryID, int? receiverID, string handlingUnitIDs, bool isReadonly)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingHandlingUnit> pendingHandlingUnits = base.TotalSmartPortalEntities.GetPendingHandlingUnits(goodsDeliveryID, receiverID, handlingUnitIDs, isReadonly).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingHandlingUnits;
        }
    }

}
