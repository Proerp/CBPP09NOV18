using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;
using TotalDTO.Helpers;
using TotalDTO.Commons;

namespace TotalDTO.Commons
{
    public interface IBomBaseDTO
    {
        Nullable<int> BomID { get; set; }
        [Display(Name = "Màng")]
        [UIHint("AutoCompletes/BomBase")]
        [Required(ErrorMessage = "Vui lòng nhập màng")]
        string Code { get; set; }
        string Name { get; set; }
    }

    public class BomBaseDTO : BaseDTO, IBomBaseDTO
    {
        public Nullable<int> BomID { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập MCT")]
        public string Code { get; set; }
        public string Name { get; set; }
    }





    public class BomPrimitiveDTO : QuantityDTO<BomDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Bom; } }

        public int GetID() { return this.BomID; }
        public void SetID(int id) { this.BomID = id; }

        public int BomID { get; set; }

        public virtual Nullable<int> CustomerID { get; set; }
        public virtual Nullable<int> CommodityID { get; set; }

        [Display(Name = "Mã công thức")]
        [Required(ErrorMessage = "Vui lòng nhập MCT")]
        public string Code { get; set; }
        public string OfficialCode { get { return this.Code; } }

        [Display(Name = "Tên hỗn hợp")]
        [Required(ErrorMessage = "Vui lòng nhập tên hỗn hợp")]
        public string Name { get; set; }

        [Display(Name = "Ngày hiệu lực")]
        [Required(ErrorMessage = "Vui lòng nhập ngày hiệu lực")]
        public Nullable<System.DateTime> EffectiveDate { get; set; }

        public override string Reference { get { return "####000"; } }
        public override int PreparedPersonID { get { return 1; } }

        public int LayerCount { get { return this.DtoDetails().GroupBy(g => g.LayerCode).Count(); } }

        public bool CheckBlockUnit { get { bool checkBomID = false; this.DtoDetails().ToList().ForEach(e => { if (checkBomID == false && this.DtoDetails().Where(w => w.LayerCode == e.LayerCode && w.BlockUnit != e.BlockUnit).Count() > 0) checkBomID = true; }); return checkBomID; } }
        public bool CheckBlockUnitTotal { get { return this.DtoDetails().GroupBy(g => g.LayerCode).Select(s => new { gBlockUnit = s.First().BlockUnit }).Select(o => o.gBlockUnit).Sum() != 100; } }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }
            if (this.CheckBlockUnit) yield return new ValidationResult("Lỗi tỷ lệ %. Lưu ý: cùng một trục phải cùng tỷ lệ %", new[] { "BOM" });
            if (this.CheckBlockUnitTotal) yield return new ValidationResult("Lỗi tổng tỷ lệ %. Lưu ý: tổng tỷ lệ các trục phải bằng 100", new[] { "BOM" });
        }
    }

    public class BomDTO : BomPrimitiveDTO, IBaseDetailEntity<BomDetailDTO>
    {
        public BomDTO()
        {
            this.BomViewDetails = new List<BomDetailDTO>();
        }

        public override Nullable<int> CustomerID { get { return (this.Customer != null ? (this.Customer.CustomerID > 0 ? (Nullable<int>)this.Customer.CustomerID : null) : null); } }
        [Display(Name = "Khách hàng")]
        [UIHint("AutoCompletes/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override Nullable<int> CommodityID { get { return (this.Commodity != null ? (this.Commodity.CommodityID > 0 ? (Nullable<int>)this.Commodity.CommodityID : null) : null); } }
        [Display(Name = "Mã màng")]
        [UIHint("AutoCompletes/Commodity")]
        public CommodityBaseDTO Commodity { get; set; }

        public List<BomDetailDTO> BomViewDetails { get; set; }
        public List<BomDetailDTO> ViewDetails { get { return this.BomViewDetails; } set { this.BomViewDetails = value; } }

        public ICollection<BomDetailDTO> GetDetails() { return this.BomViewDetails; }

        protected override IEnumerable<BomDetailDTO> DtoDetails() { return this.BomViewDetails; }
    }

}
