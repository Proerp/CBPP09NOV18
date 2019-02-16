using System.Web;

namespace TotalPortal.Areas.Productions.Controllers.Sessions
{
    public class FinishedItemSession
    {
        public static string GetCrucialWorker(HttpContextBase context)
        {
            if (context.Session["FinishedItem-CrucialWorker"] == null)
                return null;
            else
                return (string)context.Session["FinishedItem-CrucialWorker"];
        }

        public static void SetCrucialWorker(HttpContextBase context, int storekeeperID, string storekeeperName)
        {
            context.Session["FinishedItem-CrucialWorker"] = storekeeperID.ToString() + "#@#" + storekeeperName;
        }
    }
}