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
    public class GoodsReceiptsApiController<TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel> : GenericViewDetailApiController<GoodsReceipt, GoodsReceiptDetail, GoodsReceiptViewDetail, TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
        where TViewDetailViewModel : TDto, IViewDetailViewModel<TDtoDetail>, IGoodsReceiptViewModel, new()
    {
        protected readonly IGoodsReceiptAPIRepository goodsReceiptAPIRepository;

        public GoodsReceiptsApiController(IGoodsReceiptService<TDto, TPrimitiveDto, TDtoDetail> goodsReceiptService, IGoodsReceiptViewModelSelectListBuilder<TViewDetailViewModel> goodsReceiptViewModelSelectListBuilder, IGoodsReceiptAPIRepository goodsReceiptAPIRepository)
            : base(goodsReceiptService, goodsReceiptViewModelSelectListBuilder, true)
        {
            this.goodsReceiptAPIRepository = goodsReceiptAPIRepository;
        }

        protected override void ReloadAfterSave(TViewDetailViewModel simpleViewModel)
        {
            if (simpleViewModel.Reference == null)
            {
                simpleViewModel.Reference = this.goodsReceiptAPIRepository.GetReference(simpleViewModel.GoodsReceiptID);
            }
            base.ReloadAfterSave(simpleViewModel);
        }

        [HttpGet]
        [Route("GetGoodsReceiptIndexes/{nmvnTaskID}/{fromDay}/{fromMonth}/{fromYear}/{toDay}/{toMonth}/{toYear}")]
        public ICollection<GoodsReceiptIndex> GetGoodsReceiptIndexes(string nmvnTaskID, int fromDay, int fromMonth, int fromYear, int toDay, int toMonth, int toYear)
        {
            this.goodsReceiptAPIRepository.RepositoryBag["NMVNTaskID"] = nmvnTaskID;
            return this.goodsReceiptAPIRepository.GetEntityIndexes<GoodsReceiptIndex>(User.Identity.GetUserId(), Helpers.InitDateTime(fromYear, fromMonth, fromDay), Helpers.InitDateTime(toYear, toMonth, toDay, 23, 59, 59));
        }

        [HttpGet]
        [Route("GetPurchasings/{locationID}")]
        public IEnumerable<GoodsReceiptPendingPurchasing> GetPurchasings(int? locationID)
        {
            return this.goodsReceiptAPIRepository.GetPurchasings(locationID, (int)GlobalEnums.NmvnTaskID.MaterialReceipt);
        }

        [HttpGet]
        [Route("GetGoodsArrivals/{locationID}")]
        public IEnumerable<GoodsReceiptPendingGoodsArrival> GetGoodsArrivals(int? locationID)
        {
            return this.goodsReceiptAPIRepository.GetGoodsArrivals(locationID, (int)GlobalEnums.NmvnTaskID.MaterialReceipt);
        }

        [HttpGet]
        [Route("GetPendingGoodsArrivalPackages/{locationID}/{goodsReceiptID}/{goodsArrivalID}/{barcode}/{goodsArrivalPackageIDs}")]
        public IEnumerable<GoodsReceiptPendingGoodsArrivalPackage> GetPendingGoodsArrivalPackages(int? locationID, int? goodsReceiptID, int? goodsArrivalID, string barcode, string goodsArrivalPackageIDs)
        {
            return this.goodsReceiptAPIRepository.GetPendingGoodsArrivalPackages(true, locationID, (int)GlobalEnums.NmvnTaskID.MaterialReceipt, goodsReceiptID, goodsArrivalID, barcode, goodsArrivalPackageIDs);
        }

        #region HELPER API
        [HttpGet]
        [Route("GetPendingPackages/{locationID}/{goodsReceiptID}/{goodsArrivalID}/{barcode}/{goodsArrivalPackageIDs}")]
        public IEnumerable<GoodsReceiptPendingPackage> GetPendingPackages(int? locationID, int? goodsReceiptID, int? goodsArrivalID, string barcode, string goodsArrivalPackageIDs)
        {
            return this.goodsReceiptAPIRepository.GetPendingGoodsArrivalPackages(true, locationID, (int)GlobalEnums.NmvnTaskID.MaterialReceipt, goodsReceiptID, goodsArrivalID, barcode, goodsArrivalPackageIDs).Select(p => new GoodsReceiptPendingPackage() { GoodsArrivalPackageID = p.GoodsArrivalPackageID, PurchaseOrderCodes = p.PurchaseOrderCodes, CommodityCode = p.CommodityCode, BatchCode = p.BatchCode, Barcode = p.Barcode, QuantityRemains = p.QuantityRemains });
        }

        [HttpGet]
        [Route("GetPendingSummary/{locationID}/{goodsReceiptID}/{goodsArrivalID}/{barcode}/{goodsArrivalPackageIDs}")]
        public GoodsReceiptPendingSummary GetPendingSummary(int? locationID, int? goodsReceiptID, int? goodsArrivalID, string barcode, string goodsArrivalPackageIDs)
        {
            IEnumerable<GoodsReceiptPendingGoodsArrivalPackage> goodsReceiptPendingGoodsArrivalPackages = this.goodsReceiptAPIRepository.GetPendingGoodsArrivalPackages(true, locationID, (int)GlobalEnums.NmvnTaskID.MaterialReceipt, goodsReceiptID, goodsArrivalID, barcode, goodsArrivalPackageIDs);
            return new GoodsReceiptPendingSummary() { PackageCount = goodsReceiptPendingGoodsArrivalPackages.Count(), TotalQuantityRemains = goodsReceiptPendingGoodsArrivalPackages.Sum(a => a.QuantityRemains) };
        }

        public class GoodsReceiptPendingPackage
        {
            public int GoodsArrivalPackageID { get; set; }
            public string PurchaseOrderCodes { get; set; }
            public string CommodityCode { get; set; }
            public string BatchCode { get; set; }
            public string Barcode { get; set; }

            public Nullable<decimal> QuantityRemains { get; set; }
        }

        public class GoodsReceiptPendingSummary
        {
            public int PackageCount { get; set; }
            public Nullable<decimal> TotalQuantityRemains { get; set; }
        }
        #endregion HELPER API


        [HttpGet]
        [Route("GetGoodsReceiptDetailAvailables/{locationID}/{warehouseID}/{warehouseReceiptID}/{commodityID}/{commodityIDs}/{batchID}/{barcode}/{goodsReceiptDetailIDs}/{onlyApproved}/{onlyIssuable}")]
        public IEnumerable<GoodsReceiptDetailAvailable> GetGoodsReceiptDetailAvailables(int? locationID, int? warehouseID, int? warehouseReceiptID, int? commodityID, string commodityIDs, int? batchID, string barcode, string goodsReceiptDetailIDs, bool onlyApproved, bool onlyIssuable)
        {
            return this.goodsReceiptAPIRepository.GetGoodsReceiptDetailAvailables(locationID, warehouseID, warehouseReceiptID, commodityID, commodityIDs, batchID, barcode, goodsReceiptDetailIDs, onlyApproved, onlyIssuable);
        }


        #region HELPER API
        [HttpGet]
        [Route("GetBarcodeAvailables/{barcode}/0")]
        public BarcodeAvailableSummary GetBarcodeAvailables(string barcode)
        {
            IEnumerable<GoodsReceiptBarcodeAvailable> barcodeAvailables = this.goodsReceiptAPIRepository.GetGoodsReceiptBarcodeAvailables(barcode);
            if (barcodeAvailables.Count() > 0)
                return new BarcodeAvailableSummary() { Reference = barcodeAvailables.Min(p => p.GoodsReceiptReference), EntryDate = barcodeAvailables.Min(p => p.GoodsReceiptEntryDate), BatchEntryDate = barcodeAvailables.Min(p => p.BatchEntryDate), ExpiryDate = barcodeAvailables.Min(p => p.ExpiryDate), BatchCode = barcodeAvailables.Min(p => p.BatchCode), LabCode = barcodeAvailables.Min(p => p.LabCode), Barcode = barcodeAvailables.Min(p => p.Barcode), CommodityCode = barcodeAvailables.Min(p => p.CommodityCode), BinLocationCode = barcodeAvailables.Min(p => p.BinLocationCode) + (barcodeAvailables.Count() > 1 ? "***(" + barcodeAvailables.Count().ToString("N0") + ")" : ""), UnitWeight = barcodeAvailables.Min(p => p.UnitWeight), TareWeight = barcodeAvailables.Min(p => p.TareWeight), QuantityAvailables = barcodeAvailables.Min(p => p.QuantityAvailables), Approved = barcodeAvailables.Min(p => p.Approved), LabApproved = barcodeAvailables.Min(p => p.LabApproved), LabHold = barcodeAvailables.Min(p => p.LabHold), LabInActive = barcodeAvailables.Min(p => p.LabInActive), LabInActiveCode = barcodeAvailables.Min(p => p.LabInActiveCode) };
            else return new BarcodeAvailableSummary();
        }

        public class BarcodeAvailableSummary
        {
            public string Reference { get; set; }
            public System.DateTime EntryDate { get; set; }
            public System.DateTime BatchEntryDate { get; set; }
            public Nullable<System.DateTime> ExpiryDate { get; set; }
            public string BatchCode { get; set; }
            public string LabCode { get; set; }
            public string Barcode { get; set; }
            public string CommodityCode { get; set; }
            public string BinLocationCode { get; set; }
            public decimal UnitWeight { get; set; }
            public decimal TareWeight { get; set; }
            public Nullable<decimal> QuantityAvailables { get; set; }
            public bool Approved { get; set; }
            public bool LabApproved { get; set; }
            public bool LabHold { get; set; }
            public bool LabInActive { get; set; }
            public string LabInActiveCode { get; set; }
        }
        #endregion HELPER API
    }





    [RoutePrefix("Api/Inventories/MaterialReceipts")]
    public class MaterialReceiptsApiController : GoodsReceiptsApiController<GoodsReceiptDTO<GROptionMaterial>, GoodsReceiptPrimitiveDTO<GROptionMaterial>, GoodsReceiptDetailDTO, MaterialReceiptViewModel>
    {
        public MaterialReceiptsApiController(IMaterialReceiptService materialReceiptService, IMaterialReceiptViewModelSelectListBuilder materialReceiptViewModelSelectListBuilder, IGoodsReceiptAPIRepository goodsReceiptAPIRepository)
            : base(materialReceiptService, materialReceiptViewModelSelectListBuilder, goodsReceiptAPIRepository)
        {
        }
    }

    public class ItemReceiptsApiController : GoodsReceiptsApiController<GoodsReceiptDTO<GROptionItem>, GoodsReceiptPrimitiveDTO<GROptionItem>, GoodsReceiptDetailDTO, ItemReceiptViewModel>
    {
        public ItemReceiptsApiController(IItemReceiptService itemReceiptService, IItemReceiptViewModelSelectListBuilder itemReceiptViewModelSelectListBuilder, IGoodsReceiptAPIRepository goodsReceiptAPIRepository)
            : base(itemReceiptService, itemReceiptViewModelSelectListBuilder, goodsReceiptAPIRepository)
        {
        }
    }


    public class ProductReceiptsApiController : GoodsReceiptsApiController<GoodsReceiptDTO<GROptionProduct>, GoodsReceiptPrimitiveDTO<GROptionProduct>, GoodsReceiptDetailDTO, ProductReceiptViewModel>
    {
        public ProductReceiptsApiController(IProductReceiptService productReceiptService, IProductReceiptViewModelSelectListBuilder productReceiptViewModelSelectListBuilder, IGoodsReceiptAPIRepository goodsReceiptAPIRepository)
            : base(productReceiptService, productReceiptViewModelSelectListBuilder, goodsReceiptAPIRepository)
        {
        }
    }
}
