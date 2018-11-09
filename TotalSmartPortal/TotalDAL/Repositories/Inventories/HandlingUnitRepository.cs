using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalCore.Repositories.Inventories;
using TotalDAL.Helpers;

namespace TotalDAL.Repositories.Inventories
{
    public class HandlingUnitRepository : GenericWithDetailRepository<HandlingUnit, HandlingUnitDetail>, IHandlingUnitRepository
    {
        public HandlingUnitRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "HandlingUnitEditable") { }
    }


    public class HandlingUnitAPIRepository : GenericAPIRepository, IHandlingUnitAPIRepository
    {
        public HandlingUnitAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetHandlingUnitIndexes")
        {
        }

        public IEnumerable<HandlingUnitPendingCustomer> GetCustomers(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<HandlingUnitPendingCustomer> pendingGoodsIssueCustomers = base.TotalSmartPortalEntities.GetHandlingUnitPendingCustomers(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssueCustomers;
        }

        public IEnumerable<HandlingUnitPendingGoodsIssue> GetGoodsIssues(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<HandlingUnitPendingGoodsIssue> pendingGoodsIssues = base.TotalSmartPortalEntities.GetHandlingUnitPendingGoodsIssues(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssues;
        }


        public IEnumerable<HandlingUnitPendingGoodsIssueDetail> GetPendingGoodsIssueDetails(int? locationID, int? handlingUnitID, int? goodsIssueID, int? customerID, int? receiverID, string shippingAddress, string addressee, string goodsIssueDetailIDs, bool isReadonly)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<HandlingUnitPendingGoodsIssueDetail> pendingGoodsIssueDetails = base.TotalSmartPortalEntities.GetHandlingUnitPendingGoodsIssueDetails(locationID, handlingUnitID, goodsIssueID, customerID, receiverID, shippingAddress, addressee, goodsIssueDetailIDs, isReadonly).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssueDetails;
        }
    }

}
