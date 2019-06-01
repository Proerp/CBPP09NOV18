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

        public int WorkshiftID { get; set; }

        public int WarehouseID { get; set; }
        public int CrucialWorkerID { get; set; }
        public int StorekeeperID { get; set; }


        public int SemifinishedProductID { get; set; }
        [Display(Name = "Phôi")]
        [UIHint("StringReadonly")]
        public string SemifinishedProductReference { get; set; }
        [Display(Name = "Giờ SX")]
        [UIHint("TimeReadonly")]
        public DateTime SemifinishedProductEntryDate { get; set; }

        [Display(Name = "Máy")]
        [UIHint("StringReadonly")]
        public string ProductionLineCode { get; set; }
        [Display(Name = "Số ĐH")]
        [UIHint("StringReadonly")]
        public string FirmOrderCode { get; set; }
        [Display(Name = "Mặt hàng")]
        [UIHint("StringReadonly")]
        public string Specification { get; set; }

        [Display(Name = "Tấm hư")]
        [UIHint("QuantityReadonly")]        
        public decimal RejectWeights { get; set; }
        [Display(Name = "Biên, pp")]
        [UIHint("QuantityReadonly")]        
        public decimal FailureWeights { get; set; }

        [Display(Name = "TC pp")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityRemains { get; set; }

        [Display(Name = "KL thực giao")]
        [UIHint("Quantity")]
        public override decimal Quantity { get; set; }

        
        [Display(Name = "Mã NVL")]
        [UIHint("StringReadonly")]
        public override string CommodityCode { get; set; }
        [Display(Name = "Tên NVL")]
        [UIHint("StringReadonly")]
        public override string CommodityName { get; set; }


        public Nullable<int> RecycleCommodityID { get; set; }
        public virtual int RecycleCommodityTypeID { get; set; }
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
        [Range(1, 99999999999, ErrorMessage = "Lỗi bắt buộc phải có id loại hàng hóa")]
        [Required(ErrorMessage = "Lỗi bắt buộc phải có loại hàng hóa")]
        public virtual int CommodityTypeID { get; set; }

        [Display(Name = "Số kg tấm hư")]
        [UIHint("QuantityReadonly")]
        public decimal RejectWeights { get; set; }
        [Display(Name = "Số kg biên, pp")]
        [UIHint("QuantityReadonly")]
        public decimal FailureWeights { get; set; }

        [Display(Name = "Tổng tồn phế phẩm")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityRemains { get; set; }

        [Display(Name = "KL thực giao")]
        [UIHint("Quantity")]
        public decimal Quantity { get; set; }
    }
}