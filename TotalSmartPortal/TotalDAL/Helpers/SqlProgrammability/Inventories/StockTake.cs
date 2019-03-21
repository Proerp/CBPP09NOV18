using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Inventories
{
    public class StockTake
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public StockTake(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetStockTakeIndexes();

            this.GetStockTakeViewDetails();

            this.StockTakeSaveRelative();
            this.StockTakePostSaveValidate();

            this.StockTakeApproved();
            this.StockTakeEditable();

            this.StockTakeToggleApproved();

            this.StockTakeInitReference();

            this.StockTakeSheet();
        }


        private void GetStockTakeIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      StockTakes.StockTakeID, CAST(StockTakes.EntryDate AS DATE) AS EntryDate, StockTakes.Reference, Locations.Code AS LocationCode, BinLocations.Code AS CommodityCode, Workshifts.Name AS WorkshiftName, Workshifts.EntryDate AS WorkshiftEntryDate, Users.FirstName AS UserFirstName, Users.LastName AS UserLastName, StockTakes.Description, StockTakes.Caption, StockTakes.TotalQuantity, StockTakes.Approved " + "\r\n";
            queryString = queryString + "       FROM        StockTakes " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON StockTakes.EntryDate >= @FromDate AND StockTakes.EntryDate <= @ToDate AND StockTakes.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.StockTake + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = StockTakes.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN BinLocations ON StockTakes.BinLocationID = BinLocations.BinLocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON StockTakes.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Users ON StockTakes.UserID = Users.UserID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetStockTakeIndexes", queryString);
        }


        private void GetStockTakeViewDetails()
        {
            string queryString;

            queryString = " @StockTakeID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      StockTakeDetails.StockTakeDetailID, StockTakeDetails.StockTakeID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails.GoodsReceiptID, GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceiptDetails.Reference AS GoodsReceiptReference, GoodsReceiptDetails.Code AS GoodsReceiptCode, GoodsReceiptDetails.EntryDate AS GoodsReceiptEntryDate, GoodsReceiptDetails.ExpiryDate, GoodsReceiptDetails.BatchID, GoodsReceiptDetails.BatchEntryDate, StockTakeDetails.Barcode, StockTakeDetails.BatchCode, StockTakeDetails.SealCode, StockTakeDetails.LabCode, StockTakeDetails.Quantity, StockTakeDetails.Remarks " + "\r\n";

            queryString = queryString + "       FROM        StockTakeDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON StockTakeDetails.StockTakeID = @StockTakeID AND StockTakeDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsReceiptDetails ON StockTakeDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";

            queryString = queryString + "       ORDER BY    StockTakeDetails.StockTakeDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetStockTakeViewDetails", queryString);
        }






        private void StockTakeSaveRelative()
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
            queryString = queryString + "                   SELECT          @EntryDate = CONVERT(date, EntryDate), @ShiftID = ShiftID FROM StockTakes WHERE StockTakeID = @EntityID " + "\r\n";
            queryString = queryString + "                   SET             @WorkshiftID = (SELECT TOP 1 WorkshiftID FROM Workshifts WHERE EntryDate = @EntryDate AND ShiftID = @ShiftID) " + "\r\n";

            queryString = queryString + "                   IF             (@WorkshiftID IS NULL) " + "\r\n";
            queryString = queryString + "                       BEGIN ";
            queryString = queryString + "                           INSERT INTO     Workshifts(EntryDate, ShiftID, Code, Name, WorkingHours, Remarks) SELECT @EntryDate, ShiftID, Code, Name, WorkingHours, Remarks FROM Shifts WHERE ShiftID = @ShiftID " + "\r\n";
            queryString = queryString + "                           SELECT          @WorkshiftID = SCOPE_IDENTITY(); " + "\r\n";
            queryString = queryString + "                       END ";

            queryString = queryString + "                   UPDATE          StockTakes        SET WorkshiftID = @WorkshiftID WHERE StockTakeID = @EntityID " + "\r\n";
            queryString = queryString + "                   UPDATE          StockTakeDetails  SET WorkshiftID = @WorkshiftID WHERE StockTakeID = @EntityID " + "\r\n";
            #endregion UPDATE WorkshiftID

            queryString = queryString + "               END " + "\r\n";


            //queryString = queryString + "           DECLARE         @StockTakeDetails TABLE (GoodsReceiptDetailID int NOT NULL PRIMARY KEY, Quantity decimal(18, 2) NOT NULL)" + "\r\n";
            //queryString = queryString + "           INSERT INTO     @StockTakeDetails (GoodsReceiptDetailID, Quantity) SELECT GoodsReceiptDetailID, SUM(Quantity) AS Quantity FROM StockTakeDetails WHERE StockTakeID = @EntityID GROUP BY GoodsReceiptDetailID " + "\r\n";

            //queryString = queryString + "           DECLARE         @AffectedROWCOUNT int ";

            //#region UPDATE GoodsReceiptDetails
            //queryString = queryString + "           UPDATE          GoodsReceiptDetails " + "\r\n";
            //queryString = queryString + "           SET             GoodsReceiptDetails.QuantityIssued = ROUND(GoodsReceiptDetails.QuantityIssued + StockTakeDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            //queryString = queryString + "           FROM            GoodsReceiptDetails " + "\r\n";
            //queryString = queryString + "                           INNER JOIN @StockTakeDetails StockTakeDetails ON GoodsReceiptDetails.GoodsReceiptDetailID = StockTakeDetails.GoodsReceiptDetailID AND GoodsReceiptDetails.Approved = 1 " + "\r\n";

            //queryString = queryString + "           IF @@ROWCOUNT <> (SELECT COUNT(*) FROM @StockTakeDetails) " + "\r\n";
            //queryString = queryString + "               BEGIN " + "\r\n";
            //queryString = queryString + "                   SET         @msg = N'Phiếu nhập kho đã hủy, chưa duyệt hoặc đã xóa.' ; " + "\r\n";
            //queryString = queryString + "                   THROW       61001,  @msg, 1; " + "\r\n";
            //queryString = queryString + "               END " + "\r\n";
            //#endregion

            queryString = queryString + "       END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("StockTakeSaveRelative", queryString);
        }

        private void StockTakePostSaveValidate()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày nhập kho: ' + CAST(GoodsReceipts.EntryDate AS nvarchar) FROM StockTakeDetails INNER JOIN GoodsReceipts ON StockTakeDetails.StockTakeID = @EntityID AND StockTakeDetails.GoodsReceiptID = GoodsReceipts.GoodsReceiptID AND StockTakeDetails.EntryDate < GoodsReceipts.EntryDate ";
            //queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Số lượng xuất vượt quá số lượng tồn kho: ' + CAST(ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM GoodsReceiptDetails WHERE (ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") < 0) ";
            //queryArray[2] = " SELECT TOP 1 @FoundEntity = N'Số lượng xuất vượt quá số lượng yêu cầu: ' + CAST(ROUND(Quantity * 1005 - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM BlendingInstructionDetails WHERE (ROUND(Quantity * 1.005 - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("StockTakePostSaveValidate", queryArray);
        }




        private void StockTakeApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = StockTakeID FROM StockTakes WHERE StockTakeID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("StockTakeApproved", queryArray);
        }


        private void StockTakeEditable()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = StockTakeID FROM GoodsReceiptDetails WHERE StockTakeID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("StockTakeEditable", queryArray);
        }

        private void StockTakeToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      StockTakes  SET Approved = @Approved, ApprovedDate = GetDate() WHERE StockTakeID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          StockTakeDetails  SET Approved = @Approved WHERE StockTakeID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, N'hủy', '')  + N' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("StockTakeToggleApproved", queryString);
        }


        private void StockTakeInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("StockTakes", "StockTakeID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.StockTake));
            this.totalSmartPortalEntities.CreateTrigger("StockTakeInitReference", simpleInitReference.CreateQuery());

            string queryString = " @StockTakeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT TOP 1 Reference FROM StockTakes WHERE StockTakeID = @StockTakeID " + "\r\n";
            this.totalSmartPortalEntities.CreateStoredProcedure("StockTakeGetReference", queryString);
        }

        private void StockTakeSheet()
        {
            string queryString = " @StockTakeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE         @LocalStockTakeID int    SET @LocalStockTakeID = @StockTakeID" + "\r\n";

            queryString = queryString + "       SELECT          StockTakes.StockTakeID, StockTakes.EntryDate AS StockTakeEntryDate, StockTakes.Reference, Workshifts.Code AS WorkshiftCode, ProductionLines.Code AS ProductionLineCode, " + "\r\n";
            queryString = queryString + "                       BlendingInstructions.EntryDate AS BlendingInstructionEntryDate, BlendingInstructions.Reference AS BlendingInstructionReference, BlendingInstructions.Code AS BlendingInstructionCode, BlendingInstructions.Specs, BlendingInstructions.Specification, BlendingInstructions.TotalQuantity AS BlendingInstructionTotalQuantity, Customers.Name AS CustomerName, " + "\r\n";
            queryString = queryString + "                       Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, StockTakeDetails.BatchEntryDate, StockTakeDetails.Quantity, ISNULL(BlendingInstructions.Description, '') + ' ' + ISNULL(StockTakes.Description, '') AS Description " + "\r\n";

            queryString = queryString + "       FROM            StockTakes " + "\r\n";
            queryString = queryString + "                       INNER JOIN StockTakeDetails ON StockTakes.StockTakeID = @LocalStockTakeID AND StockTakes.StockTakeID = StockTakeDetails.StockTakeID " + "\r\n";
            queryString = queryString + "                       INNER JOIN BlendingInstructions ON StockTakes.BlendingInstructionID = BlendingInstructions.BlendingInstructionID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON StockTakes.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Workshifts ON StockTakes.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                       INNER JOIN ProductionLines ON StockTakes.ProductionLineID = ProductionLines.ProductionLineID" + "\r\n";
            queryString = queryString + "                       INNER JOIN Commodities ON StockTakeDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "       ORDER BY        StockTakeDetails.StockTakeDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            //this.totalSmartPortalEntities.CreateStoredProcedure("StockTakeSheet", queryString);
        }

    }
}

