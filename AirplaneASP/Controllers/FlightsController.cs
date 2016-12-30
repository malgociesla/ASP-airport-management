using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirportService;
using AirportService.DTO;

namespace AirplaneASP.Controllers
{
    //[RoutePrefix("Flights")]
    public class FlightsController : Controller
    {
        [HttpGet]

        public ActionResult List()
        {
            IFlightService flightService = new FlightService();
            List<FlightDTO> flightList = flightService.GetAll();

            return View("List", flightList);
        }

        [HttpGet]
        public ActionResult Remove(Guid id)
        {
            IFlightService flightService = new FlightService();
            flightService.Remove(id);

            //return List();
            return RedirectToAction("List");
        }

        public ActionResult Add()
        {
            ICompanyService companyService = new CompanyService();
            List<CompanyDTO> companyList = companyService.GetAll();
            ViewBag.CompanyList = companyList;

            ICityService cityService = new CityService();
            List<CityDTO> cityList = cityService.GetAll();
            ViewBag.CityList = cityList;

            return View();
        }

        [HttpPost]
        public ActionResult Add(FlightDTO flight)
        {
            IFlightService flightService = new FlightService();
            flightService.Add(flight);
            //return List();
            return RedirectToAction("List");
        }


        [HttpGet]
        //[Route("Edit/{flightID}")]
        public ActionResult Edit(Guid id)
        {
            IFlightService flightService = new FlightService();
            FlightDTO flightItem = flightService.GetAll().FirstOrDefault(f => f.ID == id);

            ICompanyService companyService = new CompanyService();
            List<CompanyDTO> companyList = companyService.GetAll();
            ViewBag.CompanyList = companyList;

            ICityService cityService = new CityService();
            List<CityDTO> cityList = cityService.GetAll();
            ViewBag.CityList = cityList;

            return View("Edit", flightItem);
        }

        [HttpPost]
        public ActionResult Edit(FlightDTO flight)
        {
            IFlightService flightService = new FlightService();
            flightService.Edit(flight);
            //return List();
            return RedirectToAction("List");
        }
    }
}