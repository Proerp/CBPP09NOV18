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
    public class FinishedItemPrimitiveDTO : QuantityDTO<FinishedItemDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.FinishedItem; } }

        public int GetID() { return this.FinishedItemID; }
        public void SetID(int id) { this.FinishedItemID = id; }

        public int FinishedItemID { get; set; }

        public virtual Nullable<int> CustomerID { get; set; }

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

        public string SemifinishedItemReferences { get; set; }

        public int ShiftID { get; set; }
        public int WorkshiftID { get; set; } // WHEN ADD NEW: THIS WILL BE ZERO. THEN, THE REAL VALUE OF WorkshiftID WILL BE UPDATE BY MaterialIssueSaveRelative

        [UIHint("DateTime")]
        [Display(Name = "Thời gian bắt đầu SX")]
        public Nullable<System.DateTime> StartDate { get; set; }
        [UIHint("DateTime")]
        [Display(Name = "Thời gian kết thúc SX")]
        public Nullable<System.DateTime> StopDate { get; set; }

        [Display(Name = "Độ dày thực tế: Min và Max")]
        [Range(0, 999999, ErrorMessage = "Số tấm phải >= 0")]
        public decimal ThicknessMin { get; set; }
        [Display(Name = "Max")]
        [Range(0, 999999, ErrorMessage = "Số kg >= 0")]
        public decimal ThicknessMax { get; set; }

        public virtual int CrucialWorkerID { get; set; }

        public virtual decimal TotalQuantityFailure { get; set; }
        public virtual decimal GetTotalQuantityFailure() { return this.DtoDetails().Select(o => o.QuantityFailure).Sum(); }
        public virtual decimal TotalQuantityExcess { get; set; }
        public virtual decimal GetTotalQuantityExcess() { return this.DtoDetails().Select(o => o.QuantityExcess).Sum(); }
        public virtual decimal TotalQuantityShortage { get; set; }
        public virtual decimal GetTotalQuantityShortage() { return this.DtoDetails().Select(o => o.QuantityShortage).Sum(); }
        public virtual decimal TotalSwarfs { get; set; }
        public virtual decimal GetTotalSwarfs() { return this.DtoDetails().Select(o => o.Swarfs).Sum(); }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.TotalSwarfs != this.GetTotalSwarfs()) yield return new ValidationResult("Lỗi tổng số lượng biên", new[] { "TotalSwarfs" });
            if (this.TotalQuantityFailure != this.GetTotalQuantityFailure()) yield return new ValidationResult("Lỗi tổng số lượng phế phẩm sx", new[] { "TotalQuantityFailure" });
            if (this.TotalQuantityExcess != this.GetTotalQuantityExcess()) yield return new ValidationResult("Lỗi tổng số lượng phế phẩm sx", new[] { "TotalQuantityExcess" });
            if (this.TotalQuantityShortage != this.GetTotalQuantityShortage()) yield return new ValidationResult("Lỗi tổng số lượng phế phẩm sx", new[] { "TotalQuantityShortage" });
        }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            string purchaseOrderReferences = ""; 
            this.ShiftSaving(this.ShiftID);
            this.DtoDetails().ToList().ForEach(e => { e.CustomerID = this.CustomerID; e.ShiftID = this.ShiftID; e.WorkshiftID = this.WorkshiftID; e.CrucialWorkerID = this.CrucialWorkerID; if (purchaseOrderReferences.IndexOf(e.SemifinishedItemReference) < 0) purchaseOrderReferences = purchaseOrderReferences + (purchaseOrderReferences != "" ? ", " : "") + e.SemifinishedItemReference; });
            this.SemifinishedItemReferences = purchaseOrderReferences; 
        }

    }


    public class FinishedItemDTO : FinishedItemPrimitiveDTO, IBaseDetailEntity<FinishedItemDetailDTO>
    {
        public FinishedItemDTO()
        {
            this.FinishedItemViewDetails = new List<FinishedItemDetailDTO>();

            this.FinishedItemPackages = new List<FinishedItemPackageDTO>();
        }

        public override Nullable<int> CustomerID { get { return (this.Customer != null ? (this.Customer.CustomerID > 0 ? (Nullable<int>)this.Customer.CustomerID : null) : null); } }
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override int CrucialWorkerID { get { return (this.CrucialWorker != null ? this.CrucialWorker.EmployeeID : 0); } }
        [Display(Name = "NV tạo màng")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO CrucialWorker { get; set; }

        public List<FinishedItemDetailDTO> FinishedItemViewDetails { get; set; }
        public List<FinishedItemDetailDTO> ViewDetails { get { return this.FinishedItemViewDetails; } set { this.FinishedItemViewDetails = value; } }

        public ICollection<FinishedItemDetailDTO> GetDetails() { return this.FinishedItemViewDetails; }

        protected override IEnumerable<FinishedItemDetailDTO> DtoDetails() { return this.FinishedItemViewDetails; }


        public List<FinishedItemPackageDTO> FinishedItemPackages { get; set; }


        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            this.FinishedItemPackages.ForEach(e => { e.LocationID = this.LocationID; e.EntryDate = this.EntryDate; e.Approved = this.Approved; e.ApprovedDate = this.ApprovedDate; e.InActive = this.InActive; e.InActiveDate = this.InActiveDate; e.CustomerID = this.CustomerID; e.ShiftID = this.ShiftID; e.WorkshiftID = this.WorkshiftID; e.FirmOrderID = this.FirmOrderID; e.PlannedOrderID = this.PlannedOrderID; e.SemifinishedItemReferences = this.SemifinishedItemReferences; });
        }
    }
}
