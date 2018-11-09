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
    public interface IWarehouseTransferViewModel : IWarehouseTransferDTO, IA01SimpleViewModel, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel
    {
    }

    public class MaterialTransferViewModel : WarehouseTransferDTO<WTOptionMaterial>, IViewDetailViewModel<WarehouseTransferDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IWarehouseTransferViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> WarehouseTransferTypeSelectList { get; set; }
    }

    public class ItemTransferViewModel : WarehouseTransferDTO<WTOptionItem>, IViewDetailViewModel<WarehouseTransferDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IWarehouseTransferViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> WarehouseTransferTypeSelectList { get; set; }
    }

    public class ProductTransferViewModel : WarehouseTransferDTO<WTOptionProduct>, IViewDetailViewModel<WarehouseTransferDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IWarehouseTransferViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> WarehouseTransferTypeSelectList { get; set; }
    }   
}