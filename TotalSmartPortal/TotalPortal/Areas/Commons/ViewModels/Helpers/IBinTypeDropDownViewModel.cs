using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface IBinTypeDropDownViewModel
    {
        [Display(Name = "Loại khách hàng")]
        Nullable<int> BinTypeID { get; set; }
        [Display(Name = "Loại khách hàng")]
        IEnumerable<SelectListItem> BinTypeSelectList { get; set; }
    }
}

