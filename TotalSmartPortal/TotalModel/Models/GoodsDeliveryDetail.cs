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
    using System.Collections.Generic;
    
    public partial class GoodsDeliveryDetail
    {
        public int GoodsDeliveryDetailID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public int LocationID { get; set; }
        public int GoodsDeliveryID { get; set; }
        public int CustomerID { get; set; }
        public int ReceiverID { get; set; }
        public int HandlingUnitID { get; set; }
        public decimal Quantity { get; set; }
        public decimal Weight { get; set; }
        public decimal RealWeight { get; set; }
        public string Remarks { get; set; }
    
        public virtual GoodsDelivery GoodsDelivery { get; set; }
        public virtual HandlingUnit HandlingUnit { get; set; }
    }
}
