using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface ICommodityClassDropDownViewModel
    {
        [Display(Name = "Nhóm A/C/G/F/T")]
        int CommodityClassID { get; set; }
        IEnumerable<SelectListItem> CommodityClassSelectList { get; set; }
    }
}