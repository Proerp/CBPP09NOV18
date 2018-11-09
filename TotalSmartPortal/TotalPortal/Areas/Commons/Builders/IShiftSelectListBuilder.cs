using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalPortal.Areas.Commons.Builders
{

    public interface IShiftSelectListBuilder
    {
        IEnumerable<SelectListItem> BuildSelectListItemsForShifts(IEnumerable<Shift> Shifts);
    }

    public class ShiftSelectListBuilder : IShiftSelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForShifts(IEnumerable<Shift> Shifts)
        {
            return Shifts.Select(pt => new SelectListItem { Text = pt.Code + (pt.Name != null ? " [" + pt.Name + "]" : ""), Value = pt.ShiftID.ToString() }).ToList();
        }
    }
}