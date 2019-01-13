using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalDTO.Helpers;

namespace TotalDTO.Commons
{
    public class BomDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.BomDetailID; }

        public int BomDetailID { get; set; }
        public int BomID { get; set; }

        public int MaterialID { get { return this.CommodityID; } }

        //[Display(Name = "Mã CK")]
        [UIHint("AutoCompletes/CommodityBase")]
        public override string CommodityCode { get; set; }

        [Display(Name = "KL Đ/H")]
        [UIHint("Quantity")]
        [Range(0, 99999999999, ErrorMessage = "Số lượng không hợp lệ")]
        public override decimal Quantity { get; set; }

        [UIHint("QuantityReadonly")]
        public decimal BlockQuantity { get { return this.Quantity; } set { } }

        [Display(Name = "KL Đ/H")]
        [UIHint("Quantity")]
        [Range(0, 99999999999, ErrorMessage = "Số lượng không hợp lệ")]
        public decimal BlockUnit { get; set; }
    }
}
