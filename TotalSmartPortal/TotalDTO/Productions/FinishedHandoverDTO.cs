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
    public class FinishedHandoverPrimitiveDTO : QuantityDTO<FinishedHandoverDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.FinishedHandover; } }

        public int GetID() { return this.FinishedHandoverID; }
        public void SetID(int id) { this.FinishedHandoverID = id; }

        public int FinishedHandoverID { get; set; }

        public Nullable<int> PlannedOrderID { get; set; }       

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

    public class FinishedHandoverDTO : FinishedHandoverPrimitiveDTO, IBaseDetailEntity<FinishedHandoverDetailDTO>
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
    }
}
