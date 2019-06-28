using System;
using System.Collections.Generic;

using TotalBase.Enums;
using TotalModel.Models;

namespace TotalCore.Repositories
{
    public interface IGenericAPIRepository : IBaseRepository
    {
        ICollection<TEntityIndex> GetEntityIndexes<TEntityIndex>(string aspUserID, DateTime fromDate, DateTime toDate);
        ICollection<TEntityIndex> GetEntityIndexes<TEntityIndex>(string aspUserID, DateTime fromDate, DateTime toDate, string functionNameGetEntityIndexes);







        #region HELPERS
        IEnumerable<GoodsReceiptBarcodeAvailable> GetBarcodeAvailables(string barcode);
        bool BarcodeNotFoundMessage(out int? foundCommodityID, out string message, bool goodsArrival_VS_GoodsReceipt, int? locationID, int? warehouseID, int? warehouseReceiptID, int? commodityID, string commodityIDs, int? batchID, int? blendingInstructionID, string barcode, string goodsReceiptDetailIDs, bool onlyApproved, bool onlyIssuable);
        #endregion HELPERS
    }
}
