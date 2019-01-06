using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalCore.Repositories;
using TotalModel.Models;


namespace TotalDAL.Repositories
{
    public class GenericAPIRepository : BaseRepository, IGenericAPIRepository
    {
        private readonly string functionNameGetEntityIndexes;

        public GenericAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities, string functionNameGetEntityIndexes)
            : base(totalSmartPortalEntities)
        {
            this.functionNameGetEntityIndexes = functionNameGetEntityIndexes;
        }

        public virtual ICollection<TEntityIndex> GetEntityIndexes<TEntityIndex>(string aspUserID, DateTime fromDate, DateTime toDate)
        {
            return this.GetEntityIndexes<TEntityIndex>(aspUserID, fromDate, toDate, this.functionNameGetEntityIndexes);
        }

        public virtual ICollection<TEntityIndex> GetEntityIndexes<TEntityIndex>(string aspUserID, DateTime fromDate, DateTime toDate, string functionNameGetEntityIndexes)
        {
            return base.ExecuteFunction<TEntityIndex>(functionNameGetEntityIndexes, this.GetEntityIndexParameters(aspUserID, fromDate, toDate));
        }

        protected virtual ObjectParameter[] GetEntityIndexParameters(string aspUserID, DateTime fromDate, DateTime toDate)
        {
            return new ObjectParameter[] { new ObjectParameter("AspUserID", aspUserID), new ObjectParameter("FromDate", fromDate), new ObjectParameter("ToDate", toDate) };
        }

    }
}
