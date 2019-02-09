using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface ICommodityIconDropDownViewModel
    {
        [Display(Name = "Ký hiệu cảnh báo")]
        int CommodityIconID { get; set; }
        IEnumerable<SelectListItem> CommodityIconSelectList { get; set; }
    }
}