using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirportService;
using AirportService.DTO;

namespace AirplaneASP.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ICityService cityService= new CityService();
            List<CityDTO> cityList = cityService.GetCities();
            ViewBag.CityList = cityList;
            ViewBag.CityService = cityService;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}