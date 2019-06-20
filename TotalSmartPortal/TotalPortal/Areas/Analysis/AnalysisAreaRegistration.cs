using System.Web.Mvc;

namespace TotalPortal.Areas.Analysis
{
    public class AnalysisAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Analysis";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Analysis_default",
                "Analysis/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Analysis_default_Two_Parameters",
                "Analysis/{controller}/{action}/{id}/{detailId}",
                new { action = "Index", id = UrlParameter.Optional, detailId = UrlParameter.Optional }
            );

        }
    }
}