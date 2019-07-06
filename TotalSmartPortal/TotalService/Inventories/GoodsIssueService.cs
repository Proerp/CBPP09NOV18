using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using AutoMapper;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;
using TotalDTO.Inventories;
using TotalCore.Repositories.Inventories;
using TotalCore.Services.Inventories;
using TotalCore.Repositories.Commons;
using TotalCore.Services.Sales;
using TotalService.Helpers;

namespace TotalService.Inventories
{
    public class GoodsIssueService : GenericWithViewDetailService<GoodsIssue, GoodsIssueDetail, GoodsIssueViewDetail, GoodsIssueDTO, GoodsIssuePrimitiveDTO, GoodsIssueDetailDTO>, IGoodsIssueService
    {
        private DateTime? checkedDate; //For check over stock
        private string warehouseIDList = "";
        private string commodityIDList = "";

        private readonly IInventoryRepository inventoryRepository;
        private readonly IGoodsIssueHelperService goodsIssueHelperService;

        private readonly IGoodsIssueRepository goodsIssueRepository;

        public GoodsIssueService(IGoodsIssueRepository goodsIssueRepository, IInventoryRepository inventoryRepository, IGoodsIssueHelperService goodsIssueHelperService)
            : base(goodsIssueRepository, "GoodsIssuePostSaveValidate", "GoodsIssueSaveRelative", "GoodsIssueToggleApproved", null, null, null, "GetGoodsIssueViewDetails")
        {
            this.goodsIssueRepository = goodsIssueRepository;

            this.inventoryRepository = inventoryRepository;
            this.goodsIssueHelperService = goodsIssueHelperService;
        }

        public override ICollection<GoodsIssueViewDetail> GetViewDetails(int goodsIssueID)
        {
            throw new System.ArgumentException("Invalid call GetViewDetails(id). Use GetGoodsIssueViewDetails instead.", "Purchase Invoice Service");
        }

        public ICollection<GoodsIssueViewDetail> GetGoodsIssueViewDetails(int goodsIssueID, int locationID, int deliveryAdviceID, int customerID, int receiverID, int warehouseID, string shippingAddress, string addressee, int? tradePromotionID, decimal? vatPercent, bool isReadOnly)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("GoodsIssueID", goodsIssueID), new ObjectParameter("LocationID", locationID), new ObjectParameter("DeliveryAdviceID", deliveryAdviceID), new ObjectParameter("CustomerID", customerID), new ObjectParameter("ReceiverID", receiverID), new ObjectParameter("WarehouseID", warehouseID), new ObjectParameter("ShippingAddress", shippingAddress), new ObjectParameter("Addressee", addressee), tradePromotionID.HasValue ? new ObjectParameter("TradePromotionID", tradePromotionID) : new ObjectParameter("TradePromotionID", typeof(int)), new ObjectParameter("VATPercent", vatPercent), new ObjectParameter("IsReadOnly", isReadOnly) };
            return this.GetViewDetails(parameters);
        }


        public List<GoodsIssueViewPackage> GetGoodsIssueViewPackages(int? goodsIssueID)
        {
            return this.goodsIssueRepository.GetGoodsIssueViewPackages(goodsIssueID);
        }


        public List<PendingDeliveryAdviceDescription> GetDescriptions(int locationID, int customerID, int receiverID, int warehouseID, string shippingAddress, string addressee, int? tradePromotionID, decimal? vatPercent)
        {
            return this.goodsIssueRepository.GetDescriptions(locationID, customerID, receiverID, warehouseID, shippingAddress, addressee, tradePromotionID, vatPercent);
        }


        public override bool Save(GoodsIssueDTO dto)
        {
            dto.GoodsIssueViewPackages.RemoveAll(x => x.Quantity == 0);
            return base.Save(dto);
        }

        protected override void UpdateDetail(GoodsIssueDTO dto, GoodsIssue entity)
        {
            this.goodsIssueHelperService.GetWCParameters(dto, null, ref this.checkedDate, ref this.warehouseIDList, ref this.commodityIDList);

            base.UpdateDetail(dto, entity);

            if (dto.GoodsIssueViewPackages != null && dto.GoodsIssueViewPackages.Count > 0)
                dto.GoodsIssueViewPackages.Each(detailDTO =>
                {
                    GoodsIssuePackage goodsIssuePackage;

                    if (detailDTO.GoodsIssuePackageID <= 0 || (goodsIssuePackage = entity.GoodsIssuePackages.First(detailModel => detailModel.GoodsIssuePackageID == detailDTO.GoodsIssuePackageID)) == null)
                    {
                        goodsIssuePackage = new GoodsIssuePackage();
                        entity.GoodsIssuePackages.Add(goodsIssuePackage);
                    }

                    Mapper.Map<GoodsIssuePackageDTO, GoodsIssuePackage>(detailDTO, goodsIssuePackage);
                });
        }

        protected override void UndoDetail(GoodsIssueDTO dto, GoodsIssue entity, bool isDelete)
        {
            this.goodsIssueHelperService.GetWCParameters(null, entity, ref this.checkedDate, ref this.warehouseIDList, ref this.commodityIDList);

            base.UndoDetail(dto, entity, isDelete);

            if (entity.GetID() > 0 && entity.GoodsIssuePackages.Count > 0)
                if (isDelete || dto.GoodsIssueViewPackages == null || dto.GoodsIssueViewPackages.Count == 0)
                    this.goodsIssueRepository.TotalSmartPortalEntities.GoodsIssuePackages.RemoveRange(entity.GoodsIssuePackages);
                else
                    entity.GoodsIssuePackages.ToList()//Have to use .ToList(): to convert enumerable to List before do remove. To correct this error: Collection was modified; enumeration operation may not execute. 
                            .Where(detailModel => !dto.GoodsIssueViewPackages.Any(detailDTO => detailDTO.GoodsIssuePackageID == detailModel.GoodsIssuePackageID))
                            .Each(deleted => this.goodsIssueRepository.TotalSmartPortalEntities.GoodsIssuePackages.Remove(deleted)); //remove deleted details

        }

        protected override void PostSaveValidate(GoodsIssue entity)
        {
            if (GlobalEnums.SKUWarehouse) this.inventoryRepository.CheckOverStock(this.checkedDate, this.warehouseIDList, this.commodityIDList);
            base.PostSaveValidate(entity);
        }
    }

    public class GoodsIssueHelperService : HelperService<GoodsIssue, GoodsIssueDetail, GoodsIssueDTO, GoodsIssueDetailDTO>, IGoodsIssueHelperService
    {
    }
}
