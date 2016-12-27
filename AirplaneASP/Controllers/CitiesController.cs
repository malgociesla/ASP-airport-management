using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirportService;
using AirportService.DTO;

namespace AirplaneASP.Controllers
{
    public class CitiesController : Controller
    {
        [HttpGet]
        public ActionResult List()
        {
            ICityService cityService = new CityService();
            List<CityDTO> cityList = cityService.GetAll();

            return View("List",cityList);
        }

        [HttpGet]
        public ActionResult Remove(Guid cityID)
        {
            ICityService cityService = new CityService();
            cityService.Remove(cityID);

            return List();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(CityDTO city)
        {
            ICityService cityService = new CityService();
            cityService.Add(city);
            return List();
        }
    }
}