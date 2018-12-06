using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;
using TotalDTO.Helpers;
using TotalDTO.Commons;

namespace TotalDTO.Purchases
{
    public class GoodsArrivalPrimitiveDTO : QuantityDTO<GoodsArrivalDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.GoodsArrival; } }

        public int GetID() { return this.GoodsArrivalID; }
        public void SetID(int id) { this.GoodsArrivalID = id; }

        public int GoodsArrivalID { get; set; }

        public virtual int CustomerID { get; set; }
        public virtual int TransporterID { get; set; }

        public virtual Nullable<int> WarehouseID { get; set; }

        public bool HasPurchaseOrder { get; set; }
        public Nullable<int> PurchaseOrderID { get; set; }
        public string PurchaseOrderReference { get; set; }
        public string PurchaseOrderReferences { get; set; }
        public string PurchaseOrderCode { get; set; }
        public string PurchaseOrderCodes { get; set; }
        [Display(Name = "Phiếu đặt hàng")]
        public string PurchaseOrderReferenceNote { get { return this.PurchaseOrderID != null ? this.PurchaseOrderReference : (this.PurchaseOrderReferences != "" ? this.PurchaseOrderReferences : "Giao hàng tổng hợp của nhiều ĐH"); } }
        [Display(Name = "Số đơn hàng")]
        public string PurchaseOrderCodeNote { get { return this.PurchaseOrderID != null ? this.PurchaseOrderCode : (this.PurchaseOrderCodes != "" ? this.PurchaseOrderCodes : ""); } }
        [Display(Name = "Ngày đặt hàng")]
        public Nullable<System.DateTime> PurchaseOrderEntryDate { get; set; }

        [Display(Name = "Số đơn hàng")]
        [UIHint("Commons/SOCode")]
        public string Code { get; set; }
        [Display(Name = "Chứng từ khuyến mãi")]
        public string PromotionVouchers { get; set; }


        public virtual int SalespersonID { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            string purchaseOrderReferences = ""; string purchaseOrderCodes = ""; int serialID = 0;
            this.DtoDetails().ToList().ForEach(e => { e.CustomerID = this.CustomerID; e.TransporterID = this.TransporterID; e.WarehouseID = this.WarehouseID; e.SalespersonID = this.SalespersonID; e.SerialID = ++serialID; if (this.HasPurchaseOrder && purchaseOrderReferences.IndexOf(e.PurchaseOrderReference) < 0) purchaseOrderReferences = purchaseOrderReferences + (purchaseOrderReferences != "" ? ", " : "") + e.PurchaseOrderReference; if (this.HasPurchaseOrder && purchaseOrderCodes.IndexOf(e.PurchaseOrderCode) < 0) purchaseOrderCodes = purchaseOrderCodes + (purchaseOrderCodes != "" ? ", " : "") + e.PurchaseOrderCode; });
            this.PurchaseOrderReferences = purchaseOrderReferences; this.PurchaseOrderCodes = purchaseOrderCodes != "" ? purchaseOrderCodes : null; if (this.HasPurchaseOrder) this.Code = this.PurchaseOrderCodes;
        }
    }


    public class GoodsArrivalDTO : GoodsArrivalPrimitiveDTO, IBaseDetailEntity<GoodsArrivalDetailDTO>
    {
        public GoodsArrivalDTO()
        {
            this.GoodsArrivalViewDetails = new List<GoodsArrivalDetailDTO>();
        }

        public override int CustomerID { get { return (this.Customer != null ? this.Customer.CustomerID : 0); } }
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override int TransporterID { get { return (this.Transporter != null ? this.Transporter.CustomerID : 0); } }
        [Display(Name = "Đơn vị, người nhận hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Transporter { get; set; }

        public override Nullable<int> WarehouseID { get { return (this.Warehouse != null ? this.Warehouse.WarehouseID : null); } }
        [Display(Name = "Kho hàng")]
        [UIHint("AutoCompletes/WarehouseBase")]
        public WarehouseBaseDTO Warehouse { get; set; }


        public override int SalespersonID { get { return (this.Salesperson != null ? this.Salesperson.EmployeeID : 0); } }
        [Display(Name = "Nhân viên tiếp thị")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO Salesperson { get; set; }


        public List<GoodsArrivalDetailDTO> GoodsArrivalViewDetails { get; set; }
        public List<GoodsArrivalDetailDTO> ViewDetails { get { return this.GoodsArrivalViewDetails; } set { this.GoodsArrivalViewDetails = value; } }

        public ICollection<GoodsArrivalDetailDTO> GetDetails() { return this.GoodsArrivalViewDetails; }

        protected override IEnumerable<GoodsArrivalDetailDTO> DtoDetails() { return this.GoodsArrivalViewDetails; }
    }
}
