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
    public interface IProductionOrderViewModel : IProductionOrderDTO, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel
    {
    }

    public class ItemOrderViewModel : ProductionOrderDTO<ProrderOptionItem>, IViewDetailViewModel<ProductionOrderDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IProductionOrderViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }

    public class ProductOrderViewModel : ProductionOrderDTO<ProrderOptionProduct>, IViewDetailViewModel<ProductionOrderDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IProductionOrderViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }
}