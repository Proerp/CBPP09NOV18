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

        public virtual void AddRequireJsOptions()
        {
            //int moduleDetailID = MenuSession.GetModuleDetailID(this.HttpContext);
            //int moduleID = this.baseService.GetModuleID(ref moduleDetailID);

            //MenuSession.SetModuleID(this.HttpContext, moduleID);
            //MenuSession.SetModuleDetailID(this.HttpContext, moduleDetailID);

            //RequireJsOptions.Add("LocationID", this.baseService.LocationID, RequireJsOptionsScope.Page);
            //RequireJsOptions.Add("ModuleID", moduleID, RequireJsOptionsScope.Page);
            //RequireJsOptions.Add("ModuleDetailID", moduleDetailID, RequireJsOptionsScope.Page);
            //RequireJsOptions.Add("NmvnTaskID", this.baseService.NmvnTaskID, RequireJsOptionsScope.Page);
        }
    }
}
