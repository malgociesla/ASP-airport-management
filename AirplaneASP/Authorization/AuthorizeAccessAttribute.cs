using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AirplaneASP.Authorization
{
    public class AuthorizeAccessAttribute : AuthorizeAttribute
    {
        //if user is unauthorized - return 404
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpNotFoundResult();
        }
    }
}