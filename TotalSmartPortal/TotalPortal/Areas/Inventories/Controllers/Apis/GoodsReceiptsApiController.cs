﻿using System;
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

        [HttpGet]
        [Route("GetGoodsReceiptIndexes/{nmvnTaskID}/{fromDay}/{fromMonth}/{fromYear}/{toDay}/{toMonth}/{toYear}")]
        public ICollection<GoodsReceiptIndex> GetGoodsReceiptIndexes(string nmvnTaskID, int fromDay, int fromMonth, int fromYear, int toDay, int toMonth, int toYear)
        {
            this.goodsReceiptAPIRepository.RepositoryBag["NMVNTaskID"] = nmvnTaskID;
            return this.goodsReceiptAPIRepository.GetEntityIndexes<GoodsReceiptIndex>(User.Identity.GetUserId(), Helpers.InitDateTime(fromYear, fromMonth, fromDay), Helpers.InitDateTime(toYear, toMonth, toDay, 23, 59, 59));
        }

        [HttpGet]
        [Route("GetGoodsArrivals/{locationID}")]
        public IEnumerable<GoodsReceiptPendingGoodsArrival> GetGoodsArrivals(int? locationID)
        {
            return this.goodsReceiptAPIRepository.GetGoodsArrivals(locationID);
        }

        [HttpGet]
        [Route("GetPendingGoodsArrivalDetails/{locationID}/{goodsReceiptID}/{goodsArrivalID}/{goodsArrivalDetailIDs}/{isReadonly}")]
        public IEnumerable<GoodsReceiptPendingGoodsArrivalDetail> GetPendingGoodsArrivalDetails(int? locationID, int? goodsReceiptID, int? goodsArrivalID, string goodsArrivalDetailIDs, bool isReadonly)
        {
            return this.goodsReceiptAPIRepository.GetPendingGoodsArrivalDetails(locationID, goodsReceiptID, goodsArrivalID, goodsArrivalDetailIDs, false);
        }
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