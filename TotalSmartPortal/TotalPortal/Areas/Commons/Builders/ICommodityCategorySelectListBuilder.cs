using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalPortal.Areas.Commons.Builders
{   

    public interface ICommodityCategorySelectListBuilder
    {
        IEnumerable<SelectListItem> BuildSelectListItemsForCommodityCategorys(IEnumerable<CommodityCategory> CommodityCategorys);
    }

    public class CommodityCategorySelectListBuilder : ICommodityCategorySelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForCommodityCategorys(IEnumerable<CommodityCategory> CommodityCategorys)
        {
            return CommodityCategorys.Select(pt => new SelectListItem { Text = pt.Code + " [" + pt.Name + "]", Value = pt.CommodityCategoryID.ToString() }).ToList();
        }
    }
}