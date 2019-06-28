using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalCore.Repositories;
using TotalModel.Models;


namespace TotalDAL.Repositories
{
    public class GenericAPIRepository : BaseRepository, IGenericAPIRepository
    {
        private readonly string functionNameGetEntityIndexes;

        public GenericAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities, string functionNameGetEntityIndexes)
            : base(totalSmartPortalEntities)
        {
            this.functionNameGetEntityIndexes = functionNameGetEntityIndexes;
        }

        public virtual ICollection<TEntityIndex> GetEntityIndexes<TEntityIndex>(string aspUserID, DateTime fromDate, DateTime toDate)
        {
            return this.GetEntityIndexes<TEntityIndex>(aspUserID, fromDate, toDate, this.functionNameGetEntityIndexes);
        }

        public virtual ICollection<TEntityIndex> GetEntityIndexes<TEntityIndex>(string aspUserID, DateTime fromDate, DateTime toDate, string functionNameGetEntityIndexes)
        {
            return base.ExecuteFunction<TEntityIndex>(functionNameGetEntityIndexes, this.GetEntityIndexParameters(aspUserID, fromDate, toDate));
        }

        protected virtual ObjectParameter[] GetEntityIndexParameters(string aspUserID, DateTime fromDate, DateTime toDate)
        {
            return new ObjectParameter[] { new ObjectParameter("AspUserID", aspUserID), new ObjectParameter("FromDate", fromDate), new ObjectParameter("ToDate", toDate) };
        }











        #region HELPERS
        public IEnumerable<GoodsReceiptBarcodeAvailable> GetBarcodeAvailables(string barcode)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptBarcodeAvailable> goodsReceiptBarcodeAvailables = base.TotalSmartPortalEntities.GetGoodsReceiptBarcodeAvailables(barcode).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return goodsReceiptBarcodeAvailables;
        }

        public bool BarcodeNotFoundMessage(out int? foundCommodityID, out string message, bool goodsArrival_VS_GoodsReceipt, int? locationID, int? warehouseID, int? warehouseReceiptID, int? commodityID, string commodityIDs, int? batchID, int? blendingInstructionID, string barcode, string goodsReceiptDetailIDs, bool onlyApproved, bool onlyIssuable)
        {
            List<GoodsReceiptBarcodeAvailable> barcodeAvailables = this.GetBarcodeAvailables(barcode).ToList(); foundCommodityID = null; message = "";

            if (barcodeAvailables.Count == 0) message = "Mã vạch không tồn tại" + (goodsArrival_VS_GoodsReceipt ? " hoặc Phiếu nhận hàng chưa duyệt" : "");
            else
                if (goodsArrival_VS_GoodsReceipt)
                {
                    if (barcodeAvailables[0].GoodsReceiptDetailID != null) message = "Đã nhập kho";
                    else
                        if (barcodeAvailables.Where(w => (!onlyApproved || w.Approved)).Count() == 0) message = "Phiếu nhận hàng chưa duyệt";
                        else
                            if (goodsReceiptDetailIDs != null && goodsReceiptDetailIDs != "" && goodsReceiptDetailIDs != "0")
                            {
                                foreach (GoodsReceiptBarcodeAvailable barcodeAvailable in barcodeAvailables)
                                {
                                    if ((goodsReceiptDetailIDs + ",").Contains(barcodeAvailable.GoodsArrivalPackageID.ToString() + ",")) message = "Phuy vừa mới quét xong";
                                }
                            }
                }
                else //goodsArrivalVSGoodsReceipt == false
                {
                    if (barcodeAvailables[0].GoodsReceiptDetailID == null) message = "Mã vạch mới in, chưa nhập kho";
                    else
                        if (barcodeAvailables[0].QuantityAvailables == 0) message = "Phuy đã xuất hết, không còn tồn";
                        else
                            if (barcodeAvailables.Where(w => w.WarehouseID == warehouseID).Count() == 0) message = "NVL đang ở kho khác [" + barcodeAvailables[0].WarehouseCode + "]"; //HAVE NOT CHECKED THIS YET (@OnlyIssuable = 0 OR Warehouses.Issuable = 1)
                            else
                                if (barcodeAvailables.Where(w => w.WarehouseID == warehouseID && (!onlyApproved || w.Approved)).Count() == 0) message = "Phiếu nhập chưa hoàn tất";
                                else
                                    if (warehouseReceiptID == 6 && barcodeAvailables.Where(w => w.WarehouseID == warehouseID && (!onlyApproved || w.Approved) && (bool)w.LabApproved).Count() == 0) message = "Lab chưa PASS";
                                    else
                                        if (warehouseReceiptID == 6 && barcodeAvailables.Where(w => w.WarehouseID == warehouseID && (!onlyApproved || w.Approved) && (bool)w.LabApproved && !(bool)w.LabInActive).Count() == 0) message = "Lab đang quarantine";
                                        else
                                            if (warehouseReceiptID == 6 && barcodeAvailables.Where(w => w.WarehouseID == warehouseID && (!onlyApproved || w.Approved) && (bool)w.LabApproved && !(bool)w.LabInActive && !(bool)w.LabHold).Count() == 0) message = "Lab đang hold";
                                            else
                                            {
                                                if (batchID != null || (commodityID != null && commodityID != 0) || (commodityIDs != null && commodityIDs != "" && commodityIDs != "0") || (goodsReceiptDetailIDs != null && goodsReceiptDetailIDs != "" && goodsReceiptDetailIDs != "0"))
                                                {
                                                    foreach (GoodsReceiptBarcodeAvailable barcodeAvailable in barcodeAvailables.Where(w => w.WarehouseID == warehouseID && (!onlyApproved || w.Approved) && (warehouseReceiptID != 6 || ((bool)w.LabApproved && !(bool)w.LabInActive && !(bool)w.LabHold))).ToList())
                                                    {
                                                        if (batchID != null && barcodeAvailable.BatchID != batchID) message = "Không đúng BATCH yêu cầu";
                                                        if (commodityID != null && commodityID != 0 && barcodeAvailable.CommodityID != commodityID) message = "Không đúng mã NVL yêu cầu";
                                                        if ((commodityIDs != null && commodityIDs != "" && commodityIDs != "0") && !(commodityIDs + ",").Contains(barcodeAvailable.CommodityID.ToString() + ",")) message = "Không đúng mã NVL yêu cầu";
                                                        if ((goodsReceiptDetailIDs != null && goodsReceiptDetailIDs != "" && goodsReceiptDetailIDs != "0") && (goodsReceiptDetailIDs + ",").Contains(barcodeAvailable.GoodsReceiptDetailID.ToString() + ",")) message = "Phuy vừa mới quét xong";
                                                    }
                                                }
                                            }
                }


            if (message == "" && !goodsArrival_VS_GoodsReceipt && barcodeAvailables.Count > 0) foundCommodityID = barcodeAvailables.Where(w => w.WarehouseID == warehouseID && (!onlyApproved || w.Approved) && (warehouseReceiptID != 6 || ((bool)w.LabApproved && !(bool)w.LabInActive && !(bool)w.LabHold))).ToList()[0].CommodityID;
            return message != "";
        }

        #endregion HELPERS
    }
}
