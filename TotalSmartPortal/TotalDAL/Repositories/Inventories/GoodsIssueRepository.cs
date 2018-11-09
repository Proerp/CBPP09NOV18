using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalCore.Repositories.Inventories;
using TotalDAL.Helpers;

namespace TotalDAL.Repositories.Inventories
{
    public class GoodsIssueRepository : GenericWithDetailRepository<GoodsIssue, GoodsIssueDetail>, IGoodsIssueRepository
    {
        public GoodsIssueRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GoodsIssueEditable", "GoodsIssueApproved")
        {
        }

        public List<PendingDeliveryAdviceDescription> GetDescriptions(int locationID, int customerID, int receiverID, int warehouseID, string shippingAddress, string addressee, int? tradePromotionID, decimal? vatPercent)
        {
            return this.TotalSmartPortalEntities.GetPendingDeliveryAdviceDescriptions(locationID, customerID, receiverID, warehouseID, shippingAddress, addressee, tradePromotionID, vatPercent).ToList();
        }
    }


    public class GoodsIssueAPIRepository : GenericAPIRepository, IGoodsIssueAPIRepository
    {
        public GoodsIssueAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetGoodsIssueIndexes")
        {
        }

        public ICollection<PendingDeliveryAdvice> GetDeliveryAdvices(int locationID)
        {
            return this.TotalSmartPortalEntities.GetPendingDeliveryAdvices(locationID).ToList();
        }

        public ICollection<PendingDeliveryAdviceCustomer> GetCustomers(int locationID)
        {
            return this.TotalSmartPortalEntities.GetPendingDeliveryAdviceCustomers(locationID).ToList();
        }
    }
}
