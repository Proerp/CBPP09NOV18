using System.Web.Mvc;

namespace TotalPortal.Areas.Commons
{
    public class CommonsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Commons";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Commons_default",
                "Commons/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Commons_default_Two_Parameters",
                "Commons/{controller}/{action}/{id}/{detailId}",
                new { action = "Index", id = UrlParameter.Optional, detailId = UrlParameter.Optional }
            );

            context.MapRoute(
                "Commons_default_Three_Parameters",
                "Commons/{controller}/{action}/{id}/{detailId}/{tokenid}",
                new { action = "Index", id = UrlParameter.Optional, detailId = UrlParameter.Optional, tokenid = UrlParameter.Optional }
            );
        }
    }
}