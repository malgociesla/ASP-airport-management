using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirportService;
using AirportService.DTO;
using AirplaneASP.Models.Cities;
using AirplaneASP.Models.Countries;
using AirplaneASP.Mapping;

namespace AirplaneASP.Controllers
{
    public class CitiesController : Controller
    {
        private readonly ICityService _cityService;
        private readonly ICountryService _countryService;
        private readonly IMapper<CityDTO, CityModel> _cityMaper;
        private readonly IMapper<CountryDTO, CountryModel> _countryMaper;

        public CitiesController(ICityService cityService, ICountryService countryService, IMapper<CityDTO, CityModel> cityMaper, IMapper<CountryDTO, CountryModel> countryMaper)
        {
            this._cityService = cityService;
            this._countryService = countryService;
            this._cityMaper = cityMaper;
            this._countryMaper=countryMaper;
        }

        [HttpGet]
        public ActionResult List()
        {
            List<CityDTO> ciList = _cityService.GetAll();
            var cityList = _cityMaper.Map(ciList);

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
            var countryList = _countryMaper.Map(ctrList);
            ViewBag.CountryList = countryList;
            return View();
        }

        [HttpPost]
        public ActionResult Add(CityModel city)
        {
            if (ModelState.IsValid)
            {
                var ci = _cityMaper.MapBack(city);
                _cityService.Add(ci);

                return RedirectToAction("List");
            }
            else
            {
                List<CountryDTO> ctrList = _countryService.GetAll();
                var countryList = _countryMaper.Map(ctrList);
                ViewBag.CountryList = countryList;

                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            CityDTO ciItem = _cityService.GetAll().FirstOrDefault(c => c.ID == id);
            var cityItem = _cityMaper.Map(ciItem);

            List<CountryDTO> ctrList = _countryService.GetAll();
            var countryList = _countryMaper.Map(ctrList);
            ViewBag.CountryList = countryList;

            return View("Edit", cityItem);
        }

        [HttpPost]
        public ActionResult Edit(CityModel city)
        {
            if (ModelState.IsValid)
            {
                var ci = _cityMaper.MapBack(city);
                _cityService.Edit(ci);

                return RedirectToAction("List");
            }
            else
            {
                List<CountryDTO> ctrList = _countryService.GetAll();
                var countryList = _countryMaper.Map(ctrList);
                ViewBag.CountryList = countryList;

                return View();
            }
        }
    }
}