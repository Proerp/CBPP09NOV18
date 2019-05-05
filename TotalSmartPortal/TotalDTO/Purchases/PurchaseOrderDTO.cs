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

namespace TotalDTO.Purchases
{
    public interface IPurchaseOption { GlobalEnums.NmvnTaskID NMVNTaskID { get; } }

    public class PurchaseOptionMaterial : IPurchaseOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.PurchaseMaterial; } } }
    public class PurchaseOptionItem : IPurchaseOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.PurchaseItem; } } }
    public class PurchaseOptionProduct : IPurchaseOption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.PurchaseProduct; } } }

    public interface IPurchaseOrderPrimitiveDTO : IQuantityDTO, IPrimitiveEntity, IPrimitiveDTO, IBaseDTO
    {
        int PurchaseOrderID { get; set; }

        virtual Nullable<int> CustomerID { get; set; }
        virtual Nullable<int> TransporterID { get; set; }

        [Display(Name = "Số PO")]
        string Code { get; set; }

        [Display(Name = "Mục đích")]
        string Purposes { get; set; }

        [Display(Name = "Ngày PO")]
        Nullable<System.DateTime> VoucherDate { get; set; }
        [Display(Name = "Ngày dự kiến giao hàng")]
        Nullable<System.DateTime> DeliveryDate { get; set; }
    }

    public class PurchaseOrderPrimitiveDTO : QuantityDTO<PurchaseOrderDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.PurchaseItem; } }

        public int GetID() { return this.PurchaseOrderID; }
        public void SetID(int id) { this.PurchaseOrderID = id; }

        public int PurchaseOrderID { get; set; }

        public virtual Nullable<int> CustomerID { get; set; }
        public virtual Nullable<int> TransporterID { get; set; }

        [Display(Name = "Số PO")]
        public string Code { get; set; }

        [Display(Name = "Mục đích")]
        public string Purposes { get; set; }

        [Display(Name = "Ngày PO")]
        public Nullable<System.DateTime> VoucherDate { get; set; }
        [Display(Name = "Ngày dự kiến giao hàng")]
        public Nullable<System.DateTime> DeliveryDate { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            string caption = "";
            this.DtoDetails().ToList().ForEach(e => { e.CustomerID = (int)this.CustomerID; e.TransporterID = (int)this.TransporterID; if (caption.IndexOf(e.CommodityCode) < 0) caption = caption + (caption != "" ? ", " : "") + e.CommodityCode; });
            this.Caption = caption != "" ? (caption.Length > 98 ? caption.Substring(0, 95) + "..." : caption) : null;
        }
    }

    public interface IPurchaseOrderDTO : IPurchaseOrderPrimitiveDTO
    {
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        CustomerBaseDTO Customer { get; set; }

        [Display(Name = "Đơn vị vận chuyển")]
        [UIHint("Commons/CustomerBase")]
        CustomerBaseDTO Transporter { get; set; }

        [UIHint("AutoCompletes/VoidType")]
        VoidTypeBaseDTO VoidType { get; set; }

        List<PurchaseOrderDetailDTO> PurchaseOrderViewDetails { get; set; }
        List<PurchaseOrderDetailDTO> ViewDetails { get; set; }

        string ControllerName { get; }

        bool IsItem { get; }
        bool IsProduct { get; }
    }

    public class PurchaseOrderDTO : PurchaseOrderPrimitiveDTO, IBaseDetailEntity<PurchaseOrderDetailDTO>
    {
        public PurchaseOrderDTO()
        {
            this.PurchaseOrderViewDetails = new List<PurchaseOrderDetailDTO>();
        }

        public override Nullable<int> CustomerID { get { return (this.Customer != null ? (this.Customer.CustomerID > 0 ? (Nullable<int>)this.Customer.CustomerID : null) : null); } }
        [Display(Name = "Nhà cung cấp")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override Nullable<int> TransporterID { get { return (this.Transporter != null ? (this.Transporter.CustomerID > 0 ? (Nullable<int>)this.Transporter.CustomerID : null) : null); } }
        [Display(Name = "Đơn vị vận chuyển")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Transporter { get; set; }

        public override Nullable<int> VoidTypeID { get { return (this.VoidType != null ? this.VoidType.VoidTypeID : null); } }
        [UIHint("AutoCompletes/VoidType")]
        public VoidTypeBaseDTO VoidType { get; set; }

        public List<PurchaseOrderDetailDTO> PurchaseOrderViewDetails { get; set; }
        public List<PurchaseOrderDetailDTO> ViewDetails { get { return this.PurchaseOrderViewDetails; } set { this.PurchaseOrderViewDetails = value; } }

        public ICollection<PurchaseOrderDetailDTO> GetDetails() { return this.PurchaseOrderViewDetails; }

        protected override IEnumerable<PurchaseOrderDetailDTO> DtoDetails() { return this.PurchaseOrderViewDetails; }

        public override void PrepareVoidDetail(int? detailID)
        {
            this.ViewDetails.RemoveAll(w => w.PurchaseOrderDetailID != detailID);
            if (this.ViewDetails.Count() > 0)
                this.VoidType = new VoidTypeBaseDTO() { VoidTypeID = this.ViewDetails[0].VoidTypeID, Code = this.ViewDetails[0].VoidTypeCode, Name = this.ViewDetails[0].VoidTypeName, VoidClassID = this.ViewDetails[0].VoidClassID };
            base.PrepareVoidDetail(detailID);
        }


        public string ControllerName { get { return this.NMVNTaskID.ToString() + "s"; } }

        public bool IsMaterial { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.PurchaseMaterial; } }
        public bool IsItem { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.PurchaseItem; } }
        public bool IsProduct { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.PurchaseProduct; } }
    }

}
