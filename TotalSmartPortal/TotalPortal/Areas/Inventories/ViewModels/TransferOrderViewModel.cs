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
    public interface ITransferOrderViewModel : ITransferOrderDTO, IA01SimpleViewModel, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, ITransferOrderTypeDropDownViewModel
    {
    }

    public class MaterialTransferOrderViewModel : TransferOrderDTO<TOOptionMaterial>, IViewDetailViewModel<TransferOrderDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, ITransferOrderTypeDropDownViewModel, ITransferOrderViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> TransferOrderTypeSelectList { get; set; }
    }

    public class ItemTransferOrderViewModel : TransferOrderDTO<TOOptionItem>, IViewDetailViewModel<TransferOrderDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, ITransferOrderTypeDropDownViewModel, ITransferOrderViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> TransferOrderTypeSelectList { get; set; }
    }

    public class ProductTransferOrderViewModel : TransferOrderDTO<TOOptionProduct>, IViewDetailViewModel<TransferOrderDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, ITransferOrderTypeDropDownViewModel, ITransferOrderViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> TransferOrderTypeSelectList { get; set; }
    }   
}