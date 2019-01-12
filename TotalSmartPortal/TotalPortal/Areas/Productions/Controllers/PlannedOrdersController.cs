﻿using System.Net;
using System.Web.Mvc;
using System.Text;

using RequireJsNet;

using TotalModel;
using TotalDTO;
using TotalBase.Enums;
using TotalDTO.Productions;
using TotalModel.Models;

using TotalCore.Services.Productions;

using TotalPortal.Controllers;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Areas.Productions.ViewModels;
using TotalPortal.Areas.Productions.Builders;

namespace TotalPortal.Areas.Productions.Controllers
{
    public class PlannedOrdersController<TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel> : GenericViewDetailController<PlannedOrder, PlannedOrderDetail, PlannedOrderViewDetail, TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
        where TViewDetailViewModel : TDto, IViewDetailViewModel<TDtoDetail>, IPlannedOrderViewModel, new()
    {
        public PlannedOrdersController(IPlannedOrderService<TDto, TPrimitiveDto, TDtoDetail> plannedOrderService, IPlannedOrderViewModelSelectListBuilder<TViewDetailViewModel> plannedOrderViewModelSelectListBuilder)
            : base(plannedOrderService, plannedOrderViewModelSelectListBuilder, true)
        {
        }

        public override void AddRequireJsOptions()
        {
            base.AddRequireJsOptions();

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Products);

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);
        }
    }



    public class PlannedItemsController : PlannedOrdersController<PlannedOrderDTO<PlannedOptionItem>, PlannedOrderPrimitiveDTO<PlannedOptionItem>, PlannedOrderDetailDTO, PlannedItemViewModel>
    {
        public PlannedItemsController(IPlannedItemService plannedItemService, IPlannedItemViewModelSelectListBuilder plannedItemViewModelSelectListBuilder)
            : base(plannedItemService, plannedItemViewModelSelectListBuilder)
        {
        }
    }


    public class PlannedProductsController : PlannedOrdersController<PlannedOrderDTO<PlannedOptionProduct>, PlannedOrderPrimitiveDTO<PlannedOptionProduct>, PlannedOrderDetailDTO, PlannedProductViewModel>
    {
        public PlannedProductsController(IPlannedProductService plannedProductService, IPlannedProductViewModelSelectListBuilder plannedProductViewModelSelectListBuilder)
            : base(plannedProductService, plannedProductViewModelSelectListBuilder)
        {
        }
    }
}