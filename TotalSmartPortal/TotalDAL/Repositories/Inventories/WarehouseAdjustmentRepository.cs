using System;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalCore.Repositories.Inventories;

namespace TotalDAL.Repositories.Inventories
{
    public class WarehouseAdjustmentRepository : GenericWithDetailRepository<WarehouseAdjustment, WarehouseAdjustmentDetail>, IWarehouseAdjustmentRepository
    {
        public WarehouseAdjustmentRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "WarehouseAdjustmentEditable", "WarehouseAdjustmentApproved")
        {
        }
    }








    public class WarehouseAdjustmentAPIRepository : GenericAPIRepository, IWarehouseAdjustmentAPIRepository
    {
        public WarehouseAdjustmentAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetWarehouseAdjustmentIndexes")
        {
        }

        protected override ObjectParameter[] GetEntityIndexParameters(string aspUserID, DateTime fromDate, DateTime toDate)
        {

            ObjectParameter[] baseParameters = base.GetEntityIndexParameters(aspUserID, fromDate, toDate);
            ObjectParameter[] objectParameters = new ObjectParameter[] { new ObjectParameter("NMVNTaskID", this.RepositoryBag.ContainsKey("NMVNTaskID") && this.RepositoryBag["NMVNTaskID"] != null ? this.RepositoryBag["NMVNTaskID"] : 0), baseParameters[0], baseParameters[1], baseParameters[2] };

            this.RepositoryBag.Remove("NMVNTaskID");

            return objectParameters;
        }
    }


}
