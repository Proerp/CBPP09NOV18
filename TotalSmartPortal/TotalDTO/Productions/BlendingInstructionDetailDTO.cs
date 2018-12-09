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

        [UIHint("AutoCompletes/CommodityBase")]
        public override string CommodityCode { get; set; }

        public string VoidTypeCode { get; set; }
        [Display(Name = "Lý do")]
        [UIHint("AutoCompletes/VoidTypeBase")]
        public string VoidTypeName { get; set; }
        public Nullable<int> VoidClassID { get; set; }

        [Display(Name = "SLSX")]
        [UIHint("Quantity")]
        public override decimal Quantity { get; set; }
    }
}