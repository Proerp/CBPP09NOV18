using System;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalBase.Enums;
using TotalDTO.Inventories;
using TotalPortal.Builders;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Areas.Commons.ViewModels.Helpers;
using TotalPortal.Areas.Inventories.Builders;

namespace TotalPortal.Areas.Inventories.ViewModels
{
    public interface IWarehouseAdjustmentViewModel : IWarehouseAdjustmentDTO, IA01SimpleViewModel, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IWarehouseAdjustmentTypeDropDownViewModel
    {
    }

    public class OtherMaterialReceiptViewModel : WarehouseAdjustmentDTO<WAOptionMtlRct>, IViewDetailViewModel<WarehouseAdjustmentDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IWarehouseAdjustmentTypeDropDownViewModel, IWarehouseAdjustmentViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> WarehouseAdjustmentTypeSelectList { get; set; }
    }

    public class OtherMaterialIssueViewModel : WarehouseAdjustmentDTO<WAOptionMtlIss>, IViewDetailViewModel<WarehouseAdjustmentDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IWarehouseAdjustmentTypeDropDownViewModel, IWarehouseAdjustmentViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> WarehouseAdjustmentTypeSelectList { get; set; }
    }

    public class MaterialAdjustmentViewModel : WarehouseAdjustmentDTO<WAOptionMtlAdj>, IViewDetailViewModel<WarehouseAdjustmentDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IWarehouseAdjustmentTypeDropDownViewModel, IWarehouseAdjustmentViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> WarehouseAdjustmentTypeSelectList { get; set; }
    }




    public class OtherItemReceiptViewModel : WarehouseAdjustmentDTO<WAOptionItmRct>, IViewDetailViewModel<WarehouseAdjustmentDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IWarehouseAdjustmentTypeDropDownViewModel, IWarehouseAdjustmentViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> WarehouseAdjustmentTypeSelectList { get; set; }
    }

    public class OtherItemIssueViewModel : WarehouseAdjustmentDTO<WAOptionItmIss>, IViewDetailViewModel<WarehouseAdjustmentDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IWarehouseAdjustmentTypeDropDownViewModel, IWarehouseAdjustmentViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> WarehouseAdjustmentTypeSelectList { get; set; }
    }

    public class ItemAdjustmentViewModel : WarehouseAdjustmentDTO<WAOptionItmAdj>, IViewDetailViewModel<WarehouseAdjustmentDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IWarehouseAdjustmentTypeDropDownViewModel, IWarehouseAdjustmentViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> WarehouseAdjustmentTypeSelectList { get; set; }
    }




    public class OtherProductReceiptViewModel : WarehouseAdjustmentDTO<WAOptionPrdRct>, IViewDetailViewModel<WarehouseAdjustmentDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IWarehouseAdjustmentTypeDropDownViewModel, IWarehouseAdjustmentViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> WarehouseAdjustmentTypeSelectList { get; set; }
    }

    public class OtherProductIssueViewModel : WarehouseAdjustmentDTO<WAOptionPrdIss>, IViewDetailViewModel<WarehouseAdjustmentDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IWarehouseAdjustmentTypeDropDownViewModel, IWarehouseAdjustmentViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> WarehouseAdjustmentTypeSelectList { get; set; }
    }

    public class ProductAdjustmentViewModel : WarehouseAdjustmentDTO<WAOptionPrdAdj>, IViewDetailViewModel<WarehouseAdjustmentDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IWarehouseAdjustmentTypeDropDownViewModel, IWarehouseAdjustmentViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> WarehouseAdjustmentTypeSelectList { get; set; }
    }
}