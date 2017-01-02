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
            //pagination
            if (page == null || page < 1) page = 1;
            int pageNumber = (page ?? 1);
            int pageSize;
            int.TryParse(System.Configuration.ConfigurationManager.AppSettings["pageSize"].ToString(), out pageSize);              
            IScheduleService scheduleService = new ScheduleService();
            IPagedList<ScheduleDTO> schedulePage = scheduleService.GetPage(pageNumber,pageSize);

            return View("List", schedulePage);
        }

        [HttpGet]
        public ActionResult Remove(Guid id, int? page)
        {
            IScheduleService scheduleService = new ScheduleService();
            scheduleService.Remove(id);
            //return List(page);
            return RedirectToAction("List",page);
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
            //return List(null);
            return RedirectToAction("List",0);
        }


        [HttpGet]
        public ActionResult Edit(Guid id, int? page)
        {
            IScheduleService scheduleService = new ScheduleService();
            ScheduleDTO scheduleItem = scheduleService.GetAll().FirstOrDefault(s => s.ID == id);

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
            //return List(page);
            return RedirectToAction("List",page);
        }

    }
}