using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;

using Microsoft.AspNet.Identity;
using System.Collections.Generic;


using TotalDTO;
using TotalModel;
using TotalModel.Models;

using TotalBase.Enums;
using TotalDTO.Inventories;

using TotalCore.Services.Inventories;
using TotalCore.Repositories.Commons;
using TotalCore.Repositories.Inventories;

using TotalPortal.APIs.Sessions;
using TotalPortal.Controllers.Apis;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Areas.Inventories.ViewModels;
using TotalPortal.Areas.Inventories.Builders;


namespace TotalPortal.Areas.Inventories.Controllers.Apis
{
    public class WarehouseTransfersApiController<TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel> : GenericViewDetailApiController<WarehouseTransfer, WarehouseTransferDetail, WarehouseTransferViewDetail, TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
        where TViewDetailViewModel : TDto, IViewDetailViewModel<TDtoDetail>, IWarehouseTransferViewModel, new()
    {
        protected readonly IWarehouseTransferAPIRepository warehouseTransferAPIRepository;

        public WarehouseTransfersApiController(IWarehouseTransferService<TDto, TPrimitiveDto, TDtoDetail> warehouseTransferService, IWarehouseTransferViewModelSelectListBuilder<TViewDetailViewModel> warehouseTransferViewModelSelectListBuilder, IWarehouseTransferAPIRepository warehouseTransferAPIRepository)
            : base(warehouseTransferService, warehouseTransferViewModelSelectListBuilder, true)
        {
            this.warehouseTransferAPIRepository = warehouseTransferAPIRepository;
        }

        protected override void ReloadAfterSave(TViewDetailViewModel simpleViewModel)
        {
            if (simpleViewModel.Reference == null)
            {
                simpleViewModel.Reference = this.warehouseTransferAPIRepository.GetReference(simpleViewModel.WarehouseTransferID);
            }
            base.ReloadAfterSave(simpleViewModel);
        }

        [HttpGet]
        [Route("GetWarehouseTransferIndexes/{nmvnTaskID}/{fromDay}/{fromMonth}/{fromYear}/{toDay}/{toMonth}/{toYear}")]
        public ICollection<WarehouseTransferIndex> GetWarehouseTransferIndexes(string nmvnTaskID, int fromDay, int fromMonth, int fromYear, int toDay, int toMonth, int toYear)
        {
            this.warehouseTransferAPIRepository.RepositoryBag["NMVNTaskID"] = nmvnTaskID;
            return this.warehouseTransferAPIRepository.GetEntityIndexes<WarehouseTransferIndex>(User.Identity.GetUserId(), Helpers.InitDateTime(fromYear, fromMonth, fromDay), Helpers.InitDateTime(toYear, toMonth, toDay, 23, 59, 59));
        }

        [HttpGet]
        [Route("GetAvailableWarehouses/{locationID}/{nmvnTaskID}")]
        public IEnumerable<WarehouseTransferAvailableWarehouse> GetAvailableWarehouses(int? locationID, int? nmvnTaskID)
        {
            return this.warehouseTransferAPIRepository.GetAvailableWarehouses(locationID, nmvnTaskID);
        }

        [HttpGet]
        [Route("GetPendingWarehouses/{locationID}/{nmvnTaskID}")]
        public IEnumerable<WarehouseTransferPendingWarehouse> GetPendingWarehouses(int? locationID, int? nmvnTaskID)
        {
            return this.warehouseTransferAPIRepository.GetPendingWarehouses(locationID, nmvnTaskID);
        }

        [HttpGet]
        [Route("GetTransferOrders/{locationID}/{nmvnTaskID}")]
        public IEnumerable<WarehouseTransferPendingTransferOrder> GetTransferOrders(int? locationID, int? nmvnTaskID)
        {
            return this.warehouseTransferAPIRepository.GetTransferOrders(locationID, nmvnTaskID);
        }

        [HttpGet]
        [Route("GetTransferOrderDetails/{locationID}/{nmvnTaskID}/{warehouseTransferID}/{transferOrderID}/{warehouseID}/{warehouseReceiptID}/{barcode}/{goodsReceiptDetailIDs}")]
        public IEnumerable<WarehouseTransferPendingTransferOrderDetail> GetTransferOrderDetails(int? locationID, int? nmvnTaskID, int? warehouseTransferID, int? transferOrderID, int? warehouseID, int? warehouseReceiptID, string barcode, string goodsReceiptDetailIDs)
        {
            return this.warehouseTransferAPIRepository.GetTransferOrderDetails(true, locationID, nmvnTaskID, warehouseTransferID, transferOrderID, warehouseID, warehouseReceiptID, barcode, goodsReceiptDetailIDs);
        }

        #region HELPER API
        [HttpGet]
        [Route("GetTransferOrderPendingSummaries/{locationID}/{nmvnTaskID}/{warehouseTransferID}/{transferOrderID}/{warehouseID}/{warehouseReceiptID}/{barcode}/{goodsReceiptDetailIDs}")]
        public IEnumerable<TransferOrderPendingSummary> GetTransferOrderPendingSummaries(int? locationID, int? nmvnTaskID, int? warehouseTransferID, int? transferOrderID, int? warehouseID, int? warehouseReceiptID, string barcode, string goodsReceiptDetailIDs)
        {
            IEnumerable<WarehouseTransferPendingTransferOrderDetail> transferOrderDetails = this.warehouseTransferAPIRepository.GetTransferOrderDetails(true, locationID, nmvnTaskID, warehouseTransferID, transferOrderID, warehouseID, warehouseReceiptID, barcode, goodsReceiptDetailIDs);
            return transferOrderDetails.GroupBy(g => g.CommodityCode).Select(s => new TransferOrderPendingSummary() { CommodityCode = s.Key, Weight = s.Min(f => f.Weight), TransferOrderRemains = s.Max(f => f.TransferOrderRemains), TransferOrderRemainPackages = s.Max(f => f.TransferOrderRemainPackages), QuantityAvailables = s.Sum(f => f.QuantityAvailables) });
        }

        public class TransferOrderPendingSummary
        {
            public string CommodityCode { get; set; }
            public Nullable<decimal> Weight { get; set; }
            public Nullable<decimal> TransferOrderRemains { get; set; }
            public Nullable<decimal> TransferOrderRemainPackages { get; set; }
            public Nullable<decimal> QuantityAvailables { get; set; }
        }
        #endregion HELPER API

    }





    [RoutePrefix("Api/Inventories/MaterialTransfers")]
    public class MaterialTransfersApiController : WarehouseTransfersApiController<WarehouseTransferDTO<WTOptionMaterial>, WarehouseTransferPrimitiveDTO<WTOptionMaterial>, WarehouseTransferDetailDTO, MaterialTransferViewModel>
    {
        public MaterialTransfersApiController(IMaterialTransferService materialTransferService, IMaterialTransferViewModelSelectListBuilder materialTransferViewModelSelectListBuilder, IWarehouseTransferAPIRepository warehouseTransferAPIRepository)
            : base(materialTransferService, materialTransferViewModelSelectListBuilder, warehouseTransferAPIRepository)
        {
        }
    }

    public class ItemTransfersApiController : WarehouseTransfersApiController<WarehouseTransferDTO<WTOptionItem>, WarehouseTransferPrimitiveDTO<WTOptionItem>, WarehouseTransferDetailDTO, ItemTransferViewModel>
    {
        public ItemTransfersApiController(IItemTransferService itemTransferService, IItemTransferViewModelSelectListBuilder itemTransferViewModelSelectListBuilder, IWarehouseTransferAPIRepository warehouseTransferAPIRepository)
            : base(itemTransferService, itemTransferViewModelSelectListBuilder, warehouseTransferAPIRepository)
        {
        }
    }


    public class ProductTransfersApiController : WarehouseTransfersApiController<WarehouseTransferDTO<WTOptionProduct>, WarehouseTransferPrimitiveDTO<WTOptionProduct>, WarehouseTransferDetailDTO, ProductTransferViewModel>
    {
        public ProductTransfersApiController(IProductTransferService productTransferService, IProductTransferViewModelSelectListBuilder productTransferViewModelSelectListBuilder, IWarehouseTransferAPIRepository warehouseTransferAPIRepository)
            : base(productTransferService, productTransferViewModelSelectListBuilder, warehouseTransferAPIRepository)
        {
        }
    }
}
