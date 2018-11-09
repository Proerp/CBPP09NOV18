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
    public class ProductionOrderPrimitiveDTO : QuantityDTO<ProductionOrderDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.ProductionOrder; } }

        public int GetID() { return this.ProductionOrderID; }
        public void SetID(int id) { this.ProductionOrderID = id; }

        public int ProductionOrderID { get; set; }

        public virtual Nullable<int> CustomerID { get; set; }

        public Nullable<int> PlannedOrderID { get; set; }
        public string PlannedOrderReference { get; set; }
        public string PlannedOrderReferences { get; set; }
        public string PlannedOrderCode { get; set; }
        public string PlannedOrderCodes { get; set; }
        [Display(Name = "Phiếu đặt hàng")]
        public string PlannedOrderReferenceNote { get { return this.PlannedOrderID != null ? this.PlannedOrderReference : (this.PlannedOrderReferences != "" ? this.PlannedOrderReferences : "Giao hàng tổng hợp của nhiều ĐH"); } }
        [Display(Name = "Số đơn hàng")]
        public string PlannedOrderCodeNote { get { return this.PlannedOrderID != null ? this.PlannedOrderCode : (this.PlannedOrderCodes != "" ? this.PlannedOrderCodes : ""); } }
        [Display(Name = "Ngày đặt hàng")]
        public Nullable<System.DateTime> PlannedOrderEntryDate { get; set; }
        [Display(Name = "Ngày giao hàng")]
        public Nullable<System.DateTime> PlannedOrderDeliveryDate { get; set; }

        [Display(Name = "Chứng từ")]
        public string Code { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            string plannedOrderReferences = ""; string plannedOrderCodes = ""; string caption = "";
            this.DtoDetails().ToList().ForEach(e => { if (plannedOrderReferences.IndexOf(e.FirmOrderReference) < 0) plannedOrderReferences = plannedOrderReferences + (plannedOrderReferences != "" ? ", " : "") + e.FirmOrderReference; if (e.FirmOrderCode != null && plannedOrderCodes.IndexOf(e.FirmOrderCode) < 0) plannedOrderCodes = plannedOrderCodes + (plannedOrderCodes != "" ? ", " : "") + e.FirmOrderCode; if (e.Specs != null && caption.IndexOf(e.Specs) < 0) caption = caption + (caption != "" ? ", " : "") + e.Specs; });
            this.PlannedOrderReferences = plannedOrderReferences; this.PlannedOrderCodes = plannedOrderCodes != "" ? plannedOrderCodes : null; this.Caption = caption != "" ? (caption.Length > 98 ? caption.Substring(0, 95) : caption) : null;
        }
    }

    public class ProductionOrderDTO : ProductionOrderPrimitiveDTO, IBaseDetailEntity<ProductionOrderDetailDTO>, ISearchCustomer, IPriceCategory
    {
        public ProductionOrderDTO()
        {
            this.ProductionOrderViewDetails = new List<ProductionOrderDetailDTO>();
        }

        //public override int CustomerID { get { return (this.Customer != null ? this.Customer.CustomerID : 0); } }
        public override Nullable<int> CustomerID { get { return (this.Customer != null ? (this.Customer.CustomerID > 0 ? (Nullable<int>)this.Customer.CustomerID : null) : null); } }
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override Nullable<int> VoidTypeID { get { return (this.VoidType != null ? this.VoidType.VoidTypeID : null); } }
        [UIHint("AutoCompletes/VoidType")]
        public VoidTypeBaseDTO VoidType { get; set; }

        public List<ProductionOrderDetailDTO> ProductionOrderViewDetails { get; set; }
        public List<ProductionOrderDetailDTO> ViewDetails { get { return this.ProductionOrderViewDetails; } set { this.ProductionOrderViewDetails = value; } }

        public ICollection<ProductionOrderDetailDTO> GetDetails() { return this.ProductionOrderViewDetails; }

        protected override IEnumerable<ProductionOrderDetailDTO> DtoDetails() { return this.ProductionOrderViewDetails; }

        public override void PrepareVoidDetail(int? detailID)
        {
            this.ViewDetails.RemoveAll(w => w.ProductionOrderDetailID != detailID);
            if (this.ViewDetails.Count() > 0)
                this.VoidType = new VoidTypeBaseDTO() { VoidTypeID = this.ViewDetails[0].VoidTypeID, Code = this.ViewDetails[0].VoidTypeCode, Name = this.ViewDetails[0].VoidTypeName, VoidClassID = this.ViewDetails[0].VoidClassID };
            base.PrepareVoidDetail(detailID);
        }




        #region implement ISearchCustomer only

        [Display(Name = "PriceCategoryID")]
        public int PriceCategoryID { get; set; }
        [Display(Name = "PriceCategoryCode")]
        public string PriceCategoryCode { get; set; }

        [Display(Name = "Đơn vị, người nhận hàng")]
        public int ReceiverID { get { return (this.Receiver != null ? this.Receiver.CustomerID : 0); } }
        [Display(Name = "Đơn vị, người nhận hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Receiver { get; set; }

        public WarehouseBaseDTO Warehouse { get; set; }

        public virtual Nullable<int> TradePromotionID { get; set; }
        [Display(Name = "Addressee")]
        public string ShippingAddress { get; set; }
        [Display(Name = "Addressee")]
        public string Addressee { get; set; }
        #endregion implement ISearchCustomer only
    }

}
