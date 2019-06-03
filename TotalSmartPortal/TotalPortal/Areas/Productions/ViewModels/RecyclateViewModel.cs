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
    public interface IRecyclateViewModel : IRecyclateDTO, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel
    {
    }

    public class SemifinishedProductRecyclateViewModel : RecyclateDTO<SemifinishedProductRecyclateOption>, IViewDetailViewModel<RecyclateDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IRecyclateViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }

    public class FinishedProductRecyclateViewModel : RecyclateDTO<FinishedProductRecyclateOption>, IViewDetailViewModel<RecyclateDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IRecyclateViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }

    public class FinishedItemRecyclateViewModel : RecyclateDTO<FinishedItemRecyclateOption>, IViewDetailViewModel<RecyclateDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IRecyclateViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }
}