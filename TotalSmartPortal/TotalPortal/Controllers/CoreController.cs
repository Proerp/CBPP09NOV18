using System.Web.Mvc;
using System.Text;

using TotalBase.Enums;
using TotalPortal.Configuration;

namespace TotalPortal.Controllers
{
    public class CoreController : Controller
    {
        public ActionResult GlobalJavaScriptEnums()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("var SubmitTypeOption = " + typeof(GlobalEnums.SubmitTypeOption).EnumToJson() + "; ");
            stringBuilder.Append("var Settings = " + System.Web.Helpers.Json.Encode(new MySettings()) + "; ");

            return JavaScript(stringBuilder.ToString());
        }
    }
}