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
    
    public partial class BomIndex
    {
        public int BomID { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public string Reference { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CommodityCode { get; set; }
        public string CommodityName { get; set; }
        public string CustomerName { get; set; }
        public System.DateTime EffectiveDate { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public decimal BlockUnit { get; set; }
        public decimal BlockQuantity { get; set; }
    }
}