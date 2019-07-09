using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalDTO.Sales;
using TotalDTO.Helpers;

namespace TotalDTO.Inventories
{
    public class GoodsIssueDetailDTO : SaleDetailVa1DTO, IPrimitiveEntity
    {
        public int GetID() { return this.GoodsIssueDetailID; }

        public int GoodsIssueDetailID { get; set; }
        public int GoodsIssueID { get; set; }

        public int StorekeeperID { get; set; }

        [Display(Name = "Phiếu ĐN")]
        [UIHint("StringReadonly")]
        public string DeliveryAdviceReference { get; set; }
        [Display(Name = "Đơn hàng")]
        public string DeliveryAdviceCode { get; set; }

        [UIHint("StringReadonly")]
        public override string CommodityCode { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.Quantity > this.QuantityRemains || this.FreeQuantity > this.FreeQuantityRemains) yield return new ValidationResult("Số lượng xuất không được lớn hơn số lượng còn lại [" + this.CommodityName + "]", new[] { "Quantity" });
            if ((this.Quantity != this.QuantityRemains || this.FreeQuantity != this.FreeQuantityRemains) && this.VoidTypeID == null) yield return new ValidationResult("Vui lòng chọn lý do không xuất kho [" + this.CommodityName + "]", new[] { "VoidTypeName" });
        }

    }





    public class GoodsIssuePackageDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.GoodsIssuePackageID; }

        public int GoodsIssuePackageID { get; set; }
        public int GoodsIssueID { get; set; }

        public int DeliveryAdviceID { get; set; }
        public int DeliveryAdviceDetailID { get; set; }

        public int CustomerID { get; set; }
        public int ReceiverID { get; set; }

        public Nullable<int> WarehouseID { get; set; }

        public int GoodsReceiptID { get; set; }
        public int GoodsReceiptDetailID { get; set; }

        [Display(Name = "Lô SX")]
        [UIHint("StringReadonly")]
        public string GoodsReceiptReference { get; set; }
        [Display(Name = "Mã NK")]
        [UIHint("StringReadonly")]
        public string GoodsReceiptCode { get; set; }
        [Display(Name = "Ngày NK")]
        [UIHint("DateTimeReadonly")]
        public Nullable<System.DateTime> GoodsReceiptEntryDate { get; set; }
        [Display(Name = "HSD")]
        [UIHint("DateTimeReadonly")]
        public Nullable<System.DateTime> ExpiryDate { get; set; }

        public int BatchID { get; set; }
        [Display(Name = "Ngày lô hàng")]
        [UIHint("DateTimeReadonly")]
        public System.DateTime BatchEntryDate { get; set; }


        [Display(Name = "Mã NVL")]
        [UIHint("StringReadonly")]
        public override string CommodityCode { get; set; }

        [Display(Name = "Tên NVL")]
        [UIHint("StringReadonly")]
        public override string CommodityName { get; set; }

        [Display(Name = "Mã vạch")]
        [UIHint("StringReadonly")]
        public string Barcode { get; set; }
        [Display(Name = "Số cont")]
        [UIHint("StringReadonly")]
        public string SealCode { get; set; }
        [Display(Name = "Số lô")]
        [UIHint("StringReadonly")]
        public string BatchCode { get; set; }
        [Display(Name = "Mã lab")]
        [UIHint("StringReadonly")]
        public string LabCode { get; set; }

        public int BinLocationID { get; set; }
        [Display(Name = "Vị trí")]
        [UIHint("StringReadonly")]
        public string BinLocationCode { get; set; }



        [Display(Name = "Tồn D.A.")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityRemains { get; set; }

        [Display(Name = "Tồn kho")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityAvailables { get; set; }

        [Display(Name = "KL X/K")]
        [UIHint("Quantity")]
        public override decimal Quantity { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.Quantity > this.QuantityAvailables) yield return new ValidationResult("Số lượng xuất không được lớn hơn số lượng tồn kho [" + this.CommodityName + "]", new[] { "Quantity" });
        }
    }
}