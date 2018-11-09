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
    public class MaterialIssuePrimitiveDTO : QuantityDTO<MaterialIssueDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.MaterialIssue; } }

        public int GetID() { return this.MaterialIssueID; }
        public void SetID(int id) { this.MaterialIssueID = id; }

        public int MaterialIssueID { get; set; }

        public int MaterialIssueTypeID { get; set; }

        public virtual Nullable<int> CustomerID { get; set; }

        public int ProductionOrderID { get; set; }
        public int ProductionOrderDetailID { get; set; }

        public int PlannedOrderID { get; set; }

        public int FirmOrderID { get; set; }
        [Display(Name = "Số KHSX")]
        public string FirmOrderReference { get; set; }
        [Display(Name = "Mã chứng từ")]
        public string FirmOrderCode { get; set; }
        [Display(Name = "Ngày KHSX")]
        public DateTime FirmOrderEntryDate { get; set; }
        [Display(Name = "Tên thành phẩm")]
        public string FirmOrderSpecs { get; set; }
        [Display(Name = "Mã thành phẩm")]
        public string FirmOrderSpecification { get; set; }
        [Display(Name = "Thành phẩm")]
        public string FirmOrderSpecificationSpecs { get { return this.FirmOrderSpecs + " (" + this.FirmOrderSpecification + ")"; } }

        [Display(Name = "Thông số máy")]
        [Required(ErrorMessage = "Vui lòng nhập thông số máy")]
        public string Code { get; set; }

        public int ShiftID { get; set; }
        public int WorkshiftID { get; set; } // WHEN ADD NEW: THIS WILL BE ZERO. THEN, THE REAL VALUE OF WorkshiftID WILL BE UPDATE BY MaterialIssueSaveRelative
        
        public virtual int ProductionLineID { get; set; }        

        [Display(Name = "Mã số máy, ca sx")]
        public override string Caption { get { return ""; } }

        public virtual Nullable<int> WarehouseID { get; set; }
        public virtual int StorekeeperID { get; set; }
        public virtual int CrucialWorkerID { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            this.ShiftSaving(this.ShiftID);
            this.DtoDetails().ToList().ForEach(e => { e.MaterialIssueTypeID = this.MaterialIssueTypeID; e.PlannedOrderID = this.PlannedOrderID; e.FirmOrderID = this.FirmOrderID; e.ProductionOrderID = this.ProductionOrderID; e.ProductionOrderDetailID = this.ProductionOrderDetailID; e.CustomerID = this.CustomerID; e.ShiftID = this.ShiftID; e.WorkshiftID = this.WorkshiftID; e.ProductionLineID = this.ProductionLineID; e.CrucialWorkerID = this.CrucialWorkerID; e.WarehouseID = this.WarehouseID; });
        }
    }


    public class MaterialIssueDTO : MaterialIssuePrimitiveDTO, IBaseDetailEntity<MaterialIssueDetailDTO>
    {
        public MaterialIssueDTO()
        {
            this.MaterialIssueViewDetails = new List<MaterialIssueDetailDTO>();
        }

        public override Nullable<int> CustomerID { get { return (this.Customer != null ? (this.Customer.CustomerID > 0 ? (Nullable<int>)this.Customer.CustomerID : null) : null); } }
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override Nullable<int> WarehouseID { get { return (this.Warehouse != null ? this.Warehouse.WarehouseID : null); } }
        [Display(Name = "Kho hàng")]
        [UIHint("AutoCompletes/WarehouseBase")]
        public WarehouseBaseDTO Warehouse { get; set; }

        public override int ProductionLineID { get { return (this.ProductionLine != null ? this.ProductionLine.ProductionLineID : 0); } }
        [Display(Name = "Mã số máy")]
        [UIHint("AutoCompletes/ProductionLine")]
        public ProductionLineBaseDTO ProductionLine { get; set; }

        public override int StorekeeperID { get { return (this.Storekeeper != null ? this.Storekeeper.EmployeeID : 0); } }
        [Display(Name = "Nhân viên kho")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO Storekeeper { get; set; }

        public override int CrucialWorkerID { get { return (this.CrucialWorker != null ? this.CrucialWorker.EmployeeID : 0); } }
        [Display(Name = "Công nhân ĐHCK")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO CrucialWorker { get; set; }

        public List<MaterialIssueDetailDTO> MaterialIssueViewDetails { get; set; }
        public List<MaterialIssueDetailDTO> ViewDetails { get { return this.MaterialIssueViewDetails; } set { this.MaterialIssueViewDetails = value; } }

        public ICollection<MaterialIssueDetailDTO> GetDetails() { return this.MaterialIssueViewDetails; }

        protected override IEnumerable<MaterialIssueDetailDTO> DtoDetails() { return this.MaterialIssueViewDetails; }
    }
}