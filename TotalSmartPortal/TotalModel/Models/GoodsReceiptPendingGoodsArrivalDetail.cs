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
    
    public partial class GoodsReceiptPendingGoodsArrivalDetail
    {
        public int GoodsArrivalID { get; set; }
        public int GoodsArrivalDetailID { get; set; }
        public string GoodsArrivalReference { get; set; }
        public string GoodsArrivalCode { get; set; }
        public System.DateTime GoodsArrivalEntryDate { get; set; }
        public System.DateTime BatchEntryDate { get; set; }
        public int CommodityID { get; set; }
        public string CommodityCode { get; set; }
        public string CommodityName { get; set; }
        public int CommodityTypeID { get; set; }
        public string Barcode { get; set; }
        public Nullable<decimal> QuantityRemains { get; set; }
        public decimal Quantity { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsSelected { get; set; }
    }
}
