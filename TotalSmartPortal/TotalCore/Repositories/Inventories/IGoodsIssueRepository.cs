using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Inventories
{
    public interface IGoodsIssueRepository : IGenericWithDetailRepository<GoodsIssue, GoodsIssueDetail>
    {
        List<GoodsIssueViewPackage> GetGoodsIssueViewPackages(int? goodsIssueID);
        List<PendingDeliveryAdviceDescription> GetDescriptions(int locationID, int customerID, int receiverID, int warehouseID, string shippingAddress, string addressee, int? tradePromotionID, decimal? vatPercent);
    }

    public interface IGoodsIssueAPIRepository : IGenericAPIRepository
    {
        ICollection<PendingDeliveryAdvice> GetDeliveryAdvices(int locationID);
        ICollection<PendingDeliveryAdviceCustomer> GetCustomers(int locationID);

        ICollection<PendingDeliveryAdviceDetail> GetPendingDeliveryAdviceDetails(bool webAPI, int? locationID, int? goodsIssueID, int? deliveryAdviceDetailID, int? warehouseID, string barcode, string goodsReceiptDetailIDs);
    }
}
