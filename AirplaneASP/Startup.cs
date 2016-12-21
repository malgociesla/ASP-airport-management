using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AirplaneASP.Startup))]
namespace AirplaneASP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
