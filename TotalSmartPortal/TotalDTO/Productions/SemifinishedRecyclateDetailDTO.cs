using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalModel.Helpers;
using TotalBase.Enums;
using TotalDTO.Helpers;

namespace TotalDTO.Productions
{
    public class SemifinishedRecyclateDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.SemifinishedRecyclateDetailID; }

        public int SemifinishedRecyclateDetailID { get; set; }
        public int SemifinishedRecyclateID { get; set; }

        public int ShiftID { get; set; }
        public int WorkshiftID { get; set; }
                
        public int CrucialWorkerID { get; set; }
        public int WarehouseID { get; set; }


        public int SemifinishedProductID { get; set; }

        public string SemifinishedProductReference { get; set; }

        public DateTime SemifinishedProductEntryDate { get; set; }
        
        public string ShiftCode { get; set; }
        public string ProductionLineCode { get; set; }
        public string FirmOrderCode { get; set; }
        public string Specification { get; set; }

        [Display(Name = "SỐ kg tấm hư")]
        [UIHint("QuantityReadonly")]        
        public decimal RejectWeights { get; set; }
        [Display(Name = "Số kg phế phẩm")]
        [UIHint("QuantityReadonly")]        
        public decimal FailureWeights { get; set; }

        [Display(Name = "Tổng tồn phế phẩm")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityRemains { get; set; }

        [Display(Name = "KL cân")]
        [UIHint("Quantity")]
        public override decimal Quantity { get; set; }

        
        [Display(Name = "Mã NVL")]
        [UIHint("StringReadonly")]
        public override string CommodityCode { get; set; }

        [Display(Name = "Tên NVL")]
        [UIHint("StringReadonly")]
        public override string CommodityName { get; set; }


        public Nullable<int> RecycleCommodityID { get; set; }
        [Display(Name = "Mã phế phẩm")]
        [UIHint("StringReadonly")]
        public string RecycleCommodityCode { get; set; }
        [Display(Name = "Tên phế phẩm")]
        [UIHint("StringReadonly")]
        public string RecycleCommodityName { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.Quantity > this.QuantityRemains) yield return new ValidationResult("Số lượng xuất không được lớn hơn số lượng yêu cầu [" + this.CommodityName + "]", new[] { "Quantity" });
        }
    }



    public class SemifinishedRecyclatePackageDTO
    {
        public int CommodityID { get; set; }
        [Display(Name = "Mã phế phẩm")]
        [UIHint("StringReadonly")]
        public string CommodityCode { get; set; }
        [Display(Name = "Tên phế phẩm")]
        [UIHint("StringReadonly")]
        public string CommodityName { get; set; }


        [Display(Name = "SỐ kg tấm hư")]
        [UIHint("QuantityReadonly")]
        public decimal RejectWeights { get; set; }
        [Display(Name = "Số kg phế phẩm")]
        [UIHint("QuantityReadonly")]
        public decimal FailureWeights { get; set; }

        [Display(Name = "Tổng tồn phế phẩm")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityRemains { get; set; }

        [Display(Name = "KL cân")]
        [UIHint("Quantity")]
        public decimal Quantity { get; set; }
    }
}