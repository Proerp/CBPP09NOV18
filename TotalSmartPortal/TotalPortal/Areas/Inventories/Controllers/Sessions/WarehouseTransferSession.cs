using System.Web;

namespace TotalPortal.Areas.Inventories.Controllers.Sessions
{
    public class WarehouseTransferSession
    {
        public static string GetStorekeeper(HttpContextBase context)
        {
            if (context.Session["WarehouseTransfer-Storekeeper"] == null)
                return null;
            else
                return (string)context.Session["WarehouseTransfer-Storekeeper"];
        }

        public static void SetStorekeeper(HttpContextBase context, int storekeeperID, string storekeeperName)
        {
            context.Session["WarehouseTransfer-Storekeeper"] = storekeeperID.ToString() + "#@#" + storekeeperName;
        }
    }
}