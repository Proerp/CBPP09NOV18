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
    public interface IWAOption { GlobalEnums.NmvnTaskID NMVNTaskID { get; } }

    public class WAOptionMtlRct : IWAOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.OtherMaterialReceipt; } } }
    public class WAOptionMtlIss : IWAOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.OtherMaterialIssue; } } }
    public class WAOptionMtlAdj : IWAOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.MaterialAdjustment; } } }

    public class WAOptionItmRct : IWAOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.OtherItemReceipt; } } }
    public class WAOptionItmIss : IWAOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.OtherItemIssue; } } }
    public class WAOptionItmAdj : IWAOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.ItemAdjustment; } } }

    public class WAOptionPrdRct : IWAOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.OtherProductReceipt; } } }
    public class WAOptionPrdIss : IWAOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.OtherProductIssue; } } }
    public class WAOptionPrdAdj : IWAOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.ProductAdjustment; } } }

    public interface IWarehouseAdjustmentPrimitiveDTO : IQuantityDTO, IPrimitiveEntity, IPrimitiveDTO, IBaseDTO
    {
        int WarehouseAdjustmentID { get; set; }
        int WarehouseAdjustmentTypeID { get; set; }
        Nullable<int> WarehouseID { get; set; }
        Nullable<int> WarehouseReceiptID { get; set; }
        Nullable<int> CustomerID { get; set; }

        [Display(Name = "Mục đích")]
        string AdjustmentJobs { get; set; }
        int StorekeeperID { get; set; }


        bool WarehouseReceiptEnabled { get; }
        bool HasPositiveLine { get; }


        decimal TotalQuantityPositive { get; set; }
        decimal TotalQuantityNegative { get; set; }
    }

    public class WarehouseAdjustmentPrimitiveDTO<TWAOption> : QuantityDTO<WarehouseAdjustmentDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
        where TWAOption : IWAOption, new()
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return new TWAOption().NMVNTaskID; } }

        public int GetID() { return this.WarehouseAdjustmentID; }
        public void SetID(int id) { this.WarehouseAdjustmentID = id; }

        public int WarehouseAdjustmentID { get; set; }

        public int WarehouseAdjustmentTypeID { get; set; }

        public virtual Nullable<int> WarehouseID { get; set; }
        public virtual Nullable<int> WarehouseReceiptID { get; set; }
        public virtual Nullable<int> CustomerID { get; set; }

        public string AdjustmentJobs { get; set; }

        public virtual int StorekeeperID { get; set; }


        public bool WarehouseReceiptEnabled { get { return this.WarehouseAdjustmentTypeID == (int)GlobalEnums.WarehouseAdjustmentTypeID.BothReceiptIssues; } }
        public bool HasPositiveLine { get { return this.DtoDetails().Where(w => w.Quantity > 0).Count() > 0; } }


        [Display(Name = "Tổng SL")]
        [Required(ErrorMessage = "Vui lòng nhập chi tiết phiếu")]
        public virtual decimal TotalQuantityPositive { get; set; }
        [Display(Name = "Tổng SL")]
        [Required(ErrorMessage = "Vui lòng nhập chi tiết phiếu")]
        public virtual decimal TotalQuantityNegative { get; set; }

        protected virtual decimal GetTotalQuantityPositive() { return this.DtoDetails().Select(o => o.QuantityPositive).Sum(); }
        protected virtual decimal GetTotalQuantityNegative() { return this.DtoDetails().Select(o => o.QuantityNegative).Sum(); }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.TotalQuantityPositive != this.GetTotalQuantityPositive()) yield return new ValidationResult("Lỗi tổng số lượng [TotalQuantityPositive]", new[] { "TotalQuantityPositive" });
            if (this.TotalQuantityNegative != this.GetTotalQuantityNegative()) yield return new ValidationResult("Lỗi tổng số lượng [TotalQuantityNegative]", new[] { "TotalQuantityNegative" });
        }


        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            this.DtoDetails().ToList().ForEach(e => { e.WarehouseAdjustmentTypeID = this.WarehouseAdjustmentTypeID; e.NMVNTaskID = this.NMVNTaskID; e.WarehouseID = (int)this.WarehouseID; e.WarehouseReceiptID = this.WarehouseReceiptID; e.CustomerID = this.CustomerID; });
        }

    }

    public interface IWarehouseAdjustmentDTO : IWarehouseAdjustmentPrimitiveDTO, IMaterialItemProduct
    {
        [Display(Name = "Kho xuất")]
        [UIHint("AutoCompletes/WarehouseBase")]
        WarehouseBaseDTO Warehouse { get; set; }
        [Display(Name = "Kho nhập")]
        [UIHint("AutoCompletes/WarehouseBase")]
        WarehouseBaseDTO WarehouseReceipt { get; set; }
        [Display(Name = "Nhân viên kho")]
        [UIHint("AutoCompletes/EmployeeBase")]
        EmployeeBaseDTO Storekeeper { get; set; }
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        CustomerBaseDTO Customer { get; set; }

        List<WarehouseAdjustmentDetailDTO> WarehouseAdjustmentViewDetails { get; set; }

        string ControllerName { get; }

        bool NegativeOnly { get; }
        bool PositiveOnly { get; }
        bool BothAdjustment { get; }
    }

    public class WarehouseAdjustmentDTO<TWAOption> : WarehouseAdjustmentPrimitiveDTO<TWAOption>, IBaseDetailEntity<WarehouseAdjustmentDetailDTO>, IWarehouseAdjustmentDTO
        where TWAOption : IWAOption, new()
    {
        public WarehouseAdjustmentDTO()
        {
            this.WarehouseAdjustmentViewDetails = new List<WarehouseAdjustmentDetailDTO>();
        }

        public override Nullable<int> WarehouseID { get { return (this.Warehouse != null ? this.Warehouse.WarehouseID : null); } }
        public WarehouseBaseDTO Warehouse { get; set; }

        public override Nullable<int> WarehouseReceiptID { get { return (this.WarehouseReceipt != null ? this.WarehouseReceipt.WarehouseID : null); } }
        public WarehouseBaseDTO WarehouseReceipt { get; set; }

        public override int StorekeeperID { get { return (this.Storekeeper != null ? this.Storekeeper.EmployeeID : 0); } }
        public EmployeeBaseDTO Storekeeper { get; set; }

        public override Nullable<int> CustomerID { get { int? customerID = null; if (this.Customer != null) customerID = this.Customer.CustomerID; return customerID; } }
        public CustomerBaseDTO Customer { get; set; }


        public List<WarehouseAdjustmentDetailDTO> WarehouseAdjustmentViewDetails { get; set; }
        public List<WarehouseAdjustmentDetailDTO> ViewDetails { get { return this.WarehouseAdjustmentViewDetails; } set { this.WarehouseAdjustmentViewDetails = value; } }

        public ICollection<WarehouseAdjustmentDetailDTO> GetDetails() { return this.WarehouseAdjustmentViewDetails; }

        protected override IEnumerable<WarehouseAdjustmentDetailDTO> DtoDetails() { return this.WarehouseAdjustmentViewDetails; }



        public string ControllerName { get { return this.NMVNTaskID.ToString() + "s"; } }


        public bool NegativeOnly { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.OtherMaterialIssue || this.NMVNTaskID == GlobalEnums.NmvnTaskID.OtherItemIssue || this.NMVNTaskID == GlobalEnums.NmvnTaskID.OtherProductIssue; } }
        public bool PositiveOnly { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.OtherMaterialReceipt || this.NMVNTaskID == GlobalEnums.NmvnTaskID.OtherItemReceipt || this.NMVNTaskID == GlobalEnums.NmvnTaskID.OtherProductReceipt; } }
        public bool BothAdjustment { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.MaterialAdjustment || this.NMVNTaskID == GlobalEnums.NmvnTaskID.ItemAdjustment || this.NMVNTaskID == GlobalEnums.NmvnTaskID.ProductAdjustment; } }

        public bool IsMaterial { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.OtherMaterialIssue || this.NMVNTaskID == GlobalEnums.NmvnTaskID.OtherMaterialReceipt || this.NMVNTaskID == GlobalEnums.NmvnTaskID.MaterialAdjustment; } }
        public bool IsItem { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.OtherItemIssue || this.NMVNTaskID == GlobalEnums.NmvnTaskID.OtherItemReceipt || this.NMVNTaskID == GlobalEnums.NmvnTaskID.ItemAdjustment; } }
        public bool IsProduct { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.OtherProductIssue || this.NMVNTaskID == GlobalEnums.NmvnTaskID.OtherProductReceipt || this.NMVNTaskID == GlobalEnums.NmvnTaskID.ProductAdjustment; } }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.PositiveOnly && (this.WarehouseAdjustmentTypeID == (int)GlobalEnums.WarehouseAdjustmentTypeID.BothReceiptIssues || this.WarehouseAdjustmentTypeID >= 50)) yield return new ValidationResult("Vui lòng chọn phân loại", new[] { "WarehouseAdjustmentTypeID" });
            if (this.NegativeOnly && (this.WarehouseAdjustmentTypeID == (int)GlobalEnums.WarehouseAdjustmentTypeID.BothReceiptIssues || this.WarehouseAdjustmentTypeID < 50)) yield return new ValidationResult("Vui lòng chọn phân loại", new[] { "WarehouseAdjustmentTypeID" });
            if (this.BothAdjustment && this.WarehouseAdjustmentTypeID != (int)GlobalEnums.WarehouseAdjustmentTypeID.BothReceiptIssues) yield return new ValidationResult("Vui lòng chọn phân loại", new[] { "WarehouseAdjustmentTypeID" });
        }
    }

}
