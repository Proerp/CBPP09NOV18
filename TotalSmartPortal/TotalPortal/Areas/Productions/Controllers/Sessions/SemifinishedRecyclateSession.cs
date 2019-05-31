using System.Web;

namespace TotalPortal.Areas.Productions.Controllers.Sessions
{
    public class SemifinishedRecyclateSession
    {
        public static string GetCrucialWorker(HttpContextBase context)
        {
            if (context.Session["SemifinishedRecyclate-CrucialWorker"] == null)
                return null;
            else
                return (string)context.Session["SemifinishedRecyclate-CrucialWorker"];
        }

        public static void SetCrucialWorker(HttpContextBase context, int crucialWorkerID, string crucialWorkerName)
        {
            context.Session["SemifinishedRecyclate-CrucialWorker"] = crucialWorkerID.ToString() + "#@#" + crucialWorkerName;
        }

        public static string GetStorekeeper(HttpContextBase context)
        {
            if (context.Session["SemifinishedRecyclate-Storekeeper"] == null)
                return null;
            else
                return (string)context.Session["SemifinishedRecyclate-Storekeeper"];
        }

        public static void SetStorekeeper(HttpContextBase context, int storekeeperID, string storekeeperName)
        {
            context.Session["SemifinishedRecyclate-Storekeeper"] = storekeeperID.ToString() + "#@#" + storekeeperName;
        }
    }
}