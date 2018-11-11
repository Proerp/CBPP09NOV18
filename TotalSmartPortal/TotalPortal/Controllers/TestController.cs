using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace TotalPortal.Controllers
{
    [RoutePrefix("Test")]
    public class TestController : ApiController
    {
        [Route("HelloWorld")]
        [Authorize]
        [HttpGet]
        public HttpResponseMessage HelloWorld()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Hello, SAY FROM LOCAL: http://localhost:49200/, " + this.User.Identity.Name + " (" + this.RequestContext.Principal.Identity.GetUserId() + ")");
        }
    }
}
