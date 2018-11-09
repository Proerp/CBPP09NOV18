using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalModel.Models;
using System;

namespace TotalPortal.Areas.Commons.Builders
{
    public interface ICommodityClassSelectListBuilder
    {
        IEnumerable<SelectListItem> BuildSelectListItemsForCommodityClasss(IEnumerable<CommodityClass> CommodityClasss);
    }

    public class CommodityClassSelectListBuilder : ICommodityClassSelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForCommodityClasss(IEnumerable<CommodityClass> CommodityClasss)
        {
            return CommodityClasss.Select(pt => new SelectListItem { Text = (!String.IsNullOrWhiteSpace(pt.Code) ? pt.Code + " [" + pt.Name + "]" : ""), Value = pt.CommodityClassID.ToString() }).ToList();
        }
    }
}