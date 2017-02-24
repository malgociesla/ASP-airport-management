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
        private readonly IScheduleService _scheduleService;

       public ScheduleController()
        {
            this._scheduleService = new ScheduleService();
        }

        public IEnumerable<ScheduleDetailsDTO> GetSchesules()
        {
            int totalItemsCount = 0;
            //List<ScheduleDetailsDTO> scheduleDTOPage = _scheduleService.GetList(pageNumber, pageSize, out totalItemsCount, from, to);
            List<ScheduleDetailsDTO> scheduleDTOPage = _scheduleService.GetList(1, 12, out totalItemsCount);

            return scheduleDTOPage;
        }

        public IEnumerable<ScheduleDetailsDTO> GetListByCity(DateTime from, DateTime to, List<Guid> selectedCityIDs = null)
        {
            List<ScheduleDetailsDTO> scheduleDTO = _scheduleService.GetListByCity(from, to, selectedCityIDs);
            return scheduleDTO;
        }
    }
}
