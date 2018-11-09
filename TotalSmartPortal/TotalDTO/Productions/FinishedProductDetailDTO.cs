using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalDTO.Helpers;

namespace TotalDTO.Productions
{
    public class FinishedProductDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.FinishedProductDetailID; }

        public int FinishedProductDetailID { get; set; }
        public int FinishedProductID { get; set; }

        public int FirmOrderID { get; set; }
        public int FirmOrderDetailID { get; set; }

        public int PlannedOrderID { get; set; }
        public int PlannedOrderDetailID { get; set; }

        public int SemifinishedProductID { get; set; }
        public int SemifinishedProductDetailID { get; set; }

        public Nullable<int> CustomerID { get; set; }
        public int CrucialWorkerID { get; set; }

        public int SemifinishedHandoverID { get; set; }

        [Display(Name = "Ca SX")]
        [UIHint("StringReadonly")]
        public string WorkshiftCode { get; set; }
        [Display(Name = "Ngày SX")]
        [UIHint("DateTimeReadonly")]
        public DateTime WorkshiftEntryDate { get; set; }
        [Display(Name = "Pallet")]
        [UIHint("StringReadonly")]
        public string SemifinishedProductReference { get; set; }

        [Display(Name = "Lô SX")]
        [UIHint("StringReadonly")]
        public string GoodsReceiptReference { get; set; }
        [Display(Name = "Mã NK")]
        [UIHint("StringReadonly")]
        public string GoodsReceiptCode { get; set; }
        [Display(Name = "Ngày NK")]
        [UIHint("DateTimeReadonly")]
        public Nullable<System.DateTime> GoodsReceiptEntryDate { get; set; }

        [Display(Name = "Lô màng")]
        [UIHint("DateTimeReadonly")]
        public System.DateTime BatchEntryDate { get; set; }

        [UIHint("QuantityReadonly")]
        public decimal FoilUnitCounts { get; set; }
        [UIHint("QuantityReadonly")]
        public decimal FoilUnitWeights { get; set; }


        [Display(Name = "Tồn phôi")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityRemains { get; set; }

        [Display(Name = "Trừ phôi")]
        [UIHint("QuantityReadonly")]
        public override decimal Quantity { get; set; }

        [Display(Name = "P-Phẩm")]
        [UIHint("Quantity")]
        public decimal QuantityFailure { get; set; }

        [Display(Name = "Thừa")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityExcess { get; set; }

        [Display(Name = "Thiếu")]
        [UIHint("Quantity")]
        public decimal QuantityShortage { get; set; }

        [Display(Name = "T-Phẩm")]
        [UIHint("Quantity")]
        public decimal QuantityAndExcess { get { return this.Quantity + this.QuantityExcess; } set { } }

        [Display(Name = "Biên kg")]
        [UIHint("Quantity")]
        public decimal Swarfs { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.Quantity + this.QuantityFailure + this.QuantityShortage > this.QuantityRemains) yield return new ValidationResult("Số lượng đóng gói không được lớn hơn số lượng tồn phôi [" + this.CommodityName + " Pallet: " + this.SemifinishedProductReference + "]", new[] { "Quantity" });
        }
    }


    public class FinishedProductSummaryDTO
    {
        public int CommodityID { get; set; }
        [Display(Name = "Mã hàng")]
        [UIHint("StringReadonly")]
        public string CommodityCode { get; set; }
        [Display(Name = "Tên hàng")]
        [UIHint("StringReadonly")]
        public string CommodityName { get; set; }

        [Display(Name = "Cái/ kiện")]
        [UIHint("Integer")]
        public int PiecePerPack { get; set; }

        [UIHint("QuantityReadonly")]
        public decimal FoilUnitCounts { get; set; }
        [UIHint("QuantityReadonly")]
        public decimal FoilUnitWeights { get; set; }
        [Display(Name = "Kg/ tấm")]
        public string FoilUnitWeightsPerCounts { get { return this.FoilUnitWeights.ToString("N2") + "/" + this.FoilUnitCounts.ToString("N0"); } }


        [Display(Name = "Tồn phôi")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityRemains { get; set; }

        [Display(Name = "SLTP")]
        [UIHint("QuantityReadonly")]
        public decimal Quantity { get; set; }
        [Display(Name = "Số kiện")]
        [UIHint("QuantityReadonly")]
        public decimal Packages { get { return this.PiecePerPack > 0 ? Math.Truncate(this.Quantity / this.PiecePerPack) : 0; } set { } } 
        [Display(Name = "Số cái lẻ")]
        [UIHint("QuantityReadonly")]
        public decimal OddPackages { get { return this.PiecePerPack > 0 ? this.Quantity % this.PiecePerPack : 0; } set { } } 

        [Display(Name = "TP (kg)")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityWeights { get { return this.FoilUnitCounts > 0 ? this.Quantity * this.FoilUnitWeights / this.FoilUnitCounts : 0; } set { } }

        [Display(Name = "P-Phẩm")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityFailure { get; set; }
        [Display(Name = "PP (kg)")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityFailureWeights { get { return this.FoilUnitCounts > 0 ? this.QuantityFailure * this.FoilUnitWeights / this.FoilUnitCounts : 0; } set { } }

        [Display(Name = "Biên (kg)")]
        [UIHint("QuantityReadonly")]
        public decimal Swarfs { get; set; }
    }

}
