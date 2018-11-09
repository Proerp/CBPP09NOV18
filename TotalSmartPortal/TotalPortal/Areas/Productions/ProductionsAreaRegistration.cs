using System.Web.Mvc;

namespace TotalPortal.Areas.Productions
{
    public class ProductionsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Productions";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Productions_default",
                "Productions/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Productions_default_Two_Parameters",
                "Productions/{controller}/{action}/{id}/{detailId}",
                new { action = "Index", id = UrlParameter.Optional, detailId = UrlParameter.Optional }
            );
        }
    }
}