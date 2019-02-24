using System.Net;
using System.Web.Mvc;
using System.Text;
using System.Collections.Generic;

using AutoMapper;
using RequireJsNet;

using TotalBase.Enums;
using TotalModel;
using TotalDTO;
using TotalModel.Models;

using TotalCore.Services.Productions;
using TotalCore.Repositories.Commons;

using TotalDTO.Productions;

using TotalPortal.ViewModels.Helpers;

using TotalPortal.Controllers;
using TotalPortal.APIs.Sessions;
using TotalPortal.Areas.Productions.ViewModels;
using TotalPortal.Areas.Productions.Builders;
using TotalPortal.Areas.Productions.Controllers.Sessions;
using TotalPortal.Areas.Commons.Controllers.Sessions;

namespace TotalPortal.Areas.Productions.Controllers
{
    public class WorkOrdersController<TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel> : GenericViewDetailController<WorkOrder, WorkOrderDetail, WorkOrderViewDetail, TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
        where TViewDetailViewModel : TDto, IViewDetailViewModel<TDtoDetail>, IWorkOrderViewModel, new()
    {
        private readonly IWorkOrderService<TDto, TPrimitiveDto, TDtoDetail> workOrderService;

        public WorkOrdersController(IWorkOrderService<TDto, TPrimitiveDto, TDtoDetail> workOrderService, IWorkOrderViewModelSelectListBuilder<TViewDetailViewModel> workOrderViewModelSelectListBuilder)
            : base(workOrderService, workOrderViewModelSelectListBuilder, true)
        {
            this.workOrderService = workOrderService;
        }

        protected override ICollection<WorkOrderViewDetail> GetEntityViewDetails(TViewDetailViewModel workOrderViewModel)
        {
            ICollection<WorkOrderViewDetail> workOrderViewDetails = this.workOrderService.GetWorkOrderViewDetails(workOrderViewModel.WorkOrderID, workOrderViewModel.FirmOrderID, workOrderViewModel.WarehouseID);

            return workOrderViewDetails;
        }
    }





    public class MaterialWorkOrdersController : WorkOrdersController<WorkOrderDTO<WOOptionMaterial>, WorkOrderPrimitiveDTO<WOOptionMaterial>, WorkOrderDetailDTO, MaterialWorkOrderViewModel>
    {
        public MaterialWorkOrdersController(IMaterialWorkOrderService materialWorkOrderService, IMaterialWorkOrderViewModelSelectListBuilder materialWorkOrderViewModelSelectListBuilder)
            : base(materialWorkOrderService, materialWorkOrderViewModelSelectListBuilder)
        {
        }
    }

    public class ItemWorkOrdersController : WorkOrdersController<WorkOrderDTO<WOOptionItem>, WorkOrderPrimitiveDTO<WOOptionItem>, WorkOrderDetailDTO, ItemWorkOrderViewModel>
    {
        public ItemWorkOrdersController(IItemWorkOrderService itemWorkOrderService, IItemWorkOrderViewModelSelectListBuilder itemWorkOrderViewModelSelectListBuilder)
            : base(itemWorkOrderService, itemWorkOrderViewModelSelectListBuilder)
        {
        }
    }


    public class ProductWorkOrdersController : WorkOrdersController<WorkOrderDTO<WOOptionProduct>, WorkOrderPrimitiveDTO<WOOptionProduct>, WorkOrderDetailDTO, ProductWorkOrderViewModel>
    {
        public ProductWorkOrdersController(IProductWorkOrderService productWorkOrderService, IProductWorkOrderViewModelSelectListBuilder productWorkOrderViewModelSelectListBuilder)
            : base(productWorkOrderService, productWorkOrderViewModelSelectListBuilder)
        {
        }
    }
}