using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirportService;
using AirportService.DTO;
using AirplaneASP.Models.Flights;
using AirplaneASP.Models.Companies;
using AirplaneASP.Models.Cities;
using AirplaneASP.Mapping;

namespace AirplaneASP.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class FlightsController : Controller
    {
        private readonly IFlightService _flightService;
        private readonly ICompanyService _companyService;
        private readonly ICityService _cityService;

        private readonly IMapper<FlightDTO, FlightModel> _flightMaper;
        private readonly IMapper<CompanyDTO, CompanyModel> _companyMaper;
        private readonly IMapper<CityDTO, CityModel> _cityMaper;

        public FlightsController(IFlightService flightService,
                                 ICompanyService companyService,
                                 ICityService cityService,
                                 ICountryService countryService,

                                 IMapper<FlightDTO, FlightModel> flightMaper,
                                 IMapper<CompanyDTO, CompanyModel> companyMaper,
                                 IMapper<CityDTO, CityModel> cityMaper)
        {
            this._flightService = flightService;
            this._companyService = companyService;
            this._cityService = cityService;

            this._flightMaper = flightMaper;
            this._companyMaper = companyMaper;
            this._cityMaper = cityMaper;
        }

        [HttpGet]
        public ActionResult List()
        {
            List<FlightDTO> flightDTOList = _flightService.GetAll();
            var flightList = _flightMaper.Map(flightDTOList);          

            return View("List", flightList);
        }

        [HttpGet]
        public ActionResult Remove(Guid id)
        {
            _flightService.Remove(id);

            return RedirectToAction("List");
        }

        public ActionResult Add()
        {
            List<CompanyDTO> companyDTOList = _companyService.GetAll();
            var companyList = _companyMaper.Map(companyDTOList);
            ViewBag.CompanyList = companyList;

            List<CityDTO> cityDTOList = _cityService.GetAll();
            var cityList = _cityMaper.Map(cityDTOList);
            ViewBag.CityList = cityList;

            return View();
        }

        [HttpPost]
        public ActionResult Add(FlightModel flight)
        {
            if (ModelState.IsValid)
            {
                var flightDTO = _flightMaper.MapBack(flight);
                _flightService.Add(flightDTO);

                return RedirectToAction("List");
            }
            else
            {
                List<CompanyDTO> companyDTOList = _companyService.GetAll();
                var companyList = _companyMaper.Map(companyDTOList);
                ViewBag.CompanyList = companyList;

                List<CityDTO> cityDTOList = _cityService.GetAll();
                var cityList = _cityMaper.Map(cityDTOList);
                ViewBag.CityList = cityList;

                return View();
            }
        }


        [HttpGet]
        //[Route("Edit/{flightID}")]
        public ActionResult Edit(Guid id)
        {
            FlightDTO flightDTO = _flightService.GetAll().FirstOrDefault(f => f.ID == id);
            var flight = _flightMaper.Map(flightDTO);

            List<CompanyDTO> companyDTOList = _companyService.GetAll();
            var companyList = _companyMaper.Map(companyDTOList);
            ViewBag.CompanyList = companyList;

            List<CityDTO> cityDTOList = _cityService.GetAll();
            var cityList = _cityMaper.Map(cityDTOList);
            ViewBag.CityList = cityList;

            return View("Edit", flight);
        }

        [HttpPost]
        public ActionResult Edit(FlightModel flight)
        {
            if (ModelState.IsValid)
            {
                var flightDTO = _flightMaper.MapBack(flight);
                _flightService.Edit(flightDTO);

                return RedirectToAction("List");
            }
            else
            {
                List<CompanyDTO> companyDTOList = _companyService.GetAll();
                var companyList = _companyMaper.Map(companyDTOList);
                ViewBag.CompanyList = companyList;

                List<CityDTO> cityDTOList = _cityService.GetAll();
                var cityList = _cityMaper.Map(cityDTOList);
                ViewBag.CityList = cityList;

                return View();
            }
        }
    }
}