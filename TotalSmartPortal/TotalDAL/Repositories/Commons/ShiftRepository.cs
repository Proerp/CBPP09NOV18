using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public ShiftRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public IList<Shift> GetAllShifts()
        {
            return this.GetAllShifts(true);
        }

        public IList<Shift> GetAllShifts(bool withNull)
        {
            this.totalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IList<Shift> shifts = this.totalSmartPortalEntities.Shifts.ToList();
            if (withNull) shifts.Insert(0, new Shift());
            this.totalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;
            return shifts;
        }

    }
}
