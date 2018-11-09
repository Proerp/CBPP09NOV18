using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface IShiftDropDownViewModel
    {
        [Display(Name = "Ca SX")]
        [Range(1, 999999, ErrorMessage = "Vui lòng chọn ca sản xuất")]
        int ShiftID { get; set; }
        IEnumerable<SelectListItem> ShiftSelectList { get; set; }
    }
}