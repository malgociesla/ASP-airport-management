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
using AirplaneASP.Authorization;

namespace AirplaneASP.Controllers
{
    [AuthorizeAccess(Roles = "ADMIN")]
    public class CitiesController : Controller
    {
        private readonly ICityService _cityService;
        private readonly ICountryService _countryService;

        private readonly IMapper<CityDTO, CityModel> _cityMaper;
        private readonly IMapper<CountryDTO, CountryModel> _countryMaper;

        public CitiesController(ICityService cityService,
                                ICountryService countryService,

                                IMapper<CityDTO, CityModel> cityMaper,
                                IMapper<CountryDTO, CountryModel> countryMaper)
        {
            this._cityService = cityService;
            this._countryService = countryService;
            this._cityMaper = cityMaper;
            this._countryMaper=countryMaper;
        }

        [HttpGet]
        public ActionResult List()
        {
            List<CityDTO> cityDTOList = _cityService.GetAll();
            var cityList = _cityMaper.Map(cityDTOList);

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
            List<CountryDTO> countryDTOList = _countryService.GetAll();
            var countryList = _countryMaper.Map(countryDTOList);
            ViewBag.CountryList = countryList;
            return View();
        }

        [HttpPost]
        public ActionResult Add(CityModel city)
        {
            if (ModelState.IsValid)
            {
                var cityDTO = _cityMaper.MapBack(city);
                _cityService.Add(cityDTO);

                return RedirectToAction("List");
            }
            else
            {
                List<CountryDTO> countryDTOList = _countryService.GetAll();
                var countryList = _countryMaper.Map(countryDTOList);
                ViewBag.CountryList = countryList;

                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            CityDTO cityDTO = _cityService.GetAll().FirstOrDefault(c => c.ID == id);
            var city = _cityMaper.Map(cityDTO);

            List<CountryDTO> countryDTOList = _countryService.GetAll();
            var countryList = _countryMaper.Map(countryDTOList);
            ViewBag.CountryList = countryList;

            return View("Edit", city);
        }

        [HttpPost]
        public ActionResult Edit(CityModel city)
        {
            if (ModelState.IsValid)
            {
                var cityDTO = _cityMaper.MapBack(city);
                _cityService.Edit(cityDTO);

                return RedirectToAction("List");
            }
            else
            {
                List<CountryDTO> countryDTOList = _countryService.GetAll();
                var countryList = _countryMaper.Map(countryDTOList);
                ViewBag.CountryList = countryList;

                return View();
            }
        }
    }
}