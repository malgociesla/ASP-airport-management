using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirportService;
using AirportService.DTO;

namespace AirplaneASP.Controllers
{
    public class CountriesController : Controller
    {
        [HttpGet]
        public ActionResult List()
        {
            ICountryService countryService = new CountryService();
            List<CountryDTO> countryList = countryService.GetAll();

            return View("List",countryList);
        }

        [HttpGet]
        public ActionResult Remove(Guid id)
        {
            ICountryService countryService = new CountryService();
            countryService.Remove(id);
            //return List();
            return RedirectToAction("List");
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(CountryDTO country)
        {
            ICountryService countryService = new CountryService();
            countryService.Add(country);
            //return List();
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            ICountryService countryService = new CountryService();
            CountryDTO countryItem = countryService.GetAll().FirstOrDefault(c => c.ID == id);
            return View("Edit", countryItem);
        }

        [HttpPost]
        public ActionResult Edit(CountryDTO country)
        {
            ICountryService countryService = new CountryService();
            countryService.Edit(country);
            //return List();
            return RedirectToAction("List");
        }
    }
}