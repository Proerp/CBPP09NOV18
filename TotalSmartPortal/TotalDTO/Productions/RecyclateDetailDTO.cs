using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalModel.Helpers;
using TotalBase.Enums;
using TotalDTO.Helpers;

namespace TotalDTO.Productions
{
    public class RecyclateDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.RecyclateDetailID; }

        public int RecyclateDetailID { get; set; }
        public int RecyclateID { get; set; }

        public int WorkshiftID { get; set; }
        public int WarehouseID { get; set; }
        public int CrucialWorkerID { get; set; }
        public int StorekeeperID { get; set; }


        public Nullable<int> SemifinishedProductID { get; set; }
        public Nullable<int> FinishedProductPackageID { get; set; }
        public Nullable<int> FinishedItemDetailID { get; set; }

        [Display(Name = "Phôi")]
        [UIHint("StringReadonly")]
        public string RootReference { get; set; }
        [Display(Name = "Giờ SX")]
        [UIHint("TimeReadonly")]
        public DateTime RootEntryDate { get; set; }

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
        public decimal QuantityFailures { get; set; }
        [Display(Name = "Biên, pp")]
        [UIHint("QuantityReadonly")]        
        public decimal QuantitySwarfs { get; set; }

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


        [Required(ErrorMessage = "Vui lòng kiểm tra mã phế phẩm")]
        public Nullable<int> RecycleCommodityID { get; set; }
        public virtual int RecycleCommodityTypeID { get; set; }
        [Display(Name = "Mã phế phẩm")]
        [UIHint("StringReadonly")]
        public string RecycleCommodityCode { get; set; }
        [Display(Name = "Tên phế phẩm")]
        [UIHint("StringReadonly")]
        public string RecycleCommodityName { get; set; }
    }



    public class RecyclatePackageDTO : BaseModel
    {
        public int RecyclatePackageID { get; set; }
        public int RecyclateID { get; set; }

        public int WorkshiftID { get; set; }
        public int WarehouseID { get; set; }

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
        public decimal QuantityFailures { get; set; }
        [Display(Name = "Số kg biên, pp")]
        [UIHint("QuantityReadonly")]
        public decimal QuantitySwarfs { get; set; }

        [Display(Name = "TC tồn pp")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityRemains { get; set; }

        [Display(Name = "Kg thực giao")]
        [UIHint("Quantity")]
        public decimal Quantity { get; set; }

        public int BatchID { get; set; }
        [Display(Name = "Ngày lô hàng")]
        [UIHint("DateTimeReadonly")]
        public DateTime BatchEntryDate { get; set; }
    }
}