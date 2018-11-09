using System.Web;

namespace TotalPortal.Areas.Commons.Controllers.Sessions
{
    public class ShiftSession
    {
        public static string GetShift(HttpContextBase context)
        {
            if (context.Session["Commons-Shift"] == null)
                return null;
            else
                return (string)context.Session["Commons-Shift"];
        }

        public static void SetShift(HttpContextBase context, int shiftID)
        {
            context.Session["Commons-Shift"] = shiftID.ToString() + "#@#";
        }
    }
}