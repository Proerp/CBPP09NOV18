using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalDTO.Helpers;
using TotalBase.Enums;

namespace TotalDTO.Productions
{
    public class PlannedOrderDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.PlannedOrderDetailID; }

        public int PlannedOrderDetailID { get; set; }
        public int PlannedOrderID { get; set; }

        public int CustomerID { get; set; }

        public int MoldID { get; set; }
        [Display(Name = "Khuôn")]
        [Required(ErrorMessage = "Vui lòng chọn mã khuôn")]
        [UIHint("AutoCompletes/MoldBase")]
        public string MoldCode { get; set; }
        [Display(Name = "P/M")]
        [UIHint("DecimalN0")]
        public decimal MoldQuantity { get; set; }

        public int BomID { get; set; }
        [Display(Name = "NVL")]
        [Required(ErrorMessage = "Vui lòng chọn nguyên liệu")]
        [UIHint("AutoCompletes/BomBase")]
        public string BomCode { get; set; }
        [UIHint("DecimalN0")]
        public decimal BlockUnit { get; set; }
        [UIHint("Quantity")]
        public decimal BlockQuantity { get; set; }

        [UIHint("AutoCompletes/CommodityBase")]
        public override string CommodityCode { get; set; }
        [UIHint("Integer")]
        [Range(1, 3000, ErrorMessage = "Vui lòng kiểm tra số cái/ kiện [P/P]")]
        public override int PiecePerPack { get; set; }

        [Display(Name = "Ngày giao hàng")]
        public Nullable<System.DateTime> DeliveryDate { get; set; }

        public string VoidTypeCode { get; set; }
        [Display(Name = "Lý do")]
        [UIHint("AutoCompletes/VoidTypeBase")]
        public string VoidTypeName { get; set; }
        public Nullable<int> VoidClassID { get; set; }

        [Display(Name = "SLYC")]
        [UIHint("Quantity")]
        public decimal QuantityRequested { get; set; }
        [Display(Name = "TK")]
        [UIHint("Quantity")]
        public decimal QuantityOnhand { get; set; }

        [Display(Name = "SLSX")]
        [UIHint("Quantity")]
        public override decimal Quantity { get; set; }

        [Display(Name = "#")]
        [UIHint("Integer")]
        public int? CombineIndex { get; set; }

        public string Specs { get; set; }
        public string Description { get; set; }

        public string GetSpecs() { return this.CommodityName + (this.CombineIndex != null ? " [" + this.Quantity.ToString("N" + GlobalEnums.rndQuantity.ToString()) + "] " : ""); }
        public string GetDescription() { return this.CommodityCode + (this.CombineIndex != null ? " [" + this.Quantity.ToString("N" + GlobalEnums.rndQuantity.ToString()) + "] " : ""); }        
    }
}