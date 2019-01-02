using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalDTO.Helpers;
using TotalBase.Enums;

namespace TotalDTO.Inventories
{
    public class TransferOrderDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.TransferOrderDetailID; }

        public int TransferOrderDetailID { get; set; }
        public int TransferOrderID { get; set; }

        public int TransferOrderTypeID { get; set; }
        public GlobalEnums.NmvnTaskID NMVNTaskID { get; set; }

        public int WarehouseID { get; set; }
        public Nullable<int> LocationIssuedID { get; set; }
        public Nullable<int> WarehouseReceiptID { get; set; }
        public Nullable<int> LocationReceiptID { get; set; }

        [UIHint("AutoCompletes/CommodityBase")]
        public override string CommodityCode { get; set; }

        [Display(Name = "Tồn kho")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityAvailables { get; set; }

        [Display(Name = "SL Y/C")]
        [UIHint("QuantityWithMinus")]
        public override decimal Quantity { get; set; }

        [Display(Name = "Đã C/K")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityIssued { get; set; }

        [Display(Name = "Còn lại")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityRemains { get { return this.Quantity - QuantityIssued; } set { } }

        public string VoidTypeCode { get; set; }
        [Display(Name = "Lý do")]
        [UIHint("AutoCompletes/VoidTypeBase")]
        public string VoidTypeName { get; set; }
        public Nullable<int> VoidClassID { get; set; }
    }
}