using System;
using AirplaneASP.Loggers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web;

namespace AirplaneASP
{
    public class MvcApplication : HttpApplication
    {
        private readonly IExceptionLogger _exceptionLogger = DependencyResolver.Current.GetService<IExceptionLogger>();
        private readonly IRequestLogger _requestLogger = DependencyResolver.Current.GetService<IRequestLogger>();

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
            _exceptionLogger.LogException(raisedException);

            //Process exception
            Server.ClearError();
            Response.Redirect("/Shared/Error");
        }

        protected void Application_LogRequest(Object sender, EventArgs e)
        {
            _requestLogger.LogRequest(Request);
        }
    }
}
