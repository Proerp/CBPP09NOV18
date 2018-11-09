using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalPortal.Areas.Commons.Builders
{   

    public interface ICommodityTypeSelectListBuilder
    {
        IEnumerable<SelectListItem> BuildSelectListItemsForCommodityTypes(IEnumerable<CommodityType> CommodityTypes);
    }

    public class CommodityTypeSelectListBuilder : ICommodityTypeSelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForCommodityTypes(IEnumerable<CommodityType> CommodityTypes)
        {
            return CommodityTypes.Select(pt => new SelectListItem { Text = pt.Name, Value = pt.CommodityTypeID.ToString() }).ToList();
        }
    }
}