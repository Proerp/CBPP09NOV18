using System.Web;

namespace TotalPortal.Areas.Productions.Controllers.Sessions
{
    public class FinishedProductSession
    {
        public static string GetCrucialWorker(HttpContextBase context)
        {
            if (context.Session["FinishedProduct-CrucialWorker"] == null)
                return null;
            else
                return (string)context.Session["FinishedProduct-CrucialWorker"];
        }

        public static void SetCrucialWorker(HttpContextBase context, int storekeeperID, string storekeeperName)
        {
            context.Session["FinishedProduct-CrucialWorker"] = storekeeperID.ToString() + "#@#" + storekeeperName;
        }
    }
}