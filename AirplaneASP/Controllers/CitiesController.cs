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
            //return List();
            return RedirectToAction("List");
        }

        public ActionResult Add()
        {
            ICountryService countryService = new CountryService();
            List<CountryDTO> countryList = countryService.GetAll();
            ViewBag.CountryList = countryList;
            return View();
        }

        [HttpPost]
        public ActionResult Add(CityDTO city)
        {
            ICityService cityService = new CityService();
            cityService.Add(city);
            //return List();
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Edit(Guid cityID)
        {
            ICityService cityService = new CityService();
            CityDTO cityItem = cityService.GetAll().FirstOrDefault(c => c.ID == cityID);

            ICountryService countryService = new CountryService();
            List<CountryDTO> countryList = countryService.GetAll();
            ViewBag.CountryList = countryList;

            return View("Edit", cityItem);
        }

        [HttpPost]
        public ActionResult Edit(CityDTO city)
        {
            ICityService cityService = new CityService();
            cityService.Edit(city);
            //return List();
            return RedirectToAction("List");
        }
    }
}