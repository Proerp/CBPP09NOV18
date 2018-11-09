using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalDTO.Helpers;
using TotalBase.Enums;

namespace TotalDTO.Inventories
{
    public class WarehouseTransferDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.WarehouseTransferDetailID; }

        public int WarehouseTransferDetailID { get; set; }
        public int WarehouseTransferID { get; set; }

        public virtual Nullable<int> TransferOrderID { get; set; }
        public virtual Nullable<int> TransferOrderDetailID { get; set; }

        public int WarehouseTransferTypeID { get; set; }
        public GlobalEnums.NmvnTaskID NMVNTaskID { get; set; }

        public int WarehouseID { get; set; }
        public Nullable<int> LocationIssuedID { get; set; }
        public Nullable<int> WarehouseReceiptID { get; set; }
        public Nullable<int> LocationReceiptID { get; set; }

        [UIHint("AutoCompletes/CommodityBase")]
        public override string CommodityCode { get; set; }

        public Nullable<int> CustomerID { get; set; }

        public Nullable<int> GoodsReceiptID { get; set; }
        public Nullable<int> GoodsReceiptDetailID { get; set; }

        [Display(Name = "Lô SX")]
        [UIHint("StringReadonly")]
        public string GoodsReceiptReference { get; set; }
        [Display(Name = "Mã NK")]
        [UIHint("StringReadonly")]
        public string GoodsReceiptCode { get; set; }
        [Display(Name = "Ngày NK")]
        [UIHint("DateTimeReadonly")]
        public Nullable<System.DateTime> GoodsReceiptEntryDate { get; set; }

        public int BatchID { get; set; }
        [Display(Name = "Ngày lô hàng")]
        [UIHint("DateTimeReadonly")]
        public System.DateTime BatchEntryDate { get; set; }

        [Display(Name = "Tồn lệnh")]
        [UIHint("QuantityReadonly")]
        public decimal TransferOrderRemains { get; set; }

        [Display(Name = "Tồn lệnh")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityRemains { get; set; }

        [Display(Name = "Tồn kho")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityAvailables { get; set; }

        [Display(Name = "SL")]
        [UIHint("Quantity")]
        public override decimal Quantity { get; set; }

        public string VoidTypeCode { get; set; }
        [Display(Name = "Lý do")]
        [UIHint("AutoCompletes/VoidTypeBase")]
        public string VoidTypeName { get; set; }
        public Nullable<int> VoidClassID { get; set; }
    }
}