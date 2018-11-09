using TotalModel.Models;

using TotalDTO.Commons;
using TotalCore.Services.Commons;

using TotalPortal.Controllers;
using TotalPortal.Areas.Commons.ViewModels;
using TotalPortal.Areas.Commons.Builders;


namespace TotalPortal.Areas.Commons.Controllers
{
    public class MoldsController : GenericSimpleController<Mold, MoldDTO, MoldPrimitiveDTO, MoldViewModel>
    {
        public MoldsController(IMoldService moldService, IMoldSelectListBuilder moldViewModelSelectListBuilder)
            : base(moldService, moldViewModelSelectListBuilder)
        {
        }
    }
}

