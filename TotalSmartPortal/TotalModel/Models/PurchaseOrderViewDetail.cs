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
    
    public partial class PurchaseOrderViewDetail
    {
        public int PurchaseOrderDetailID { get; set; }
        public int PurchaseOrderID { get; set; }
        public int CommodityID { get; set; }
        public string CommodityCode { get; set; }
        public string CommodityName { get; set; }
        public int CommodityTypeID { get; set; }
        public Nullable<int> VoidTypeID { get; set; }
        public string VoidTypeCode { get; set; }
        public string VoidTypeName { get; set; }
        public Nullable<int> VoidClassID { get; set; }
        public decimal Quantity { get; set; }
        public bool InActivePartial { get; set; }
        public Nullable<System.DateTime> InActivePartialDate { get; set; }
        public string Remarks { get; set; }
    }
}
