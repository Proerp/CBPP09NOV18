using System.Web.Http;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalPortal.Areas.Commons.Controllers.Apis
{
    [Authorize]
    [RoutePrefix("Api/Commons/BinLocations")]
    public class BinLocationsApiController : ApiController
    {
        private readonly IBinLocationAPIRepository binLocationAPIRepository;

        public BinLocationsApiController(IBinLocationAPIRepository binLocationAPIRepository)
        {
            this.binLocationAPIRepository = binLocationAPIRepository;
        }

        [HttpGet]
        [Route("GetBinLocationBases/{warehouseID}/{searchText}")]
        public IEnumerable<BinLocationBase> GetBinLocationBases(int? warehouseID, string searchText)
        {
            return this.binLocationAPIRepository.GetBinLocationBases(warehouseID, searchText);
        }
    }
}