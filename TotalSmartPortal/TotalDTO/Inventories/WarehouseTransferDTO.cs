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
    public interface IWTOption { GlobalEnums.NmvnTaskID NMVNTaskID { get; } }

    public class WTOptionMaterial : IWTOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.MaterialTransfer; } } }
    public class WTOptionItem : IWTOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.ItemTransfer; } } }
    public class WTOptionProduct : IWTOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.ProductTransfer; } } }

    public interface IWarehouseTransferPrimitiveDTO : IQuantityDTO, IPrimitiveEntity, IPrimitiveDTO, IBaseDTO
    {
        int WarehouseTransferID { get; set; }

        bool OneStep { get; set; }
        bool HasTransferOrder { get; set; }
        int? BlendingInstructionID { get; set; }

        int ShiftID { get; set; }
        int WorkshiftID { get; set; }

        Nullable<int> WarehouseID { get; set; }
        Nullable<int> LocationIssuedID { get; set; }
        Nullable<int> WarehouseReceiptID { get; set; }
        Nullable<int> LocationReceiptID { get; set; }

        Nullable<int> BinLocation_WarehouseID { get; set; }

        Nullable<int> TransferOrderID { get; set; }
        [Display(Name = "Lệnh VCNB")]
        string TransferOrderReference { get; set; }
        [Display(Name = "Ngày lệnh VCNB")]
        Nullable<System.DateTime> TransferOrderEntryDate { get; set; }

        [Display(Name = "Mục đích điều chuyển")]
        string WarehouseTransferJobs { get; set; }
        int StorekeeperID { get; set; }
    }

    public class WarehouseTransferPrimitiveDTO<TWTOption> : QuantityDTO<WarehouseTransferDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
        where TWTOption : IWTOption, new()
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return new TWTOption().NMVNTaskID; } }

        public int GetID() { return this.WarehouseTransferID; }
        public void SetID(int id) { this.WarehouseTransferID = id; }

        public int WarehouseTransferID { get; set; }

        public bool OneStep { get; set; }
        public bool HasTransferOrder { get; set; }
        public int? BlendingInstructionID { get; set; }

        public int ShiftID { get; set; }
        public int WorkshiftID { get; set; }

        public virtual Nullable<int> WarehouseID { get; set; }
        public virtual Nullable<int> LocationIssuedID { get; set; }
        public virtual Nullable<int> WarehouseReceiptID { get; set; }
        public virtual Nullable<int> LocationReceiptID { get; set; }

        public virtual Nullable<int> BinLocation_WarehouseID { get; set; }

        public virtual Nullable<int> TransferOrderID { get; set; }
        [Display(Name = "Lệnh VCNB")]
        public string TransferOrderReference { get; set; }
        [Display(Name = "Ngày lệnh VCNB")]
        public Nullable<System.DateTime> TransferOrderEntryDate { get; set; }

        public string WarehouseTransferJobs { get; set; }

        public virtual int StorekeeperID { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            string caption = "";
            this.DtoDetails().ToList().ForEach(e => { e.NMVNTaskID = this.NMVNTaskID; e.ShiftID = this.ShiftID; e.WorkshiftID = this.WorkshiftID; e.WarehouseID = (int)this.WarehouseID; e.WarehouseReceiptID = this.WarehouseReceiptID; e.LocationIssuedID = this.LocationIssuedID; e.LocationReceiptID = this.LocationReceiptID; e.HasTransferOrder = this.HasTransferOrder; e.OneStep = this.OneStep; if (caption.IndexOf(e.CommodityCode) < 0) caption = caption + (caption != "" ? ", " : "") + e.CommodityCode; });
            this.Caption = caption != "" ? (caption.Length > 98 ? caption.Substring(0, 95) + "..." : caption) : null;
        }

    }

    public interface IWarehouseTransferDTO : IWarehouseTransferPrimitiveDTO, IMaterialItemProduct
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

        List<WarehouseTransferDetailDTO> ViewDetails { get; set; }

        [UIHint("AutoCompletes/VoidType")]
        VoidTypeBaseDTO VoidType { get; set; }


        [Display(Name = "Mã số máy")]
        string UserFirstName { get; set; }
        [Display(Name = "Người thực hiện")]
        string UserLastName { get; set; }

        string WarehouseTransferBriefs { get; }
        bool IsSameWarehouse { get; }

        string ControllerName { get; }
        string ControllerTransferOrder { get; }
    }

    public class WarehouseTransferDTO<TWTOption> : WarehouseTransferPrimitiveDTO<TWTOption>, IBaseDetailEntity<WarehouseTransferDetailDTO>, IWarehouseTransferDTO
        where TWTOption : IWTOption, new()
    {
        public WarehouseTransferDTO()
        {
            this.ViewDetails = new List<WarehouseTransferDetailDTO>();

            this.OneStep = GlobalEnums.CBPP || GlobalEnums.DMC;
            if (GlobalEnums.CBPP) { this.Storekeeper = new EmployeeBaseDTO() { EmployeeID = 1, PreparedPersonID = 1, Name = "NONAME" }; }
        }

        public override Nullable<int> WarehouseID { get { return (this.Warehouse != null ? this.Warehouse.WarehouseID : null); } }
        public override Nullable<int> LocationIssuedID { get { return (this.Warehouse != null ? (Nullable<int>)this.Warehouse.LocationID : null); } }
        public WarehouseBaseDTO Warehouse { get; set; }

        public override Nullable<int> WarehouseReceiptID { get { return (this.WarehouseReceipt != null ? this.WarehouseReceipt.WarehouseID : null); } }
        public override Nullable<int> LocationReceiptID { get { return (this.WarehouseReceipt != null ? (Nullable<int>)this.WarehouseReceipt.LocationID : null); } }
        public WarehouseBaseDTO WarehouseReceipt { get; set; }


        public override Nullable<int> BinLocation_WarehouseID { get { return (this.WarehouseReceipt != null ? this.WarehouseReceipt.WarehouseID : null); } set { } }


        public override int StorekeeperID { get { return (this.Storekeeper != null ? this.Storekeeper.EmployeeID : 0); } }
        public EmployeeBaseDTO Storekeeper { get; set; }

        public override Nullable<int> VoidTypeID { get { return (this.VoidType != null ? this.VoidType.VoidTypeID : null); } }
        public VoidTypeBaseDTO VoidType { get; set; }


        [Display(Name = "Mã số máy")]
        public string UserFirstName { get; set; }
        [Display(Name = "Người thực hiện")]
        public string UserLastName { get; set; }


        public List<WarehouseTransferDetailDTO> ViewDetails { get; set; }

        public ICollection<WarehouseTransferDetailDTO> GetDetails() { return this.ViewDetails; }

        protected override IEnumerable<WarehouseTransferDetailDTO> DtoDetails() { return this.ViewDetails; }


        [Display(Name = "Lệnh VCNB")]
        public string WarehouseTransferBriefs { get { return (this.TransferOrderID != null ? this.TransferOrderReference + " [" + ((DateTime)this.TransferOrderEntryDate).ToShortDateString() + "]" : (this.IsSameWarehouse ? "Chuyển vị trí tại kho " + (this.Warehouse != null ? this.Warehouse.Name : "") + " (không có lệnh)" : (GlobalEnums.CBPP ? "Chuyển kho nội bộ" : "VCNB không có lệnh điều chuyển"))); } }

        public string ControllerName { get { return this.NMVNTaskID.ToString() + "s"; } }
        public string ControllerTransferOrder { get { return this.IsMaterial ? "MaterialTransferOrders" : (this.IsItem ? "ItemTransferOrders" : "ProductTransferOrders"); } }

        public bool IsSameWarehouse { get { return this.WarehouseID == this.WarehouseReceiptID; } }

        public bool IsMaterial { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.MaterialTransfer; } }
        public bool IsItem { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.ItemTransfer; } }
        public bool IsProduct { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.ProductTransfer; } }
    }

}
