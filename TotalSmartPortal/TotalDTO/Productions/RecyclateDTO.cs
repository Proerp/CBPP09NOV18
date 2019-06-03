using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase;
using TotalBase.Enums;
using TotalDTO.Helpers;
using TotalDTO.Commons;
using TotalDTO.Helpers.Interfaces;

namespace TotalDTO.Productions
{
    public interface IRecyclateOption { GlobalEnums.NmvnTaskID NMVNTaskID { get; } }

    public class SemifinishedProductRecyclateOption : IRecyclateOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.SemifinishedProductRecyclate; } } }
    public class FinishedProductRecyclateOption : IRecyclateOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.FinishedProductRecyclate; } } }
    public class FinishedItemRecyclateOption : IRecyclateOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.FinishedItemRecyclate; } } }

    public interface IRecyclatePrimitiveDTO : IQuantityDTO, IPrimitiveEntity, IPrimitiveDTO, IBaseDTO
    {
        int RecyclateID { get; set; }

        int WorkshiftID { get; set; }
        [Display(Name = "Ca sản xuất")]
        string WorkshiftCode { get; set; }
        [Display(Name = "Ngày sản xuất")]
        DateTime WorkshiftEntryDate { get; set; }


        [Required(ErrorMessage = "Vui lòng chọn kho nhập")]
        Nullable<int> WarehouseID { get; set; }

        int CrucialWorkerID { get; set; }
        int StorekeeperID { get; set; }
    }

    public class RecyclatePrimitiveDTO : QuantityDTO<RecyclateDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.SemifinishedProductRecyclate; } }

        public int GetID() { return this.RecyclateID; }
        public void SetID(int id) { this.RecyclateID = id; }

        public int RecyclateID { get; set; }

        public virtual int WorkshiftID { get; set; }
        [Display(Name = "Ca sản xuất")]
        public string WorkshiftCode { get; set; }
        [Display(Name = "Ngày sản xuất")]
        public DateTime WorkshiftEntryDate { get; set; }


        [Required(ErrorMessage = "Vui lòng chọn kho nhập")]
        public virtual Nullable<int> WarehouseID { get; set; }

        public virtual int CrucialWorkerID { get; set; }
        public virtual int StorekeeperID { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            this.DtoDetails().ToList().ForEach(e => { e.WarehouseID = (int)this.WarehouseID; e.WorkshiftID = this.WorkshiftID; e.CrucialWorkerID = this.CrucialWorkerID; e.StorekeeperID = this.StorekeeperID; });
        }
    }


    public class RecyclateDTO : RecyclatePrimitiveDTO, IBaseDetailEntity<RecyclateDetailDTO>
    {
        public RecyclateDTO()
        {
            this.RecyclateViewDetails = new List<RecyclateDetailDTO>();

            this.RecyclatePackages = new List<RecyclatePackageDTO>();
        }

        public override int CrucialWorkerID { get { return (this.CrucialWorker != null ? this.CrucialWorker.EmployeeID : 0); } }
        [Display(Name = "NV bàn giao")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO CrucialWorker { get; set; }

        public override int StorekeeperID { get { return (this.Storekeeper != null ? this.Storekeeper.EmployeeID : 0); } }
        [Display(Name = "Nhân viên kho")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO Storekeeper { get; set; }

        public List<RecyclateDetailDTO> RecyclateViewDetails { get; set; }
        public List<RecyclateDetailDTO> ViewDetails { get { return this.RecyclateViewDetails; } set { this.RecyclateViewDetails = value; } }

        public ICollection<RecyclateDetailDTO> GetDetails() { return this.RecyclateViewDetails; }

        protected override IEnumerable<RecyclateDetailDTO> DtoDetails() { return this.RecyclateViewDetails; }


        public List<RecyclatePackageDTO> RecyclatePackages { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.RecyclatePackages.Count <= 0) yield return new ValidationResult("", new[] { "TotalQuantity" }); //RecyclatesController for more detail: WHERE THERE IS A RecycleCommodityID == null ===> THIS .RecyclatePackages.Count WILL BE 0
        }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            this.DtoDetails().ToList().ForEach(e => e.Quantity = 0); string caption = "";
            this.RecyclatePackages.ForEach(e =>
            {
                e.LocationID = this.LocationID; e.EntryDate = this.EntryDate; e.BatchEntryDate = (DateTime)this.EntryDate; e.Approved = this.Approved; e.ApprovedDate = this.ApprovedDate; e.WarehouseID = (int)this.WarehouseID; e.WorkshiftID = this.WorkshiftID;
                if (e.Quantity > 0 && caption.IndexOf(e.CommodityName) < 0) caption = caption + (caption != "" ? ", " : "") + e.CommodityName;

                decimal quantity = e.Quantity; //ALLOCATED RecyclatePackageDTO.Quantity TO RecyclateViewDetail.Quantity
                this.DtoDetails().Where(w => w.RecycleCommodityID == e.CommodityID).Each(ea => { ea.Quantity = (ea.QuantityRemains <= quantity ? ea.QuantityRemains : quantity); quantity = Math.Round(quantity - ea.Quantity, GlobalEnums.rndQuantity, MidpointRounding.AwayFromZero); });
                if (quantity > 0) { RecyclateDetailDTO demifinishedRecyclateDetailDTO = this.DtoDetails().Where(w => w.RecycleCommodityID == e.CommodityID).Last(); demifinishedRecyclateDetailDTO.Quantity = Math.Round(demifinishedRecyclateDetailDTO.Quantity + quantity, GlobalEnums.rndQuantity, MidpointRounding.AwayFromZero); }
            });
            this.TotalQuantity = this.GetTotalQuantity();
            this.Caption = caption != "" ? (caption.Length > 98 ? caption.Substring(0, 95) + "..." : caption) : null;
        }
    }
}
