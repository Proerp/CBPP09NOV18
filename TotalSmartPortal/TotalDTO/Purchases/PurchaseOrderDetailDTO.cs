using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalDTO.Helpers;

namespace TotalDTO.Purchases
{
    public class PurchaseOrderDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.PurchaseOrderDetailID; }

        public int PurchaseOrderDetailID { get; set; }
        public int PurchaseOrderID { get; set; }

        public int CustomerID { get; set; }
        public int TransporterID { get; set; }

        //[Display(Name = "Mã CK")]
        [UIHint("AutoCompletes/CommodityBase")]
        public override string CommodityCode { get; set; }

        [Display(Name = "KL Đ/H")]
        [UIHint("Quantity")]
        [Range(0, 99999999999, ErrorMessage = "Số lượng không hợp lệ")]
        public override decimal Quantity { get; set; }

        [Display(Name = "Đã nhận")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityArrived { get; set; }

        [Display(Name = "Còn lại")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityRemains { get { return this.Quantity - QuantityArrived; } set { } }


        [Display(Name = "Lab code")] //[Required(ErrorMessage = "Vui lòng nhập lab code")]
        public virtual string LabCode { get; set; }
        [Display(Name = "Ngày SX")]
        [UIHint("Date")]
        public Nullable<System.DateTime> ProductionDate { get; set; }
        [Display(Name = "HSD")]
        [UIHint("Date")]
        public Nullable<System.DateTime> ExpiryDate { get; set; }


        public string VoidTypeCode { get; set; }
        [Display(Name = "Lý do")]
        [UIHint("AutoCompletes/VoidTypeBase")]
        public string VoidTypeName { get; set; }
        public Nullable<int> VoidClassID { get; set; }
    }
}
