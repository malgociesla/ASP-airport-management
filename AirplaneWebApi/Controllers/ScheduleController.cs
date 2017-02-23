using AirportService;
using AirportService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AirplaneWebApi.Controllers
{
    public class ScheduleController : ApiController
    {
        private readonly ICityService _cityService;
        private readonly ICountryService _countryService;
        private readonly IScheduleService _scheduleService;

       public ScheduleController()
        {
            this._cityService = new CityService();
            this._countryService = new CountryService();
            this._scheduleService = new ScheduleService();
        }

        public IEnumerable<object> GetAll()
        {
            int totalItemsCount = 0;
            //List<ScheduleDetailsDTO> scheduleDTOPage = _scheduleService.GetList(pageNumber, pageSize, out totalItemsCount, from, to);
            List<ScheduleDetailsDTO> scheduleDTOPage = _scheduleService.GetList(1, 12, out totalItemsCount);
            //get subset of IPagedList and translate from ScheduleDetailsDTO to ScheduleDetailsModel

            return scheduleDTOPage;
        }
    }
}
