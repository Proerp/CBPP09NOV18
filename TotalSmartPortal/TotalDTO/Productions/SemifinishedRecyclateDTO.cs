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
    public class SemifinishedRecyclatePrimitiveDTO : QuantityDTO<SemifinishedRecyclateDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.SemifinishedRecyclate; } }

        public int GetID() { return this.SemifinishedRecyclateID; }
        public void SetID(int id) { this.SemifinishedRecyclateID = id; }

        public int SemifinishedRecyclateID { get; set; }

        public int ShiftID { get; set; }
        public int WorkshiftID { get; set; } // WHEN ADD NEW: THIS WILL BE ZERO. THEN, THE REAL VALUE OF WorkshiftID WILL BE UPDATE BY MaterialIssueSaveRelative

        [Required(ErrorMessage = "Vui lòng chọn kho nhập")]
        public virtual Nullable<int> WarehouseID { get; set; }

        public virtual int CrucialWorkerID { get; set; }
        public virtual int StorekeeperID { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            this.ShiftSaving(this.ShiftID);
            this.DtoDetails().ToList().ForEach(e => { e.WarehouseID = (int)this.WarehouseID; e.ShiftID = this.ShiftID; e.WorkshiftID = this.WorkshiftID; e.CrucialWorkerID = this.CrucialWorkerID; e.StorekeeperID = this.StorekeeperID; });
        }
    }


    public class SemifinishedRecyclateDTO : SemifinishedRecyclatePrimitiveDTO, IBaseDetailEntity<SemifinishedRecyclateDetailDTO>
    {
        public SemifinishedRecyclateDTO()
        {
            this.SemifinishedRecyclateViewDetails = new List<SemifinishedRecyclateDetailDTO>();
        }

        public override Nullable<int> WarehouseID { get { return (this.Warehouse != null ? (this.Warehouse.WarehouseID > 0 ? (Nullable<int>)this.Warehouse.WarehouseID : null) : null); } }
        [Display(Name = "Kho phế phẩm")]
        [UIHint("Commons/WarehouseBase")]
        public WarehouseBaseDTO Warehouse { get; set; }

        public override int CrucialWorkerID { get { return (this.CrucialWorker != null ? this.CrucialWorker.EmployeeID : 0); } }
        [Display(Name = "NV bàn giao")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO CrucialWorker { get; set; }

        public override int StorekeeperID { get { return (this.Storekeeper != null ? this.Storekeeper.EmployeeID : 0); } }
        [Display(Name = "Nhân viên kho")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO Storekeeper { get; set; }

        public List<SemifinishedRecyclateDetailDTO> SemifinishedRecyclateViewDetails { get; set; }
        public List<SemifinishedRecyclateDetailDTO> ViewDetails { get { return this.SemifinishedRecyclateViewDetails; } set { this.SemifinishedRecyclateViewDetails = value; } }

        public ICollection<SemifinishedRecyclateDetailDTO> GetDetails() { return this.SemifinishedRecyclateViewDetails; }

        protected override IEnumerable<SemifinishedRecyclateDetailDTO> DtoDetails() { return this.SemifinishedRecyclateViewDetails; }


        public List<SemifinishedRecyclatePackageDTO> SemifinishedRecyclateSummaries { get; set; }
    }
}
