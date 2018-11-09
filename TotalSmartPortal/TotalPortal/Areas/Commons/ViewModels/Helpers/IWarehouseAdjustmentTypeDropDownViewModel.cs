using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{

    public interface IWarehouseAdjustmentTypeDropDownViewModel
    {
        [Display(Name = "Phân loại")]
        [Range(1, 90, ErrorMessage = "Vui lòng chọn phân loại")]
        int WarehouseAdjustmentTypeID { get; set; }
        IEnumerable<SelectListItem> WarehouseAdjustmentTypeSelectList { get; set; }
    }
}