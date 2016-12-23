﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirportService;
using AirportService.DTO;

namespace AirplaneASP.Controllers
{
    public class CountriesController : Controller
    {
        [HttpGet]
        public ActionResult Countries()
        {
            ICountryService countryService = new CountryService();
            List<CountryDTO> countryList = countryService.GetAll();

            return View("Countries",countryList);
        }

        [HttpGet]
        public ActionResult Remove(Guid countryID)
        {
            ICountryService countryService = new CountryService();
            countryService.Remove(countryID);

            return Countries();
        }
    }
}