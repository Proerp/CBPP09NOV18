using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalModel.Helpers;
using TotalBase.Enums;
using TotalDTO.Helpers;

namespace TotalDTO.Inventories
{
    public class PackageIssueDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.PackageIssueDetailID; }

        public int PackageIssueDetailID { get; set; }
        public int PackageIssueID { get; set; }

        public int BlendingInstructionID { get; set; }
        public int BlendingInstructionDetailID { get; set; }

        public int ShiftID { get; set; }
        public int WorkshiftID { get; set; }

        public int ProductionLineID { get; set; }
        public int CrucialWorkerID { get; set; }

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


        [Display(Name = "KL Y/C")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityBIS { get; set; }

        [Display(Name = "Tồn BIS")]
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

            if (this.Quantity > (this.QuantityRemains * (decimal)1.005)) yield return new ValidationResult("Số lượng xuất không được lớn hơn số lượng yêu cầu [" + this.CommodityName + "]", new[] { "Quantity" });
            if (this.Quantity > this.QuantityAvailables) yield return new ValidationResult("Số lượng xuất không được lớn hơn số lượng tồn kho [" + this.CommodityName + "]", new[] { "Quantity" });
        }
    }
}
