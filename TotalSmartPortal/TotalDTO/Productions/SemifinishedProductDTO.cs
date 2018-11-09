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
    public class SemifinishedProductPrimitiveDTO : QuantityDTO<SemifinishedProductDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.SemifinishedProduct; } }

        public int GetID() { return this.SemifinishedProductID; }
        public void SetID(int id) { this.SemifinishedProductID = id; }

        public int SemifinishedProductID { get; set; }

        public virtual Nullable<int> CustomerID { get; set; }

        public int MaterialIssueID { get; set; }
        public int MaterialIssueDetailID { get; set; }

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


        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }

        public int GoodsReceiptID { get; set; }
        public string GoodsReceiptReference { get; set; }
        public string GoodsReceiptCode { get; set; }
        public Nullable<System.DateTime> GoodsReceiptEntryDate { get; set; }
        public int GoodsReceiptDetailID { get; set; }

        public decimal MaterialQuantity { get; set; }
        public decimal MaterialQuantityRemains { get; set; }

        [Display(Name = "Số chứng từ")]
        [UIHint("Commons/SOCode")]
        public string Code { get; set; }

        public int MaterialIssueDetailWorkshiftID { get; set; }
        [Display(Name = "Ca sản xuất")]
        public string MaterialIssueDetailWorkshiftCode { get; set; }
        [Display(Name = "Ngày sản xuất")]
        public DateTime MaterialIssueDetailWorkshiftEntryDate { get; set; }

        public virtual int ProductionLineID { get; set; }
        [Display(Name = "Mã số máy")]
        public string ProductionLineCode { get; set; }

        [Display(Name = "Mã số máy, ca sx")]
        public string WorkDescription { get { return this.ProductionLineCode + ", " + this.MaterialIssueDetailWorkshiftCode + " [" + this.MaterialIssueDetailWorkshiftEntryDate.ToString("dd/MM/yyyy") + "]"; } }

        [Display(Name = "Cuộn màng")]
        public string GoodsReceiptDescription { get { return this.MaterialCode + ", " + this.GoodsReceiptReference + (this.GoodsReceiptEntryDate != null ? " [" + this.GoodsReceiptEntryDate.Value.ToString("dd/MM/yyyy") + "]" : "") + ", " + this.MaterialQuantity.ToString("N2") + ", " + this.MaterialQuantityRemains.ToString("N2"); } }

        public int ShiftID { get; set; }
        public int WorkshiftID { get; set; } // WHEN ADD NEW: THIS WILL BE ZERO. THEN, THE REAL VALUE OF WorkshiftID WILL BE UPDATE BY SemifinishedProductSaveRelative

        public virtual int CrucialWorkerID { get; set; }

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

            foreach (SemifinishedProductDetailDTO semifinishedProductDetailDTO in this.DtoDetails())
            {
                if (Math.Round(this.FoilCounts * semifinishedProductDetailDTO.MoldQuantity, GlobalEnums.rndQuantity, MidpointRounding.AwayFromZero) != semifinishedProductDetailDTO.Quantity) yield return new ValidationResult("Lỗi số thành phẩm", new[] { "FoilCounts" });
            }
        }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            this.ShiftSaving(this.ShiftID); string caption = "";
            this.DtoDetails().ToList().ForEach(e => { e.MaterialIssueID = this.MaterialIssueID; e.MaterialIssueDetailID = this.MaterialIssueDetailID; e.FirmOrderID = this.FirmOrderID; e.GoodsReceiptID = this.GoodsReceiptID; e.GoodsReceiptDetailID = this.GoodsReceiptDetailID; e.CustomerID = this.CustomerID; e.ShiftID = this.ShiftID; e.WorkshiftID = this.WorkshiftID; e.ProductionLineID = this.ProductionLineID; e.CrucialWorkerID = this.CrucialWorkerID; caption = caption + (caption != "" ? ", " : "") + e.GetCaption(this.DtoDetails().Count()); });
            this.Caption = caption;
        }
    }


    public class SemifinishedProductDTO : SemifinishedProductPrimitiveDTO, IBaseDetailEntity<SemifinishedProductDetailDTO>
    {
        public SemifinishedProductDTO()
        {
            this.SemifinishedProductViewDetails = new List<SemifinishedProductDetailDTO>();
        }

        public override Nullable<int> CustomerID { get { return (this.Customer != null ? (this.Customer.CustomerID > 0 ? (Nullable<int>)this.Customer.CustomerID : null) : null); } }
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override int CrucialWorkerID { get { return (this.CrucialWorker != null ? this.CrucialWorker.EmployeeID : 0); } }
        [Display(Name = "Công nhân ĐHCK")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO CrucialWorker { get; set; }


        public List<SemifinishedProductDetailDTO> SemifinishedProductViewDetails { get; set; }
        public List<SemifinishedProductDetailDTO> ViewDetails { get { return this.SemifinishedProductViewDetails; } set { this.SemifinishedProductViewDetails = value; } }

        public ICollection<SemifinishedProductDetailDTO> GetDetails() { return this.SemifinishedProductViewDetails; }

        protected override IEnumerable<SemifinishedProductDetailDTO> DtoDetails() { return this.SemifinishedProductViewDetails; }
    }
}