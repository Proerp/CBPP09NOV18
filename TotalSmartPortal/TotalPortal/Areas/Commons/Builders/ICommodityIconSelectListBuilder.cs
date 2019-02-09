using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalPortal.Areas.Commons.Builders
{
    public interface ICommodityIconSelectListBuilder
    {
        IEnumerable<SelectListItem> BuildSelectListItemsForCommodityIcons(IEnumerable<CommodityIconBase> commodityIconBases);
    }

    public class CommodityIconSelectListBuilder : ICommodityIconSelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForCommodityIcons(IEnumerable<CommodityIconBase> commodityIconBases)
        {
            return commodityIconBases.Select(pt => new SelectListItem { Text = pt.Name, Value = pt.CommodityIconID.ToString() }).ToList();
        }
    }
}