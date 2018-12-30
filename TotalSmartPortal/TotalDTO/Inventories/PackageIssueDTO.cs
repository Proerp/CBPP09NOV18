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

namespace TotalDTO.Inventories
{
    public class PackageIssuePrimitiveDTO : QuantityDTO<PackageIssueDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.PackageIssue; } }

        public int GetID() { return this.PackageIssueID; }
        public void SetID(int id) { this.PackageIssueID = id; }

        public int PackageIssueID { get; set; }

        public int BlendingInstructionID { get; set; }
        [Display(Name = "Số BIS")]
        public string BlendingInstructionReference { get; set; }
        [Display(Name = "Mã chứng từ")]
        public string BlendingInstructionCode { get; set; }
        [Display(Name = "Ngày BIS")]
        public DateTime BlendingInstructionEntryDate { get; set; }
        [Display(Name = "Diễn giải")]
        public string BlendingInstructionDescription { get; set; }

        public virtual int CommodityID { get; set; }
        public int ShiftID { get; set; }
        public int WorkshiftID { get; set; }

        public virtual int ProductionLineID { get; set; }

        public virtual Nullable<int> WarehouseID { get; set; }
        public virtual int StorekeeperID { get; set; }
        public virtual int CrucialWorkerID { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            this.DtoDetails().ToList().ForEach(e => { e.BlendingInstructionID = this.BlendingInstructionID; e.ShiftID = this.ShiftID; e.WorkshiftID = this.WorkshiftID; e.ProductionLineID = this.ProductionLineID; e.CrucialWorkerID = this.CrucialWorkerID; e.WarehouseID = this.WarehouseID; });
        }
    }


    public class PackageIssueDTO : PackageIssuePrimitiveDTO, IBaseDetailEntity<PackageIssueDetailDTO>
    {
        public PackageIssueDTO()
        {
            this.PackageIssueViewDetails = new List<PackageIssueDetailDTO>();
            if (GlobalEnums.CBPP) { this.ProductionLine = new ProductionLineBaseDTO() { ProductionLineID = 10, Code = "UNKNOWN", Name = "UNKNOWN" }; this.Storekeeper = new EmployeeBaseDTO() { EmployeeID = 1, PreparedPersonID = 1, Name = "NONAME" }; this.CrucialWorker = new EmployeeBaseDTO() { EmployeeID = 1, PreparedPersonID = 1, Name = "NONAME" }; }
        }

        public override Nullable<int> WarehouseID { get { return (this.Warehouse != null ? this.Warehouse.WarehouseID : null); } }
        [Display(Name = "Kho hàng")]
        [UIHint("AutoCompletes/WarehouseBase")]
        public WarehouseBaseDTO Warehouse { get; set; }

        public override int CommodityID { get { return (this.Commodity != null ? this.Commodity.CommodityID : 0); } }
        [Display(Name = "Thành phẩm")]
        [UIHint("Commons/Commodity")]
        public CommodityBaseDTO Commodity { get; set; }

        public override int ProductionLineID { get { return (this.ProductionLine != null ? this.ProductionLine.ProductionLineID : 0); } }
        [Display(Name = "Mã số máy")]
        [UIHint("AutoCompletes/ProductionLine")]
        public ProductionLineBaseDTO ProductionLine { get; set; }

        public override int StorekeeperID { get { return (this.Storekeeper != null ? this.Storekeeper.EmployeeID : 0); } }
        [Display(Name = "Thủ kho")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO Storekeeper { get; set; }

        public override int CrucialWorkerID { get { return (this.CrucialWorker != null ? this.CrucialWorker.EmployeeID : 0); } }
        [Display(Name = "Nhân viên pha chế")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO CrucialWorker { get; set; }

        public List<PackageIssueDetailDTO> PackageIssueViewDetails { get; set; }
        public List<PackageIssueDetailDTO> ViewDetails { get { return this.PackageIssueViewDetails; } set { this.PackageIssueViewDetails = value; } }

        public ICollection<PackageIssueDetailDTO> GetDetails() { return this.PackageIssueViewDetails; }

        protected override IEnumerable<PackageIssueDetailDTO> DtoDetails() { return this.PackageIssueViewDetails; }
    }
}