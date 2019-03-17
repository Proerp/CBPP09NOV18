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
    public interface IFinishedHandoverOption { GlobalEnums.NmvnTaskID NMVNTaskID { get; } }

    public class FinishedItemHandoverOption : IFinishedHandoverOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.FinishedItemHandover; } } }
    public class FinishedProductHandoverOption : IFinishedHandoverOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.FinishedProductHandover; } } }

    public interface IFinishedHandoverPrimitiveDTO : IQuantityDTO, IPrimitiveEntity, IPrimitiveDTO, IBaseDTO
    {
        int FinishedHandoverID { get; set; }


        int WorkshiftID { get; set; }
        [Display(Name = "Ca sản xuất")]
        string WorkshiftCode { get; set; }
        [Display(Name = "Ngày sản xuất")]
        DateTime WorkshiftEntryDate { get; set; }

        Nullable<int> PlannedOrderID { get; set; }
        [Display(Name = "KHSX")]
        string PlannedOrderCode { get; set; }
        [Display(Name = "Ngày KHSX")]
        Nullable<DateTime> PlannedOrderEntryDate { get; set; }

        Nullable<int> CustomerID { get; set; }

        int FinishedLeaderID { get; set; }
        int StorekeeperID { get; set; }
    }

    public class FinishedHandoverPrimitiveDTO<TFinishedHandoverOption> : QuantityDTO<FinishedHandoverDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
        where TFinishedHandoverOption : IFinishedHandoverOption, new()
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return new TFinishedHandoverOption().NMVNTaskID; } }

        public int GetID() { return this.FinishedHandoverID; }
        public void SetID(int id) { this.FinishedHandoverID = id; }

        public int FinishedHandoverID { get; set; }


        public virtual int WorkshiftID { get; set; }
        [Display(Name = "Ca sản xuất")]
        public string WorkshiftCode { get; set; }
        [Display(Name = "Ngày sản xuất")]
        public DateTime WorkshiftEntryDate { get; set; }

        public Nullable<int> PlannedOrderID { get; set; }
        [Display(Name = "KHSX")]
        public string PlannedOrderCode { get; set; }
        [Display(Name = "Ngày KHSX")]
        public Nullable<DateTime> PlannedOrderEntryDate { get; set; }

        public virtual Nullable<int> CustomerID { get; set; }

        public virtual int FinishedLeaderID { get; set; }
        public virtual int StorekeeperID { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            string caption = "";
            this.DtoDetails().ToList().ForEach(e => { if (caption.IndexOf(e.CommodityName) < 0) caption = caption + (caption != "" ? ", " : "") + e.CommodityName; });
            this.Caption = caption != "" ? (caption.Length > 98 ? caption.Substring(0, 95) + "..." : caption) : null;
        }
    }

    public interface IFinishedHandoverDTO : IFinishedHandoverPrimitiveDTO
    {
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        CustomerBaseDTO Customer { get; set; }

        [Display(Name = "Nhân viên kho")]
        [UIHint("AutoCompletes/EmployeeBase")]
        EmployeeBaseDTO Storekeeper { get; set; }

        [Display(Name = "Tổ trưởng đóng gói")]
        [UIHint("AutoCompletes/EmployeeBase")]
        EmployeeBaseDTO FinishedLeader { get; set; }

        List<FinishedHandoverDetailDTO> FinishedHandoverViewDetails { get; set; }
        List<FinishedHandoverDetailDTO> ViewDetails { get; set; }

        string ControllerName { get; }

        bool IsItem { get; }
        bool IsProduct { get; }
    }

    public class FinishedHandoverDTO<TFinishedHandoverOption> : FinishedHandoverPrimitiveDTO<TFinishedHandoverOption>, IBaseDetailEntity<FinishedHandoverDetailDTO>, IFinishedHandoverDTO
        where TFinishedHandoverOption : IFinishedHandoverOption, new()
    {
        public FinishedHandoverDTO()
        {
            this.FinishedHandoverViewDetails = new List<FinishedHandoverDetailDTO>();
        }
        
        public override Nullable<int> CustomerID { get { return (this.Customer != null ? (this.Customer.CustomerID > 0 ? (Nullable<int>)this.Customer.CustomerID : null) : null); } }
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override int StorekeeperID { get { return (this.Storekeeper != null ? this.Storekeeper.EmployeeID : 0); } }
        [Display(Name = "Nhân viên kho")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO Storekeeper { get; set; }

        public override int FinishedLeaderID { get { return (this.FinishedLeader != null ? this.FinishedLeader.EmployeeID : 0); } }
        [Display(Name = "Tổ trưởng ĐG")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO FinishedLeader { get; set; }

        public List<FinishedHandoverDetailDTO> FinishedHandoverViewDetails { get; set; }
        public List<FinishedHandoverDetailDTO> ViewDetails { get { return this.FinishedHandoverViewDetails; } set { this.FinishedHandoverViewDetails = value; } }

        public ICollection<FinishedHandoverDetailDTO> GetDetails() { return this.FinishedHandoverViewDetails; }

        protected override IEnumerable<FinishedHandoverDetailDTO> DtoDetails() { return this.FinishedHandoverViewDetails; }

        public string ControllerName { get { return this.NMVNTaskID.ToString() + "s"; } }

        public bool IsItem { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.FinishedItemHandover; } }
        public bool IsProduct { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.FinishedProductHandover; } }
    }


}
