using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Http.Controllers;

using System.Collections.Generic;
using Microsoft.Owin.Security.OAuth;

namespace TotalPortal.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            //////config.MapHttpAttributeRoutes(); COMMENT THIS LINE => TO BE REPLACED BY THE LINE BELOW.
            config.MapHttpAttributeRoutes(new CustomDirectRouteProvider()); //THIS LINE IS ADDED: To Support inheritance of Route attributes [New Features in ASP.NET Web API 2.2] https://docs.microsoft.com/en-us/aspnet/web-api/overview/releases/whats-new-in-aspnet-web-api-22    https://stackoverflow.com/questions/24958190/webapi2-attribute-routing-inherited-controllers

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Enforce HTTPS
            //////-HIEPconfig.Filters.Add(new TotalPortal.Filters.RequireHttpsAttribute());
        }
    }

    public class CustomDirectRouteProvider : DefaultDirectRouteProvider
    {
        protected override IReadOnlyList<IDirectRouteFactory>
        GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
        {
            return actionDescriptor.GetCustomAttributes<IDirectRouteFactory>
            (inherit: true);
        }
    }
}