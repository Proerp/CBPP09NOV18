using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using TotalModel.Models;
using TotalCore.Repositories;



using TotalModel; //for Loading (09/07/2015) - let review and optimize Loading laster




namespace TotalDAL.Repositories
{
    public class GenericWithDetailRepository<TEntity, TEntityDetail> : GenericRepository<TEntity>, IGenericWithDetailRepository<TEntity, TEntityDetail>
        where TEntity : class, IAccessControlAttribute //IAccessControlAttribute: for Loading (09/07/2015) - let review and optimize Loading laster
        where TEntityDetail : class
    {
        private DbSet<TEntityDetail> modelDetailDbSet = null;

        public GenericWithDetailRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : this(totalSmartPortalEntities, null) { }

        public GenericWithDetailRepository(TotalSmartPortalEntities totalSmartPortalEntities, string functionNameEditable)
            : this(totalSmartPortalEntities, functionNameEditable, null) { }

        public GenericWithDetailRepository(TotalSmartPortalEntities totalSmartPortalEntities, string functionNameEditable, string functionNameApproved)
            : this(totalSmartPortalEntities, functionNameEditable, functionNameApproved, null) { }

        public GenericWithDetailRepository(TotalSmartPortalEntities totalSmartPortalEntities, string functionNameEditable, string functionNameApproved, string functionNameDeletable)
            : this(totalSmartPortalEntities, functionNameEditable, functionNameApproved, functionNameDeletable, null) { }

        public GenericWithDetailRepository(TotalSmartPortalEntities totalSmartPortalEntities, string functionNameEditable, string functionNameApproved, string functionNameDeletable, string functionNameVoidable)
            : base(totalSmartPortalEntities, functionNameEditable, functionNameApproved, functionNameDeletable, functionNameVoidable)
        {
            modelDetailDbSet = this.TotalSmartPortalEntities.Set<TEntityDetail>();
        }


        public virtual TEntityDetail RemoveDetail(TEntityDetail entityDetail)
        {
            return this.modelDetailDbSet.Remove(entityDetail);
        }

        public virtual IEnumerable<TEntityDetail> RemoveRangeDetail(IEnumerable<TEntityDetail> entityDetails)
        {
            return this.modelDetailDbSet.RemoveRange(entityDetails);
        }
    }
}
