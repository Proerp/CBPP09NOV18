using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;


using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Accounts;



namespace TotalDAL.Repositories.Accounts
{
    public class AccountInvoiceRepository : GenericWithDetailRepository<AccountInvoice, AccountInvoiceDetail>, IAccountInvoiceRepository
    {
        public AccountInvoiceRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities) { }
    }


    public class AccountInvoiceAPIRepository : GenericAPIRepository, IAccountInvoiceAPIRepository
    {
        public AccountInvoiceAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetAccountInvoiceIndexes")
        {
        }

        public IEnumerable<PendingGoodsIssue> GetGoodsIssues(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingGoodsIssue> pendingGoodsIssues = base.TotalSmartPortalEntities.GetPendingGoodsIssues(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssues;
        }

        public IEnumerable<PendingGoodsIssueConsumer> GetConsumers(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingGoodsIssueConsumer> pendingGoodsIssueConsumers = base.TotalSmartPortalEntities.GetPendingGoodsIssueConsumers(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssueConsumers;
        }

        public IEnumerable<PendingGoodsIssueReceiver> GetReceivers(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingGoodsIssueReceiver> pendingGoodsIssueReceivers = base.TotalSmartPortalEntities.GetPendingGoodsIssueReceivers(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssueReceivers;
        }





        public IEnumerable<PendingGoodsIssueDetail> GetPendingGoodsIssueDetails(int? accountInvoiceID, int? goodsIssueID, int? customerID, int? receiverID, int? tradePromotionID, decimal? vatPercent, int? commodityTypeID, string aspUserID, int? locationID, DateTime fromDate, DateTime toDate, string goodsIssueDetailIDs, bool isReadonly)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingGoodsIssueDetail> pendingGoodsIssueDetails = base.TotalSmartPortalEntities.GetPendingGoodsIssueDetails(accountInvoiceID, locationID, goodsIssueID, customerID, receiverID, tradePromotionID, vatPercent, commodityTypeID, aspUserID, fromDate, toDate, goodsIssueDetailIDs, isReadonly).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssueDetails;
        }
    }

}
