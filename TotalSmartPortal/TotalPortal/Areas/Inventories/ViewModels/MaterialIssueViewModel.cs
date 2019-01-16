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
    public interface IMaterialIssueViewModel : IMaterialIssueDTO, IViewDetailViewModel<MaterialIssueDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IShiftDropDownViewModel
    {
    }

    public class MaterialStagingViewModel : MaterialIssueDTO<MIOptionMaterial>, IViewDetailViewModel<MaterialIssueDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IMaterialIssueViewModel, IShiftDropDownViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> ShiftSelectList { get; set; }
    }

    public class ItemStagingViewModel : MaterialIssueDTO<MIOptionItem>, IViewDetailViewModel<MaterialIssueDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IMaterialIssueViewModel, IShiftDropDownViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> ShiftSelectList { get; set; }
    }

    public class ProductStagingViewModel : MaterialIssueDTO<MIOptionProduct>, IViewDetailViewModel<MaterialIssueDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IMaterialIssueViewModel, IShiftDropDownViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> ShiftSelectList { get; set; }
    }
}