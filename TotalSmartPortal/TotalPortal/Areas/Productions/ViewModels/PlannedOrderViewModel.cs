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
    public class IPlannedOrderViewModel : IPlannedOrderDTO, IViewDetailViewModel<PlannedOrderDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel
    {
    }

    public class PlannedItemViewModel : PlannedOrderDTO<PlannedOptionItem>, IViewDetailViewModel<PlannedOrderDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IPlannedOrderViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }

    public class PlannedProductViewModel : PlannedOrderDTO<PlannedOptionProduct>, IViewDetailViewModel<PlannedOrderDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IPlannedOrderViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }   
}