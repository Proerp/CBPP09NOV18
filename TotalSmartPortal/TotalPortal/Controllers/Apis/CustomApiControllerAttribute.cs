using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;

using Microsoft.AspNet.Identity;

using TotalBase.Enums;
using TotalPortal.Models;


namespace TotalPortal.Controllers.Apis
{
    public class GenericSimpleApiAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var Db = new ApplicationDbContext();

            BaseApiController baseController = actionContext.ControllerContext.Controller as BaseApiController;

            string aspUserID = actionContext.RequestContext.Principal.Identity.GetUserId();

            if (aspUserID != null) baseController.BaseService.UserID = Db.Users.Where(w => w.Id == aspUserID).FirstOrDefault().UserID;

            base.OnAuthorization(actionContext);
        }
    }



    public class AccessLevelApiAuthorizeAttribute : AuthorizeAttribute
    {
        private BaseApiController baseController;

        private GlobalEnums.AccessLevel accessLevel;

        public AccessLevelApiAuthorizeAttribute()
            : this(GlobalEnums.AccessLevel.Editable)
        { }

        public AccessLevelApiAuthorizeAttribute(GlobalEnums.AccessLevel accessLevel)
        {
            this.accessLevel = accessLevel;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            this.baseController = actionContext.ControllerContext.Controller as BaseApiController;
            base.OnAuthorization(actionContext);
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var authorized = base.IsAuthorized(actionContext);
            if (!authorized) return false;

            return this.baseController.BaseService.GetAccessLevel() >= this.accessLevel;
        }
    }


    public class OnResultExecutingApiFilterAttribute : ActionFilterAttribute
    {
        //NOW, NO NEED TO DO ANY THING WHEN OnActionExecuting
        //public override void OnActionExecuting(HttpActionContext actionContext)
        //{
        //    base.OnActionExecuting(actionContext);

        //    if (actionContext.Result is ViewResult)
        //    {
        //        var controller = filterContext.Controller as BaseController;
        //        controller.AddRequireJsOptions();
        //    }
        //}
    }
}