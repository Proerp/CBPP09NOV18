using System.Web;

namespace TotalPortal.Areas.Inventories.Controllers.Sessions
{
    public class PackageIssueSession
    {
        public static string GetStorekeeper(HttpContextBase context)
        {
            if (context.Session["PackageIssue-Storekeeper"] == null)
                return null;
            else
                return (string)context.Session["PackageIssue-Storekeeper"];
        }

        public static void SetStorekeeper(HttpContextBase context, int storekeeperID, string storekeeperName)
        {
            context.Session["PackageIssue-Storekeeper"] = storekeeperID.ToString() + "#@#" + storekeeperName;
        }


        public static string GetCrucialWorker(HttpContextBase context)
        {
            if (context.Session["PackageIssue-CrucialWorker"] == null)
                return null;
            else
                return (string)context.Session["PackageIssue-CrucialWorker"];
        }

        public static void SetCrucialWorker(HttpContextBase context, int storekeeperID, string storekeeperName)
        {
            context.Session["PackageIssue-CrucialWorker"] = storekeeperID.ToString() + "#@#" + storekeeperName;
        }
    }
}