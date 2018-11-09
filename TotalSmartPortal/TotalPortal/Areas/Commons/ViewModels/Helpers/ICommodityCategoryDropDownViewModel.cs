using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface ICommodityCategoryDropDownViewModel
    {
        [Display(Name = "Phân loại")]
        int CommodityCategoryID { get; set; }
        IEnumerable<SelectListItem> CommodityCategorySelectList { get; set; }
    }
}

