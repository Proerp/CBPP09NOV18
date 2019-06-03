using System.Web;

namespace TotalPortal.Areas.Productions.Controllers.Sessions
{
    public class RecyclateSession
    {
        public static string GetCrucialWorker(HttpContextBase context)
        {
            if (context.Session["Recyclate-CrucialWorker"] == null)
                return null;
            else
                return (string)context.Session["Recyclate-CrucialWorker"];
        }

        public static void SetCrucialWorker(HttpContextBase context, int crucialWorkerID, string crucialWorkerName)
        {
            context.Session["Recyclate-CrucialWorker"] = crucialWorkerID.ToString() + "#@#" + crucialWorkerName;
        }

        public static string GetStorekeeper(HttpContextBase context)
        {
            if (context.Session["Recyclate-Storekeeper"] == null)
                return null;
            else
                return (string)context.Session["Recyclate-Storekeeper"];
        }

        public static void SetStorekeeper(HttpContextBase context, int storekeeperID, string storekeeperName)
        {
            context.Session["Recyclate-Storekeeper"] = storekeeperID.ToString() + "#@#" + storekeeperName;
        }
    }
}