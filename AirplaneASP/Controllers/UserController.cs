using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirplaneASP.Controllers
{
    public class UserController : Controller
    {
        [Authorize(Roles = "USER")]
        public ActionResult Index()
        {
            return View();
        }
    }
}