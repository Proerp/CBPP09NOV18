using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface ITransferOrderTypeDropDownViewModel
    {
        [Display(Name = "Phân loại")]
        int TransferOrderTypeID { get; set; }
        IEnumerable<SelectListItem> TransferOrderTypeSelectList { get; set; }
    }
}