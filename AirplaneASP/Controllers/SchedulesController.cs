using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirportService;
using AirportService.DTO;
using AirplaneASP.Models.Schedules;
using PagedList;
using AirplaneASP.Models.Flights;

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

            //PagedList<ScheduleModel> schedulePage = new PagedList<ScheduleModel>(pageNumber, pageSize);

            //IPagedList<ScheduleModel> schedulePage = schdPage.All();
            //foreach (ScheduleDTO schdItem in schdPage)
            //{
            //    ScheduleModel schd = new ScheduleModel { ID = schdItem.ID, FlightID = schdItem.FlightID, FlightStateID = schdItem.FlightStateID, DepartureDT = schdItem.DepartureDT, ArrivalDT = schdItem.ArrivalDT, Comment = schdItem.Comment };
            //    schedulePage.Add(schd);
            //}

            return View("List", schedulePage);
        }

        [HttpGet]
        public ActionResult Remove(Guid id, int? page)
        {
            IScheduleService scheduleService = new ScheduleService();
            scheduleService.Remove(id);

            return RedirectToAction("List",page);
        }

        public ActionResult GenerateSchedule()
        {
            IFlightService flightService = new FlightService();
            List<FlightDTO> fliList = flightService.GetAll();
            List<FlightModel> flightList = new List<FlightModel>();
            foreach (FlightDTO fli in fliList)
            {
                FlightModel fliItem = new FlightModel { ID = fli.ID, CompanyID = fli.CompanyID, Name = fli.Name, DayOfWeek = fli.DayOfWeek, CityDepartureID = fli.CityDepartureID, CityArrivalID = fli.CityArrivalID, DepartureTime = fli.DepartureTime, ArrivalTime = fli.ArrivalTime };
                flightList.Add(fliItem);
            }
            ViewBag.FlightList = flightList;

            return View();
        }

        [HttpPost]
        public ActionResult GenerateSchedule(GenerateScheduleModel generateScheduleModel)
        {
            if (ModelState.IsValid)
            {
                IScheduleService scheduleService = new ScheduleService();
                scheduleService.GenerateSchedule(generateScheduleModel.StartDate, generateScheduleModel.EndDate, generateScheduleModel.FlightID);

                return RedirectToAction("List", 0);
            }
            else
            {
                IFlightService flightService = new FlightService();
                List<FlightDTO> fliList = flightService.GetAll();
                List<FlightModel> flightList = new List<FlightModel>();
                foreach (FlightDTO fli in fliList)
                {
                    FlightModel fliItem = new FlightModel { ID = fli.ID, CompanyID = fli.CompanyID, Name = fli.Name, DayOfWeek = fli.DayOfWeek, CityDepartureID = fli.CityDepartureID, CityArrivalID = fli.CityArrivalID, DepartureTime = fli.DepartureTime, ArrivalTime = fli.ArrivalTime };
                    flightList.Add(fliItem);
                }
                ViewBag.FlightList = flightList;

                return View();
            }
        }


        [HttpGet]
        public ActionResult Edit(Guid id, int? page)
        {
            IScheduleService scheduleService = new ScheduleService();
            ScheduleDTO schdItem = scheduleService.GetAll().FirstOrDefault(s => s.ID == id);
            ScheduleModel scheduleItem = new ScheduleModel { ID = schdItem.ID, FlightID = schdItem.FlightID, FlightStateID = schdItem.FlightStateID, DepartureDT = schdItem.DepartureDT, ArrivalDT = schdItem.ArrivalDT, Comment = schdItem.Comment };

            IFlightService flightService = new FlightService();
            List<FlightDTO> fliList = flightService.GetAll();
            List<FlightModel> flightList = new List<FlightModel>();
            foreach (FlightDTO fli in fliList)
            {
                FlightModel fliItem = new FlightModel { ID = fli.ID, CompanyID = fli.CompanyID, Name = fli.Name, DayOfWeek = fli.DayOfWeek, CityDepartureID = fli.CityDepartureID, CityArrivalID = fli.CityArrivalID, DepartureTime = fli.DepartureTime, ArrivalTime = fli.ArrivalTime };
                flightList.Add(fliItem);
            }
            ViewBag.FlightList = flightList;

            IFlightStateService flightStateService = new FlightStateService();
            List<FlightStateDTO> flightStateList = flightStateService.GetAll();
            ViewBag.FlightStateList = flightStateList;

            ViewBag.Page = page;

            return View("Edit", scheduleItem);
        }

        [HttpPost]
        public ActionResult Edit(ScheduleModel schedule, int? page)
        {
            if (ModelState.IsValid)
            {
                IScheduleService scheduleService = new ScheduleService();
                ScheduleDTO schd = new ScheduleDTO { ID = schedule.ID, FlightID = schedule.FlightID, FlightStateID = schedule.FlightStateID, DepartureDT = schedule.DepartureDT, ArrivalDT = schedule.ArrivalDT, Comment = schedule.Comment };
                scheduleService.Edit(schd);

                return RedirectToAction("List", page);
            }
            else return View();
        }

    }
}