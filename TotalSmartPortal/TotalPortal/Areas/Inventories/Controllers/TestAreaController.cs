using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
 
namespace TotalPortal.Areas.Inventories.Controllers
{
    [RoutePrefix("API/Inventories/TestArea")]
    public class TestAreaController : ApiController
    {
        [Route("HelloWorldArea")]
        [Authorize]
        [HttpGet]
        public HttpResponseMessage HelloWorldArea()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Hello, SAY FROM AREA [API OK] INVENTORY [LOCAL], AREA CONTROLLER, " + this.User.Identity.Name + " (" + this.RequestContext.Principal.Identity.GetUserId() + ")");
        }
    }
}
