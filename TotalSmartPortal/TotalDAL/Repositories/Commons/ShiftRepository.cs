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
            IList<Shift> shifts = this.totalSmartPortalEntities.Shifts.ToList();
            shifts.Insert(0, new Shift());
            return shifts;
        }

    }
}
