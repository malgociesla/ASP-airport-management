using System;
using AirplaneASP.Loggers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AirplaneASP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var raisedException = Server.GetLastError();

            //Logg exception
            //ExceptionLogger exLogger = new ExceptionLogger();
            //exLogger.LogException(raisedException);

            // Process exception
            //Server.ClearError();
            //Response.Redirect("Shared/Error");
        }
    }
}
