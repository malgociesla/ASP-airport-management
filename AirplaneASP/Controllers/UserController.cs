using AirplaneASP.Authorization;
using AirplaneASP.Mapping;
using AirplaneASP.Models.Cities;
using AirplaneASP.Models.Countries;
using AirplaneASP.Models.Schedules;
using AirportService;
using AirportService.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirplaneASP.Controllers
{
    [AuthorizeAccess(Roles = "USER")]
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}