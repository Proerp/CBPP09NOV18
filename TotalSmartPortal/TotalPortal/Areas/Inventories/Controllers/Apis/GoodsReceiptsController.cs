using System;
using System.Linq;
using Microsoft.AspNet.Identity;

using System.Net;
//using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http;
using System.Text;

using AutoMapper;
using RequireJsNet;

using TotalBase.Enums;
using TotalDTO;
using TotalModel;
using TotalModel.Models;

using TotalCore.Services.Inventories;
using TotalCore.Repositories.Commons;

using TotalDTO.Inventories;

using TotalPortal.Controllers.Apis;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Areas.Inventories.ViewModels;
using TotalPortal.Areas.Inventories.Builders;
using TotalCore.Repositories.Inventories;
using System.Collections.Generic;

namespace TotalPortal.Areas.Inventories.Controllers.Apis
{
    [Authorize]
    //[RoutePrefix("Api/Inventories/GoodsReceipts")]
    public class GoodsReceiptsController<TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel> : GenericViewDetailApiController<GoodsReceipt, GoodsReceiptDetail, GoodsReceiptViewDetail, TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
        where TViewDetailViewModel : TDto, IViewDetailViewModel<TDtoDetail>, IGoodsReceiptViewModel, new()
    {
        protected readonly IGoodsReceiptAPIRepository goodsReceiptAPIRepository;

        public GoodsReceiptsController(IGoodsReceiptService<TDto, TPrimitiveDto, TDtoDetail> goodsReceiptService, IGoodsReceiptViewModelSelectListBuilder<TViewDetailViewModel> goodsReceiptViewModelSelectListBuilder, IGoodsReceiptAPIRepository goodsReceiptAPIRepository)
            : base(goodsReceiptService, goodsReceiptViewModelSelectListBuilder, true)
        {
            this.goodsReceiptAPIRepository = goodsReceiptAPIRepository;
        }

        public override void AddRequireJsOptions()
        {
            //MVC: base.AddRequireJsOptions();

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Items);
            commodityTypeIDList.Append(","); commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Consumables);

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);


            StringBuilder warehouseTaskIDList = new StringBuilder();
            warehouseTaskIDList.Append((int)GlobalEnums.WarehouseTaskID.DeliveryAdvice);

            //MVC: ViewBag.WarehouseTaskID = (int)GlobalEnums.WarehouseTaskID.DeliveryAdvice;
            //MVC: ViewBag.WarehouseTaskIDList = warehouseTaskIDList.ToString();
        }

        [HttpGet]
        [Route("GetGoodsArrivals/{locationID}")]
        public IEnumerable<GoodsReceiptPendingGoodsArrival> GetGoodsArrivals(int? locationID)
        {
            return this.goodsReceiptAPIRepository.GetGoodsArrivals(locationID);
        }

        public IEnumerable<GoodsReceiptPendingGoodsArrivalDetail> GetPendingGoodsArrivalDetails(int? locationID, int? goodsReceiptID, int? goodsArrivalID, string goodsArrivalDetailIDs, bool isReadonly)
        {
            return this.goodsReceiptAPIRepository.GetPendingGoodsArrivalDetails(locationID, goodsReceiptID, goodsArrivalID, goodsArrivalDetailIDs, false);
        }
    }





    //[Authorize]
    [RoutePrefix("Api/Inventories/GoodsReceipts")]
    public class MaterialReceiptsApiController : GoodsReceiptsController<GoodsReceiptDTO<GROptionMaterial>, GoodsReceiptPrimitiveDTO<GROptionMaterial>, GoodsReceiptDetailDTO, MaterialReceiptViewModel>
    {
        public MaterialReceiptsApiController(IMaterialReceiptService materialReceiptService, IMaterialReceiptViewModelSelectListBuilder materialReceiptViewModelSelectListBuilder, IGoodsReceiptAPIRepository goodsReceiptAPIRepository)
            : base(materialReceiptService, materialReceiptViewModelSelectListBuilder, goodsReceiptAPIRepository)
        {
        }

        //////[HttpGet]
        //////[Route("GetGoodsArrivals/{locationID}")]
        //////public IEnumerable<GoodsReceiptPendingGoodsArrival> GetGoodsArrivals(int? locationID)
        //////{
        //////    return base.goodsReceiptAPIRepository.GetGoodsArrivals(locationID);
        //////}
    }

    public class ItemReceiptsApiController : GoodsReceiptsController<GoodsReceiptDTO<GROptionItem>, GoodsReceiptPrimitiveDTO<GROptionItem>, GoodsReceiptDetailDTO, ItemReceiptViewModel>
    {
        public ItemReceiptsApiController(IItemReceiptService itemReceiptService, IItemReceiptViewModelSelectListBuilder itemReceiptViewModelSelectListBuilder, IGoodsReceiptAPIRepository goodsReceiptAPIRepository)
            : base(itemReceiptService, itemReceiptViewModelSelectListBuilder, goodsReceiptAPIRepository)
        {
        }
    }


    public class ProductReceiptsApiController : GoodsReceiptsController<GoodsReceiptDTO<GROptionProduct>, GoodsReceiptPrimitiveDTO<GROptionProduct>, GoodsReceiptDetailDTO, ProductReceiptViewModel>
    {
        public ProductReceiptsApiController(IProductReceiptService productReceiptService, IProductReceiptViewModelSelectListBuilder productReceiptViewModelSelectListBuilder, IGoodsReceiptAPIRepository goodsReceiptAPIRepository)
            : base(productReceiptService, productReceiptViewModelSelectListBuilder, goodsReceiptAPIRepository)
        {
        }
    }
}
