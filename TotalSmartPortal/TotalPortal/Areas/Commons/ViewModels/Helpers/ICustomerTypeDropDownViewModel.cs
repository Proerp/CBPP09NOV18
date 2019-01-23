using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface ICustomerTypeDropDownViewModel
    {
        [Display(Name = "Khu vực")]
        Nullable<int> CustomerTypeID { get; set; }
        [Display(Name = "Khu vực")]
        IEnumerable<SelectListItem> CustomerTypeSelectList { get; set; }
    }
}

