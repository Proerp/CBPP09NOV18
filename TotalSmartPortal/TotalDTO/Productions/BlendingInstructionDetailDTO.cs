using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalDTO.Helpers;
using TotalBase.Enums;

namespace TotalDTO.Productions
{
    public class BlendingInstructionDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.BlendingInstructionDetailID; }

        public int BlendingInstructionDetailID { get; set; }
        public int BlendingInstructionID { get; set; }

        public string Code { get; set; }

        [Display(Name = "Mã NVL")]
        [Required(ErrorMessage = "Vui lòng chọn NVL")]        
        [UIHint("AutoCompletes/CommodityBase")]
        public override string CommodityCode { get; set; }

        [Display(Name = "Tên NVL")]
        [UIHint("StringReadonly")]
        public override string CommodityName { get; set; }

        [Display(Name = "SL Y/C")]
        [UIHint("Quantity")]
        public override decimal Quantity { get; set; }

        [Display(Name = "Đã xuất")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityIssued { get; set; }

        [Display(Name = "Còn lại")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityRemains { get { return this.Quantity - QuantityIssued; } set { } }

        public string VoidTypeCode { get; set; }
        [Display(Name = "Lý do")]
        [UIHint("AutoCompletes/VoidTypeBase")]
        public string VoidTypeName { get; set; }
        public Nullable<int> VoidClassID { get; set; }
    }
}