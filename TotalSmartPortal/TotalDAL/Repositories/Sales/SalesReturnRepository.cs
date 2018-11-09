using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Sales;


namespace TotalDAL.Repositories.Sales
{
    public class SalesReturnRepository : GenericWithDetailRepository<SalesReturn, SalesReturnDetail>, ISalesReturnRepository
    {
        public SalesReturnRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "SalesReturnEditable", "SalesReturnApproved", null, "SalesReturnVoidable")
        {
        }
    }








    public class SalesReturnAPIRepository : GenericAPIRepository, ISalesReturnAPIRepository
    {
        public SalesReturnAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetSalesReturnIndexes")
        {
        }

        public IEnumerable<SalesReturnPendingGoodsIssue> GetGoodsIssues(int? locationID, int? customerID, int? receiverID, DateTime? fromDate, DateTime? toDate)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<SalesReturnPendingGoodsIssue> pendingGoodsIssues = base.TotalSmartPortalEntities.GetSalesReturnPendingGoodsIssues(locationID, customerID, receiverID, fromDate, toDate).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssues;
        }

        public IEnumerable<SalesReturnPendingGoodsIssueDetail> GetPendingGoodsIssueDetails(int? locationID, int? salesReturnID, int? goodsIssueID, int? customerID, int? receiverID, int? tradePromotionID, decimal? vATPercent, DateTime? fromDate, DateTime? toDate, string goodsIssueDetailIDs, bool isReadonly)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<SalesReturnPendingGoodsIssueDetail> pendingGoodsIssueDetails = base.TotalSmartPortalEntities.GetSalesReturnPendingGoodsIssueDetails(locationID, salesReturnID, goodsIssueID, customerID, receiverID, tradePromotionID, vATPercent, fromDate, toDate, goodsIssueDetailIDs, isReadonly).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingGoodsIssueDetails;
        }
    }


}
