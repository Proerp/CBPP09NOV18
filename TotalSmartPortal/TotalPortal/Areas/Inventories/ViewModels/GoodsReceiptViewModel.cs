using System;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalBase.Enums;
using TotalDTO.Inventories;
using TotalPortal.Builders;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Areas.Commons.ViewModels.Helpers;

namespace TotalPortal.Areas.Inventories.ViewModels
{
    public interface IGoodsReceiptViewModel : IGoodsReceiptDTO, IA01SimpleViewModel, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel
    {
    }

    public class MaterialReceiptViewModel : GoodsReceiptDTO<GROptionMaterial>, IViewDetailViewModel<GoodsReceiptDetailDTO>, IA01SimpleViewModel, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IGoodsReceiptViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }

    public class ItemReceiptViewModel : GoodsReceiptDTO<GROptionItem>, IViewDetailViewModel<GoodsReceiptDetailDTO>, IA01SimpleViewModel, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IGoodsReceiptViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }

    public class ProductReceiptViewModel : GoodsReceiptDTO<GROptionProduct>, IViewDetailViewModel<GoodsReceiptDetailDTO>, IA01SimpleViewModel, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IGoodsReceiptViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }
}
