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

namespace TotalDTO.Productions
{
    public class PlannedOrderPrimitiveDTO : QuantityDTO<PlannedOrderDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.PlannedOrder; } }

        public int GetID() { return this.PlannedOrderID; }
        public void SetID(int id) { this.PlannedOrderID = id; }

        public int PlannedOrderID { get; set; }

        [Display(Name = "Số chứng từ")]
        public string Code { get; set; }

        [Display(Name = "Ngày chứng từ")]
        public Nullable<System.DateTime> VoucherDate { get; set; }

        [Display(Name = "Ngày giao hàng")]
        [Required(ErrorMessage = "Vui lòng nhập ngày giao hàng")]
        public Nullable<System.DateTime> DeliveryDate { get; set; }

        [Display(Name = "Mục đích")]
        public string Purposes { get; set; }

        public virtual int CustomerID { get; set; }

        public virtual bool CheckBomID { get { bool checkBomID = false; this.DtoDetails().ToList().ForEach(e => { if (e.CombineIndex != null && checkBomID == false && this.DtoDetails().Where(w => w.CombineIndex == e.CombineIndex && w.BomID != e.BomID).Count() > 0) checkBomID = true; }); return checkBomID; } }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }
            if (this.CheckBomID) yield return new ValidationResult("Lỗi nguyên liệu [BOM]. Ghép khuôn phải sử dụng chung nguyên liệu.", new[] { "BOM" });
        }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            string caption = "";
            this.DtoDetails().ToList().ForEach(e => { e.CustomerID = this.CustomerID; e.DeliveryDate = this.DeliveryDate; e.Description = e.CombineIndex == null ? e.GetDescription() : string.Join(", ", this.DtoDetails().Where(w => w.CombineIndex == e.CombineIndex).Select(o => o.GetDescription())); e.Specs = e.CombineIndex == null ? e.GetSpecs() : string.Join(", ", this.DtoDetails().Where(w => w.CombineIndex == e.CombineIndex).Select(o => o.GetSpecs())); if (caption.IndexOf(e.CommodityName) < 0) caption = caption + (caption != "" ? ", " : "") + e.CommodityName; });
            this.Caption = caption != "" ? (caption.Length > 98 ? caption.Substring(0, 95) + "..." : caption) : null;
        }
    }

    public class PlannedOrderDTO : PlannedOrderPrimitiveDTO, IBaseDetailEntity<PlannedOrderDetailDTO>
    {
        public PlannedOrderDTO()
        {
            this.PlannedOrderViewDetails = new List<PlannedOrderDetailDTO>();
        }

        public override int CustomerID { get { return (this.Customer != null ? this.Customer.CustomerID : 0); } }
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override Nullable<int> VoidTypeID { get { return (this.VoidType != null ? this.VoidType.VoidTypeID : null); } }
        [UIHint("AutoCompletes/VoidType")]
        public VoidTypeBaseDTO VoidType { get; set; }

        public List<PlannedOrderDetailDTO> PlannedOrderViewDetails { get; set; }
        public List<PlannedOrderDetailDTO> ViewDetails { get { return this.PlannedOrderViewDetails; } set { this.PlannedOrderViewDetails = value; } }

        public ICollection<PlannedOrderDetailDTO> GetDetails() { return this.PlannedOrderViewDetails; }

        protected override IEnumerable<PlannedOrderDetailDTO> DtoDetails() { return this.PlannedOrderViewDetails; }

        public override void PrepareVoidDetail(int? detailID)
        {
            this.ViewDetails.RemoveAll(w => w.PlannedOrderDetailID != detailID);
            if (this.ViewDetails.Count() > 0)
                this.VoidType = new VoidTypeBaseDTO() { VoidTypeID = this.ViewDetails[0].VoidTypeID, Code = this.ViewDetails[0].VoidTypeCode, Name = this.ViewDetails[0].VoidTypeName, VoidClassID = this.ViewDetails[0].VoidClassID };
            base.PrepareVoidDetail(detailID);
        }
    }

}
