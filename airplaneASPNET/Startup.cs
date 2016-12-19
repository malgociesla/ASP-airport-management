using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(airplaneASPNET.Startup))]
namespace airplaneASPNET
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
