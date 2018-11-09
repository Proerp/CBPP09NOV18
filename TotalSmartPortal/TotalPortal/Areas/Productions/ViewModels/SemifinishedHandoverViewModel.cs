using System;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalBase.Enums;
using TotalDTO.Productions;
using TotalPortal.Builders;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Areas.Commons.ViewModels.Helpers;


namespace TotalPortal.Areas.Productions.ViewModels
{   
    public class SemifinishedHandoverViewModel : SemifinishedHandoverDTO, IViewDetailViewModel<SemifinishedHandoverDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }        
    }
}