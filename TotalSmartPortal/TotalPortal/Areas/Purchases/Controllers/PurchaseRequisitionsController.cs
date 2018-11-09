using System.Net;
using System.Web.Mvc;
using System.Text;

using RequireJsNet;

using TotalBase.Enums;
using TotalDTO.Purchases;
using TotalModel.Models;

using TotalCore.Services.Purchases;

using TotalPortal.Controllers;
using TotalPortal.Areas.Purchases.ViewModels;
using TotalPortal.Areas.Purchases.Builders;

namespace TotalPortal.Areas.Purchases.Controllers
{
    public class PurchaseRequisitionsController : GenericViewDetailController<PurchaseRequisition, PurchaseRequisitionDetail, PurchaseRequisitionViewDetail, PurchaseRequisitionDTO, PurchaseRequisitionPrimitiveDTO, PurchaseRequisitionDetailDTO, PurchaseRequisitionViewModel>
    {
        public PurchaseRequisitionsController(IPurchaseRequisitionService purchaseRequisitionService, IPurchaseRequisitionViewModelSelectListBuilder purchaseRequisitionViewModelSelectListBuilder)
            : base(purchaseRequisitionService, purchaseRequisitionViewModelSelectListBuilder, true)
        {
        }

        public override void AddRequireJsOptions()
        {
            base.AddRequireJsOptions();

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Items);
            commodityTypeIDList.Append(","); commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Materials);

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);
        }
    }

}