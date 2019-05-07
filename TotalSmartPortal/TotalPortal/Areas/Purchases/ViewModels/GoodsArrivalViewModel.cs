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
    public interface IGoodsArrivalViewModel : IGoodsArrivalDTO, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel
    {
    }

    public class MaterialArrivalViewModel : GoodsArrivalDTO<GoodsArrivalOptionMaterial>, IViewDetailViewModel<GoodsArrivalDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IGoodsArrivalViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }

    public class ItemArrivalViewModel : GoodsArrivalDTO<GoodsArrivalOptionItem>, IViewDetailViewModel<GoodsArrivalDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IGoodsArrivalViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }

    public class ProductArrivalViewModel : GoodsArrivalDTO<GoodsArrivalOptionProduct>, IViewDetailViewModel<GoodsArrivalDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IGoodsArrivalViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }   
}
