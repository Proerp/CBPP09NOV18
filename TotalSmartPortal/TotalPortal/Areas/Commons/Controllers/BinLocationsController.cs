using TotalModel.Models;

using TotalDTO.Commons;
using TotalCore.Services.Commons;

using TotalPortal.Controllers;
using TotalPortal.Areas.Commons.ViewModels;
using TotalPortal.Areas.Commons.Builders;


namespace TotalPortal.Areas.Commons.Controllers
{
    public class BinLocationsController : GenericSimpleController<BinLocation, BinLocationDTO, BinLocationPrimitiveDTO, BinLocationViewModel>
    {
        public BinLocationsController(IBinLocationService binLocationService, IBinLocationSelectListBuilder binLocationViewModelSelectListBuilder)
            : base(binLocationService, binLocationViewModelSelectListBuilder)
        {
        }
    }
}

