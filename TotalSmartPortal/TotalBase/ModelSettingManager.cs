using TotalBase.Enums;

namespace TotalBase
{

    public static class ModelSettingManager
    {

        public static int ReferenceLength = 6;
        public static string ReferencePrefix(GlobalEnums.NmvnTaskID nmvnTaskID)
        {
            switch (nmvnTaskID)
            {
                case GlobalEnums.NmvnTaskID.PurchaseRequisition:
                    return "PR";
                case GlobalEnums.NmvnTaskID.PurchaseOrder:
                    return "PO";
                case GlobalEnums.NmvnTaskID.GoodsArrival:
                    return "GA";
                case GlobalEnums.NmvnTaskID.Lab:
                    return "LB";

                case GlobalEnums.NmvnTaskID.PurchaseInvoice:
                    return "PV";






                case GlobalEnums.NmvnTaskID.BlendingInstruction:
                    return "FI";

                case GlobalEnums.NmvnTaskID.PlannedOrder:
                    return @"CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.PlannedItem + @" THEN 'PM' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.PlannedProduct + @" THEN 'PL' ELSE '#' END
                             END ";

                case GlobalEnums.NmvnTaskID.ProductionOrder:
                    return @"CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ItemOrder + @" THEN 'IO' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ProductOrder + @" THEN 'KO' ELSE '#' END
                             END ";


                case GlobalEnums.NmvnTaskID.SemifinishedItem:
                    return "SI";
                case GlobalEnums.NmvnTaskID.SemifinishedProduct:
                    return "SP";

                case GlobalEnums.NmvnTaskID.SemifinishedHandover:
                    return @"CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.SemifinishedItemHandover + @" THEN 'HI' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.SemifinishedProductHandover + @" THEN 'HP' ELSE '#' END
                             END ";

                case GlobalEnums.NmvnTaskID.MaterialIssue:
                    return @"CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.MaterialStaging + @" THEN 'MM' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ItemStaging + @" THEN 'MI' ELSE
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ProductStaging + @" THEN 'MP' ELSE '#' END
                             END END ";

                case GlobalEnums.NmvnTaskID.PackageIssue:
                    return "PI";


                case GlobalEnums.NmvnTaskID.FinishedHandover:
                    return "OP";

                case GlobalEnums.NmvnTaskID.FinishedProduct:
                    return "FP";




                case GlobalEnums.NmvnTaskID.SalesOrder:
                    return "O";
                case GlobalEnums.NmvnTaskID.DeliveryAdvice:
                    return "D";
                case GlobalEnums.NmvnTaskID.SalesReturn:
                    return "SR";

                case GlobalEnums.NmvnTaskID.GoodsIssue:
                    return "K";
                case GlobalEnums.NmvnTaskID.HandlingUnit:
                    return "C";
                case GlobalEnums.NmvnTaskID.GoodsDelivery:
                    return "X";

                case GlobalEnums.NmvnTaskID.Quotation:
                    return "Q";

                case GlobalEnums.NmvnTaskID.SalesInvoice:
                    return @"CASE WHEN @SalesInvoiceTypeID = 
                                    " + (int)GlobalEnums.SalesInvoiceTypeID.VehiclesInvoice + @" THEN 'X' ELSE 
                             CASE WHEN @SalesInvoiceTypeID = 
                                    " + (int)GlobalEnums.SalesInvoiceTypeID.PartsInvoice + @" THEN 'P' ELSE 
                             CASE WHEN @SalesInvoiceTypeID = 
                                    " + (int)GlobalEnums.SalesInvoiceTypeID.ServicesInvoice + @" THEN 'S' ELSE '#' END
                             END END";

                case GlobalEnums.NmvnTaskID.AccountInvoice:
                    return "I";
                case GlobalEnums.NmvnTaskID.Receipt:
                    return "R";
                case GlobalEnums.NmvnTaskID.CreditNote:
                    return "CR";

                case GlobalEnums.NmvnTaskID.GoodsReceipt:                    
                    return @"CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.MaterialReceipt + @" AND @LocationID = 1 THEN 'N' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.MaterialReceipt + @" AND @LocationID = 2 THEN 'H' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ItemReceipt + @" AND @LocationID = 1 THEN 'M' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ItemReceipt + @" AND @LocationID = 2 THEN 'D' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ProductReceipt + @" AND @LocationID = 1 THEN 'T' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ProductReceipt + @" AND @LocationID = 2 THEN 'B' ELSE '#' END
                             END END END END END";

                case GlobalEnums.NmvnTaskID.TransferOrder:
                    return @"CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.MaterialTransferOrder + @" AND @LocationID = 1 THEN 'TH' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.MaterialTransferOrder + @" AND @LocationID = 2 THEN 'TR' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ItemTransferOrder + @" AND @LocationID = 1 THEN 'TD' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ItemTransferOrder + @" AND @LocationID = 2 THEN 'TO' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ProductTransferOrder + @" AND @LocationID = 1 THEN 'TK' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ProductTransferOrder + @" AND @LocationID = 2 THEN 'TP' ELSE '#' END
                             END END END END END";

                case GlobalEnums.NmvnTaskID.WarehouseTransfer:
                    return @"CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.MaterialTransfer + @" AND @LocationID = 1 THEN 'NC' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.MaterialTransfer + @" AND @LocationID = 2 THEN 'HC' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ItemTransfer + @" AND @LocationID = 1 THEN 'MC' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ItemTransfer + @" AND @LocationID = 2 THEN 'DC' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ProductTransfer + @" AND @LocationID = 1 THEN 'TC' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ProductTransfer + @" AND @LocationID = 2 THEN 'KC' ELSE '#' END
                             END END END END END";

                case GlobalEnums.NmvnTaskID.WarehouseAdjustment:                    
                    return @"CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.MaterialAdjustment + @" THEN 'NA' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.OtherMaterialReceipt + @" THEN 'NN' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.OtherMaterialIssue + @" THEN 'NX' ELSE 


                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ItemAdjustment + @" THEN 'MA' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.OtherItemReceipt + @" THEN 'MN' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.OtherItemIssue + @" THEN 'MX' ELSE 


                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ProductAdjustment + @" THEN 'TA' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.OtherProductReceipt + @" THEN 'TN' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.OtherProductIssue + @" THEN 'TX' ELSE '#' END

                             END END END END END END END END";

                case GlobalEnums.NmvnTaskID.ServiceContract:
                    return "SC";


                case GlobalEnums.NmvnTaskID.StockTransfer:
                    return @"CASE WHEN @StockTransferTypeID = 
                                    " + (int)GlobalEnums.StockTransferTypeID.VehicleTransfer + @" THEN 'DX' ELSE 
                             CASE WHEN @StockTransferTypeID = 
                                    " + (int)GlobalEnums.StockTransferTypeID.PartTransfer + @" THEN 'DP' ELSE '#' END 
                             END";
                case GlobalEnums.NmvnTaskID.InventoryAdjustment:
                    return @"CASE WHEN @InventoryAdjustmentTypeID = 
                                    " + (int)GlobalEnums.InventoryAdjustmentTypeID.VehicleAdjustment + @" THEN 'AX' ELSE 
                             CASE WHEN @InventoryAdjustmentTypeID = 
                                    " + (int)GlobalEnums.InventoryAdjustmentTypeID.PartAdjustment + @" THEN 'AP' ELSE '#' END 
                             END";

                case GlobalEnums.NmvnTaskID.Promotion:
                    return "PS";

                default:
                    return "A";
            }


        }
    }
}
