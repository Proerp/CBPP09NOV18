using System;
namespace TotalBase.Enums
{
    public static class GlobalEnums
    {
        public static bool ERPConnected = false;

        public static int CalculatingTypeID = 1; //JUST CHANGE FROM [0] TO [1] AT 12.00PM ON 24.MAR.2O18 => TO SHOW THE ORIGINAL GROSS PRICE, GROSS AMOUNT ON PXK. 
        //THEO SỰ XEM XÉT NGÀY 24.MAR.2O18 THÌ THAY ĐỔI NÀY SẼ THAY ĐỔI 4 GIÁ TRỊ: ListedVATAmount, VATAmount, ListedGrossAmount, GrossAmount. 
        //TUY NHIÊN, NÓ HOÀN TOÀN KHÔNG THAY ĐỔI GIÁ TRỊ CỦA TABLE MASTER, VÌ: VATbyRow = false: TỨC LÀ: TotalListedVATAmount, TotalVATAmount, TotalListedGrossAmount, TotalGrossAmount ĐƯỢC TÍNH BỞI definedExemplar.prototype._updateTotalVATAmountToModelProperty: CÁCH TÍNH VAT 1 DÒNG
        //TÚM LẠI: CalculatingTypeID = [0] HAY [1] KHI VATbyRow = false: THÌ GIÁ TRỊ TỔNG CỦA TotalListedVATAmount, TotalVATAmount, TotalListedGrossAmount, TotalGrossAmount LÀ KHÔNG ĐỔI [DO: VATbyRow = false: CÁCH TÍNH VAT 1 DÒNG]

        public static bool VATbyRow = false;
        public static decimal VATPercent = 10;

        public static int rndN0 = 0;
        public static int rndQuantity = 2;
        public static int rndAmount = 0;
        public static int rndDiscountPercent = 1;

        public static int rndWeight = 2;

        public enum SubmitTypeOption
        {
            Save = 0, //Save and return (keep) current view
            Popup = 1, //Save popup windows
            Create = 3, //Save and the create new
            Closed = 9 //Save and close (return index view)
        };

        public enum NmvnTaskID
        {
            UnKnown = 0,



            Customer = 8001,

            Material = 8002,
            Item = 8002006,
            Product = 8002008,

            Mold = 8007,

            Promotion = 8003,
            Employee = 8005,
            CommodityPrice = 8006,

            PurchaseRequisition = 8020,
            PurchaseOrder = 8021,
            PurchaseInvoice = 8022,

            PlannedOrder = 680016,
            ProductionOrder = 680018,
            SemifinishedProduct = 680020,
            SemifinishedHandover = 680026,

            FinishedProduct = 680028,
            FinishedHandover = 680029,

            Quotation = 8031,
            SalesOrder = 8032,
            DeliveryAdvice = 8035,
            GoodsIssue = 8037,
            MaterialIssue = 8039,
            WarehouseAdjustment = 8036,

            MaterialAdjustment = 8036006,
            OtherMaterialReceipt = 80360061,
            OtherMaterialIssue = 80360062,

            ItemAdjustment = 8036007,
            OtherItemReceipt = 80360071,
            OtherItemIssue = 80360072,

            ProductAdjustment = 8036008,
            OtherProductReceipt = 80360081,
            OtherProductIssue = 80360082,


            SalesInvoice = 8051,

            VehiclesInvoice = 8052,
            PartsInvoice = 8053,
            ServicesInvoice = 8055,


            ServiceContract = 8056,

            AccountInvoice = 8057,
            Receipt = 8059,


            SalesReturn = 8038,
            CreditNote = 8060,



            InventoryAdjustment = 8078,
            VehicleAdjustment = 8078008,
            PartAdjustment = 8078009,

            HandlingUnit = 9010,
            GoodsDelivery = 9012,


            VehicleTransferOrder = 8071008,
            PartTransferOrder = 8071009,

            StockTransfer = 8073,
            VehicleTransfer = 8075,
            PartTransfer = 8076,


            TransferOrder = 8071,
            MaterialTransferOrder = 8081008,
            ItemTransferOrder = 8081009,
            ProductTransferOrder = 8081010,

            WarehouseTransfer = 1008071,
            MaterialTransfer = 9081008,
            ItemTransfer = 9081009,
            ProductTransfer = 9081010,


            GoodsReceipt = 8077,
            MaterialReceipt = 10081008,
            ItemReceipt = 10081009,
            ProductReceipt = 10081010
        };

        public enum ActiveOption
        {
            Active = 0,
            InActive = 1,
            Both = -1
        }

        public static int RootNode = 999999900; //1 billion
        public static int AncestorNode = 1000000000; //1 billion


        public enum GoodsReceiptTypeID
        {
            AllGoodsReceipt = 999,
            PurchaseInvoice = 1,
            GoodsReturn = 2,
            StockTransfer = 3,
            InventoryAdjustment = 4, //ERP VCP VB6
            WarehouseAdjustments = 5,
            PurchaseRequisition = 6,
            MaterialIssue = 7,
            FinishedProduct = 8,


            WarehouseTransfer = 9,


            GoodsIssueTransfer = 3333,
            Pickup = 686868
        };

        public enum MaterialIssueTypeID
        {
            AllMaterialIssue = 999,
            FirmOrders = 6
        };

        public enum WarehouseAdjustmentTypeID
        {
            ALL = 0,
            
            ReceiptPRO = 10,
            ReceiptSAM = 11, //Sample
            ReceiptRTN = 12, //Return
            ReceiptADJ = 13, //Adjustment
            ReceiptPUR = 14, //Purchasing
            ReceiptREG = 15, //Regenerate
            ReceiptOSS = 16, //Outsourcing services
            ReceiptBOR = 17, //Borrow
            ReceiptOTH = 18, //Other
            ReceiptFOI = 19, //Foil
            ReceiptTXF = 25, //Transfer

            IssueINV = 60, //Invoicing (Sales)
            IssueOFS = 61, //Offset //Xuất bù
            IssueADJ = 62, //Adjustment
            IssueSAM = 63, //Sample
            IssueDST = 64, //Destroy
            IssuePRO = 65, //Production
            IssueREG = 66, //Regenerate
            IssueRTN = 67, //Return
            IssueOSS = 68, //Outsourcing services
            IssueOTH = 69, //Other
            IssueTXF = 75, //Transfer

            BothReceiptIssues = 90,

            ReceiptPENDING = 110, //Production
            IssuePENDING = 160 //Production
        };

        public enum SalesInvoiceTypeID
        {
            AllInvoice = 1,
            VehiclesInvoice = 10,
            PartsInvoice = 20,
            ServicesInvoice = 30
        };

        public enum StockTransferTypeID
        {
            VehicleTransfer = 10,
            PartTransfer = 20
        };

        public enum ServiceContractTypeID
        {
            Warranty = 1,
            Repair = 2,
            Maintenance = 3
        };

        public enum InventoryAdjustmentTypeID
        {
            AllAdjustment = 1,
            VehicleAdjustment = 10,
            PartAdjustment = 20
        };


        public enum CommodityTypeID
        {
            All = 999,

            Products = 1,
            Items = 2,
            Materials = 3,


            Consumables = 3333,
            Services = 6,
            CreditNote = 8,

            Unknown = 99
        };

        public enum WarehouseTaskID
        {
            Unknown = -1,
            SalesOrder = 8032,
            DeliveryAdvice = 8035,
            SalesReturn = 8038,

            MaterialAdjustment = 8036006,
            ItemAdjustment = 8036007,
            ProductAdjustment = 8036008
        };

        public enum WarehouseClassID
        {
            L1 = 1,//Loại 1
            L2 = 2,//Loại 2
            L3 = 3,//Loại 3
            L4 = 4,//Thiếu
            L5 = 5,//Dư
            LD = 9 //Lên Đời
        }

        public enum WarehouseGroupID
        {
            CO = 1,
            HD = 2,
            MT = 3,
            TT = 4,
            XK = 5,
            ST = 6,
            BC = 7
        }


        public enum ReceiptTypeID
        {
            ReceiveMoney = 1,
            ApplyCredit = 2
        };




        public static int ApplyToSalesVersusReturns_ApplyToAll = (int)GlobalEnums.ApplyToSalesVersusReturns.ApplyToAll;
        public enum ApplyToSalesVersusReturns
        {
            ApplyToAll = 0,
            ApplyToSales = 1,
            ApplyToReturns = -1
        };



        public enum UpdateWarehouseBalanceOption
        {
            Add = 1,
            Minus = -1
        };


        public enum AccessLevel
        {
            Deny = 0,
            Readable = 1,
            Editable = 2
        };











        public enum FillingLine
        {
            None = 0,
            Smallpack = 1,
            Pail = 2,
            Drum = 3,

            BatchMaster = 18,

            TDV = 68,
            GoodsIssue = 88
        }


        public static int ConfigID = (int)GlobalEnums.FillingLine.TDV;
        public static int ConfigVersionID(int configID)
        {
            if (configID == (int)GlobalEnums.FillingLine.None)
                return 2;
            else if (configID == (int)GlobalEnums.FillingLine.TDV)
                return 2;
            else if (configID == (int)GlobalEnums.FillingLine.GoodsIssue)
                return 2;
            else if (configID == (int)GlobalEnums.FillingLine.BatchMaster)
                return 2;


            else if (configID == (int)GlobalEnums.FillingLine.Smallpack || configID == (int)GlobalEnums.FillingLine.Pail || configID == (int)GlobalEnums.FillingLine.Drum)
                return 2; //PAY ATTENTION WHEN CHANGE THIS VALUE BECAUSE: THIS IS USING ON THE FILLING LINES
            else
                return -1;
        }

        public static int MaxConfigVersionID()
        {
            int maxConfigVersionID = 0;
            foreach (GlobalEnums.FillingLine fillingLine in Enum.GetValues(typeof(GlobalEnums.FillingLine)))
            {
                if (maxConfigVersionID < GlobalEnums.ConfigVersionID((int)fillingLine)) maxConfigVersionID = GlobalEnums.ConfigVersionID((int)fillingLine);
            }

            return maxConfigVersionID;
        }

    }


    public static class GlobalReceiptTypeID
    {
        public static int ApplyCredit { get { return (int)GlobalEnums.ReceiptTypeID.ApplyCredit; } }
        public static int ReceiveMoney { get { return (int)GlobalEnums.ReceiptTypeID.ReceiveMoney; } }
    }

    public static class GlobalCreditTypeID
    {
        public static int AdvanceReceipt { get { return (int)GlobalEnums.NmvnTaskID.Receipt; } }
        public static int SalesReturn { get { return (int)GlobalEnums.NmvnTaskID.SalesReturn; } }
        public static int CreditNote { get { return (int)GlobalEnums.NmvnTaskID.CreditNote; } }
    }

}
