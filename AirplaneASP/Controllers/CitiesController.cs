using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirportService;
using AirportService.DTO;
using AirplaneASP.Models.Cities;
using AirplaneASP.Models.Countries;

namespace AirplaneASP.Controllers
{
    public class CitiesController : Controller
    {
        [HttpGet]
        public ActionResult List()
        {
            ICityService cityService = new CityService();
            List<CityDTO> ciList = cityService.GetAll();
            List<CityModel> cityList = ciList.Select(ci => new CityModel
            {
                ID = ci.ID,
                CountryID = ci.CountryID,
                Name = ci.Name
            }).ToList();

            return View("List",cityList);
        }

        [HttpGet]
        public ActionResult Remove(Guid id)
        {
            ICityService cityService = new CityService();
            cityService.Remove(id);

            return RedirectToAction("List");
        }

        public ActionResult Add()
        {
            ICountryService countryService = new CountryService();
            List<CountryDTO> ctrList = countryService.GetAll();
            List<CountryModel> countryList = ctrList.Select(ctr => new CountryModel
            {
                ID = ctr.ID,
                Name = ctr.Name
            }).ToList();
            ViewBag.CountryList = countryList;
            return View();
        }

        [HttpPost]
        public ActionResult Add(CityModel city)
        {
            if (ModelState.IsValid)
            {
                ICityService cityService = new CityService();
                CityDTO ci = new CityDTO { ID = city.ID, CountryID = city.CountryID, Name = city.Name };
                cityService.Add(ci);

                return RedirectToAction("List");
            }
            else
            {
                ICountryService countryService = new CountryService();
                List<CountryDTO> ctrList = countryService.GetAll();
                List<CountryModel> countryList = ctrList.Select(ctr => new CountryModel
                {
                    ID = ctr.ID,
                    Name = ctr.Name
                }).ToList();
                ViewBag.CountryList = countryList;

                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            ICityService cityService = new CityService();
            CityDTO ciItem = cityService.GetAll().FirstOrDefault(c => c.ID == id);
            CityModel cityItem = new CityModel { ID = ciItem.ID, CountryID = ciItem.CountryID, Name = ciItem.Name };

            ICountryService countryService = new CountryService();
            List<CountryDTO> ctrList = countryService.GetAll();
            List<CountryModel> countryList = ctrList.Select(ctr => new CountryModel
            {
                ID = ctr.ID,
                Name = ctr.Name
            }).ToList();
            ViewBag.CountryList = countryList;

            return View("Edit", cityItem);
        }

        [HttpPost]
        public ActionResult Edit(CityModel city)
        {
            if (ModelState.IsValid)
            {
                ICityService cityService = new CityService();
                CityDTO ci = new CityDTO { ID = city.ID, CountryID = city.CountryID, Name = city.Name };
                cityService.Edit(ci);

                return RedirectToAction("List");
            }
            else
            {
                ICountryService countryService = new CountryService();
                List<CountryDTO> ctrList = countryService.GetAll();
                List<CountryModel> countryList = ctrList.Select(ctr => new CountryModel
                {
                    ID = ctr.ID,
                    Name = ctr.Name
                }).ToList();
                ViewBag.CountryList = countryList;

                return View();
            }
        }
    }
}