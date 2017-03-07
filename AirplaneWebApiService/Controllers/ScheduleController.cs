﻿using AirportService;
using AirportService.DTO;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace AirplaneWebApiService.Controllers
{
    [RoutePrefix("api/schedules")]
    public class ScheduleController : ApiController
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            this._scheduleService = scheduleService;
        }

        public IEnumerable<ScheduleDetailsDTO> GetListByCity(DateTime startDate, DateTime endDate, [FromUri]List<Guid> guid)
        {
            List<ScheduleDetailsDTO> scheduleDTO = _scheduleService.GetListByCity(startDate, endDate, guid);
            return scheduleDTO;
        }
    }
}
