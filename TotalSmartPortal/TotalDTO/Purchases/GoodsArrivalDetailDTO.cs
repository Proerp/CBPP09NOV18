using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalDTO.Helpers;

namespace TotalDTO.Purchases
{
    public class GoodsArrivalDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.GoodsArrivalDetailID; }

        public int GoodsArrivalDetailID { get; set; }
        public int GoodsArrivalID { get; set; }

        public int CustomerID { get; set; }
        public int TransporterID { get; set; }
        public int SalespersonID { get; set; }
        public int WarehouseID { get; set; }

        public Nullable<int> PurchaseOrderID { get; set; }
        public Nullable<int> PurchaseOrderDetailID { get; set; }

        [Display(Name = "Phiếu ĐH")]
        [UIHint("StringReadonly")]
        public string PurchaseOrderReference { get; set; }
        [Display(Name = "Số ĐH")]
        [UIHint("StringReadonly")]
        public string PurchaseOrderCode { get; set; }
        [Display(Name = "Ngày ĐH")]
        [UIHint("DateTimeReadonly")]
        public Nullable<System.DateTime> PurchaseOrderEntryDate { get; set; }


        [UIHint("AutoCompletes/CommodityBase")]
        public override string CommodityCode { get; set; }


        [Display(Name = "Tồn đơn")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityRemains { get; set; }

        [UIHint("Quantity")]
        public override decimal Quantity { get; set; }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.PurchaseOrderID > 0 && (this.Quantity > this.QuantityRemains)) yield return new ValidationResult("Số lượng xuất không được lớn hơn số lượng còn lại [" + this.CommodityName + "]", new[] { "Quantity" });
        }
    }
}
