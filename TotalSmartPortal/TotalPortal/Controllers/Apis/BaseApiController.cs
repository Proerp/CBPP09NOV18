using TotalCore.Services;
using TotalPortal.APIs.Sessions;


namespace TotalPortal.Controllers.Apis
{
    public abstract class BaseApiController : CoreApiController
    {
        private readonly IBaseService baseService;
        public BaseApiController(IBaseService baseService)
        { this.baseService = baseService; }


        public IBaseService BaseService { get { return this.baseService; } }
    }
}
