using System.Web.Http;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalPortal.Areas.Commons.Controllers.Apis
{
    [Authorize]
    [RoutePrefix("Api/Commons/Shifts")]
    public class ShiftsApiController : ApiController
    {
        private readonly IShiftRepository shiftAPIRepository;

        public ShiftsApiController(IShiftRepository shiftAPIRepository)
        {
            this.shiftAPIRepository = shiftAPIRepository;
        }

        [HttpGet]
        [Route("GetAllShifts")]
        public IList<Shift> GetAllShifts()
        {
            return this.shiftAPIRepository.GetAllShifts(false);
        }
    }
}