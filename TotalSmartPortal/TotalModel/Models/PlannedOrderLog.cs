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
    
    public partial class PlannedOrderLog
    {
        public int MaterialIssueDetailID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string ProductionLineCode { get; set; }
        public Nullable<System.DateTime> MaterialIssueWorkshiftEntryDate { get; set; }
        public string MaterialIssueWorkshiftCode { get; set; }
        public string GoodsReceiptReference { get; set; }
        public Nullable<System.DateTime> BatchEntryDate { get; set; }
        public Nullable<decimal> MaterialIssueDetailQuantity { get; set; }
        public Nullable<System.DateTime> SemifinishedProductWorkshiftEntryDate { get; set; }
        public string SemifinishedProductWorkshiftCode { get; set; }
        public string CommoditiyCode { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> Weights { get; set; }
    }
}
