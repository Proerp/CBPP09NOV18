using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface IShiftRepository
    {
        IList<Shift> GetAllShifts();
    }
}
