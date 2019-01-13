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

        [Required(ErrorMessage = "Nhập TV")]
        [Display(Name = "Trục")]
        public string LayerCode { get; set; }

        [Display(Name = "Mã NVL")]
        [UIHint("AutoCompletes/CommodityBase")]
        public override string CommodityCode { get; set; }
        [Display(Name = "Tên NVL")]
        public override string CommodityName { get; set; }
        

        [Display(Name = "KL")]
        [UIHint("Quantity")]
        [Range(0, 99999999999, ErrorMessage = "Số lượng không hợp lệ")]
        public override decimal Quantity { get; set; }

        public decimal BlockQuantity { get { return this.Quantity; } set { } }

        [Display(Name = "%")]
        [UIHint("DecimalN0")]
        [Range(1, 100, ErrorMessage = "Vui lòng nhập %")]
        public decimal BlockUnit { get; set; }
    }
}
