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
        private readonly ICityService _cityService;
        private readonly ICountryService _countryService;

        public CitiesController(ICityService cityService, ICountryService countryService)
        {
            this._cityService = cityService;
            this._countryService = countryService;
        }

        [HttpGet]
        public ActionResult List()
        {
            List<CityDTO> ciList = _cityService.GetAll();
            List<CityModel> cityList = ciList.Select(ci => new CityModel
            {
                ID = ci.ID,
                CountryID = ci.CountryID,
                Name = ci.Name
            }).ToList();

            return View("List", cityList);
        }

        [HttpGet]
        public ActionResult Remove(Guid id)
        {
            _cityService.Remove(id);

            return RedirectToAction("List");
        }

        public ActionResult Add()
        {
            List<CountryDTO> ctrList = _countryService.GetAll();
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
                CityDTO ci = new CityDTO
                {
                    ID = city.ID,
                    CountryID = city.CountryID,
                    Name = city.Name
                };
                _cityService.Add(ci);

                return RedirectToAction("List");
            }
            else
            {
                List<CountryDTO> ctrList = _countryService.GetAll();
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
            CityDTO ciItem = _cityService.GetAll().FirstOrDefault(c => c.ID == id);
            CityModel cityItem = new CityModel
            {
                ID = ciItem.ID,
                CountryID = ciItem.CountryID,
                Name = ciItem.Name
            };

            List<CountryDTO> ctrList = _countryService.GetAll();
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
                CityDTO ci = new CityDTO
                {
                    ID = city.ID,
                    CountryID = city.CountryID,
                    Name = city.Name
                };
                _cityService.Edit(ci);

                return RedirectToAction("List");
            }
            else
            {
                List<CountryDTO> ctrList = _countryService.GetAll();
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