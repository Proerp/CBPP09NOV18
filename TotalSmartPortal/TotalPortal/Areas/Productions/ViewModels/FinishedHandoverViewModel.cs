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
    public interface IFinishedHandoverViewModel : IFinishedHandoverDTO, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel
    {
    }

    public class FinishedItemHandoverViewModel : FinishedHandoverDTO<FinishedItemHandoverOption>, IViewDetailViewModel<FinishedHandoverDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IFinishedHandoverViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }

    public class FinishedProductHandoverViewModel : FinishedHandoverDTO<FinishedProductHandoverOption>, IViewDetailViewModel<FinishedHandoverDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IFinishedHandoverViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }
}