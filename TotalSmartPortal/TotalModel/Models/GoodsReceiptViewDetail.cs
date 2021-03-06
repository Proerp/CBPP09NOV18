//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TotalModel.Models
{
    using System;
    
    public partial class GoodsReceiptViewDetail
    {
        public int GoodsReceiptDetailID { get; set; }
        public int GoodsReceiptID { get; set; }
        public Nullable<int> PurchaseRequisitionID { get; set; }
        public Nullable<int> PurchaseRequisitionDetailID { get; set; }
        public string PurchaseRequisitionReference { get; set; }
        public string PurchaseRequisitionCode { get; set; }
        public Nullable<System.DateTime> PurchaseRequisitionEntryDate { get; set; }
        public Nullable<int> WarehouseTransferID { get; set; }
        public Nullable<int> WarehouseTransferDetailID { get; set; }
        public string WarehouseTransferReference { get; set; }
        public Nullable<System.DateTime> WarehouseTransferEntryDate { get; set; }
        public Nullable<int> WarehouseAdjustmentID { get; set; }
        public Nullable<int> WarehouseAdjustmentDetailID { get; set; }
        public string WarehouseAdjustmentReference { get; set; }
        public Nullable<System.DateTime> WarehouseAdjustmentEntryDate { get; set; }
        public Nullable<int> WarehouseAdjustmentTypeID { get; set; }
        public int CommodityID { get; set; }
        public string CommodityCode { get; set; }
        public string CommodityName { get; set; }
        public int CommodityTypeID { get; set; }
        public Nullable<decimal> QuantityRemains { get; set; }
        public decimal Quantity { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> MaterialIssueDetailID { get; set; }
        public Nullable<int> MaterialIssueID { get; set; }
        public Nullable<System.DateTime> MaterialIssueEntryDate { get; set; }
        public string WorkshiftName { get; set; }
        public Nullable<System.DateTime> WorkshiftEntryDate { get; set; }
        public string ProductionLinesCode { get; set; }
        public Nullable<int> FinishedProductPackageID { get; set; }
        public Nullable<int> FinishedProductID { get; set; }
        public Nullable<System.DateTime> FinishedProductEntryDate { get; set; }
        public string FirmOrderReference { get; set; }
        public string FirmOrderCode { get; set; }
        public string SemifinishedProductReferences { get; set; }
        public string FirmOrderSpecs { get; set; }
        public string GoodsReceiptReference { get; set; }
        public Nullable<System.DateTime> GoodsReceiptEntryDate { get; set; }
        public System.DateTime BatchEntryDate { get; set; }
        public Nullable<int> GoodsArrivalID { get; set; }
        public Nullable<int> GoodsArrivalDetailID { get; set; }
        public string GoodsArrivalReference { get; set; }
        public string GoodsArrivalCode { get; set; }
        public Nullable<System.DateTime> GoodsArrivalEntryDate { get; set; }
        public string Barcode { get; set; }
        public string BatchCode { get; set; }
        public string SealCode { get; set; }
        public string LabCode { get; set; }
        public int BinLocationID { get; set; }
        public string BinLocationCode { get; set; }
        public Nullable<int> GoodsArrivalPackageID { get; set; }
        public int LabID { get; set; }
        public Nullable<System.DateTime> ProductionDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public int BatchID { get; set; }
        public Nullable<int> FinishedItemPackageID { get; set; }
        public Nullable<int> FinishedItemID { get; set; }
        public Nullable<System.DateTime> FinishedItemEntryDate { get; set; }
        public string SemifinishedItemReferences { get; set; }
        public string PurchaseOrderCodes { get; set; }
        public string CustomerCode { get; set; }
        public decimal UnitWeight { get; set; }
        public decimal TareWeight { get; set; }
        public Nullable<int> RecyclatePackageID { get; set; }
        public Nullable<int> RecyclateID { get; set; }
        public Nullable<System.DateTime> RecyclateEntryDate { get; set; }
    }
}
