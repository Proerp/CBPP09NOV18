using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TotalModel.Helpers;

namespace TotalModel.Models
{
    public partial class TaskIndex
    {
        public string ModuleSerialName { get { return this.ModuleSerialID + "." + this.ModuleName; } }
    }

    public partial class UserIndex
    {
        public string LocationOU { get { return this.LocationName + "\\" + this.OrganizationalUnitName; } }
    }

    public partial class UserAccessControl
    {
        public bool NoAccess
        {
            get { return this.AccessLevel == 0; }
            set { if (this.NoAccess != value) { this.AccessLevel = 0; } }
        }
        public bool ReadOnly
        {
            get { return this.AccessLevel == 1; }
            set { if (this.ReadOnly != value) { this.AccessLevel = 1; } }
        }
        public bool Editable
        {
            get { return this.AccessLevel == 2; }
            set { if (this.Editable != value) { this.AccessLevel = 2; } }
        }
    }







    public partial class PurchaseRequisition : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<PurchaseRequisitionDetail>
    {
        public int GetID() { return this.PurchaseRequisitionID; }

        public ICollection<PurchaseRequisitionDetail> GetDetails() { return this.PurchaseRequisitionDetails; }
    }


    public partial class PurchaseRequisitionDetail : IPrimitiveEntity, IHelperEntryDate, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.PurchaseRequisitionDetailID; }
    }






    public partial class GoodsReceipt : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<GoodsReceiptDetail>
    {
        public int GetID() { return this.GoodsReceiptID; }

        public virtual Warehouse WarehouseIssue { get { return this.Warehouse1; } }

        public ICollection<GoodsReceiptDetail> GetDetails() { return this.GoodsReceiptDetails; }
    }


    public partial class GoodsReceiptDetail : IPrimitiveEntity, IHelperEntryDate, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.GoodsReceiptDetailID; }
    }

    public partial class GoodsReceiptDetailAvailable
    {
        public virtual decimal? QuantityRemains { get { return this.QuantityAvailables; } }
    }

    public partial class MaterialIssue : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<MaterialIssueDetail>
    {
        public int GetID() { return this.MaterialIssueID; }

        public virtual Employee Storekeeper { get { return this.Employee; } }
        public virtual Employee CrucialWorker { get { return this.Employee1; } }

        public ICollection<MaterialIssueDetail> GetDetails() { return this.MaterialIssueDetails; }
    }


    public partial class MaterialIssueDetail : IPrimitiveEntity, IHelperEntryDate, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.MaterialIssueDetailID; }
    }


    public partial class MaterialIssueViewDetail
    {
        public Nullable<decimal> WorkshiftFirmOrderRemains
        {
            get
            {
                Nullable<decimal> workshiftQuantity = this.FirmOrderRemains; // (this.WorkingHours * this.CyclePerHours * this.MoldQuantity) * this.BlockQuantity / this.BlockUnit;
                return this.FirmOrderRemains < workshiftQuantity ? this.FirmOrderRemains : workshiftQuantity;
            }
        }
        public Nullable<decimal> QuantityRemains { get { return this.WorkshiftFirmOrderRemains < this.QuantityAvailables ? this.WorkshiftFirmOrderRemains : this.QuantityAvailables; } }
    }

    public partial class MaterialIssuePendingFirmOrderMaterial
    {
        public Nullable<decimal> WorkshiftFirmOrderRemains
        {
            get
            {
                Nullable<decimal> workshiftQuantity = this.FirmOrderRemains;// (this.WorkingHours * this.CyclePerHours * this.MoldQuantity) * this.BlockQuantity / this.BlockUnit;
                return this.FirmOrderRemains < workshiftQuantity ? this.FirmOrderRemains : workshiftQuantity;
            }
        }
        public Nullable<decimal> QuantityRemains { get { return this.QuantityAvailables; } } //NOW: ALLOW TO ISSUE A WHOLE ROLL OF TAPE        { get { return this.WorkshiftFirmOrderRemains < this.QuantityAvailables ? this.WorkshiftFirmOrderRemains : this.QuantityAvailables; } }
    }


    public partial class WarehouseAdjustmentIndex
    {
        public virtual decimal TotalQuantityPositive { get { return this.TotalQuantity; } }

        public virtual decimal TotalQuantityNegative { get { return -this.TotalQuantity; } }
    }


    public partial class WarehouseAdjustment : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<WarehouseAdjustmentDetail>
    {
        public int GetID() { return this.WarehouseAdjustmentID; }

        public virtual decimal TotalQuantityPositive { get { return this.TotalQuantity; } }

        public virtual decimal TotalQuantityNegative { get { return -this.TotalQuantity; } }

        public virtual Warehouse WarehouseReceipt { get { return this.Warehouse1; } }

        public ICollection<WarehouseAdjustmentDetail> GetDetails() { return this.WarehouseAdjustmentDetails; }
    }


    public partial class WarehouseAdjustmentDetail : IPrimitiveEntity, IHelperEntryDate, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.WarehouseAdjustmentDetailID; }
    }



    public partial class TransferOrder : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<TransferOrderDetail>
    {
        public int GetID() { return this.TransferOrderID; }    

        public virtual Warehouse WarehouseReceipt { get { return this.Warehouse1; } }

        public ICollection<TransferOrderDetail> GetDetails() { return this.TransferOrderDetails; }
    }


    public partial class TransferOrderDetail : IPrimitiveEntity, IHelperEntryDate, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.TransferOrderDetailID; }
    }



    public partial class WarehouseTransfer : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<WarehouseTransferDetail>
    {
        public int GetID() { return this.WarehouseTransferID; }

        public virtual Warehouse WarehouseReceipt { get { return this.Warehouse1; } }

        public virtual Employee Storekeeper { get { return this.Employee; } }

        public ICollection<WarehouseTransferDetail> GetDetails() { return this.WarehouseTransferDetails; }
    }


    public partial class WarehouseTransferDetail : IPrimitiveEntity, IHelperEntryDate, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.WarehouseTransferDetailID; }
    }

    public partial class WarehouseTransferViewDetail
    {
        public virtual decimal? QuantityRemains { get { return this.QuantityAvailables; } }
    }

    public partial class WarehouseTransferPendingTransferOrderDetail
    {
        public virtual decimal? QuantityRemains { get { return this.QuantityAvailables; } }
    }

    public partial class PlannedOrder : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<PlannedOrderDetail>
    {
        public int GetID() { return this.PlannedOrderID; }

        public ICollection<PlannedOrderDetail> GetDetails() { return this.PlannedOrderDetails; }
    }


    public partial class PlannedOrderDetail : IPrimitiveEntity, IHelperEntryDate, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.PlannedOrderDetailID; }
    }



    public partial class ProductionOrder : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<ProductionOrderDetail>
    {
        public int GetID() { return this.ProductionOrderID; }

        public ICollection<ProductionOrderDetail> GetDetails() { return this.ProductionOrderDetails; }
    }


    public partial class ProductionOrderDetail : IPrimitiveEntity, IHelperEntryDate, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.ProductionOrderDetailID; }

        public int CommodityID { get; set; }
        public int CommodityTypeID { get; set; }
    }


    public partial class SemifinishedProduct : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<SemifinishedProductDetail>
    {
        public int GetID() { return this.SemifinishedProductID; }

        public string MaterialCode { get { return this.MaterialIssueDetail.Commodity.Code; } }
        public string MaterialName { get { return this.MaterialIssueDetail.Commodity.Name; } }
        public decimal MaterialQuantity { get { return this.MaterialIssueDetail.Quantity; } }
        public decimal MaterialQuantityRemains { get { return this.MaterialIssueDetail.Quantity - this.MaterialIssueDetail.QuantitySemifinished - this.MaterialIssueDetail.QuantityFailure - this.MaterialIssueDetail.QuantityReceipted - this.MaterialIssueDetail.QuantityLoss + this.FoilWeights + this.FailureWeights; } }

        public virtual Employee CrucialWorker { get { return this.Employee; } }

        public ICollection<SemifinishedProductDetail> GetDetails() { return this.SemifinishedProductDetails; }
    }


    public partial class SemifinishedProductDetail : IPrimitiveEntity, IHelperEntryDate, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.SemifinishedProductDetailID; }
    }


    public partial class SemifinishedHandover : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<SemifinishedHandoverDetail>
    {
        public int GetID() { return this.SemifinishedHandoverID; }

        public virtual Employee SemifinishedLeader { get { return this.Employee; } }
        public virtual Employee FinishedLeader { get { return this.Employee1; } }

        public ICollection<SemifinishedHandoverDetail> GetDetails() { return this.SemifinishedHandoverDetails; }
    }


    public partial class SemifinishedHandoverDetail : IPrimitiveEntity, IHelperEntryDate
    {
        public int GetID() { return this.SemifinishedHandoverDetailID; }
    }



    public partial class FinishedProduct : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<FinishedProductDetail>
    {
        public int GetID() { return this.FinishedProductID; }

        public virtual Employee CrucialWorker { get { return this.Employee; } }

        public ICollection<FinishedProductDetail> GetDetails() { return this.FinishedProductDetails; }
    }


    public partial class FinishedProductDetail : IPrimitiveEntity, IHelperEntryDate, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.FinishedProductDetailID; }
    }



    public partial class FinishedHandover : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<FinishedHandoverDetail>
    {
        public int GetID() { return this.FinishedHandoverID; }

        public virtual Employee FinishedLeader { get { return this.Employee; } }
        public virtual Employee Storekeeper { get { return this.Employee1; } }

        public ICollection<FinishedHandoverDetail> GetDetails() { return this.FinishedHandoverDetails; }
    }


    public partial class FinishedHandoverDetail : IPrimitiveEntity, IHelperEntryDate
    {
        public int GetID() { return this.FinishedHandoverDetailID; }
    }




    public partial class SalesOrder : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<SalesOrderDetail>
    {
        public int GetID() { return this.SalesOrderID; }

        public virtual Employee Salesperson { get { return this.Employee; } }
        public virtual Customer Receiver { get { return this.Customer1; } }

        public virtual Promotion TradePromotion { get { return this.Promotion1; } }

        public ICollection<SalesOrderDetail> GetDetails() { return this.SalesOrderDetails; }
    }


    public partial class SalesOrderDetail : IPrimitiveEntity, IHelperEntryDate, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.SalesOrderDetailID; }
        public int GetWarehouseID() { return (int)this.WarehouseID; }
    }


    public partial class SalesOrderIndex
    {
        public decimal GrandTotalQuantity { get { return this.TotalQuantity + this.TotalFreeQuantity; } }
        public decimal GrandTotalQuantityAdvice { get { return this.TotalQuantityAdvice + this.TotalFreeQuantityAdvice; } }
    }



    public partial class DeliveryAdvice : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<DeliveryAdviceDetail>
    {
        public int GetID() { return this.DeliveryAdviceID; }

        public virtual Employee Salesperson { get { return this.Employee; } }
        public virtual Customer Receiver { get { return this.Customer1; } }

        public virtual Promotion TradePromotion { get { return this.Promotion1; } }

        public ICollection<DeliveryAdviceDetail> GetDetails() { return this.DeliveryAdviceDetails; }
    }


    public partial class DeliveryAdviceDetail : IPrimitiveEntity, IHelperEntryDate, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.DeliveryAdviceDetailID; }
        public int GetWarehouseID() { return (int)this.WarehouseID; }
    }


    public partial class DeliveryAdviceIndex
    {
        public decimal GrandTotalQuantity { get { return this.TotalQuantity + this.TotalFreeQuantity; } }
        public decimal GrandTotalQuantityIssue { get { return this.TotalQuantityIssue + this.TotalFreeQuantityIssue; } }
    }






    public partial class SalesReturn : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<SalesReturnDetail>
    {
        public int GetID() { return this.SalesReturnID; }

        public virtual Employee Salesperson { get { return this.Employee; } }
        public virtual Customer Receiver { get { return this.Customer1; } }

        public virtual Promotion TradePromotion { get { return this.Promotion1; } }

        public ICollection<SalesReturnDetail> GetDetails() { return this.SalesReturnDetails; }
    }


    public partial class SalesReturnDetail : IPrimitiveEntity, IHelperEntryDate, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.SalesReturnDetailID; }
        public int GetWarehouseID() { return (int)this.WarehouseID; }
    }


    public partial class SalesReturnIndex
    {
        public decimal GrandTotalQuantity { get { return this.TotalQuantity + this.TotalFreeQuantity; } }
        public decimal GrandTotalQuantityReceived { get { return this.TotalQuantityReceived + this.TotalFreeQuantityReceived; } }
    }



    public partial class GoodsIssue : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<GoodsIssueDetail>
    {
        public int GetID() { return this.GoodsIssueID; }

        public virtual Employee Storekeeper { get { return this.Employee; } }
        public virtual Customer Receiver { get { return this.Customer1; } }

        public virtual Promotion TradePromotion { get { return this.Promotion; } }

        public ICollection<GoodsIssueDetail> GetDetails() { return this.GoodsIssueDetails; }
    }


    public partial class GoodsIssueDetail : IPrimitiveEntity, IHelperEntryDate, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.GoodsIssueDetailID; }
        public int GetWarehouseID() { return (int)this.WarehouseID; }
    }

    public partial class DeliveryAdvicePendingCustomer
    {
        public string ReceiverDescription { get { return (this.CustomerID != this.ReceiverID ? this.ReceiverName + ", " : "") + this.ShippingAddress; } }
    }

    public partial class DeliveryAdvicePendingSalesOrder
    {
        public string ReceiverDescription { get { return (this.CustomerID != this.ReceiverID ? this.ReceiverName + ", " : "") + this.ShippingAddress; } }
    }

    public partial class PendingDeliveryAdvice
    {
        public string ReceiverDescription { get { return (this.CustomerID != this.ReceiverID ? this.ReceiverName + ", " : "") + this.ShippingAddress; } }
    }

    public partial class PendingDeliveryAdviceCustomer
    {
        public string ReceiverDescription { get { return (this.CustomerID != this.ReceiverID ? this.ReceiverName + ", " : "") + this.ShippingAddress; } }
    }

    public partial class HandlingUnitIndex
    {
        public string CustomerDescription { get { return this.CustomerName + (this.CustomerName != this.ReceiverName || this.Addressee != "" ? ", Người nhận: " + (this.Addressee != "" ? this.Addressee : this.ReceiverName) : "") + ", Giao hàng: " + this.ShippingAddress; } }
    }

    public partial class HandlingUnit : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<HandlingUnitDetail>
    {
        public int GetID() { return this.HandlingUnitID; }

        public virtual Employee PackagingStaff { get { return this.Employee; } }
        public virtual Customer Receiver { get { return this.Customer1; } }

        public ICollection<HandlingUnitDetail> GetDetails() { return this.HandlingUnitDetails; }
    }


    public partial class HandlingUnitDetail : IPrimitiveEntity, IHelperEntryDate
    {
        public int GetID() { return this.HandlingUnitDetailID; }
    }





    public partial class GoodsDelivery : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<GoodsDeliveryDetail>
    {
        public int GetID() { return this.GoodsDeliveryID; }

        public virtual Employee Driver { get { return this.Employee; } }
        public virtual Employee Collector { get { return this.Employee1; } }
        public virtual Customer Receiver { get { return this.Customer; } }

        public ICollection<GoodsDeliveryDetail> GetDetails() { return this.GoodsDeliveryDetails; }
    }


    public partial class GoodsDeliveryDetail : IPrimitiveEntity, IHelperEntryDate
    {
        public int GetID() { return this.GoodsDeliveryDetailID; }
    }



    public partial class PendingHandlingUnit
    {
        public string ReceiverDescription { get { return (this.Addressee != "" ? this.Addressee : this.ReceiverName); } }
    }


    public partial class AccountInvoiceIndex
    {
        public string CodeCustomerPO { get { return ((this.Code != null ? this.Code + " " : "") + (this.CustomerPO != null ? this.CustomerPO : "")).Trim(); } }
    }


    public partial class AccountInvoice : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<AccountInvoiceDetail>
    {
        public int GetID() { return this.AccountInvoiceID; }

        public virtual Customer Consumer { get { return this.Customer1; } }
        public virtual Customer Receiver { get { return this.Customer2; } }

        public virtual Promotion TradePromotion { get { return this.Promotion; } }

        public ICollection<AccountInvoiceDetail> GetDetails() { return this.AccountInvoiceDetails; }
    }


    public partial class AccountInvoiceDetail : IPrimitiveEntity, IHelperEntryDate
    {
        public int GetID() { return this.AccountInvoiceDetailID; }
    }

    public partial class ReceiptIndex
    {
        public string DebitAccountType { get { return (this.MonetaryAccountCode != null ? this.MonetaryAccountCode : (this.AdvanceReceiptReference != null ? "CT TT" : (this.SalesReturnReference != null ? "CT TH" : "CT CK"))); } }
        public string DebitAccountCode { get { return (this.MonetaryAccountCode != null ? null : (this.AdvanceReceiptReference != null ? this.AdvanceReceiptReference : (this.SalesReturnReference != null ? this.SalesReturnReference : this.CreditNoteReference))); } }
        public Nullable<System.DateTime> DebitAccountDate { get { return (this.MonetaryAccountCode != null ? null : (this.AdvanceReceiptDate != null ? this.AdvanceReceiptDate : (this.SalesReturnDate != null ? this.SalesReturnDate : this.CreditNoteDate))); } }
    }


    public partial class ReceiptViewDetail
    {
        public decimal AmountRemains { get { return (decimal)this.AmountDue - this.ReceiptAmount - this.CashDiscount - this.OtherDiscount; } }

        public string ReceiverDescription { get { return (this.CustomerID != this.ReceiverID ? this.ReceiverName + ", " : "") + this.Description; } }
    }


    public partial class Receipt : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<ReceiptDetail>
    {
        public int GetID() { return this.ReceiptID; }

        public virtual Receipt AdvanceReceipt { get { return this.Receipt1; } }
        public virtual Employee Cashier { get { return this.Employee; } }

        public decimal TotalReceiptAmountSaved { get { return this.TotalReceiptAmount; } }
        public decimal TotalFluctuationAmountSaved { get { return this.TotalFluctuationAmount; } }

        public ICollection<ReceiptDetail> GetDetails() { return this.ReceiptDetails; }
    }


    public partial class ReceiptDetail : IPrimitiveEntity
    {
        public int GetID() { return this.ReceiptDetailID; }
    }





    public partial class CreditNote : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<CreditNoteDetail>
    {
        public int GetID() { return this.CreditNoteID; }

        public virtual Employee Salesperson { get { return this.Employee; } }

        public ICollection<CreditNoteDetail> GetDetails() { return this.CreditNoteDetails; }
    }


    public partial class CreditNoteDetail : IPrimitiveEntity, IHelperEntryDate, IHelperCommodityID, IHelperCommodityTypeID
    {
        public int GetID() { return this.CreditNoteDetailID; }
    }





    public partial class VoidType : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.VoidTypeID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }
        public int LocationID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }

    public partial class Warehouse : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.WarehouseID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }

    public partial class Employee : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.EmployeeID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }

    public partial class Commodity : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.CommodityID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }
        public int LocationID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }

    public partial class CommodityPrice : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.CommodityPriceID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }
        public int LocationID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }


        public virtual CodePartDTO CodePartDTOA { get { return new CodePartDTO() { CodePart = this.CodePartA }; } }
        public virtual CodePartDTO CodePartDTOB { get { return new CodePartDTO() { CodePart = this.CodePartB }; } }
        public virtual CodePartDTO CodePartDTOC { get { return new CodePartDTO() { CodePart = this.CodePartC }; } }
    }

    public partial class Customer : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.CustomerID; }

        public virtual Employee Salesperson { get { return this.Employee; } }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }
        public int LocationID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }

    public partial class PromotionIndex
    {
        public string FilterText { get { return this.Brand + " " + this.Category + " " + this.Code + " " + this.Name; } }
    }

    public partial class Promotion : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<PromotionCommodityCodePart>
    {
        public int GetID() { return this.PromotionID; }

        public int PreparedPersonID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }

        public ICollection<PromotionCommodityCodePart> GetDetails() { return this.PromotionCommodityCodeParts; }
    }


    public partial class PromotionCommodityCodePart : IPrimitiveEntity
    {
        public int GetID() { return this.PromotionCommodityCodePartID; }
    }



    public partial class Mold : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.MoldID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }
        public int LocationID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }

    public partial class Bom : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.BomID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }
        public int LocationID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }

    public partial class ProductionLine : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.ProductionLineID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }
        public int LocationID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }



}
