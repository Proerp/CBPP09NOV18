using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Microsoft.AspNet.Identity;

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

            baseController.BaseService.UserID = Db.Users.Where(w => w.Id == aspUserID).FirstOrDefault().UserID;

            base.OnAuthorization(actionContext);
        }
    }

}