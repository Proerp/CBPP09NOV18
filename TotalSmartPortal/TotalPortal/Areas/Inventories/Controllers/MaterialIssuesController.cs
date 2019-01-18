using System.Net;
using System.Web.Mvc;
using System.Text;

using AutoMapper;
using RequireJsNet;

using TotalBase.Enums;
using TotalModel;
using TotalDTO;
using TotalModel.Models;

using TotalCore.Services.Inventories;
using TotalCore.Repositories.Commons;

using TotalDTO.Inventories;

using TotalPortal.ViewModels.Helpers;

using TotalPortal.Controllers;
using TotalPortal.APIs.Sessions;
using TotalPortal.Areas.Inventories.ViewModels;
using TotalPortal.Areas.Inventories.Builders;
using TotalPortal.Areas.Inventories.Controllers.Sessions;
using TotalPortal.Areas.Commons.Controllers.Sessions;

namespace TotalPortal.Areas.Inventories.Controllers
{
    public class MaterialIssuesController<TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel> : GenericViewDetailController<MaterialIssue, MaterialIssueDetail, MaterialIssueViewDetail, TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
        where TViewDetailViewModel : TDto, IViewDetailViewModel<TDtoDetail>, IMaterialIssueViewModel, new()
    {
        public MaterialIssuesController(IMaterialIssueService<TDto, TPrimitiveDto, TDtoDetail> materialIssueService, IMaterialIssueViewModelSelectListBuilder<TViewDetailViewModel> materialIssueViewModelSelectListBuilder)
            : base(materialIssueService, materialIssueViewModelSelectListBuilder, true)
        {
        }

        public override void AddRequireJsOptions()
        {
            base.AddRequireJsOptions();

            TViewDetailViewModel viewDetailViewModel = new TViewDetailViewModel();

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)(viewDetailViewModel.IsMaterial ? GlobalEnums.CommodityTypeID.Materials : (viewDetailViewModel.IsItem ? GlobalEnums.CommodityTypeID.Items : (viewDetailViewModel.IsProduct ? GlobalEnums.CommodityTypeID.Products : GlobalEnums.CommodityTypeID.Unknown))));

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);


            StringBuilder warehouseTaskIDList = new StringBuilder();
            warehouseTaskIDList.Append((int)GlobalEnums.WarehouseTaskID.DeliveryAdvice);

            ViewBag.WarehouseTaskID = (int)GlobalEnums.WarehouseTaskID.DeliveryAdvice;
            ViewBag.WarehouseTaskIDList = warehouseTaskIDList.ToString();
        }

        public virtual ActionResult GetPendingFirmOrderMaterials()
        {
            this.AddRequireJsOptions();
            TViewDetailViewModel viewDetailViewModel = new TViewDetailViewModel();
            return View(this.InitViewModel(viewDetailViewModel));
        }

        protected override TViewDetailViewModel InitViewModelByDefault(TViewDetailViewModel simpleViewModel)
        {
            simpleViewModel = base.InitViewModelByDefault(simpleViewModel);

            if (((IMaterialIssuePrimitiveDTO)simpleViewModel).ShiftID == 0)
            {
                string shiftSession = ShiftSession.GetShift(this.HttpContext);
                if (HomeSession.TryParseID(shiftSession) > 0) ((IMaterialIssuePrimitiveDTO)simpleViewModel).ShiftID = (int)HomeSession.TryParseID(shiftSession);
            }

            if (simpleViewModel.Storekeeper == null)
            {
                string storekeeperSession = MaterialIssueSession.GetStorekeeper(this.HttpContext);

                if (HomeSession.TryParseID(storekeeperSession) > 0)
                {
                    simpleViewModel.Storekeeper = new TotalDTO.Commons.EmployeeBaseDTO();
                    simpleViewModel.Storekeeper.EmployeeID = (int)HomeSession.TryParseID(storekeeperSession);
                    simpleViewModel.Storekeeper.Name = HomeSession.TryParseName(storekeeperSession);
                }
            }

            if (simpleViewModel.CrucialWorker == null)
            {
                string storekeeperSession = MaterialIssueSession.GetCrucialWorker(this.HttpContext);

                if (HomeSession.TryParseID(storekeeperSession) > 0)
                {
                    simpleViewModel.CrucialWorker = new TotalDTO.Commons.EmployeeBaseDTO();
                    simpleViewModel.CrucialWorker.EmployeeID = (int)HomeSession.TryParseID(storekeeperSession);
                    simpleViewModel.CrucialWorker.Name = HomeSession.TryParseName(storekeeperSession);
                }
            }

            return simpleViewModel;
        }

        protected override void BackupViewModelToSession(TViewDetailViewModel simpleViewModel)
        {
            base.BackupViewModelToSession(simpleViewModel);
            ShiftSession.SetShift(this.HttpContext, ((IMaterialIssuePrimitiveDTO)simpleViewModel).ShiftID);
            MaterialIssueSession.SetStorekeeper(this.HttpContext, simpleViewModel.Storekeeper.EmployeeID, simpleViewModel.Storekeeper.Name);
            MaterialIssueSession.SetCrucialWorker(this.HttpContext, simpleViewModel.CrucialWorker.EmployeeID, simpleViewModel.CrucialWorker.Name);
        }
    }





    public class MaterialStagingsController : MaterialIssuesController<MaterialIssueDTO<MIOptionMaterial>, MaterialIssuePrimitiveDTO<MIOptionMaterial>, MaterialIssueDetailDTO, MaterialStagingViewModel>
    {
        public MaterialStagingsController(IMaterialStagingService materialStagingService, IMaterialStagingViewModelSelectListBuilder materialStagingViewModelSelectListBuilder)
            : base(materialStagingService, materialStagingViewModelSelectListBuilder)
        {
        }
    }

    public class ItemStagingsController : MaterialIssuesController<MaterialIssueDTO<MIOptionItem>, MaterialIssuePrimitiveDTO<MIOptionItem>, MaterialIssueDetailDTO, ItemStagingViewModel>
    {
        public ItemStagingsController(IItemStagingService itemStagingService, IItemStagingViewModelSelectListBuilder itemStagingViewModelSelectListBuilder)
            : base(itemStagingService, itemStagingViewModelSelectListBuilder)
        {
        }
    }


    public class ProductStagingsController : MaterialIssuesController<MaterialIssueDTO<MIOptionProduct>, MaterialIssuePrimitiveDTO<MIOptionProduct>, MaterialIssueDetailDTO, ProductStagingViewModel>
    {
        public ProductStagingsController(IProductStagingService productStagingService, IProductStagingViewModelSelectListBuilder productStagingViewModelSelectListBuilder)
            : base(productStagingService, productStagingViewModelSelectListBuilder)
        {
        }
    }
}