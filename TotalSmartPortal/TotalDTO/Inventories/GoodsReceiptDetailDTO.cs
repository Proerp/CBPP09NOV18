using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalModel.Helpers;
using TotalBase.Enums;
using TotalDTO.Helpers;

namespace TotalDTO.Inventories
{
    public class GoodsReceiptDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.GoodsReceiptDetailID; }

        public int GoodsReceiptDetailID { get; set; }
        public int GoodsReceiptID { get; set; }

        public GlobalEnums.NmvnTaskID NMVNTaskID { get; set; }
        public Nullable<int> GoodsReceiptTypeID { get; set; }

        public Nullable<int> PurchaseRequisitionID { get; set; }
        public Nullable<int> PurchaseRequisitionDetailID { get; set; }

        [Display(Name = "Phiếu ĐH")]
        [UIHint("StringReadonly")]
        public string PurchaseRequisitionReference { get; set; }
        [Display(Name = "Số ĐH")]
        [UIHint("StringReadonly")]
        public string PurchaseRequisitionCode { get; set; }
        [Display(Name = "Ngày ĐH")]
        [UIHint("DateTimeReadonly")]
        public Nullable<System.DateTime> PurchaseRequisitionEntryDate { get; set; }



        public Nullable<int> WarehouseTransferID { get; set; }
        public Nullable<int> WarehouseTransferDetailID { get; set; }

        [Display(Name = "VCNB")]
        [UIHint("StringReadonly")]
        public string WarehouseTransferReference { get; set; }
        [Display(Name = "Ngày VCNB")]
        [UIHint("DateTimeReadonly")]
        public Nullable<System.DateTime> WarehouseTransferEntryDate { get; set; }
        [Display(Name = "Lô hàng")]
        [UIHint("StringReadonly")]
        public string GoodsReceiptReference { get; set; }
        [Display(Name = "Ngày NK")]
        [UIHint("DateTimeReadonly")]
        public Nullable<System.DateTime> GoodsReceiptEntryDate { get; set; }
        [Display(Name = "Ngày lô hàng")]
        [UIHint("DateTimeReadonly")]
        public Nullable<System.DateTime> BatchEntryDate { get; set; }


        public Nullable<int> FinishedProductID { get; set; }
        public Nullable<int> FinishedProductPackageID { get; set; }

        [Display(Name = "KHSX")]
        [UIHint("StringReadonly")]
        public string FirmOrderReference { get; set; }
        [Display(Name = "Số CT")]
        [UIHint("StringReadonly")]
        public string FirmOrderCode { get; set; }
        [Display(Name = "TP")]
        [UIHint("StringReadonly")]
        public string FirmOrderSpecs { get; set; }
        [Display(Name = "Pallet")]
        [UIHint("DateTimeReadonly")]
        public string SemifinishedProductReferences { get; set; }
        [Display(Name = "Ngày ĐG")]
        [UIHint("DateTimeReadonly")]
        public Nullable<System.DateTime> FinishedProductEntryDate { get; set; }


        public Nullable<int> MaterialIssueID { get; set; }
        public Nullable<int> MaterialIssueDetailID { get; set; }

        [Display(Name = "Ngày XK")]
        [UIHint("DateTimeReadonly")]
        public Nullable<System.DateTime> MaterialIssueEntryDate { get; set; }

        [Display(Name = "Ca SX")]
        [UIHint("StringReadonly")]
        public string WorkshiftName { get; set; }
        [Display(Name = "Máy")]
        [UIHint("StringReadonly")]
        public string ProductionLinesCode { get; set; }


        public Nullable<int> WarehouseAdjustmentID { get; set; }
        public Nullable<int> WarehouseAdjustmentDetailID { get; set; }

        [Display(Name = "Phiếu ĐH")]
        [UIHint("StringReadonly")]
        public string WarehouseAdjustmentReference { get; set; }
        [Display(Name = "Số ĐH")]
        [UIHint("StringReadonly")]
        public string WarehouseAdjustmentCode { get; set; }
        [Display(Name = "Ngày ĐH")]
        [UIHint("DateTimeReadonly")]
        public Nullable<System.DateTime> WarehouseAdjustmentEntryDate { get; set; }
        public Nullable<int> WarehouseAdjustmentTypeID { get; set; }


        public int BatchID { get; set; }


        public string Code { get; set; }

        public Nullable<int> CustomerID { get; set; }
        public Nullable<int> WarehouseID { get; set; }
        public Nullable<int> WarehouseIssueID { get; set; }

        [Display(Name = "Kho")]
        [UIHint("StringReadonly")]
        public string WarehouseCode { get; set; }


        [UIHint("AutoCompletes/CommodityAvailable")]
        public override string CommodityCode { get; set; }

        [Display(Name = "SL Tồn")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityRemains { get; set; }

        [UIHint("Quantity")]
        public override decimal Quantity { get; set; }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.MaterialIssueDetailID == 0 && this.Quantity > this.QuantityRemains) yield return new ValidationResult("Số lượng nhập kho không được lớn hơn số lượng còn lại [" + this.CommodityName + "]", new[] { "Quantity" });
        }
    }
}