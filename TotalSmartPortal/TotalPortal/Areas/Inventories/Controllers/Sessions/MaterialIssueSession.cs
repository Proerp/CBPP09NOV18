using System.Web;

namespace TotalPortal.Areas.Inventories.Controllers.Sessions
{
    public class MaterialIssueSession
    {
        public static string GetStorekeeper(HttpContextBase context)
        {
            if (context.Session["MaterialIssue-Storekeeper"] == null)
                return null;
            else
                return (string)context.Session["MaterialIssue-Storekeeper"];
        }

        public static void SetStorekeeper(HttpContextBase context, int storekeeperID, string storekeeperName)
        {
            context.Session["MaterialIssue-Storekeeper"] = storekeeperID.ToString() + "#@#" + storekeeperName;
        }


        public static string GetCrucialWorker(HttpContextBase context)
        {
            if (context.Session["MaterialIssue-CrucialWorker"] == null)
                return null;
            else
                return (string)context.Session["MaterialIssue-CrucialWorker"];
        }

        public static void SetCrucialWorker(HttpContextBase context, int storekeeperID, string storekeeperName)
        {
            context.Session["MaterialIssue-CrucialWorker"] = storekeeperID.ToString() + "#@#" + storekeeperName;
        }
    }
}