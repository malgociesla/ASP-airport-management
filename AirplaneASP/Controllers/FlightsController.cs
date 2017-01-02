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
    //[RoutePrefix("Flights")]
    public class FlightsController : Controller
    {
        [HttpGet]

        public ActionResult List()
        {
            IFlightService flightService = new FlightService();
            List<FlightDTO> fliList = flightService.GetAll();
            List<FlightModel> flightList = new List<FlightModel>();
            foreach (FlightDTO fli in fliList)
            {
                FlightModel fliItem = new FlightModel { ID = fli.ID, CompanyID = fli.CompanyID, Name = fli.Name, DayOfWeek = fli.DayOfWeek, CityDepartureID = fli.CityDepartureID, CityArrivalID = fli.CityArrivalID, DepartureTime = fli.DepartureTime, ArrivalTime = fli.ArrivalTime };
                flightList.Add(fliItem);
            }

            return View("List", flightList);
        }

        [HttpGet]
        public ActionResult Remove(Guid id)
        {
            IFlightService flightService = new FlightService();
            flightService.Remove(id);

            return RedirectToAction("List");
        }

        public ActionResult Add()
        {
            ICompanyService companyService = new CompanyService();
            List<CompanyDTO> cmpList = companyService.GetAll();
            List<CompanyModel> companyList = new List<CompanyModel>();
            foreach (CompanyDTO cmp in cmpList)
            {
                CompanyModel cmpItem = new CompanyModel { ID = cmp.ID, Name = cmp.Name };
                companyList.Add(cmpItem);
            }
            ViewBag.CompanyList = companyList;

            ICityService cityService = new CityService();
            List<CityDTO> ciList = cityService.GetAll();
            List<CityModel> cityList = new List<CityModel>();
            foreach (CityDTO ci in ciList)
            {
                CityModel ciItem = new CityModel { ID = ci.ID, CountryID = ci.CountryID, Name = ci.Name };
                cityList.Add(ciItem);
            }
            ViewBag.CityList = cityList;

            return View();
        }

        [HttpPost]
        public ActionResult Add(FlightModel flight)
        {
            IFlightService flightService = new FlightService();
            FlightDTO fli = new FlightDTO { ID = flight.ID, CompanyID = flight.CompanyID, Name = flight.Name, DayOfWeek = flight.DayOfWeek, CityDepartureID = flight.CityDepartureID, CityArrivalID = flight.CityArrivalID, DepartureTime = flight.DepartureTime, ArrivalTime = flight.ArrivalTime };
            flightService.Add(fli);

            return RedirectToAction("List");
        }


        [HttpGet]
        //[Route("Edit/{flightID}")]
        public ActionResult Edit(Guid id)
        {
            IFlightService flightService = new FlightService();
            FlightDTO fliItem = flightService.GetAll().FirstOrDefault(f => f.ID == id);
            FlightModel flightItem = new FlightModel { ID = fliItem.ID, CompanyID = fliItem.CompanyID, Name = fliItem.Name, DayOfWeek = fliItem.DayOfWeek, CityDepartureID = fliItem.CityDepartureID, CityArrivalID = fliItem.CityArrivalID, DepartureTime = fliItem.DepartureTime, ArrivalTime = fliItem.ArrivalTime };

            ICompanyService companyService = new CompanyService();
            List<CompanyDTO> cmpList = companyService.GetAll();
            List<CompanyModel> companyList = new List<CompanyModel>();
            foreach (CompanyDTO cmp in cmpList)
            {
                CompanyModel cmpItem = new CompanyModel { ID = cmp.ID, Name = cmp.Name };
                companyList.Add(cmpItem);
            }
            ViewBag.CompanyList = companyList;

            ICityService cityService = new CityService();
            List<CityDTO> ciList = cityService.GetAll();
            List<CityModel> cityList = new List<CityModel>();
            foreach (CityDTO ci in ciList)
            {
                CityModel ciItem = new CityModel { ID = ci.ID, CountryID = ci.CountryID, Name = ci.Name };
                cityList.Add(ciItem);
            }
            ViewBag.CityList = cityList;

            return View("Edit", flightItem);
        }

        [HttpPost]
        public ActionResult Edit(FlightModel flight)
        {
            IFlightService flightService = new FlightService();
            FlightDTO fli = new FlightDTO { ID = flight.ID, CompanyID = flight.CompanyID, Name = flight.Name, DayOfWeek = flight.DayOfWeek, CityDepartureID = flight.CityDepartureID, CityArrivalID = flight.CityArrivalID, DepartureTime = flight.DepartureTime, ArrivalTime = flight.ArrivalTime };
            flightService.Edit(fli);

            return RedirectToAction("List");
        }
    }
}