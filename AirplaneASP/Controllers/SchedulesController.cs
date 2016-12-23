﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirportService;
using AirportService.DTO;

namespace AirplaneASP.Controllers
{
    public class SchedulesController : Controller
    {
        [HttpGet]
        public ActionResult Schedules()
        {
            IScheduleService scheduleService = new ScheduleService();
            List<ScheduleDTO> scheduleList = scheduleService.GetAll();

            return View("Schedules", scheduleList);
        }

        [HttpGet]
        public ActionResult Remove(Guid scheduleID)
        {
            IScheduleService scheduleService = new ScheduleService();
            scheduleService.Remove(scheduleID);

            return Schedules();
        }
    }
}