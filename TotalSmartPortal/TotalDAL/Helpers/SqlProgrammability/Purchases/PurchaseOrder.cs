using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Purchases
{
    public class PurchaseOrder
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public PurchaseOrder(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetPurchaseOrderIndexes();

            this.GetPurchaseOrderViewDetails();
            this.PurchaseOrderSaveRelative();
            this.PurchaseOrderPostSaveValidate();

            this.PurchaseOrderApproved();
            this.PurchaseOrderEditable();
            this.PurchaseOrderVoidable();

            this.PurchaseOrderToggleApproved();
            this.PurchaseOrderToggleVoid();
            this.PurchaseOrderToggleVoidDetail();

            this.PurchaseOrderInitReference();
        }


        private void GetPurchaseOrderIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      PurchaseOrders.PurchaseOrderID, CAST(PurchaseOrders.EntryDate AS DATE) AS EntryDate, PurchaseOrders.Reference, PurchaseOrders.Code, PurchaseOrders.VoucherDate, Locations.Code AS LocationCode, Customers.Name AS CustomerName, ISNULL(VoidTypeDetails.Name, VoidTypes.Name) AS VoidTypeName, PurchaseOrders.DeliveryDate, PurchaseOrders.Purposes, PurchaseOrders.Description, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, PurchaseOrderDetails.Quantity, PurchaseOrderDetails.QuantityArrived, PurchaseOrderDetails.Quantity - PurchaseOrderDetails.QuantityArrived AS QuantityRemains, PurchaseOrders.TotalQuantity, PurchaseOrders.TotalQuantityArrived, PurchaseOrders.Approved, PurchaseOrders.InActive, PurchaseOrders.InActivePartial " + "\r\n";
            queryString = queryString + "       FROM        PurchaseOrders " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON PurchaseOrders.EntryDate >= @FromDate AND PurchaseOrders.EntryDate <= @ToDate AND PurchaseOrders.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.PurchaseOrder + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = PurchaseOrders.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON PurchaseOrders.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN PurchaseOrderDetails ON PurchaseOrders.PurchaseOrderID = PurchaseOrderDetails.PurchaseOrderID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN Commodities ON PurchaseOrderDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN VoidTypes ON PurchaseOrders.VoidTypeID = VoidTypes.VoidTypeID" + "\r\n";
            queryString = queryString + "                   LEFT JOIN VoidTypes VoidTypeDetails ON PurchaseOrderDetails.VoidTypeID = VoidTypeDetails.VoidTypeID" + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetPurchaseOrderIndexes", queryString);
        }

        #region X


        private void GetPurchaseOrderViewDetails()
        {
            string queryString;
            SqlProgrammability.Inventories.Inventories inventories = new Inventories.Inventories(this.totalSmartPortalEntities);

            queryString = " @PurchaseOrderID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      PurchaseOrderDetails.PurchaseOrderDetailID, PurchaseOrderDetails.PurchaseOrderID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, PurchaseOrderDetails.CommodityTypeID, VoidTypes.VoidTypeID, VoidTypes.Code AS VoidTypeCode, VoidTypes.Name AS VoidTypeName, VoidTypes.VoidClassID, " + "\r\n";
            queryString = queryString + "                   PurchaseOrderDetails.Quantity, PurchaseOrderDetails.InActivePartial, PurchaseOrderDetails.InActivePartialDate, PurchaseOrderDetails.Remarks, PurchaseOrderDetails.LabCode, PurchaseOrderDetails.ProductionDate, PurchaseOrderDetails.ExpiryDate " + "\r\n";
            queryString = queryString + "       FROM        PurchaseOrderDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON PurchaseOrderDetails.PurchaseOrderID = @PurchaseOrderID AND PurchaseOrderDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN VoidTypes ON PurchaseOrderDetails.VoidTypeID = VoidTypes.VoidTypeID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetPurchaseOrderViewDetails", queryString);
        }

        private void PurchaseOrderSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       BEGIN " + "\r\n";
            queryString = queryString + "           IF (@SaveRelativeOption = 1) ";
            queryString = queryString + "               BEGIN ";
            queryString = queryString + "                   UPDATE          PurchaseOrderDetails " + "\r\n";
            queryString = queryString + "                   SET             PurchaseOrderDetails.Reference = PurchaseOrders.Reference " + "\r\n";
            queryString = queryString + "                   FROM            PurchaseOrders INNER JOIN PurchaseOrderDetails ON PurchaseOrders.PurchaseOrderID = @EntityID AND PurchaseOrders.PurchaseOrderID = PurchaseOrderDetails.PurchaseOrderID " + "\r\n";
            queryString = queryString + "               END ";
            queryString = queryString + "       END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("PurchaseOrderSaveRelative", queryString);
        }

        private void PurchaseOrderPostSaveValidate()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = 'TEST Date: ' + CAST(EntryDate AS nvarchar) FROM PurchaseOrders WHERE PurchaseOrderID = @EntityID "; //FOR TEST TO BREAK OUT WHEN SAVE -> CHECK ROLL BACK OF TRANSACTION
            //queryArray[0] = " SELECT TOP 1 @FoundEntity = 'Service Date: ' + CAST(ServiceInvoices.EntryDate AS nvarchar) FROM PurchaseOrders INNER JOIN PurchaseOrders AS ServiceInvoices ON PurchaseOrders.PurchaseOrderID = @EntityID AND PurchaseOrders.ServiceInvoiceID = ServiceInvoices.PurchaseOrderID AND PurchaseOrders.EntryDate < ServiceInvoices.EntryDate ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("PurchaseOrderPostSaveValidate", queryArray);
        }




        private void PurchaseOrderApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = PurchaseOrderID FROM PurchaseOrders WHERE PurchaseOrderID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("PurchaseOrderApproved", queryArray);
        }


        private void PurchaseOrderEditable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = PurchaseOrderID FROM PurchaseOrders WHERE PurchaseOrderID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            queryArray[1] = " SELECT TOP 1 @FoundEntity = PurchaseOrderID FROM GoodsArrivalDetails WHERE PurchaseOrderID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("PurchaseOrderEditable", queryArray);
        }

        private void PurchaseOrderVoidable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = PurchaseOrderID FROM PurchaseOrders WHERE PurchaseOrderID = @EntityID AND Approved = 0"; //Must approve in order to allow void
            queryArray[1] = " SELECT TOP 1 @FoundEntity = PurchaseOrderID FROM GoodsArrivalDetails WHERE PurchaseOrderID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("PurchaseOrderVoidable", queryArray);
        }


        private void PurchaseOrderToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      PurchaseOrders  SET Approved = @Approved, ApprovedDate = GetDate() WHERE PurchaseOrderID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          PurchaseOrderDetails  SET Approved = @Approved WHERE PurchaseOrderID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, N'hủy', '')  + N' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("PurchaseOrderToggleApproved", queryString);
        }

        private void PurchaseOrderToggleVoid()
        {
            string queryString = " @EntityID int, @InActive bit, @VoidTypeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      PurchaseOrders  SET InActive = @InActive, InActiveDate = GetDate(), VoidTypeID = IIF(@InActive = 1, @VoidTypeID, NULL) WHERE PurchaseOrderID = @EntityID AND InActive = ~@InActive" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          PurchaseOrderDetails  SET InActive = @InActive WHERE PurchaseOrderID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@InActive = 0, 'phục hồi lệnh', '')  + ' hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            this.totalSmartPortalEntities.CreateStoredProcedure("PurchaseOrderToggleVoid", queryString);
        }

        private void PurchaseOrderToggleVoidDetail()
        {
            string queryString = " @EntityID int, @EntityDetailID int, @InActivePartial bit, @VoidTypeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      PurchaseOrderDetails  SET InActivePartial = @InActivePartial, InActivePartialDate = GetDate(), VoidTypeID = IIF(@InActivePartial = 1, @VoidTypeID, NULL) WHERE PurchaseOrderID = @EntityID AND PurchaseOrderDetailID = @EntityDetailID AND InActivePartial = ~@InActivePartial ; " + "\r\n";
            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE         @MaxInActivePartial bit     SET @MaxInActivePartial = (SELECT MAX(CAST(InActivePartial AS int)) FROM PurchaseOrderDetails WHERE PurchaseOrderID = @EntityID) ;" + "\r\n";
            queryString = queryString + "               UPDATE          PurchaseOrders  SET InActivePartial = @MaxInActivePartial WHERE PurchaseOrderID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@InActivePartial = 0, 'phục hồi lệnh', '')  + ' hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            this.totalSmartPortalEntities.CreateStoredProcedure("PurchaseOrderToggleVoidDetail", queryString);
        }


        private void PurchaseOrderInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("PurchaseOrders", "PurchaseOrderID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.PurchaseOrder));
            this.totalSmartPortalEntities.CreateTrigger("PurchaseOrderInitReference", simpleInitReference.CreateQuery());
        }


        #endregion
    }
}
