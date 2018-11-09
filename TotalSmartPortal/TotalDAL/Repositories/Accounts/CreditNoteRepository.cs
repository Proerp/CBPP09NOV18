using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Accounts;


namespace TotalDAL.Repositories.Accounts
{
    public class CreditNoteRepository : GenericWithDetailRepository<CreditNote, CreditNoteDetail>, ICreditNoteRepository
    {
        public CreditNoteRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "CreditNoteEditable", "CreditNoteApproved")
        {
        }
    }








    public class CreditNoteAPIRepository : GenericAPIRepository, ICreditNoteAPIRepository
    {
        public CreditNoteAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetCreditNoteIndexes")
        {
        }
    }


}
