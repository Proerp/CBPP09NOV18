using System;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalBase.Enums;
using TotalDTO.Commons;
using TotalPortal.Builders;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Areas.Commons.ViewModels.Helpers;

namespace TotalPortal.Areas.Commons.ViewModels
{
    public class BomViewModel : BomDTO, IViewDetailViewModel<BomDetailDTO>
    {
    }
}