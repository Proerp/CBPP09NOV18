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
    public interface ISemifinishedHandoverViewModel : ISemifinishedHandoverDTO, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel
    {
    }

    public class SemifinishedItemHandoverViewModel : SemifinishedHandoverDTO<SemifinishedItemHandoverOption>, IViewDetailViewModel<SemifinishedHandoverDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, ISemifinishedHandoverViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }

    public class SemifinishedProductHandoverViewModel : SemifinishedHandoverDTO<SemifinishedProductHandoverOption>, IViewDetailViewModel<SemifinishedHandoverDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, ISemifinishedHandoverViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }
}