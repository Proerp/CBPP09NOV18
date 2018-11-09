using Owin;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;

[assembly: OwinStartupAttribute(typeof(TotalPortal.Startup))]
namespace TotalPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            ConfigureAuth(app);
            app.UseCors(CorsOptions.AllowAll); //SHOULD???: must be the first line in the Configuration function (according to https://stackoverflow.com/a/29012337/6210773)
            app.UseWebApi(config);
        }
    }
}
