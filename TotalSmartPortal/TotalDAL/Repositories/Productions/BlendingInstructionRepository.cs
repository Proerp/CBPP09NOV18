using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalCore.Repositories.Productions;

namespace TotalDAL.Repositories.Productions
{
    public class BlendingInstructionRepository : GenericWithDetailRepository<BlendingInstruction, BlendingInstructionDetail>, IBlendingInstructionRepository
    {
        public BlendingInstructionRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "BlendingInstructionEditable", "BlendingInstructionApproved", null, "BlendingInstructionVoidable")
        {
        }
    }








    public class BlendingInstructionAPIRepository : GenericAPIRepository, IBlendingInstructionAPIRepository
    {
        public BlendingInstructionAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetBlendingInstructionIndexes")
        {
        }

        protected override ObjectParameter[] GetEntityIndexParameters(string aspUserID, System.DateTime fromDate, System.DateTime toDate)
        {
            ObjectParameter[] baseParameters = base.GetEntityIndexParameters(aspUserID, fromDate, toDate);
            ObjectParameter[] objectParameters = new ObjectParameter[] { baseParameters[0], baseParameters[1], baseParameters[2], new ObjectParameter("LabOptionID", this.RepositoryBag.ContainsKey("LabOptionID") && this.RepositoryBag["LabOptionID"] != null ? this.RepositoryBag["LabOptionID"] : 0), new ObjectParameter("FilterOptionID", this.RepositoryBag.ContainsKey("FilterOptionID") && this.RepositoryBag["FilterOptionID"] != null ? this.RepositoryBag["FilterOptionID"] : 0) };

            this.RepositoryBag.Remove("LabOptionID");
            this.RepositoryBag.Remove("FilterOptionID");

            return objectParameters;
        }

        public IEnumerable<BlendingInstructionRunning> GetRunnings(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<BlendingInstructionRunning> blendingInstructionRunnings = base.TotalSmartPortalEntities.GetBlendingInstructionRunnings(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return blendingInstructionRunnings;
        }

        public IEnumerable<BlendingInstructionLog> GetBlendingInstructionLogs(int? blendingInstructionID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<BlendingInstructionLog> blendingInstructionLogs = base.TotalSmartPortalEntities.GetBlendingInstructionLogs(blendingInstructionID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return blendingInstructionLogs;
        }
    }


}