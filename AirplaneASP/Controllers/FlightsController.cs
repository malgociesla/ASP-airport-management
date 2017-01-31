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

namespace AirplaneASP.Controllers
{
    public class FlightsController : Controller
    {
        private readonly IFlightService _flightService;
        private readonly ICompanyService _companyService;
        private readonly ICityService _cityService;

        public FlightsController(IFlightService flightService, ICompanyService companyService, ICityService cityService, ICountryService countryService)
        {
            this._flightService = flightService;
            this._companyService = companyService;
            this._cityService = cityService;
        }

        [HttpGet]
        public ActionResult List()
        {
            List<FlightDTO> fliList = _flightService.GetAll();
            List<FlightModel> flightList = new List<FlightModel>();
            foreach (FlightDTO fli in fliList)
            {
                FlightModel fliItem = new FlightModel
                {
                    ID = fli.ID,
                    CompanyID = fli.CompanyID,
                    Name = fli.Name,
                    DayOfWeek = fli.DayOfWeek,
                    CityDepartureID = fli.CityDepartureID,
                    CityArrivalID = fli.CityArrivalID,
                    DepartureTime = fli.DepartureTime,
                    ArrivalTime = fli.ArrivalTime
                };
                flightList.Add(fliItem);
            }

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
            List<CompanyDTO> cmpList = _companyService.GetAll();
            List<CompanyModel> companyList = new List<CompanyModel>();
            foreach (CompanyDTO cmp in cmpList)
            {
                CompanyModel cmpItem = new CompanyModel
                {
                    ID = cmp.ID,
                    Name = cmp.Name
                };
                companyList.Add(cmpItem);
            }
            ViewBag.CompanyList = companyList;

            List<CityDTO> ciList = _cityService.GetAll();
            List<CityModel> cityList = new List<CityModel>();
            foreach (CityDTO ci in ciList)
            {
                CityModel ciItem = new CityModel
                {
                    ID = ci.ID,
                    CountryID = ci.CountryID,
                    Name = ci.Name
                };
                cityList.Add(ciItem);
            }
            ViewBag.CityList = cityList;

            return View();
        }

        [HttpPost]
        public ActionResult Add(FlightModel flight)
        {
            if (ModelState.IsValid)
            {
                FlightDTO fli = new FlightDTO
                {
                    ID = flight.ID,
                    CompanyID = flight.CompanyID,
                    Name = flight.Name,
                    DayOfWeek = flight.DayOfWeek,
                    CityDepartureID = flight.CityDepartureID,
                    CityArrivalID = flight.CityArrivalID,
                    DepartureTime = flight.DepartureTime,
                    ArrivalTime = flight.ArrivalTime
                };
                _flightService.Add(fli);

                return RedirectToAction("List");
            }
            else
            {
                List<CompanyDTO> cmpList = _companyService.GetAll();
                List<CompanyModel> companyList = new List<CompanyModel>();
                foreach (CompanyDTO cmp in cmpList)
                {
                    CompanyModel cmpItem = new CompanyModel
                    {
                        ID = cmp.ID,
                        Name = cmp.Name
                    };
                    companyList.Add(cmpItem);
                }
                ViewBag.CompanyList = companyList;

                List<CityDTO> ciList = _cityService.GetAll();
                List<CityModel> cityList = new List<CityModel>();
                foreach (CityDTO ci in ciList)
                {
                    CityModel ciItem = new CityModel
                    {
                        ID = ci.ID,
                        CountryID = ci.CountryID,
                        Name = ci.Name
                    };
                    cityList.Add(ciItem);
                }
                ViewBag.CityList = cityList;

                return View();
            }
        }


        [HttpGet]
        //[Route("Edit/{flightID}")]
        public ActionResult Edit(Guid id)
        {
            FlightDTO fliItem = _flightService.GetAll().FirstOrDefault(f => f.ID == id);
            FlightModel flightItem = new FlightModel
            {
                ID = fliItem.ID,
                CompanyID = fliItem.CompanyID,
                Name = fliItem.Name,
                DayOfWeek = fliItem.DayOfWeek,
                CityDepartureID = fliItem.CityDepartureID,
                CityArrivalID = fliItem.CityArrivalID,
                DepartureTime = fliItem.DepartureTime,
                ArrivalTime = fliItem.ArrivalTime
            };

            List<CompanyDTO> cmpList = _companyService.GetAll();
            List<CompanyModel> companyList = new List<CompanyModel>();
            foreach (CompanyDTO cmp in cmpList)
            {
                CompanyModel cmpItem = new CompanyModel
                {
                    ID = cmp.ID,
                    Name = cmp.Name
                };
                companyList.Add(cmpItem);
            }
            ViewBag.CompanyList = companyList;

            List<CityDTO> ciList = _cityService.GetAll();
            List<CityModel> cityList = new List<CityModel>();
            foreach (CityDTO ci in ciList)
            {
                CityModel ciItem = new CityModel
                {
                    ID = ci.ID,
                    CountryID = ci.CountryID,
                    Name = ci.Name
                };
                cityList.Add(ciItem);
            }
            ViewBag.CityList = cityList;

            return View("Edit", flightItem);
        }

        [HttpPost]
        public ActionResult Edit(FlightModel flight)
        {
            if (ModelState.IsValid)
            {
                FlightDTO fli = new FlightDTO
                {
                    ID = flight.ID,
                    CompanyID = flight.CompanyID,
                    Name = flight.Name,
                    DayOfWeek = flight.DayOfWeek,
                    CityDepartureID = flight.CityDepartureID,
                    CityArrivalID = flight.CityArrivalID,
                    DepartureTime = flight.DepartureTime,
                    ArrivalTime = flight.ArrivalTime
                };
                _flightService.Edit(fli);

                return RedirectToAction("List");
            }
            else
            {
                List<CompanyDTO> cmpList = _companyService.GetAll();
                List<CompanyModel> companyList = new List<CompanyModel>();
                foreach (CompanyDTO cmp in cmpList)
                {
                    CompanyModel cmpItem = new CompanyModel
                    {
                        ID = cmp.ID,
                        Name = cmp.Name
                    };
                    companyList.Add(cmpItem);
                }
                ViewBag.CompanyList = companyList;

                List<CityDTO> ciList = _cityService.GetAll();
                List<CityModel> cityList = new List<CityModel>();
                foreach (CityDTO ci in ciList)
                {
                    CityModel ciItem = new CityModel
                    {
                        ID = ci.ID,
                        CountryID = ci.CountryID,
                        Name = ci.Name
                    };
                    cityList.Add(ciItem);
                }
                ViewBag.CityList = cityList;

                return View();
            }
        }
    }
}