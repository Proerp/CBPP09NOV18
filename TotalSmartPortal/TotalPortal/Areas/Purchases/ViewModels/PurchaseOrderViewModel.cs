using System;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalBase.Enums;
using TotalDTO.Purchases;
using TotalPortal.Builders;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Areas.Commons.ViewModels.Helpers;

namespace TotalPortal.Areas.Purchases.ViewModels
{
    public interface IPurchaseOrderViewModel : IPurchaseOrderDTO, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel
    {
    }

    public class PurchaseMaterialViewModel : PurchaseOrderDTO<PurchaseOptionMaterial>, IViewDetailViewModel<PurchaseOrderDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IPurchaseOrderViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }

    public class PurchaseItemViewModel : PurchaseOrderDTO<PurchaseOptionItem>, IViewDetailViewModel<PurchaseOrderDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IPurchaseOrderViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }

    public class PurchaseProductViewModel : PurchaseOrderDTO<PurchaseOptionProduct>, IViewDetailViewModel<PurchaseOrderDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IPurchaseOrderViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }
}