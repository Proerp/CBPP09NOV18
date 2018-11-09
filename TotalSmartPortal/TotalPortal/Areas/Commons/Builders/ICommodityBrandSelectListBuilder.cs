using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalPortal.Areas.Commons.Builders
{
    public interface ICommodityBrandSelectListBuilder
    {
        IEnumerable<SelectListItem> BuildSelectListItemsForCommodityBrands(IEnumerable<CommodityBrand> CommodityBrands);
    }

    public class CommodityBrandSelectListBuilder : ICommodityBrandSelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForCommodityBrands(IEnumerable<CommodityBrand> CommodityBrands)
        {
            return CommodityBrands.Select(pt => new SelectListItem { Text = pt.Name, Value = pt.CommodityBrandID.ToString() }).ToList();
        }
    }
}