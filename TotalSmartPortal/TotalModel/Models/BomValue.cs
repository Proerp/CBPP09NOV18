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
    
    public partial class BomValue
    {
        public int BomID { get; set; }
        public int BomDetailID { get; set; }
        public string LayerCode { get; set; }
        public int CommodityID { get; set; }
        public string CommodityCode { get; set; }
        public string CommodityName { get; set; }
        public int CommodityTypeID { get; set; }
        public decimal BlockUnit { get; set; }
        public decimal BlockQuantity { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public decimal LayerQuantity { get; set; }
        public Nullable<decimal> QuantityAvailables { get; set; }
    }
}