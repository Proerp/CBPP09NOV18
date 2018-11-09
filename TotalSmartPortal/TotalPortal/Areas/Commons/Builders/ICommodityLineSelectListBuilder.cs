using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalPortal.Areas.Commons.Builders
{
    public interface ICommodityLineSelectListBuilder
    {
        IEnumerable<SelectListItem> BuildSelectListItemsForCommodityLines(IEnumerable<CommodityLine> CommodityLines);
    }

    public class CommodityLineSelectListBuilder : ICommodityLineSelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForCommodityLines(IEnumerable<CommodityLine> CommodityLines)
        {
            return CommodityLines.Select(pt => new SelectListItem { Text = pt.Code + " [" + pt.Name + "]", Value = pt.CommodityLineID.ToString() }).ToList();
        }
    }
}