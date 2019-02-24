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
    public interface IWorkOrderViewModel : IWorkOrderDTO, IViewDetailViewModel<WorkOrderDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel
    {
    }

    public class MaterialWorkOrderViewModel : WorkOrderDTO<WOOptionMaterial>, IViewDetailViewModel<WorkOrderDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IWorkOrderViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }

    public class ItemWorkOrderViewModel : WorkOrderDTO<WOOptionItem>, IViewDetailViewModel<WorkOrderDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IWorkOrderViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }

    public class ProductWorkOrderViewModel : WorkOrderDTO<WOOptionProduct>, IViewDetailViewModel<WorkOrderDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IWorkOrderViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }
}