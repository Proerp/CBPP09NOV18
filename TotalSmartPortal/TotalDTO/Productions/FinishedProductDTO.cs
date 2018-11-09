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
    public class FinishedProductPrimitiveDTO : QuantityDTO<FinishedProductDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.FinishedProduct; } }

        public int GetID() { return this.FinishedProductID; }
        public void SetID(int id) { this.FinishedProductID = id; }

        public int FinishedProductID { get; set; }     

        public virtual Nullable<int> CustomerID { get; set; }

        public int FirmOrderID { get; set; }
        [Display(Name = "Số KHSX")]
        public string FirmOrderReference { get; set; }
        [Display(Name = "Mã chứng từ")]
        public string FirmOrderCode { get; set; }
        [Display(Name = "Ngày KHSX")]
        public DateTime FirmOrderEntryDate { get; set; }
        [Display(Name = "Thành phẩm khay")]
        public string FirmOrderSpecification { get; set; }

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

            this.DtoDetails().ToList().ForEach(e => { e.CustomerID = this.CustomerID; e.CrucialWorkerID = this.CrucialWorkerID; });            
        }
        
    }


    public class FinishedProductDTO : FinishedProductPrimitiveDTO, IBaseDetailEntity<FinishedProductDetailDTO>
    {
        public FinishedProductDTO()
        {
            this.FinishedProductViewDetails = new List<FinishedProductDetailDTO>();
        }

        public override Nullable<int> CustomerID { get { return (this.Customer != null ? (this.Customer.CustomerID > 0 ? (Nullable<int>)this.Customer.CustomerID : null) : null); } }
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override int CrucialWorkerID { get { return (this.CrucialWorker != null ? this.CrucialWorker.EmployeeID : 0); } }
        [Display(Name = "NV đóng gói")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO CrucialWorker { get; set; }        

        public List<FinishedProductDetailDTO> FinishedProductViewDetails { get; set; }
        public List<FinishedProductDetailDTO> ViewDetails { get { return this.FinishedProductViewDetails; } set { this.FinishedProductViewDetails = value; } }

        public ICollection<FinishedProductDetailDTO> GetDetails() { return this.FinishedProductViewDetails; }

        protected override IEnumerable<FinishedProductDetailDTO> DtoDetails() { return this.FinishedProductViewDetails; }


        public List<FinishedProductSummaryDTO> FinishedProductSummaries { get; set; }
    }
}
