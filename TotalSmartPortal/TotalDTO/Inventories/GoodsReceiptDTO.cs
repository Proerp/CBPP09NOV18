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

        Nullable<int> BinLocation_WarehouseID { get; set; }

        int ShiftID { get; set; }
        int WorkshiftID { get; set; }

        bool OneStep { get; set; }
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



        Nullable<int> GoodsArrivalID { get; set; }
        string GoodsArrivalReference { get; set; }
        string GoodsArrivalReferences { get; set; }
        string GoodsArrivalCode { get; set; }
        string GoodsArrivalCodes { get; set; }
        [Display(Name = "Phiếu đặt hàng")]
        string GoodsArrivalReferenceNote { get; }
        [Display(Name = "Số đơn hàng")]
        string GoodsArrivalCodeNote { get; }
        [Display(Name = "Ngày đặt hàng")]
        Nullable<System.DateTime> GoodsArrivalEntryDate { get; set; }


        string GoodsArrivalPurchaseOrderCodes { get; set; }
        Nullable<System.DateTime> GoodsArrivalPurchaseOrderVoucherDate { get; set; }
        string GoodsArrivalCustomsDeclaration { get; set; }
        Nullable<System.DateTime> GoodsArrivalCustomsDeclarationDate { get; set; }

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

        
        Nullable<int> RecyclateID { get; set; }
        [Display(Name = "Phiếu giao phế phẩm")]
        string RecyclateReference { get; set; }
        [Display(Name = "Ngày giao")]
        Nullable<System.DateTime> RecyclateEntryDate { get; set; }


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

        public virtual Nullable<int> BinLocation_WarehouseID { get; set; }

        public int ShiftID { get; set; }
        public int WorkshiftID { get; set; }

        public bool OneStep { get; set; }
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




        public Nullable<int> GoodsArrivalID { get; set; }
        public string GoodsArrivalReference { get; set; }
        public string GoodsArrivalReferences { get; set; }
        public string GoodsArrivalCode { get; set; }
        public string GoodsArrivalCodes { get; set; }
        [Display(Name = "Phiếu đặt hàng")]
        public string GoodsArrivalReferenceNote { get { return this.GoodsArrivalID != null ? this.GoodsArrivalReference : (this.GoodsArrivalReferences != "" ? this.GoodsArrivalReferences : "Giao hàng tổng hợp của nhiều ĐH"); } }
        [Display(Name = "Số đơn hàng")]
        public string GoodsArrivalCodeNote { get { return this.GoodsArrivalID != null ? this.GoodsArrivalCode : (this.GoodsArrivalCodes != "" ? this.GoodsArrivalCodes : ""); } }
        [Display(Name = "Ngày đặt hàng")]
        public Nullable<System.DateTime> GoodsArrivalEntryDate { get; set; }

        public string GoodsArrivalPurchaseOrderCodes { get; set; }
        public Nullable<System.DateTime> GoodsArrivalPurchaseOrderVoucherDate { get; set; }
        public string GoodsArrivalCustomsDeclaration { get; set; }
        public Nullable<System.DateTime> GoodsArrivalCustomsDeclarationDate { get; set; }

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



        public Nullable<int> RecyclateID { get; set; }
        [Display(Name = "Phiếu giao phế phẩm")]
        public string RecyclateReference { get; set; }
        [Display(Name = "Ngày giao")]
        public Nullable<System.DateTime> RecyclateEntryDate { get; set; }



        public Nullable<int> WarehouseAdjustmentID { get; set; }

        [Display(Name = "Số đơn hàng")]
        [UIHint("Commons/SOCode")]
        public string Code { get; set; }

        public string Purposes { get; set; }
        public string PrimaryReferences { get; set; }

        public virtual int StorekeeperID { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            string purchaseRequisitionReferences = ""; string purchaseRequisitionCodes = ""; string goodsArrivalReferences = ""; string goodsArrivalCodes = ""; string warehouseTransferReferences = ""; string purposes = ""; string primaryReferences = ""; string caption = "";
            this.DtoDetails().ToList().ForEach(e =>
            {
                e.NMVNTaskID = this.NMVNTaskID; e.GoodsReceiptTypeID = this.GoodsReceiptTypeID; e.ShiftID = this.ShiftID; e.WorkshiftID = this.WorkshiftID; e.CustomerID = this.CustomerID; e.WarehouseID = this.WarehouseID; e.WarehouseIssueID = this.WarehouseIssueID; e.Code = Code;
                if (this.GoodsReceiptTypeID == (int)GlobalEnums.GoodsReceiptTypeID.PurchaseRequisition && purchaseRequisitionReferences.IndexOf(e.PurchaseRequisitionReference) < 0) purchaseRequisitionReferences = purchaseRequisitionReferences + (purchaseRequisitionReferences != "" ? ", " : "") + e.PurchaseRequisitionReference;
                if (this.GoodsReceiptTypeID == (int)GlobalEnums.GoodsReceiptTypeID.PurchaseRequisition && e.PurchaseRequisitionCode != null && purchaseRequisitionCodes.IndexOf(e.PurchaseRequisitionCode) < 0) purchaseRequisitionCodes = purchaseRequisitionCodes + (purchaseRequisitionCodes != "" ? ", " : "") + e.PurchaseRequisitionCode;
                if (this.GoodsReceiptTypeID == (int)GlobalEnums.GoodsReceiptTypeID.GoodsArrival && goodsArrivalReferences.IndexOf(e.GoodsArrivalReference) < 0) goodsArrivalReferences = goodsArrivalReferences + (goodsArrivalReferences != "" ? ", " : "") + e.GoodsArrivalReference;
                if (this.GoodsReceiptTypeID == (int)GlobalEnums.GoodsReceiptTypeID.GoodsArrival && e.GoodsArrivalCode != null && goodsArrivalCodes.IndexOf(e.GoodsArrivalCode) < 0) goodsArrivalCodes = goodsArrivalCodes + (goodsArrivalCodes != "" ? ", " : "") + e.GoodsArrivalCode;
                if (this.GoodsReceiptTypeID == (int)GlobalEnums.GoodsReceiptTypeID.WarehouseTransfer && warehouseTransferReferences.IndexOf(e.WarehouseTransferReference) < 0) warehouseTransferReferences = warehouseTransferReferences + (warehouseTransferReferences != "" ? ", " : "") + e.WarehouseTransferReference;
                if (this.GoodsReceiptTypeID == (int)GlobalEnums.GoodsReceiptTypeID.GoodsArrival && e.CustomerCode != null && purposes.IndexOf(e.CustomerCode) < 0) purposes = purposes + (purposes != "" ? ", " : "") + e.CustomerCode;
                if (this.GoodsReceiptTypeID == (int)GlobalEnums.GoodsReceiptTypeID.GoodsArrival && e.PurchaseOrderCodes != null && primaryReferences.IndexOf(e.PurchaseOrderCodes) < 0) primaryReferences = primaryReferences + (primaryReferences != "" ? ", " : "") + e.PurchaseOrderCodes;
                if (caption.IndexOf((this.NMVNTaskID == GlobalEnums.NmvnTaskID.ProductReceipt ? e.CommodityName : e.CommodityCode)) < 0) caption = caption + (caption != "" ? ", " : "") + (this.NMVNTaskID == GlobalEnums.NmvnTaskID.ProductReceipt ? e.CommodityName : e.CommodityCode);
            });
            this.PurchaseRequisitionReferences = purchaseRequisitionReferences; this.PurchaseRequisitionCodes = purchaseRequisitionCodes != "" ? purchaseRequisitionCodes : null; this.GoodsArrivalReferences = goodsArrivalReferences; this.GoodsArrivalCodes = goodsArrivalCodes != "" ? goodsArrivalCodes : null; this.WarehouseTransferReferences = warehouseTransferReferences;

            this.Caption = caption != "" ? (caption.Length > 98 ? caption.Substring(0, 95) + "..." : caption) : null;

            if (this.GoodsReceiptTypeID == (int)GlobalEnums.GoodsReceiptTypeID.GoodsArrival) this.Code = this.GoodsArrivalCodes != null ? (this.GoodsArrivalCodes.Length > 46 ? this.GoodsArrivalCodes.Substring(0, 46) + "..." : this.GoodsArrivalCodes) : "";
            if (purposes != "") this.Purposes = purposes.Length > 98 ? purposes.Substring(0, 95) + "..." : purposes;
            if (primaryReferences != "") this.PrimaryReferences = primaryReferences.Length > 98 ? primaryReferences.Substring(0, 95) + "..." : primaryReferences;
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


        [Display(Name = "Mã số máy")]
        string UserFirstName { get; set; }
        [Display(Name = "Người thực hiện")]
        string UserLastName { get; set; }
        [Display(Name = "Số chứng từ")]
        string Features { get; }

        List<GoodsReceiptDetailDTO> ViewDetails { get; set; }

        string ControllerName { get; }
        string ArrivalController { get; }
    }



    public class GoodsReceiptDTO<TGROption> : GoodsReceiptPrimitiveDTO<TGROption>, IBaseDetailEntity<GoodsReceiptDetailDTO>, IPriceCategory, IWarehouse, IGoodsReceiptDTO
        where TGROption : IGROption, new()
    {
        public GoodsReceiptDTO()
        {
            this.ViewDetails = new List<GoodsReceiptDetailDTO>();

            if (!GlobalEnums.CBPP) { this.ShiftID = 1; }
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


        public override Nullable<int> BinLocation_WarehouseID { get { return (this.Warehouse != null ? this.Warehouse.WarehouseID : null); } set { } }


        public override int StorekeeperID { get { return (this.Storekeeper != null ? this.Storekeeper.EmployeeID : 0); } }
        [Display(Name = "Nhân viên kho")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO Storekeeper { get; set; }


        [Display(Name = "Mã số máy")]
        public string UserFirstName { get; set; }
        [Display(Name = "Người thực hiện")]
        public string UserLastName { get; set; }

        [Display(Name = "CT")]
        public string Features
        {
            get
            {
                switch (this.GoodsReceiptTypeID)
                {
                    case (int)GlobalEnums.GoodsReceiptTypeID.PurchaseRequisition:
                        return this.PurchaseRequisitionReferenceNote + (this.PurchaseRequisitionCodeNote != null ? " [" + this.PurchaseRequisitionCodeNote + "] " : "") + (this.PurchaseRequisitionEntryDate != null ? " Ngày: " + this.PurchaseRequisitionEntryDate.ToString() : "");
                    case (int)GlobalEnums.GoodsReceiptTypeID.GoodsArrival:
                        return this.GoodsArrivalReferenceNote + (this.GoodsArrivalCodeNote != null ? " [HĐ: " + this.GoodsArrivalCodeNote + "] " : (this.PrimaryReferences != null ? "PO: " + this.PrimaryReferences : "")) + (this.GoodsArrivalEntryDate != null ? " Ngày nhận: " + ((DateTime)this.GoodsArrivalEntryDate).ToString("dd/MM/yy") : "") + (this.GoodsArrivalPurchaseOrderCodes != null ? " [PO: " + this.GoodsArrivalPurchaseOrderCodes + "] " : "") + (this.GoodsArrivalCustomsDeclaration != null ? " [TKHQ: " + this.GoodsArrivalCustomsDeclaration + "] " : "");
                    case (int)GlobalEnums.GoodsReceiptTypeID.WarehouseTransfer:
                        return "Kho xuất: " + WarehouseIssue.Name + ", Phiếu VCNB: " + this.WarehouseTransferReferenceNote + (this.WarehouseTransferEntryDate != null ? " Ngày: " + this.WarehouseTransferEntryDate.ToString() : "");
                    case (int)GlobalEnums.GoodsReceiptTypeID.FinishedProduct:
                        return this.PlannedOrderReference + (this.PlannedOrderCode != null ? " [" + this.PlannedOrderCode + "] " : "") + (this.PlannedOrderEntryDate != null ? " Ngày: " + this.PlannedOrderEntryDate.ToString() : "");
                    case (int)GlobalEnums.GoodsReceiptTypeID.Recyclate:
                        return this.RecyclateReference + (this.RecyclateEntryDate != null ? " Ngày: " + this.RecyclateEntryDate.ToString() : "");
                    case (int)GlobalEnums.GoodsReceiptTypeID.WarehouseAdjustments:
                        return "[OTHER ADJUSTMENT]";
                    default:
                        return "[OTHERS]";
                }
            }
        }





        public List<GoodsReceiptDetailDTO> ViewDetails { get; set; }

        public ICollection<GoodsReceiptDetailDTO> GetDetails() { return this.ViewDetails; }

        protected override IEnumerable<GoodsReceiptDetailDTO> DtoDetails() { return this.ViewDetails; }





        public string ControllerName { get { return this.NMVNTaskID.ToString() + "s"; } }
        public string ArrivalController { get { return this.IsMaterial ? "MaterialArrivals" : (this.IsItem ? "ItemArrivals" : (this.IsProduct ? "ProductArrivals" : "")); } }

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

