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
    public interface IGROption { GlobalEnums.NmvnTaskID NMVNTaskID { get; } }

    public class GROptionMaterial : IGROption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.MaterialReceipt; } } }
    public class GROptionItem : IGROption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.ItemReceipt; } } }
    public class GROptionProduct : IGROption { public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.ProductReceipt; } } }

    public interface IGoodsReceiptPrimitiveDTO : IQuantityDTO, IPrimitiveEntity, IPrimitiveDTO, IBaseDTO
    {
        int GoodsReceiptID { get; set; }

        Nullable<int> CustomerID { get; set; }

        Nullable<int> WarehouseID { get; set; }
        Nullable<int> WarehouseIssueID { get; set; }

        int GoodsReceiptTypeID { get; set; }

        Nullable<int> PurchaseRequisitionID { get; set; }
        string PurchaseRequisitionReference { get; set; }
        string PurchaseRequisitionReferences { get; set; }
        string PurchaseRequisitionCode { get; set; }
        string PurchaseRequisitionCodes { get; set; }
        [Display(Name = "Phiếu đặt hàng")]
        string PurchaseRequisitionReferenceNote { get; }
        [Display(Name = "Số đơn hàng")]
        string PurchaseRequisitionCodeNote { get; }
        [Display(Name = "Ngày đặt hàng")]
        Nullable<System.DateTime> PurchaseRequisitionEntryDate { get; set; }



        Nullable<int> WarehouseTransferID { get; set; }
        string WarehouseTransferReference { get; set; }
        string WarehouseTransferReferences { get; set; }
        [Display(Name = "Phiếu VCNB")]
        string WarehouseTransferReferenceNote { get; }
        [Display(Name = "Ngày VCNB")]
        Nullable<System.DateTime> WarehouseTransferEntryDate { get; set; }



        Nullable<int> PlannedOrderID { get; set; }
        [Display(Name = "Số KHSX")]
        string PlannedOrderReference { get; set; }
        [Display(Name = "Số CT")]
        string PlannedOrderCode { get; set; }
        [Display(Name = "Ngày KHSX")]
        Nullable<System.DateTime> PlannedOrderEntryDate { get; set; }

        Nullable<int> WarehouseAdjustmentID { get; set; }

        [Display(Name = "Số đơn hàng")]
        [UIHint("Commons/SOCode")]
        string Code { get; set; }

        [Display(Name = "Mục đích")]
        string Purposes { get; set; }

        int StorekeeperID { get; set; }
    }


    public class GoodsReceiptPrimitiveDTO<TGROption> : QuantityDTO<GoodsReceiptDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
        where TGROption : IGROption, new()
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return new TGROption().NMVNTaskID; } }

        public int GetID() { return this.GoodsReceiptID; }
        public void SetID(int id) { this.GoodsReceiptID = id; }

        public int GoodsReceiptID { get; set; }

        public virtual Nullable<int> CustomerID { get; set; }

        public virtual Nullable<int> WarehouseID { get; set; }
        public virtual Nullable<int> WarehouseIssueID { get; set; }

        public int GoodsReceiptTypeID { get; set; }

        public Nullable<int> PurchaseRequisitionID { get; set; }
        public string PurchaseRequisitionReference { get; set; }
        public string PurchaseRequisitionReferences { get; set; }
        public string PurchaseRequisitionCode { get; set; }
        public string PurchaseRequisitionCodes { get; set; }
        [Display(Name = "Phiếu đặt hàng")]
        public string PurchaseRequisitionReferenceNote { get { return this.PurchaseRequisitionID != null ? this.PurchaseRequisitionReference : (this.PurchaseRequisitionReferences != "" ? this.PurchaseRequisitionReferences : "Giao hàng tổng hợp của nhiều ĐH"); } }
        [Display(Name = "Số đơn hàng")]
        public string PurchaseRequisitionCodeNote { get { return this.PurchaseRequisitionID != null ? this.PurchaseRequisitionCode : (this.PurchaseRequisitionCodes != "" ? this.PurchaseRequisitionCodes : ""); } }
        [Display(Name = "Ngày đặt hàng")]
        public Nullable<System.DateTime> PurchaseRequisitionEntryDate { get; set; }



        public Nullable<int> WarehouseTransferID { get; set; }
        public string WarehouseTransferReference { get; set; }
        public string WarehouseTransferReferences { get; set; }
        [Display(Name = "Phiếu đặt hàng")]
        public string WarehouseTransferReferenceNote { get { return this.WarehouseTransferID != null ? this.WarehouseTransferReference : (this.WarehouseTransferReferences != "" ? this.WarehouseTransferReferences : "Nhập kho tổng hợp của nhiều phiếu VCNB"); } }
        [Display(Name = "Ngày đặt hàng")]
        public Nullable<System.DateTime> WarehouseTransferEntryDate { get; set; }



        public Nullable<int> PlannedOrderID { get; set; }
        [Display(Name = "Số CT đóng gói")]
        public string PlannedOrderReference { get; set; }
        public string PlannedOrderCode { get; set; }
        [Display(Name = "Ngày đóng gói")]
        public Nullable<System.DateTime> PlannedOrderEntryDate { get; set; }

        public Nullable<int> WarehouseAdjustmentID { get; set; }

        [Display(Name = "Số đơn hàng")]
        [UIHint("Commons/SOCode")]
        public string Code { get; set; }

        [Display(Name = "Mục đích")]
        public string Purposes { get; set; }

        public virtual int StorekeeperID { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            string purchaseRequisitionReferences = ""; string purchaseRequisitionCodes = ""; string warehouseTransferReferences = ""; string caption = "";
            this.DtoDetails().ToList().ForEach(e => { e.NMVNTaskID = this.NMVNTaskID; e.GoodsReceiptTypeID = this.GoodsReceiptTypeID; e.CustomerID = this.CustomerID; e.WarehouseID = this.WarehouseID; e.WarehouseIssueID = this.WarehouseIssueID; e.Code = Code; if (this.GoodsReceiptTypeID == (int)GlobalEnums.GoodsReceiptTypeID.PurchaseRequisition && purchaseRequisitionReferences.IndexOf(e.PurchaseRequisitionReference) < 0) purchaseRequisitionReferences = purchaseRequisitionReferences + (purchaseRequisitionReferences != "" ? ", " : "") + e.PurchaseRequisitionReference; if (this.GoodsReceiptTypeID == (int)GlobalEnums.GoodsReceiptTypeID.PurchaseRequisition && e.PurchaseRequisitionCode != null && purchaseRequisitionCodes.IndexOf(e.PurchaseRequisitionCode) < 0) purchaseRequisitionCodes = purchaseRequisitionCodes + (purchaseRequisitionCodes != "" ? ", " : "") + e.PurchaseRequisitionCode; if (this.GoodsReceiptTypeID == (int)GlobalEnums.GoodsReceiptTypeID.WarehouseTransfer && warehouseTransferReferences.IndexOf(e.WarehouseTransferReference) < 0) warehouseTransferReferences = warehouseTransferReferences + (warehouseTransferReferences != "" ? ", " : "") + e.WarehouseTransferReference; if (caption.IndexOf((this.NMVNTaskID == GlobalEnums.NmvnTaskID.ProductReceipt ? e.CommodityName : e.CommodityCode)) < 0) caption = caption + (caption != "" ? ", " : "") + (this.NMVNTaskID == GlobalEnums.NmvnTaskID.ProductReceipt ? e.CommodityName : e.CommodityCode); });
            this.PurchaseRequisitionReferences = purchaseRequisitionReferences; this.PurchaseRequisitionCodes = purchaseRequisitionCodes != "" ? purchaseRequisitionCodes : null; this.WarehouseTransferReferences = warehouseTransferReferences; if (this.GoodsReceiptTypeID == (int)GlobalEnums.GoodsReceiptTypeID.PurchaseRequisition) this.Code = this.PurchaseRequisitionCodes;
            this.Caption = caption != "" ? (caption.Length > 98 ? caption.Substring(0, 95) + "..." : caption) : null;
        }
    }


    public interface IGoodsReceiptDTO : IGoodsReceiptPrimitiveDTO, IMaterialItemProduct
    {
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        CustomerBaseDTO Customer { get; set; }

        [Display(Name = "Kho hàng")]
        [UIHint("AutoCompletes/WarehouseBase")]
        WarehouseBaseDTO Warehouse { get; set; }

        [Display(Name = "Kho xuất VCNB")]
        [UIHint("AutoCompletes/WarehouseBase")]
        WarehouseBaseDTO WarehouseIssue { get; set; }


        [Display(Name = "Nhân viên kho")]
        [UIHint("AutoCompletes/EmployeeBase")]
        EmployeeBaseDTO Storekeeper { get; set; }

        List<GoodsReceiptDetailDTO> GoodsReceiptViewDetails { get; set; }
        List<GoodsReceiptDetailDTO> ViewDetails { get; set; }

        string ControllerName { get; }
    }



    public class GoodsReceiptDTO<TGROption> : GoodsReceiptPrimitiveDTO<TGROption>, IBaseDetailEntity<GoodsReceiptDetailDTO>, IPriceCategory, IWarehouse, IGoodsReceiptDTO
        where TGROption : IGROption, new()
    {
        public GoodsReceiptDTO()
        {
            this.GoodsReceiptViewDetails = new List<GoodsReceiptDetailDTO>();
        }

        public override Nullable<int> CustomerID { get { int? customerID = null; if (this.Customer != null) customerID = this.Customer.CustomerID; return customerID; } }
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override Nullable<int> WarehouseID { get { return (this.Warehouse != null ? this.Warehouse.WarehouseID : null); } }
        [Display(Name = "Kho hàng")]
        [UIHint("AutoCompletes/WarehouseBase")]
        public WarehouseBaseDTO Warehouse { get; set; }

        public override Nullable<int> WarehouseIssueID { get { return (this.WarehouseIssue != null ? this.WarehouseIssue.WarehouseID : null); } }
        [Display(Name = "Kho hàng")]
        [UIHint("AutoCompletes/WarehouseBase")]
        public WarehouseBaseDTO WarehouseIssue { get; set; }


        public override int StorekeeperID { get { return (this.Storekeeper != null ? this.Storekeeper.EmployeeID : 0); } }
        [Display(Name = "Nhân viên kho")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO Storekeeper { get; set; }

        public List<GoodsReceiptDetailDTO> GoodsReceiptViewDetails { get; set; }
        public List<GoodsReceiptDetailDTO> ViewDetails { get { return this.GoodsReceiptViewDetails; } set { this.GoodsReceiptViewDetails = value; } }

        public ICollection<GoodsReceiptDetailDTO> GetDetails() { return this.GoodsReceiptViewDetails; }

        protected override IEnumerable<GoodsReceiptDetailDTO> DtoDetails() { return this.GoodsReceiptViewDetails; }





        public string ControllerName { get { return this.NMVNTaskID.ToString() + "s"; } }


        public bool IsMaterial { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.MaterialReceipt; } }
        public bool IsItem { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.ItemReceipt; } }
        public bool IsProduct { get { return this.NMVNTaskID == GlobalEnums.NmvnTaskID.ProductReceipt; } }

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

        public virtual Nullable<int> TradePromotionID { get; set; }
        [Display(Name = "Addressee")]
        public string ShippingAddress { get; set; }
        [Display(Name = "Addressee")]
        public string Addressee { get; set; }
        #endregion implement ISearchCustomer only
    }
}

