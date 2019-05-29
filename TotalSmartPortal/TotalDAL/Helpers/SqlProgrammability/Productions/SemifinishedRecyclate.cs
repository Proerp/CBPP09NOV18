using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Productions
{
    public class SemifinishedRecyclate
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public SemifinishedRecyclate(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetSemifinishedRecyclateIndexes();

            this.GetSemifinishedRecyclatePendingWorkshifts();

            this.GetSemifinishedRecyclateViewDetails();

            this.SemifinishedRecyclateSaveRelative();
            this.SemifinishedRecyclatePostSaveValidate();

            this.SemifinishedRecyclateApproved();
            this.SemifinishedRecyclateEditable();

            this.SemifinishedRecyclateToggleApproved();

            this.SemifinishedRecyclateInitReference();

            this.SemifinishedRecyclateSheet();
        }


        private void GetSemifinishedRecyclateIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      SemifinishedRecyclates.SemifinishedRecyclateID, CAST(SemifinishedRecyclates.EntryDate AS DATE) AS EntryDate, SemifinishedRecyclates.Reference, Locations.Code AS LocationCode, Workshifts.Name AS WorkshiftName, Users.FirstName AS UserFirstName, Users.LastName AS UserLastName, SemifinishedRecyclates.Description, SemifinishedRecyclates.Caption, SemifinishedRecyclates.TotalQuantity, SemifinishedRecyclates.Approved, " + "\r\n";
            queryString = queryString + "                   Workshifts.EntryDate AS WorkshiftEntryDate " + "\r\n";
            queryString = queryString + "       FROM        SemifinishedRecyclates " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON SemifinishedRecyclates.EntryDate >= @FromDate AND SemifinishedRecyclates.EntryDate <= @ToDate AND SemifinishedRecyclates.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.SemifinishedRecyclate + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = SemifinishedRecyclates.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON SemifinishedRecyclates.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Users ON SemifinishedRecyclates.UserID = Users.UserID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetSemifinishedRecyclateIndexes", queryString);
        }


        private void GetSemifinishedRecyclatePendingWorkshifts()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          Workshifts.WorkshiftID AS SemifinishedProductWorkshiftID, Workshifts.ShiftID AS SemifinishedProductShiftID, Workshifts.EntryDate AS SemifinishedProductEntryDate, SemifinishedProductRemains.TotalQuantityRemains " + "\r\n";

            queryString = queryString + "       FROM           (SELECT WorkshiftID, ROUND(SUM(RejectWeights + FailureWeights - RecycleWeights), " + (int)GlobalEnums.rndQuantity + ") AS TotalQuantityRemains FROM SemifinishedProducts WHERE Approved = 1 AND ROUND(RejectWeights + FailureWeights - RecycleWeights - RecycleLoss, " + (int)GlobalEnums.rndQuantity + ") > 0 " + " GROUP BY WorkshiftID) AS SemifinishedProductRemains " + "\r\n";
            queryString = queryString + "                       INNER JOIN Workshifts ON SemifinishedProductRemains.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Warehouses ON Warehouses.WarehouseID = 1 " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetSemifinishedRecyclatePendingWorkshifts", queryString);
        }

        private void GetSemifinishedRecyclateViewDetails()
        {
            string queryString;

            queryString = " @SemifinishedRecyclateID Int, @LocationID Int, @WorkshiftID Int, @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@SemifinishedRecyclateID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BUILDSQLNew() + "\r\n";
            queryString = queryString + "                   ORDER BY ProductionLineCode, SemifinishedProducts.SemifinishedProductID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BUILDSQLEdit() + "\r\n";
            queryString = queryString + "                       ORDER BY ProductionLineCode, SemifinishedProducts.SemifinishedProductID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BUILDSQLNew() + " AND SemifinishedProducts.SemifinishedProductID NOT IN (SELECT SemifinishedProductID FROM SemifinishedRecyclateDetails WHERE SemifinishedRecyclateID = @SemifinishedRecyclateID) " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       " + this.BUILDSQLEdit() + "\r\n";
            queryString = queryString + "                       ORDER BY ProductionLineCode, SemifinishedProducts.SemifinishedProductID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetSemifinishedRecyclateViewDetails", queryString);
        }

        private string BUILDSQLNew()
        {
            string queryString = "";

            queryString = queryString + "       SELECT      0 AS SemifinishedRecyclateDetailID, 0 AS SemifinishedRecyclateID, SemifinishedProducts.SemifinishedProductID, SemifinishedProducts.EntryDate, SemifinishedProducts.Reference, Shifts.Code AS ShiftCode, ProductionLines.Code AS ProductionLineCode, FirmOrders.Code AS FirmOrderCode, FirmOrders.Specification, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.CommodityTypeID, Commodities.RecycleCommodityID, RecycleCommodities.Code AS RecycleCommodityCode, SemifinishedProducts.RejectWeights, SemifinishedProducts.FailureWeights, ROUND(SemifinishedProducts.RejectWeights + SemifinishedProducts.FailureWeights - SemifinishedProducts.RecycleWeights, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, 0.0 AS Quantity " + "\r\n"; //QuantityRemains NOT INCLUDED RecycleLoss, BUT: CHECK PENDING MUST MINUS RecycleLoss!!!!!!

            queryString = queryString + "       FROM        SemifinishedProducts " + "\r\n";
            queryString = queryString + "                   INNER JOIN Shifts ON SemifinishedProducts.WorkshiftID = @WorkshiftID AND SemifinishedProducts.Approved = 1 AND ROUND(SemifinishedProducts.RejectWeights + SemifinishedProducts.FailureWeights - SemifinishedProducts.RecycleWeights - SemifinishedProducts.RecycleLoss, " + (int)GlobalEnums.rndQuantity + ") > 0 AND SemifinishedProducts.ShiftID = Shifts.ShiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN ProductionLines ON SemifinishedProducts.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";
            queryString = queryString + "                   INNER JOIN FirmOrders ON SemifinishedProducts.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "                   INNER JOIN MaterialIssueDetails ON SemifinishedProducts.MaterialIssueDetailID = MaterialIssueDetails.MaterialIssueDetailID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON MaterialIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   LEFT  JOIN Commodities AS RecycleCommodities ON Commodities.RecycleCommodityID = RecycleCommodities.CommodityID " + "\r\n";

            return queryString;
        }

        private string BUILDSQLEdit()
        {
            string queryString = "";

            queryString = queryString + "       SELECT      SemifinishedRecyclateDetails.SemifinishedRecyclateDetailID, SemifinishedRecyclateDetails.SemifinishedRecyclateID, SemifinishedProducts.SemifinishedProductID, SemifinishedProducts.EntryDate, SemifinishedProducts.Reference, Shifts.Code AS ShiftCode, ProductionLines.Code AS ProductionLineCode, FirmOrders.Code AS FirmOrderCode, FirmOrders.Specification, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.CommodityTypeID, Commodities.RecycleCommodityID, RecycleCommodities.Code AS RecycleCommodityCode, SemifinishedProducts.RejectWeights, SemifinishedProducts.FailureWeights, ROUND(SemifinishedProducts.RejectWeights + SemifinishedProducts.FailureWeights - SemifinishedProducts.RecycleWeights + SemifinishedRecyclateDetails.Quantity, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, SemifinishedRecyclateDetails.Quantity " + "\r\n";

            queryString = queryString + "       FROM        SemifinishedRecyclateDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN SemifinishedProducts ON SemifinishedRecyclateDetails.SemifinishedRecyclateID = @SemifinishedRecyclateID AND SemifinishedRecyclateDetails.SemifinishedProductID = SemifinishedProducts.SemifinishedProductID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Shifts ON SemifinishedProducts.ShiftID = Shifts.ShiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN ProductionLines ON SemifinishedProducts.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";
            queryString = queryString + "                   INNER JOIN FirmOrders ON SemifinishedProducts.FirmOrderID = FirmOrders.FirmOrderID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON SemifinishedRecyclateDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   LEFT  JOIN Commodities AS RecycleCommodities ON Commodities.RecycleCommodityID = RecycleCommodities.CommodityID " + "\r\n";
            
            return queryString;
        }
        
        private void SemifinishedRecyclateSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       BEGIN  " + "\r\n";

            queryString = queryString + "           DECLARE @msg NVARCHAR(300) ";

            queryString = queryString + "           IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";

            #region UPDATE WorkshiftID
            queryString = queryString + "                   DECLARE         @EntryDate Datetime, @ShiftID int, @WorkshiftID int " + "\r\n";
            queryString = queryString + "                   SELECT          @EntryDate = CONVERT(date, EntryDate), @ShiftID = ShiftID FROM SemifinishedRecyclates WHERE SemifinishedRecyclateID = @EntityID " + "\r\n";
            queryString = queryString + "                   SET             @WorkshiftID = (SELECT TOP 1 WorkshiftID FROM Workshifts WHERE EntryDate = @EntryDate AND ShiftID = @ShiftID) " + "\r\n";

            queryString = queryString + "                   IF             (@WorkshiftID IS NULL) " + "\r\n";
            queryString = queryString + "                       BEGIN ";
            queryString = queryString + "                           INSERT INTO     Workshifts(EntryDate, ShiftID, Code, Name, WorkingHours, Remarks) SELECT @EntryDate, ShiftID, Code, Name, WorkingHours, Remarks FROM Shifts WHERE ShiftID = @ShiftID " + "\r\n";
            queryString = queryString + "                           SELECT          @WorkshiftID = SCOPE_IDENTITY(); " + "\r\n";
            queryString = queryString + "                       END ";

            queryString = queryString + "                   UPDATE          SemifinishedRecyclates        SET WorkshiftID = @WorkshiftID WHERE SemifinishedRecyclateID = @EntityID " + "\r\n";
            queryString = queryString + "                   UPDATE          SemifinishedRecyclateDetails  SET WorkshiftID = @WorkshiftID WHERE SemifinishedRecyclateID = @EntityID " + "\r\n";
            #endregion UPDATE WorkshiftID

            queryString = queryString + "               END " + "\r\n";


            queryString = queryString + "           UPDATE          SemifinishedProducts " + "\r\n";
            queryString = queryString + "           SET             SemifinishedProducts.RecycleWeights = ROUND(SemifinishedProducts.RecycleWeights + SemifinishedRecyclateDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + "), " + "\r\n";
            queryString = queryString + "                           SemifinishedProducts.RecycleLoss = IIF(@SaveRelativeOption = 1, ROUND(SemifinishedProducts.RejectWeights + SemifinishedProducts.FailureWeights - (SemifinishedProducts.RecycleWeights + SemifinishedRecyclateDetails.Quantity), " + (int)GlobalEnums.rndQuantity + "), 0) " + "\r\n"; //UPDATE: SET RecycleLoss = Remains, UNDO: SET RecycleLoss = 0            
            queryString = queryString + "           FROM            SemifinishedRecyclateDetails " + "\r\n";
            queryString = queryString + "                           INNER JOIN SemifinishedProducts ON (SemifinishedProducts.Approved = 1 OR @SaveRelativeOption = -1) AND SemifinishedRecyclateDetails.SemifinishedRecyclateID = @EntityID AND SemifinishedRecyclateDetails.SemifinishedProductID = SemifinishedProducts.SemifinishedProductID " + "\r\n";

            queryString = queryString + "           IF @@ROWCOUNT <> (SELECT COUNT(*) FROM SemifinishedRecyclateDetails WHERE SemifinishedRecyclateID = @EntityID) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   SET         @msg = N'Phiếu giao phế phẩm đã hủy, hoặc chưa duyệt; hoặc số lượng nhập kho không phù hợp' ; " + "\r\n";
            queryString = queryString + "                   THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";

            queryString = queryString + "       END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("SemifinishedRecyclateSaveRelative", queryString);
        }

        private void SemifinishedRecyclatePostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày nhập kho: ' + CAST(SemifinishedProducts.EntryDate AS nvarchar) FROM SemifinishedRecyclateDetails INNER JOIN SemifinishedProducts ON SemifinishedRecyclateDetails.SemifinishedRecyclateID = @EntityID AND SemifinishedRecyclateDetails.SemifinishedProductID = SemifinishedProducts.SemifinishedProductID AND SemifinishedRecyclateDetails.EntryDate < SemifinishedProducts.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Số lượng kết toán vượt quá số lượng ghi nhận: ' + SemifinishedProducts.Reference + ': ' + CAST(ROUND(RejectWeights + FailureWeights - RecycleWeights - RecycleLoss, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM SemifinishedProducts WHERE (ROUND(RejectWeights + FailureWeights - RecycleWeights - RecycleLoss, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("SemifinishedRecyclatePostSaveValidate", queryArray);
        }




        private void SemifinishedRecyclateApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = SemifinishedRecyclateID FROM SemifinishedRecyclates WHERE SemifinishedRecyclateID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("SemifinishedRecyclateApproved", queryArray);
        }


        private void SemifinishedRecyclateEditable()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = SemifinishedRecyclateID FROM GoodsReceiptDetails WHERE SemifinishedRecyclateID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("SemifinishedRecyclateEditable", queryArray);
        }

        private void SemifinishedRecyclateToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      SemifinishedRecyclates  SET Approved = @Approved, ApprovedDate = GetDate() WHERE SemifinishedRecyclateID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          SemifinishedRecyclateDetails  SET Approved = @Approved WHERE SemifinishedRecyclateID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, N'hủy', '')  + N' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("SemifinishedRecyclateToggleApproved", queryString);
        }


        private void SemifinishedRecyclateInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("SemifinishedRecyclates", "SemifinishedRecyclateID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.SemifinishedRecyclate));
            this.totalSmartPortalEntities.CreateTrigger("SemifinishedRecyclateInitReference", simpleInitReference.CreateQuery());
        }

        private void SemifinishedRecyclateSheet()
        {
            string queryString = " @SemifinishedRecyclateID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @LocalSemifinishedRecyclateID int    SET @LocalSemifinishedRecyclateID = @SemifinishedRecyclateID" + "\r\n";

            queryString = queryString + "       SELECT          SemifinishedRecyclates.SemifinishedRecyclateID, SemifinishedRecyclates.EntryDate AS SemifinishedRecyclateEntryDate, SemifinishedRecyclates.Reference, Workshifts.Code AS WorkshiftCode, ProductionLines.Code AS ProductionLineCode, " + "\r\n";
            queryString = queryString + "                       SemifinishedProducts.EntryDate AS SemifinishedProductEntryDate, SemifinishedProducts.Reference AS SemifinishedProductReference, SemifinishedProducts.Code AS SemifinishedProductCode, SemifinishedProducts.Specs, SemifinishedProducts.Specification, SemifinishedProducts.TotalQuantity AS SemifinishedProductTotalQuantity, Customers.Name AS CustomerName, " + "\r\n";
            queryString = queryString + "                       Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, SemifinishedRecyclateDetails.BatchEntryDate, SemifinishedRecyclateDetails.Quantity, ISNULL(SemifinishedProducts.Description, '') + ' ' + ISNULL(SemifinishedRecyclates.Description, '') AS Description " + "\r\n";

            queryString = queryString + "       FROM            SemifinishedRecyclates " + "\r\n";
            queryString = queryString + "                       INNER JOIN SemifinishedRecyclateDetails ON SemifinishedRecyclates.SemifinishedRecyclateID = @LocalSemifinishedRecyclateID AND SemifinishedRecyclates.SemifinishedRecyclateID = SemifinishedRecyclateDetails.SemifinishedRecyclateID " + "\r\n";
            queryString = queryString + "                       INNER JOIN SemifinishedProducts ON SemifinishedRecyclates.SemifinishedProductID = SemifinishedProducts.SemifinishedProductID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON SemifinishedRecyclates.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Workshifts ON SemifinishedRecyclates.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                       INNER JOIN ProductionLines ON SemifinishedRecyclates.ProductionLineID = ProductionLines.ProductionLineID" + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities ON SemifinishedRecyclateDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "       ORDER BY        SemifinishedRecyclateDetails.SemifinishedRecyclateDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            //this.totalSmartPortalEntities.CreateStoredProcedure("SemifinishedRecyclateSheet", queryString);
        }
    }
}

