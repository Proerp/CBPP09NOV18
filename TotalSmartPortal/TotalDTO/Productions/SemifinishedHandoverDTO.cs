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
using TotalDTO.Helpers.Interfaces;

namespace TotalDTO.Productions
{
    public class SemifinishedHandoverPrimitiveDTO : BaseWithDetailDTO<SemifinishedHandoverDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.SemifinishedHandover; } }

        public int GetID() { return this.SemifinishedHandoverID; }
        public void SetID(int id) { this.SemifinishedHandoverID = id; }

        public int SemifinishedHandoverID { get; set; }


        public virtual int WorkshiftID { get; set; }
        [Display(Name = "Ca sản xuất")]
        public string WorkshiftCode { get; set; }
        [Display(Name = "Ngày sản xuất")]
        public DateTime WorkshiftEntryDate { get; set; }

        public virtual Nullable<int> CustomerID { get; set; }

        public virtual int FinishedLeaderID { get; set; }
        public virtual int SemifinishedLeaderID { get; set; }


        [Display(Name = "Tổng SL")]
        [Required(ErrorMessage = "Vui lòng nhập chi tiết phiếu")]
        public virtual decimal TotalQuantity { get; set; }
    }

    public class SemifinishedHandoverDTO : SemifinishedHandoverPrimitiveDTO, IBaseDetailEntity<SemifinishedHandoverDetailDTO>
    {
        public SemifinishedHandoverDTO()
        {
            this.SemifinishedHandoverViewDetails = new List<SemifinishedHandoverDetailDTO>();
        }
        
        public override Nullable<int> CustomerID { get { return (this.Customer != null ? (this.Customer.CustomerID > 0 ? (Nullable<int>)this.Customer.CustomerID : null) : null); } }
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override int SemifinishedLeaderID { get { return (this.SemifinishedLeader != null ? this.SemifinishedLeader.EmployeeID : 0); } }
        [Display(Name = "Tổ trưởng Phôi")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO SemifinishedLeader { get; set; }

        public override int FinishedLeaderID { get { return (this.FinishedLeader != null ? this.FinishedLeader.EmployeeID : 0); } }
        [Display(Name = "Tổ trưởng ĐG")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO FinishedLeader { get; set; }

        public List<SemifinishedHandoverDetailDTO> SemifinishedHandoverViewDetails { get; set; }
        public List<SemifinishedHandoverDetailDTO> ViewDetails { get { return this.SemifinishedHandoverViewDetails; } set { this.SemifinishedHandoverViewDetails = value; } }

        public ICollection<SemifinishedHandoverDetailDTO> GetDetails() { return this.SemifinishedHandoverViewDetails; }

        protected override IEnumerable<SemifinishedHandoverDetailDTO> DtoDetails() { return this.SemifinishedHandoverViewDetails; }



        public virtual decimal GetTotalQuantity() { return this.DtoDetails().Select(o => o.Quantity).Sum(); }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.TotalQuantity != this.GetTotalQuantity()) yield return new ValidationResult("Lỗi tổng số lượng", new[] { "TotalQuantity" });
        }
    }
}
