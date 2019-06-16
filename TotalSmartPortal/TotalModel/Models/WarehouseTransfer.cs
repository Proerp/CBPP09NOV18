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
    
    public partial class WarehouseTransfer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WarehouseTransfer()
        {
            this.WarehouseTransferDetails = new HashSet<WarehouseTransferDetail>();
            this.GoodsReceipts = new HashSet<GoodsReceipt>();
        }
    
        public int WarehouseTransferID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string Reference { get; set; }
        public int NMVNTaskID { get; set; }
        public int ShiftID { get; set; }
        public int WorkshiftID { get; set; }
        public bool OneStep { get; set; }
        public bool HasTransferOrder { get; set; }
        public Nullable<int> TransferOrderID { get; set; }
        public int WarehouseID { get; set; }
        public int LocationIssuedID { get; set; }
        public int WarehouseReceiptID { get; set; }
        public int LocationReceiptID { get; set; }
        public int StorekeeperID { get; set; }
        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }
        public int LocationID { get; set; }
        public int ApproverID { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalQuantityReceipted { get; set; }
        public string WarehouseTransferJobs { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
        public bool Approved { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public Nullable<int> BlendingInstructionID { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Location Location { get; set; }
        public virtual Location Location1 { get; set; }
        public virtual Location Location2 { get; set; }
        public virtual Shift Shift { get; set; }
        public virtual TransferOrder TransferOrder { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual Warehouse Warehouse1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WarehouseTransferDetail> WarehouseTransferDetails { get; set; }
        public virtual Workshift Workshift { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodsReceipt> GoodsReceipts { get; set; }
        public virtual User User { get; set; }
    }
}
