using System.Web;

namespace TotalPortal.Areas.Inventories.Controllers.Sessions
{
    public class GoodsReceiptSession
    {
        public static string GetStorekeeper(HttpContextBase context)
        {
            if (context.Session["GoodsReceipt-Storekeeper"] == null)
                return null;
            else
                return (string)context.Session["GoodsReceipt-Storekeeper"];
        }

        public static void SetStorekeeper(HttpContextBase context, int storekeeperID, string storekeeperName)
        {
            context.Session["GoodsReceipt-Storekeeper"] = storekeeperID.ToString() + "#@#" + storekeeperName;
        }
    }
}