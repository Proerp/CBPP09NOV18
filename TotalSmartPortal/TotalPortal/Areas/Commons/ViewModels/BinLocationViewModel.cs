using System.Web.Mvc;
using System.Collections.Generic;

using TotalDTO.Commons;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Areas.Commons.ViewModels.Helpers;

namespace TotalPortal.Areas.Commons.ViewModels
{
    public class BinLocationViewModel : BinLocationDTO, ISimpleViewModel, IBinTypeDropDownViewModel
    {
        public IEnumerable<SelectListItem> BinTypeSelectList { get; set; }
    }
}
