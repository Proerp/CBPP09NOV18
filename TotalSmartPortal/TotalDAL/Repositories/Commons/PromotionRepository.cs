using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class PromotionRepository : GenericWithDetailRepository<Promotion, PromotionCommodityCodePart>, IPromotionRepository
    {
        public PromotionRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "PromotionEditable", "PromotionApproved", "PromotionDeletable", "PromotionVoidable")
        { }

    }


    public class PromotionAPIRepository : GenericAPIRepository, IPromotionAPIRepository
    {
        public PromotionAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetPromotionIndexes")
        {
        }


        public IList<Promotion> GetPromotionByCustomers(int? customerID, int? applyToSalesVersusReturns, int? filterApplyToTradeDiscount)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            List<Promotion> promotions = this.TotalSmartPortalEntities.GetPromotionByCustomers(customerID, applyToSalesVersusReturns, filterApplyToTradeDiscount).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return promotions;
        }

        public IList<CustomerCategory> GetPromotionCustomerCategories(int? promotionID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            List<CustomerCategory> promotionCustomerCategory = this.TotalSmartPortalEntities.GetPromotionCustomerCategories(promotionID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return promotionCustomerCategory;
        }




        public void AddPromotionCustomerCategories(int? promotionID, int? customerCategoryID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("PromotionID", promotionID), new ObjectParameter("CustomerCategoryID", customerCategoryID) };
            this.ExecuteFunction("AddPromotionCustomerCategories", parameters);
        }

        public void RemovePromotionCustomerCategories(int? promotionID, int? customerCategoryID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("PromotionID", promotionID), new ObjectParameter("CustomerCategoryID", customerCategoryID) };
            this.ExecuteFunction("RemovePromotionCustomerCategories", parameters);
        }


        public void AddPromotionCustomers(int? promotionID, int? customerID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("PromotionID", promotionID), new ObjectParameter("CustomerID", customerID) };
            this.ExecuteFunction("AddPromotionCustomers", parameters);
        }

        public void RemovePromotionCustomers(int? promotionID, int? customerID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("PromotionID", promotionID), new ObjectParameter("CustomerID", customerID) };
            this.ExecuteFunction("RemovePromotionCustomers", parameters);
        }

    }

}
