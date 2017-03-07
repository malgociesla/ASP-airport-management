using AirportService;
using AirportService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AirplaneWebApiService.Controllers
{
    public class CityController : ApiController
    {
        private readonly ICityService _cityService;
        private readonly ICountryService _countryService;

        public CityController(ICityService cityService, ICountryService countryService)
        {
            this._cityService = cityService;
            this._countryService = countryService;
        }
        public IEnumerable<CityDTO> GetAll()
        {
            List<CityDTO> cityList = _cityService.GetAll();
            return cityList;
        }
    }
}
