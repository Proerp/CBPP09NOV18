using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Purchases
{
    public class PurchaseRequisition
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public PurchaseRequisition(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetPurchaseRequisitionIndexes();

            this.GetPurchaseRequisitionViewDetails();
            this.PurchaseRequisitionSaveRelative();
            this.PurchaseRequisitionPostSaveValidate();

            this.PurchaseRequisitionApproved();
            this.PurchaseRequisitionEditable();
            this.PurchaseRequisitionVoidable();

            this.PurchaseRequisitionToggleApproved();
            this.PurchaseRequisitionToggleVoid();
            this.PurchaseRequisitionToggleVoidDetail();

            this.PurchaseRequisitionInitReference();
        }


        private void GetPurchaseRequisitionIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      PurchaseRequisitions.PurchaseRequisitionID, CAST(PurchaseRequisitions.EntryDate AS DATE) AS EntryDate, PurchaseRequisitions.Reference, PurchaseRequisitions.Code, Locations.Code AS LocationCode, Customers.Name AS CustomerName, ISNULL(VoidTypes.Name, CASE PurchaseRequisitions.InActivePartial WHEN 1 THEN N'Hủy một phần đh' ELSE N'' END) AS VoidTypeName, PurchaseRequisitions.DeliveryDate, PurchaseRequisitions.Purposes, PurchaseRequisitions.Description, PurchaseRequisitions.TotalQuantity, PurchaseRequisitions.TotalQuantityReceipted, PurchaseRequisitions.Approved, PurchaseRequisitions.InActive, PurchaseRequisitions.InActivePartial " + "\r\n";
            queryString = queryString + "       FROM        PurchaseRequisitions " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON PurchaseRequisitions.EntryDate >= @FromDate AND PurchaseRequisitions.EntryDate <= @ToDate AND PurchaseRequisitions.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.PurchaseRequisition + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = PurchaseRequisitions.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON PurchaseRequisitions.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN VoidTypes ON PurchaseRequisitions.VoidTypeID = VoidTypes.VoidTypeID" + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetPurchaseRequisitionIndexes", queryString);
        }

        #region X


        private void GetPurchaseRequisitionViewDetails()
        {
            string queryString;
            SqlProgrammability.Inventories.Inventories inventories = new Inventories.Inventories(this.totalSmartPortalEntities);

            queryString = " @PurchaseRequisitionID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      PurchaseRequisitionDetails.PurchaseRequisitionDetailID, PurchaseRequisitionDetails.PurchaseRequisitionID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, PurchaseRequisitionDetails.CommodityTypeID, VoidTypes.VoidTypeID, VoidTypes.Code AS VoidTypeCode, VoidTypes.Name AS VoidTypeName, VoidTypes.VoidClassID, " + "\r\n";
            queryString = queryString + "                   PurchaseRequisitionDetails.Quantity, PurchaseRequisitionDetails.InActivePartial, PurchaseRequisitionDetails.InActivePartialDate, PurchaseRequisitionDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        PurchaseRequisitionDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON PurchaseRequisitionDetails.PurchaseRequisitionID = @PurchaseRequisitionID AND PurchaseRequisitionDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN VoidTypes ON PurchaseRequisitionDetails.VoidTypeID = VoidTypes.VoidTypeID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetPurchaseRequisitionViewDetails", queryString);
        }

        private void PurchaseRequisitionSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("PurchaseRequisitionSaveRelative", queryString);
        }

        private void PurchaseRequisitionPostSaveValidate()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = 'TEST Date: ' + CAST(EntryDate AS nvarchar) FROM PurchaseRequisitions WHERE PurchaseRequisitionID = @EntityID "; //FOR TEST TO BREAK OUT WHEN SAVE -> CHECK ROLL BACK OF TRANSACTION
            //queryArray[0] = " SELECT TOP 1 @FoundEntity = 'Service Date: ' + CAST(ServiceInvoices.EntryDate AS nvarchar) FROM PurchaseRequisitions INNER JOIN PurchaseRequisitions AS ServiceInvoices ON PurchaseRequisitions.PurchaseRequisitionID = @EntityID AND PurchaseRequisitions.ServiceInvoiceID = ServiceInvoices.PurchaseRequisitionID AND PurchaseRequisitions.EntryDate < ServiceInvoices.EntryDate ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("PurchaseRequisitionPostSaveValidate", queryArray);
        }




        private void PurchaseRequisitionApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = PurchaseRequisitionID FROM PurchaseRequisitions WHERE PurchaseRequisitionID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("PurchaseRequisitionApproved", queryArray);
        }


        private void PurchaseRequisitionEditable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = PurchaseRequisitionID FROM PurchaseRequisitions WHERE PurchaseRequisitionID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            queryArray[1] = " SELECT TOP 1 @FoundEntity = PurchaseRequisitionID FROM GoodsReceiptDetails WHERE PurchaseRequisitionID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("PurchaseRequisitionEditable", queryArray);
        }

        private void PurchaseRequisitionVoidable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = PurchaseRequisitionID FROM PurchaseRequisitions WHERE PurchaseRequisitionID = @EntityID AND Approved = 0"; //Must approve in order to allow void
            queryArray[1] = " SELECT TOP 1 @FoundEntity = PurchaseRequisitionID FROM GoodsReceiptDetails WHERE PurchaseRequisitionID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("PurchaseRequisitionVoidable", queryArray);
        }


        private void PurchaseRequisitionToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      PurchaseRequisitions  SET Approved = @Approved, ApprovedDate = GetDate() WHERE PurchaseRequisitionID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          PurchaseRequisitionDetails  SET Approved = @Approved WHERE PurchaseRequisitionID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, N'hủy', '')  + N' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("PurchaseRequisitionToggleApproved", queryString);
        }

        private void PurchaseRequisitionToggleVoid()
        {
            string queryString = " @EntityID int, @InActive bit, @VoidTypeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      PurchaseRequisitions  SET InActive = @InActive, InActiveDate = GetDate(), VoidTypeID = IIF(@InActive = 1, @VoidTypeID, NULL) WHERE PurchaseRequisitionID = @EntityID AND InActive = ~@InActive" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          PurchaseRequisitionDetails  SET InActive = @InActive WHERE PurchaseRequisitionID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@InActive = 0, 'phục hồi lệnh', '')  + ' hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            this.totalSmartPortalEntities.CreateStoredProcedure("PurchaseRequisitionToggleVoid", queryString);
        }

        private void PurchaseRequisitionToggleVoidDetail()
        {
            string queryString = " @EntityID int, @EntityDetailID int, @InActivePartial bit, @VoidTypeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      PurchaseRequisitionDetails  SET InActivePartial = @InActivePartial, InActivePartialDate = GetDate(), VoidTypeID = IIF(@InActivePartial = 1, @VoidTypeID, NULL) WHERE PurchaseRequisitionID = @EntityID AND PurchaseRequisitionDetailID = @EntityDetailID AND InActivePartial = ~@InActivePartial ; " + "\r\n";
            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE         @MaxInActivePartial bit     SET @MaxInActivePartial = (SELECT MAX(CAST(InActivePartial AS int)) FROM PurchaseRequisitionDetails WHERE PurchaseRequisitionID = @EntityID) ;" + "\r\n";
            queryString = queryString + "               UPDATE          PurchaseRequisitions  SET InActivePartial = @MaxInActivePartial WHERE PurchaseRequisitionID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@InActivePartial = 0, 'phục hồi lệnh', '')  + ' hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            this.totalSmartPortalEntities.CreateStoredProcedure("PurchaseRequisitionToggleVoidDetail", queryString);
        }


        private void PurchaseRequisitionInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("PurchaseRequisitions", "PurchaseRequisitionID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.PurchaseRequisition));
            this.totalSmartPortalEntities.CreateTrigger("PurchaseRequisitionInitReference", simpleInitReference.CreateQuery());
        }


        #endregion
    }
}
