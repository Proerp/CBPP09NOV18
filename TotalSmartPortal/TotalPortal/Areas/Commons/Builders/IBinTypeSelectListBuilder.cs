using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalPortal.Areas.Commons.Builders
{
    public interface IBinTypeSelectListBuilder
    {
        IEnumerable<SelectListItem> BuildSelectListItemsForBinTypes(IEnumerable<BinType> BinTypes);
    }

    public class BinTypeSelectListBuilder : IBinTypeSelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForBinTypes(IEnumerable<BinType> BinTypes)
        {
            return BinTypes.Select(pt => new SelectListItem { Text = pt.Name, Value = pt.BinTypeID.ToString() }).ToList();
        }
    }
}