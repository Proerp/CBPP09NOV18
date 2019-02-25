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
    public interface IWOOption { GlobalEnums.NmvnTaskID NMVNTaskID { get; } }

    public class WOOptionMaterial : IWOOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.MaterialWorkOrder; } } }
    public class WOOptionItem : IWOOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.ItemWorkOrder; } } }
    public class WOOptionProduct : IWOOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.ProductWorkOrder; } } }

    public interface IWorkOrderPrimitiveDTO : IQuantityDTO, IPrimitiveEntity, IPrimitiveDTO, IBaseDTO
    {
        int WorkOrderID { get; set; }

        Nullable<int> CustomerID { get; set; }

        int ProductionOrderID { get; set; }
        int ProductionOrderDetailID { get; set; }

        int PlannedOrderID { get; set; }

        int FirmOrderID { get; set; }
        [Display(Name = "Số KHSX")]
        string FirmOrderReference { get; set; }
        [Display(Name = "Mã chứng từ")]
        string FirmOrderCode { get; set; }
        [Display(Name = "Ngày KHSX")]
        DateTime FirmOrderEntryDate { get; set; }
        [Display(Name = "Tên thành phẩm")]
        string FirmOrderSpecs { get; set; }
        [Display(Name = "Mã thành phẩm")]
        string FirmOrderSpecification { get; set; }
        [Display(Name = "Thành phẩm")]
        string FirmOrderSpecificationSpecs { get; }

        decimal FirmOrderQuantityMaterialEstimated { get; set; }
        decimal FirmOrderQuantityMaterialEstimatedIssued { get; set; }
        
        [Display(Name = "KL NVL tồn")]
        decimal QuantityMaterialEstimatedRemains { get; set; }
        [Display(Name = "Khối lượng mẻ sản xuất")]
        decimal QuantityMaterialEstimated { get; set; }

        int BomID { get; set; }

        Nullable<int> WarehouseID { get; set; }
    }


    public class WorkOrderPrimitiveDTO<TWOOption> : QuantityDTO<WorkOrderDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
        where TWOOption : IWOOption, new()
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return new TWOOption().NMVNTaskID; } }

        public int GetID() { return this.WorkOrderID; }
        public void SetID(int id) { this.WorkOrderID = id; }

        public int WorkOrderID { get; set; }

        public virtual Nullable<int> CustomerID { get; set; }

        public int ProductionOrderID { get; set; }
        public int ProductionOrderDetailID { get; set; }

        public int PlannedOrderID { get; set; }

        public int FirmOrderID { get; set; }
        public string FirmOrderReference { get; set; }
        public string FirmOrderCode { get; set; }
        public DateTime FirmOrderEntryDate { get; set; }
        public string FirmOrderSpecs { get; set; }
        public string FirmOrderSpecification { get; set; }
        public string FirmOrderSpecificationSpecs { get { return this.FirmOrderSpecs + " (" + this.FirmOrderSpecification + ")"; } }

        public decimal FirmOrderQuantityMaterialEstimated { get; set; }
        public decimal FirmOrderQuantityMaterialEstimatedIssued { get; set; }

        public decimal QuantityMaterialEstimatedRemains { get; set; } //WHEN CREATE WIZARD: SET BY CREATE WIZARD JS: MUST BE CALCUALATE IN ADVANCE (T-SQL). WHEN EDIT: AUTO MAP THIS PROPERTY FROM MODEL ==> MUST MUST BE CALCUALATE BY [partial class WorkOrder]
        public decimal QuantityMaterialEstimated { get; set; }

        public int BomID { get; set; }

        public virtual Nullable<int> WarehouseID { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            this.DtoDetails().ToList().ForEach(e => { e.NMVNTaskID = this.NMVNTaskID; e.PlannedOrderID = this.PlannedOrderID; e.FirmOrderID = this.FirmOrderID; e.ProductionOrderID = this.ProductionOrderID; e.ProductionOrderDetailID = this.ProductionOrderDetailID; e.CustomerID = this.CustomerID; e.WarehouseID = this.WarehouseID; });
        }
    }


    public interface IWorkOrderDTO : IWorkOrderPrimitiveDTO, IMaterialItemProduct
    {
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        CustomerBaseDTO Customer { get; set; }

        [Display(Name = "Kho hàng")]
        [UIHint("AutoCompletes/WarehouseBase")]
        WarehouseBaseDTO Warehouse { get; set; }

        List<WorkOrderDetailDTO> WorkOrderViewDetails { get; set; }
        List<WorkOrderDetailDTO> ViewDetails { get; set; }

        string ControllerName { get; }
    }

    public class WorkOrderDTO<TWOOption> : WorkOrderPrimitiveDTO<TWOOption>, IBaseDetailEntity<WorkOrderDetailDTO>, IWorkOrderDTO
        where TWOOption : IWOOption, new()
    {
        public WorkOrderDTO()
        {
            this.WorkOrderViewDetails = new List<WorkOrderDetailDTO>();
        }

        public override Nullable<int> CustomerID { get { return (this.Customer != null ? (this.Customer.CustomerID > 0 ? (Nullable<int>)this.Customer.CustomerID : null) : null); } }
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override Nullable<int> WarehouseID { get { return (this.Warehouse != null ? this.Warehouse.WarehouseID : null); } }
        [Display(Name = "Kho hàng")]
        [UIHint("AutoCompletes/WarehouseBase")]
        public WarehouseBaseDTO Warehouse { get; set; }

        public List<WorkOrderDetailDTO> WorkOrderViewDetails { get; set; }
        public List<WorkOrderDetailDTO> ViewDetails { get { return this.WorkOrderViewDetails; } set { this.WorkOrderViewDetails = value; } }

        public ICollection<WorkOrderDetailDTO> GetDetails() { return this.WorkOrderViewDetails; }

        protected override IEnumerable<WorkOrderDetailDTO> DtoDetails() { return this.WorkOrderViewDetails; }

        public string ControllerName { get { return this.NMVNTaskID.ToString() + "s"; } }

        public bool IsMaterial { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.MaterialWorkOrder; } }
        public bool IsItem { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.ItemWorkOrder; } }
        public bool IsProduct { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.ProductWorkOrder; } }
    }
}
