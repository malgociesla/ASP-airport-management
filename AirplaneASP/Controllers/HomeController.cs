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
        [HttpGet]
        public ActionResult Index()
        {
            ICityService cityService= new CityService();
            List<CityDTO> cityList = cityService.GetAll();

            return View("Index",cityList);
        }

        [HttpGet]
        public ActionResult Remove(Guid cityID)
        {
            ICityService cityService = new CityService();
            cityService.Remove(cityID);

            return Index();
        }

        //[HttpPost]
        //public ActionResult Index(Guid cityID)
        //{
        //    ICityService cityService = new CityService();
        //    cityService.Remove(cityID);
        //   // List<CityDTO> cityList = cityService.GetCities();

        //    return Index();
        //}


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