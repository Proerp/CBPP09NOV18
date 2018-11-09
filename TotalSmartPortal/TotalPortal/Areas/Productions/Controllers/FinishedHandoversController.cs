using System.Net;
using System.Web.Mvc;
using System.Text;

using RequireJsNet;

using TotalBase.Enums;
using TotalDTO.Productions;
using TotalModel.Models;

using TotalCore.Services.Productions;

using TotalPortal.Controllers;
using TotalPortal.Areas.Productions.ViewModels;
using TotalPortal.Areas.Productions.Builders;

namespace TotalPortal.Areas.Productions.Controllers
{
    public class FinishedHandoversController : GenericViewDetailController<FinishedHandover, FinishedHandoverDetail, FinishedHandoverViewDetail, FinishedHandoverDTO, FinishedHandoverPrimitiveDTO, FinishedHandoverDetailDTO, FinishedHandoverViewModel>
    {
        public FinishedHandoversController(IFinishedHandoverService finishedHandoverService, IFinishedHandoverViewModelSelectListBuilder finishedHandoverViewModelSelectListBuilder)
            : base(finishedHandoverService, finishedHandoverViewModelSelectListBuilder, true)
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

        public virtual ActionResult GetPendingDetails()
        {
            this.AddRequireJsOptions();
            return View();
        }

    }
}