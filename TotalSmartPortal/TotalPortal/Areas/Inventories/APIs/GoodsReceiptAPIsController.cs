using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using System.Web.UI;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;


using TotalBase.Enums;
using TotalModel.Models;

using TotalDTO.Inventories;

using TotalCore.Repositories.Inventories;
using TotalPortal.Areas.Inventories.ViewModels;
using TotalPortal.APIs.Sessions;

using Microsoft.AspNet.Identity;

namespace TotalPortal.Areas.Inventories.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class GoodsReceiptAPIsController : Controller
    {
        private readonly IGoodsReceiptAPIRepository goodsReceiptAPIRepository;

        public GoodsReceiptAPIsController(IGoodsReceiptAPIRepository goodsReceiptAPIRepository)
        {
            this.goodsReceiptAPIRepository = goodsReceiptAPIRepository;
        }


        public JsonResult GetGoodsReceiptIndexes([DataSourceRequest] DataSourceRequest request, string nmvnTaskID)
        {
            this.goodsReceiptAPIRepository.RepositoryBag["NMVNTaskID"] = nmvnTaskID;
            ICollection<GoodsReceiptIndex> goodsReceiptIndexes = this.goodsReceiptAPIRepository.GetEntityIndexes<GoodsReceiptIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = goodsReceiptIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }





        public JsonResult GetCustomers([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.goodsReceiptAPIRepository.GetCustomers(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPurchaseRequisitions([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.goodsReceiptAPIRepository.GetPurchaseRequisitions(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPendingPurchaseRequisitionDetails([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? goodsReceiptID, int? purchaseRequisitionID, int? customerID, string purchaseRequisitionDetailIDs, bool isReadonly)
        {
            var result = this.goodsReceiptAPIRepository.GetPendingPurchaseRequisitionDetails(locationID, goodsReceiptID, purchaseRequisitionID, customerID, purchaseRequisitionDetailIDs, false);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }







        public JsonResult GetGoodsArrivals([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.goodsReceiptAPIRepository.GetGoodsArrivals(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPendingGoodsArrivalPackages([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? goodsReceiptID, int? goodsArrivalID, string goodsArrivalPackageIDs)
        {
            var result = this.goodsReceiptAPIRepository.GetPendingGoodsArrivalPackages(false, locationID, goodsReceiptID, goodsArrivalID, goodsArrivalPackageIDs);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }






        public JsonResult GetWarehouses([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? nmvnTaskID)
        {
            var result = this.goodsReceiptAPIRepository.GetWarehouses(locationID, nmvnTaskID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWarehouseTransfers([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? nmvnTaskID)
        {
            var result = this.goodsReceiptAPIRepository.GetWarehouseTransfers(locationID, nmvnTaskID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPendingWarehouseTransferDetails([DataSourceRequest] DataSourceRequest dataSourceRequest, int? nmvnTaskID, int? goodsReceiptID, int? warehouseTransferID, int? warehouseID, int? warehouseIssueID, string warehouseTransferDetailIDs, bool oneStep)
        {
            var result = this.goodsReceiptAPIRepository.GetPendingWarehouseTransferDetails(nmvnTaskID, goodsReceiptID, warehouseTransferID, warehouseID, warehouseIssueID, warehouseTransferDetailIDs, oneStep);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }





        public JsonResult GetPlannedOrderCustomers([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.goodsReceiptAPIRepository.GetPlannedOrderCustomers(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPlannedOrders([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.goodsReceiptAPIRepository.GetPlannedOrders(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPendingPlannedOrderDetails([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? goodsReceiptID, int? plannedOrderID, int? customerID, string finishedProductPackageIDs, bool isReadonly)
        {
            var result = this.goodsReceiptAPIRepository.GetPendingPlannedOrderDetails(locationID, goodsReceiptID, plannedOrderID, customerID, finishedProductPackageIDs, false);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPendingMaterialIssueDetails([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? goodsReceiptID, string materialIssueDetailIDs, bool isReadonly)
        {
            var result = this.goodsReceiptAPIRepository.GetPendingMaterialIssueDetails(locationID, goodsReceiptID, materialIssueDetailIDs, false);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }






        public JsonResult GetGoodsReceiptDetailAvailables([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? warehouseID, int? commodityID, string commodityIDs, int? batchID, string barcode, string goodsReceiptDetailIDs, bool onlyApproved, bool onlyIssuable)
        {
            var result = this.goodsReceiptAPIRepository.GetGoodsReceiptDetailAvailables(locationID, warehouseID, commodityID, commodityIDs, batchID, barcode, goodsReceiptDetailIDs, onlyApproved, onlyIssuable);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }       
    }
}
