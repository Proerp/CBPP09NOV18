using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class CommodityRepository : GenericRepository<Commodity>, ICommodityRepository
    {
        public CommodityRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "CommodityEditable", null, "CommodityDeletable")
        {
        }

        public IList<Commodity> SearchCommodities(string searchText, string commodityTypeIDList, bool? isOnlyAlphaNumericString)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;

            if (isOnlyAlphaNumericString != null && (bool)isOnlyAlphaNumericString) searchText = TotalBase.CommonExpressions.AlphaNumericString(searchText);

            var queryable = this.TotalSmartPortalEntities.Commodities.Where(wi => (bool)wi.InActive != true).Where(w => w.Code.Contains(searchText) || w.OfficialCode.Contains(searchText) || w.Name.Contains(searchText)).Include(i => i.CommodityCategory);
            if (commodityTypeIDList != null)
            {
                List<int> listCommodityTypeID = commodityTypeIDList.Split(',').Select(n => int.Parse(n)).ToList();
                queryable = queryable.Where(w => listCommodityTypeID.Contains(w.CommodityTypeID));
            }

            List<Commodity> commodities = queryable.ToList();

            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return commodities;
        }

        public IList<Commodity> SearchCommoditiesByIndex(int commodityCategoryID, int commodityTypeID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            List<Commodity> commodities = this.TotalSmartPortalEntities.Commodities.Where(w => w.InActive != true && (w.CommodityCategoryID == commodityCategoryID || w.CommodityTypeID == commodityTypeID)).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return commodities;
        }

        //public IList<CommoditiesInGoodsReceipt> GetCommoditiesInGoodsReceipts(int? locationID, string searchText, int? salesInvoiceID, int? stockTransferID, int? inventoryAdjustmentID)
        //{
        //    List<CommoditiesInGoodsReceipt> commoditiesInGoodsReceipts = this.TotalSmartPortalEntities.GetCommoditiesInGoodsReceipts(locationID, searchText, salesInvoiceID, stockTransferID, inventoryAdjustmentID).ToList();

        //    return commoditiesInGoodsReceipts;
        //}

        //public IList<CommoditiesInWarehouse> GetCommoditiesInWarehouses(int? locationID, DateTime? entryDate, string searchText, bool includeCommoditiesOutOfStock, int? salesInvoiceID, int? stockTransferID, int? inventoryAdjustmentID)
        //{
        //    List<CommoditiesInWarehouse> commoditiesInWarehouses;

        //    if (!includeCommoditiesOutOfStock)
        //        commoditiesInWarehouses = this.TotalSmartPortalEntities.GetCommoditiesInWarehouses(locationID, entryDate, searchText, salesInvoiceID, stockTransferID, inventoryAdjustmentID).ToList();
        //    else
        //        commoditiesInWarehouses = this.TotalSmartPortalEntities.GetCommoditiesInWarehousesIncludeOutOfStock(locationID, entryDate, searchText, salesInvoiceID, stockTransferID, inventoryAdjustmentID).ToList();

        //    return commoditiesInWarehouses;
        //}


        //public IList<CommoditiesAvailable> GetCommoditiesAvailables(int? locationID, DateTime? entryDate, string searchText)
        //{
        //    List<CommoditiesAvailable> commoditiesAvailables = this.TotalSmartPortalEntities.GetCommoditiesAvailables(locationID, entryDate, searchText).ToList();

        //    return commoditiesAvailables;
        //}

        //public IList<VehicleAvailable> GetVehicleAvailables(int? locationID, DateTime? entryDate, string searchText)
        //{
        //    List<VehicleAvailable> vehicleAvailables = this.TotalSmartPortalEntities.GetVehicleAvailables(locationID, entryDate, searchText).ToList();

        //    return vehicleAvailables;
        //}

        public IList<CommodityBase> GetCommodityBases(string commodityTypeIDList, int? nmvnTaskID, int? warehouseID, string searchText, bool? isOnlyAlphaNumericString)
        {
            if (isOnlyAlphaNumericString != null && (bool)isOnlyAlphaNumericString) searchText = TotalBase.CommonExpressions.AlphaNumericString(searchText);
            List<CommodityBase> commodityBases = this.TotalSmartPortalEntities.GetCommodityBases(commodityTypeIDList, nmvnTaskID, warehouseID, searchText).ToList();

            return commodityBases;
        }

        public IList<CommodityAvailable> GetCommodityAvailables(int? locationID, int? customerID, int? warehouseID, int? priceCategoryID, int? applyToSalesVersusReturns, int? promotionID, DateTime? entryDate, string searchText)
        {
            List<CommodityAvailable> commodityAvailables = this.TotalSmartPortalEntities.GetCommodityAvailables(locationID, customerID, warehouseID, priceCategoryID, applyToSalesVersusReturns, promotionID, entryDate, searchText).ToList();

            return commodityAvailables;
        }





        public IList<CommodityCodePart> GetCommodityCodePartA(string searchText)
        {
            List<CommodityCodePart> codePartAs = this.TotalSmartPortalEntities.GetCommodityCodePartA(searchText).ToList();

            return codePartAs;
        }

        public IList<CommodityCodePart> GetCommodityCodePartB(string searchText)
        {
            List<CommodityCodePart> codePartBs = this.TotalSmartPortalEntities.GetCommodityCodePartB(searchText).ToList();

            return codePartBs;
        }

        public IList<CommodityCodePart> GetCommodityCodePartC(string searchText)
        {
            List<CommodityCodePart> codePartCs = this.TotalSmartPortalEntities.GetCommodityCodePartC(searchText).ToList();

            return codePartCs;
        }

    }

    public class CommodityAPIRepository : GenericAPIRepository, ICommodityAPIRepository
    {
        public CommodityAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetCommodityIndexes")
        {
        }

        protected override ObjectParameter[] GetEntityIndexParameters(string aspUserID, DateTime fromDate, DateTime toDate)
        {
            ObjectParameter[] baseParameters = base.GetEntityIndexParameters(aspUserID, fromDate, toDate);
            ObjectParameter[] objectParameters = new ObjectParameter[] { new ObjectParameter("NMVNTaskID", this.RepositoryBag.ContainsKey("NMVNTaskID") && this.RepositoryBag["NMVNTaskID"] != null ? this.RepositoryBag["NMVNTaskID"] : 0), baseParameters[0], baseParameters[1], baseParameters[2] };

            this.RepositoryBag.Remove("NMVNTaskID");

            return objectParameters;
        }
    }
}
