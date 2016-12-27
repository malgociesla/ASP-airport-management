﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirportService;
using AirportService.DTO;

namespace AirplaneASP.Controllers
{
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
        public ActionResult Remove(Guid flightID)
        {
            IFlightService flightService = new FlightService();
            flightService.Remove(flightID);

            return List();
        }
    }
}