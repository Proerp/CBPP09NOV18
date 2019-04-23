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
    public class BlendingInstructionsController : GenericViewDetailController<BlendingInstruction, BlendingInstructionDetail, BlendingInstructionViewDetail, BlendingInstructionDTO, BlendingInstructionPrimitiveDTO, BlendingInstructionDetailDTO, BlendingInstructionViewModel>
    {
        public BlendingInstructionsController(IBlendingInstructionService blendingInstructionService, IBlendingInstructionViewModelSelectListBuilder blendingInstructionViewModelSelectListBuilder)
            : base(blendingInstructionService, blendingInstructionViewModelSelectListBuilder, true)
        {
        }

        public override void AddRequireJsOptions()
        {
            base.AddRequireJsOptions();

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Materials);

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);

            RequireJsOptions.Add("masterCommodityTypeIDs", ((int)GlobalEnums.CommodityTypeID.Products).ToString(), RequireJsOptionsScope.Page);
        }
    }

}