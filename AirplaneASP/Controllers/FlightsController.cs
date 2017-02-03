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
            List<FlightDTO> flightDTOList = _flightService.GetAll();
            List<FlightModel> flightList = new List<FlightModel>();
            foreach (FlightDTO flightDTO in flightDTOList)
            {
                FlightModel fliItem = new FlightModel
                {
                    ID = flightDTO.ID,
                    CompanyID = flightDTO.CompanyID,
                    Name = flightDTO.Name,
                    DayOfWeek = flightDTO.DayOfWeek,
                    CityDepartureID = flightDTO.CityDepartureID,
                    CityArrivalID = flightDTO.CityArrivalID,
                    DepartureTime = flightDTO.DepartureTime,
                    ArrivalTime = flightDTO.ArrivalTime
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
            List<CompanyDTO> companyDTOList = _companyService.GetAll();
            List<CompanyModel> companyList = new List<CompanyModel>();
            foreach (CompanyDTO cmp in companyDTOList)
            {
                CompanyModel cmpItem = new CompanyModel
                {
                    ID = cmp.ID,
                    Name = cmp.Name
                };
                companyList.Add(cmpItem);
            }
            ViewBag.CompanyList = companyList;

            List<CityDTO> cityDTOList = _cityService.GetAll();
            List<CityModel> cityList = new List<CityModel>();
            foreach (CityDTO ci in cityDTOList)
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
                FlightDTO flightDTO = new FlightDTO
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
                _flightService.Add(flightDTO);

                return RedirectToAction("List");
            }
            else
            {
                List<CompanyDTO> companyDTOList = _companyService.GetAll();
                List<CompanyModel> companyList = new List<CompanyModel>();
                foreach (CompanyDTO cmp in companyDTOList)
                {
                    CompanyModel cmpItem = new CompanyModel
                    {
                        ID = cmp.ID,
                        Name = cmp.Name
                    };
                    companyList.Add(cmpItem);
                }
                ViewBag.CompanyList = companyList;

                List<CityDTO> cityDTOList = _cityService.GetAll();
                List<CityModel> cityList = new List<CityModel>();
                foreach (CityDTO ci in cityDTOList)
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
            FlightDTO flightDTO = _flightService.GetAll().FirstOrDefault(f => f.ID == id);
            FlightModel flight = new FlightModel
            {
                ID = flightDTO.ID,
                CompanyID = flightDTO.CompanyID,
                Name = flightDTO.Name,
                DayOfWeek = flightDTO.DayOfWeek,
                CityDepartureID = flightDTO.CityDepartureID,
                CityArrivalID = flightDTO.CityArrivalID,
                DepartureTime = flightDTO.DepartureTime,
                ArrivalTime = flightDTO.ArrivalTime
            };

            List<CompanyDTO> companyDTOList = _companyService.GetAll();
            List<CompanyModel> companyList = new List<CompanyModel>();
            foreach (CompanyDTO companyDTO in companyDTOList)
            {
                CompanyModel company = new CompanyModel
                {
                    ID = companyDTO.ID,
                    Name = companyDTO.Name
                };
                companyList.Add(company);
            }
            ViewBag.CompanyList = companyList;

            List<CityDTO> cityDTOList = _cityService.GetAll();
            List<CityModel> cityList = new List<CityModel>();
            foreach (CityDTO ci in cityDTOList)
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

            return View("Edit", flight);
        }

        [HttpPost]
        public ActionResult Edit(FlightModel flight)
        {
            if (ModelState.IsValid)
            {
                FlightDTO flightDTO = new FlightDTO
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
                _flightService.Edit(flightDTO);

                return RedirectToAction("List");
            }
            else
            {
                List<CompanyDTO> companyDTOList = _companyService.GetAll();
                List<CompanyModel> companyList = new List<CompanyModel>();
                foreach (CompanyDTO cmp in companyDTOList)
                {
                    CompanyModel cmpItem = new CompanyModel
                    {
                        ID = cmp.ID,
                        Name = cmp.Name
                    };
                    companyList.Add(cmpItem);
                }
                ViewBag.CompanyList = companyList;

                List<CityDTO> cityDTOList = _cityService.GetAll();
                List<CityModel> cityList = new List<CityModel>();
                foreach (CityDTO ci in cityDTOList)
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