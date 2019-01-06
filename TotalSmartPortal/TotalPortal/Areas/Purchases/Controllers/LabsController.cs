using TotalModel.Models;

using TotalDTO.Purchases;
using TotalCore.Services.Purchases;

using TotalPortal.Controllers;
using TotalPortal.Areas.Purchases.ViewModels;
using TotalPortal.Areas.Purchases.Builders;

namespace TotalPortal.Areas.Purchases.Controllers
{
    public class LabsController : GenericSimpleController<Lab, LabDTO, LabPrimitiveDTO, LabViewModel>
    {
        public LabsController(ILabService mmployeeService, ILabSelectListBuilder labViewModelSelectListBuilder)
            : base(mmployeeService, labViewModelSelectListBuilder)
        {
        }
    }
}