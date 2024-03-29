﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalModel.Helpers;
using TotalBase.Enums;
using TotalDTO.Helpers;

namespace TotalDTO.Inventories
{
    public class MaterialIssueDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.MaterialIssueDetailID; }

        public int MaterialIssueDetailID { get; set; }
        public int MaterialIssueID { get; set; }

        public int ProductionOrderID { get; set; }
        public int ProductionOrderDetailID { get; set; }

        public int PlannedOrderID { get; set; }
        public int FirmOrderID { get; set; }
        public int FirmOrderMaterialID { get; set; }
        public int WorkOrderID { get; set; }
        public int WorkOrderDetailID { get; set; }

        public GlobalEnums.NmvnTaskID NMVNTaskID { get; set; }
        public int MaterialIssueTypeID { get; set; }

        public string Code { get; set; }

        public int BomID { get; set; }
        public int BomDetailID { get; set; }
        [Display(Name = "Trục")]
        [UIHint("StringReadonly")]
        public string LayerCode { get; set; }

        public Nullable<int> CustomerID { get; set; }
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

        public int LabID { get; set; }
        public int BatchID { get; set; }
        [Display(Name = "Ngày lô hàng")]
        [UIHint("DateTimeReadonly")]
        public System.DateTime BatchEntryDate { get; set; }

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

        public Nullable<System.DateTime> ProductionDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }

        [Display(Name = "Tồn LSX")]
        [UIHint("QuantityReadonly")]
        public decimal WorkOrderRemains { get; set; }

        [Display(Name = "Tồn kho")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityAvailables { get; set; }

        [Display(Name = "[MIN QuantityAvailables vs WorkshiftPlannedOrderRemains]Remains")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityRemains { get; set; }

        [UIHint("Quantity")]
        public override decimal Quantity { get; set; }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.ProductionOrderID > 0 && this.Quantity > this.QuantityRemains) yield return new ValidationResult("Số lượng xuất không được lớn hơn số lượng còn lại [" + this.CommodityName + "]", new[] { "Quantity" });
        }
    }
}
