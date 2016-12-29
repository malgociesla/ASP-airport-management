using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirportService;
using AirportService.DTO;
using AirplaneASP.Models.Schedule;
using PagedList;

namespace AirplaneASP.Controllers
{
    public class SchedulesController : Controller
    {
        [HttpGet]
        public ActionResult List(int? page)
        {
            IScheduleService scheduleService = new ScheduleService();
            List<ScheduleDTO> scheduleList = scheduleService.GetAll();

            if (page == null || page < 1) page = 1;

            //pagination
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            return View("List", scheduleList.ToPagedList(pageNumber,pageSize));
        }

        [HttpGet]
        public ActionResult Remove(Guid scheduleID, int? page)
        {
            IScheduleService scheduleService = new ScheduleService();
            scheduleService.Remove(scheduleID);

            return List(page);
        }

        public ActionResult GenerateSchedule()
        {
            IFlightService flightService = new FlightService();
            List<FlightDTO> flightList = flightService.GetAll();
            ViewBag.FlightList = flightList;
            return View();
        }

        [HttpPost]
        public ActionResult GenerateSchedule(GenerateScheduleModel generateScheduleModel)
        {
            IScheduleService scheduleService = new ScheduleService();
            scheduleService.GenerateSchedule(generateScheduleModel.StartDate, generateScheduleModel.EndDate, generateScheduleModel.FlightID);
            return List(null);
        }


        [HttpGet]
        public ActionResult Edit(Guid scheduleID, int? page)
        {
            IScheduleService scheduleService = new ScheduleService();
            ScheduleDTO scheduleItem = scheduleService.GetAll().FirstOrDefault(s => s.ID == scheduleID);

            IFlightService flightService = new FlightService();
            List<FlightDTO> flightList = flightService.GetAll();
            ViewBag.FlightList = flightList;

            IFlightStateService flightStateService = new FlightStateService();
            List<FlightStateDTO> flightStateList = flightStateService.GetAll();
            ViewBag.FlightStateList = flightStateList;

            ViewBag.Page = page;

            return View("Edit", scheduleItem);
        }

        [HttpPost]
        public ActionResult Edit(ScheduleDTO schedule, int? page)
        {
            IScheduleService scheduleService = new ScheduleService();
            scheduleService.Edit(schedule);
            return List(page);
        }

    }
}