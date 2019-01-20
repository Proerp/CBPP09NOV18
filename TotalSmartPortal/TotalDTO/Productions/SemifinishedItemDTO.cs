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
    public class SemifinishedItemPrimitiveDTO : QuantityDTO<SemifinishedItemDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.SemifinishedItem; } }

        public int GetID() { return this.SemifinishedItemID; }
        public void SetID(int id) { this.SemifinishedItemID = id; }

        public int SemifinishedItemID { get; set; }

        public virtual Nullable<int> CustomerID { get; set; }

        public int MaterialIssueID { get; set; }

        public int PlannedOrderID { get; set; }
        public int FirmOrderID { get; set; }
        [Display(Name = "Số KHSX")]
        public string FirmOrderReference { get; set; }
        [Display(Name = "Mã chứng từ")]
        public string FirmOrderCode { get; set; }
        [Display(Name = "Ngày KHSX")]
        public DateTime FirmOrderEntryDate { get; set; }
        [Display(Name = "Thành phẩm khay")]
        public string FirmOrderSpecification { get; set; }


        public string BomCode { get; set; }


        public decimal MaterialQuantity { get; set; }
        public decimal MaterialQuantityRemains { get; set; }

        [Display(Name = "Số chứng từ")]
        [UIHint("Commons/SOCode")]
        public string Code { get; set; }

        public int MaterialIssueWorkshiftID { get; set; }
        [Display(Name = "Ca sản xuất")]
        public string MaterialIssueWorkshiftCode { get; set; }
        [Display(Name = "Ngày sản xuất")]
        public DateTime MaterialIssueWorkshiftEntryDate { get; set; }

        public virtual int ProductionLineID { get; set; }
        [Display(Name = "Mã số máy")]
        public string ProductionLineCode { get; set; }

        [Display(Name = "Mã số máy, ca sx")]
        public string WorkDescription { get { return this.ProductionLineCode + ", " + this.MaterialIssueWorkshiftCode + " [" + this.MaterialIssueWorkshiftEntryDate.ToString("dd/MM/yyyy") + "]"; } }

        public int ShiftID { get; set; }
        public int WorkshiftID { get; set; } // WHEN ADD NEW: THIS WILL BE ZERO. THEN, THE REAL VALUE OF WorkshiftID WILL BE UPDATE BY SemifinishedItemSaveRelative

        public virtual int CrucialWorkerID { get; set; }

        [UIHint("DateTime")]
        [Display(Name = "Thời gian bắt đầu SX")]
        public Nullable<System.DateTime> StartDate { get; set; }
        [UIHint("DateTime")]
        [Display(Name = "Thời gian kết thúc SX")]
        public Nullable<System.DateTime> StopDate { get; set; }

        [Display(Name = "Số thứ tự tấm phôi đầu")]
        [Range(0, 999999, ErrorMessage = "Số thứ tự >= 0")]
        public decimal StartSequenceNo { get; set; }
        [Display(Name = "Số thứ tự tấm phôi cuối")]
        [Range(0, 999999, ErrorMessage = "Số thứ tự >= 0")]
        public decimal StopSequenceNo { get; set; }
        [Display(Name = "Tổng số tấm phôi")]
        [Range(0, 999999, ErrorMessage = "Tổng số tấm phôi >= 0")]
        public decimal FoilCounts { get; set; }
        [Display(Name = "Số kg/ Số tấm phôi mẫu")]
        [Range(1, 999999, ErrorMessage = "Số tấm phải >= 1")]
        public decimal FoilUnitCounts { get; set; }
        [Display(Name = "Số kg")]
        [Range(0, 999999, ErrorMessage = "Số kg >= 0")]
        public decimal FoilUnitWeights { get; set; }
        [Display(Name = "Tổng số kg phôi")]
        [Range(0, 999999, ErrorMessage = "Tổng số kg >= 0")]
        public decimal FoilWeights { get; set; }
        [Display(Name = "Số kg phế phẩm")]
        [Range(0, 999999, ErrorMessage = "Số kg >= 0")]
        public decimal FailureWeights { get; set; }

        public decimal TotalQuantityIssue { get; set; }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (Math.Round(this.StopSequenceNo - this.StartSequenceNo + 1, GlobalEnums.rndN0, MidpointRounding.AwayFromZero) != this.FoilCounts) yield return new ValidationResult("Lỗi số lượng tấm phôi", new[] { "FoilCounts" });
            if (Math.Round(this.FoilCounts * this.FoilUnitWeights / this.FoilUnitCounts, GlobalEnums.rndQuantity, MidpointRounding.AwayFromZero) != this.FoilWeights) yield return new ValidationResult("Lỗi tổng số kg phôi", new[] { "FoilWeights" });

            foreach (SemifinishedItemDetailDTO semifinishedItemDetailDTO in this.DtoDetails())
            {
                if (Math.Round(this.FoilCounts * semifinishedItemDetailDTO.MoldQuantity, GlobalEnums.rndQuantity, MidpointRounding.AwayFromZero) != semifinishedItemDetailDTO.Quantity) yield return new ValidationResult("Lỗi số thành phẩm", new[] { "FoilCounts" });
            }
        }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            this.ShiftSaving(this.ShiftID); string caption = "";
            this.DtoDetails().ToList().ForEach(e => { e.MaterialIssueID = this.MaterialIssueID; e.FirmOrderID = this.FirmOrderID; e.CustomerID = this.CustomerID; e.ShiftID = this.ShiftID; e.WorkshiftID = this.WorkshiftID; e.ProductionLineID = this.ProductionLineID; e.CrucialWorkerID = this.CrucialWorkerID; caption = caption + (caption != "" ? ", " : "") + e.GetCaption(this.DtoDetails().Count()); });
            this.Caption = caption;
        }
    }


    public class SemifinishedItemDTO : SemifinishedItemPrimitiveDTO, IBaseDetailEntity<SemifinishedItemDetailDTO>
    {
        public SemifinishedItemDTO()
        {
            this.SemifinishedItemViewDetails = new List<SemifinishedItemDetailDTO>();
        }

        public override Nullable<int> CustomerID { get { return (this.Customer != null ? (this.Customer.CustomerID > 0 ? (Nullable<int>)this.Customer.CustomerID : null) : null); } }
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override int CrucialWorkerID { get { return (this.CrucialWorker != null ? this.CrucialWorker.EmployeeID : 0); } }
        [Display(Name = "Công nhân ĐHCK")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO CrucialWorker { get; set; }


        public List<SemifinishedItemDetailDTO> SemifinishedItemViewDetails { get; set; }
        public List<SemifinishedItemDetailDTO> ViewDetails { get { return this.SemifinishedItemViewDetails; } set { this.SemifinishedItemViewDetails = value; } }

        public ICollection<SemifinishedItemDetailDTO> GetDetails() { return this.SemifinishedItemViewDetails; }

        protected override IEnumerable<SemifinishedItemDetailDTO> DtoDetails() { return this.SemifinishedItemViewDetails; }
    }
}