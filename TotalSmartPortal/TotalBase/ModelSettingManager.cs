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
                    return "H";


                case GlobalEnums.NmvnTaskID.BlendingInstruction:
                    return "FT";
                case GlobalEnums.NmvnTaskID.PlannedProduct:
                    return "PL";
                case GlobalEnums.NmvnTaskID.ProductOrder:
                    return "PO";

                case GlobalEnums.NmvnTaskID.ItemStaging:
                    return "MI";



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
                    return "B";

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
                    //return "R";
                    return @"CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.MaterialReceipt + @" AND @LocationID = 1 THEN 'N' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.MaterialReceipt + @" AND @LocationID = 2 THEN 'H' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ItemReceipt + @" AND @LocationID = 1 THEN 'M' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ItemReceipt + @" AND @LocationID = 2 THEN 'D' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ProductReceipt + @" AND @LocationID = 1 THEN 'K' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ProductReceipt + @" AND @LocationID = 2 THEN 'T' ELSE '#' END
                             END END END END END";

                case GlobalEnums.NmvnTaskID.WarehouseTransfer:
                    //return "R";
                    return @"CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.MaterialTransfer + @" AND @LocationID = 1 THEN 'NC' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.MaterialTransfer + @" AND @LocationID = 2 THEN 'HC' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ItemTransfer + @" AND @LocationID = 1 THEN 'MC' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ItemTransfer + @" AND @LocationID = 2 THEN 'DC' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ProductTransfer + @" AND @LocationID = 1 THEN 'KC' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.ProductTransfer + @" AND @LocationID = 2 THEN 'TC' ELSE '#' END
                             END END END END END";

                case GlobalEnums.NmvnTaskID.WarehouseAdjustment:
                    //return "R";
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
                                    " + (int)GlobalEnums.NmvnTaskID.ProductAdjustment + @" THEN 'KA' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.OtherProductReceipt + @" THEN 'KN' ELSE 
                             CASE WHEN @NmvnTaskID = 
                                    " + (int)GlobalEnums.NmvnTaskID.OtherProductIssue + @" THEN 'KX' ELSE '#' END

                             END END END END END END END END";

                case GlobalEnums.NmvnTaskID.ServiceContract:
                    return "H";

                case GlobalEnums.NmvnTaskID.TransferOrder:
                    return "TO";
                //                    return @"CASE WHEN @StockTransferTypeID = 
                //                                    " + (int)GlobalEnums.StockTransferTypeID.VehicleTransfer + @" THEN 'LX' ELSE 
                //                             CASE WHEN @StockTransferTypeID = 
                //                                    " + (int)GlobalEnums.StockTransferTypeID.PartTransfer + @" THEN 'LP' ELSE '#' END 
                //                             END";

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
