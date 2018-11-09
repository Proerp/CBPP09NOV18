using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{   
    public interface ICommodityBrandDropDownViewModel
    {
        int CommodityBrandID { get; set; }
        IEnumerable<SelectListItem> CommodityBrandSelectList { get; set; }
    }
}