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
    
    public partial class GoodsArrivalPendingPurchaseOrderDetail
    {
        public int PurchaseOrderID { get; set; }
        public int PurchaseOrderDetailID { get; set; }
        public string PurchaseOrderReference { get; set; }
        public string PurchaseOrderCode { get; set; }
        public System.DateTime PurchaseOrderEntryDate { get; set; }
        public int CommodityID { get; set; }
        public string CommodityCode { get; set; }
        public string CommodityName { get; set; }
        public int CommodityTypeID { get; set; }
        public Nullable<decimal> QuantityRemains { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsSelected { get; set; }
        public Nullable<int> Shelflife { get; set; }
        public Nullable<decimal> UnitWeight { get; set; }
    }
}
