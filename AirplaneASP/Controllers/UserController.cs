using AirplaneASP.Authorization;
using AirplaneASP.Mapping;
using AirplaneASP.Models.Cities;
using AirplaneASP.Models.Countries;
using AirportService;
using AirportService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirplaneASP.Controllers
{
    [AuthorizeAccess(Roles = "USER")]
    public class UserController : Controller
    {
        private readonly ICityService _cityService;
        private readonly ICountryService _countryService;

        private readonly IMapper<CityDTO, CityModel> _cityMaper;
        private readonly IMapper<CountryDTO, CountryModel> _countryMaper;
        public UserController(ICityService cityService,
                                ICountryService countryService,

                                IMapper<CityDTO, CityModel> cityMaper,
                                IMapper<CountryDTO, CountryModel> countryMaper)
        {
            this._cityService = cityService;
            this._countryService = countryService;
            this._cityMaper = cityMaper;
            this._countryMaper = countryMaper;
        }
        public ActionResult Index()
        {
            var cityDTOList = _cityService.GetAll();
            var cityList = _cityMaper.Map(cityDTOList);

            ViewBag.CityList = cityList;

            return View();
        }

        [HttpGet]
        public JsonResult GetCities()
        {
            var cityDTOList = _cityService.GetAll();
            var cityList = _cityMaper.Map(cityDTOList);
            return Json(cityList, JsonRequestBehavior.AllowGet);
        }
    }
}