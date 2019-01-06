using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Purchases
{
    public class Lab
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public Lab(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetLabIndexes();

            this.LabApproved();
            this.LabEditable();
            this.LabDeletable();

            this.LabToggleApproved();
        }

        private void GetLabIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      Labs.LabID, Labs.EntryDate, Labs.Reference, Labs.Code, Labs.GoodsArrivalID, GoodsArrivals.Reference AS GoodsArrivalReference, Labs.CommodityCodes, Labs.CommodityNames, Labs.SealCodes, Labs.BatchCodes, Labs.Description, Labs.TotalQuantity, Labs.Approved " + "\r\n";
            queryString = queryString + "       FROM        Labs INNER JOIN GoodsArrivals ON Labs.GoodsArrivalID = GoodsArrivals.GoodsArrivalID " + "\r\n";
            queryString = queryString + "       WHERE       Labs.EntryDate >= @FromDate AND Labs.EntryDate <= @ToDate " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetLabIndexes", queryString);
        }

        private void LabApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = LabID FROM Labs WHERE LabID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("LabApproved", queryArray);
        }

        private void LabEditable()
        {
            string[] queryArray = new string[0];

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("LabEditable", queryArray);
        }

        private void LabDeletable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = LabID FROM Labs WHERE LabID = @EntityID "; //NEVER ALLOW TO DELETE

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("LabDeletable", queryArray);
        }

        private void LabToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      Labs  SET Approved = @Approved, ApprovedDate = GetDate(), InActive = 0, InActiveDate = NULL, VoidTypeID = NULL WHERE LabID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT <> 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, N'hủy', '')  + N' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("LabToggleApproved", queryString);
        }
    }
}
