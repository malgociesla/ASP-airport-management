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
        //[HttpGet]
        public ActionResult List(int? page, DateTime? from, DateTime? to)
        {
            //pagination
            if (page == null ||
                page < 1)
            {
                page = 1;
            }
            int pageNumber = page.Value;
            int pageSize;
            int.TryParse(System.Configuration.ConfigurationManager.AppSettings["pageSize"].ToString(), out pageSize);

            //filter
            IScheduleService scheduleService = new ScheduleService();
            if (from != null && to != null)
            {
                DateTime thisFrom = (DateTime)from;
                DateTime thisTo = (DateTime)to;
                ViewBag.FilterModel = new FilterViewModel() { FromDate = thisFrom, ToDate = thisTo };
            }
            else
            {
                ViewBag.FilterModel = new FilterViewModel();
            }

            //get Page
            IPagedList schedulePage = GetPage(pageNumber, pageSize, from, to);

            return View(schedulePage);
        }

        private IPagedList GetPage(int pageNumber, int pageSize, DateTime? from = null, DateTime? to = null)
        {
            IScheduleService scheduleService = new ScheduleService();
            int totalItemsCount=0;
            List<ScheduleDetailsDTO> schdPage = scheduleService.GetList(pageNumber, pageSize, out totalItemsCount, from, to);
            //get subset of IPagedList and translate from ScheduleDTO to ScheduleModel
            var subset = schdPage
               .Select(s => new ScheduleDetailsModel
               {
                   ID = s.ID,
                   FlightStateID = s.FlightStateID,
                   FlightID = s.FlightID,
                   DepartureDT = s.DepartureDT,
                   ArrivalDT = s.ArrivalDT,
                   Comment = s.Comment,
                   CityDeparture = s.CityDeparture,
                   CountryDeparture = s.CountryDeparture,
                   CityArrival = s.CityArrival,
                   CountryArrival = s.CountryArrival,
                   Company = s.Company
               });
            IPagedList schedulePage = new StaticPagedList<ScheduleDetailsModel>(subset, pageNumber, pageSize, totalItemsCount) as IPagedList;
            return schedulePage;
        }

        [HttpPost]
        public ActionResult List(int? page, FilterViewModel filterModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("List", new { page = page, from = filterModel.FromDate, to = filterModel.ToDate });
            }
            else
            {
                //pagination
                if (page == null || 
                    page < 1)
                {
                    page = 1;
                }
                int pageNumber = page.Value;
                int pageSize;
                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["pageSize"].ToString(), out pageSize);

                ViewBag.FilterModel = filterModel;
                return View(GetPage(pageNumber, pageSize)); //returns page without filter
            }
        }

        [HttpGet]
        public ActionResult Remove(Guid id, int? page)
        {
            IScheduleService scheduleService = new ScheduleService();
            scheduleService.Remove(id);

            return RedirectToAction("List", new { page = page });
        }

        public ActionResult GenerateSchedule()
        {
            IFlightService flightService = new FlightService();
            List<FlightDTO> fliList = flightService.GetAll();
            List<FlightModel> flightList = new List<FlightModel>();
            foreach (FlightDTO fli in fliList)
            {
                FlightModel fliItem = new FlightModel
                {
                    ID = fli.ID,
                    CompanyID = fli.CompanyID,
                    Name = fli.Name,
                    DayOfWeek = fli.DayOfWeek,
                    CityDepartureID = fli.CityDepartureID,
                    CityArrivalID = fli.CityArrivalID,
                    DepartureTime = fli.DepartureTime,
                    ArrivalTime = fli.ArrivalTime
                };
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

                return RedirectToAction("List");
            }
            else
            {
                IFlightService flightService = new FlightService();
                List<FlightDTO> fliList = flightService.GetAll();
                List<FlightModel> flightList = new List<FlightModel>();
                foreach (FlightDTO fli in fliList)
                {
                    FlightModel fliItem = new FlightModel
                    {
                        ID = fli.ID,
                        CompanyID = fli.CompanyID,
                        Name = fli.Name,
                        DayOfWeek = fli.DayOfWeek,
                        CityDepartureID = fli.CityDepartureID,
                        CityArrivalID = fli.CityArrivalID,
                        DepartureTime = fli.DepartureTime,
                        ArrivalTime = fli.ArrivalTime
                    };
                    flightList.Add(fliItem);
                }
                ViewBag.FlightList = flightList;

                return View();
            }
        }

        public ActionResult ImportSchedule()
        {
            ImportViewModel model = new ImportViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult ImportSchedule(ImportViewModel model)
        {
            //import based on model.ScheduleList
            if (model.ScheduleList.Count != 0)
            {
                IScheduleService scheduleService = new ScheduleService();
                //import paraeter -> list of checked items
                scheduleService.Import(model.ScheduleList.Where(s => s.Check==true).Select(s => new ScheduleDTO()
                {
                    ID = s.ID,
                    FlightStateID = s.FlightStateID,
                    FlightID = s. FlightID,
                    DepartureDT = s.DepartureDT,
                    ArrivalDT = s.ArrivalDT,
                    Comment = s.Comment
                }
                ).ToList());
            }
            //upload items from file to view
            if (model.UploadedFile != null)
            {
                model.ScheduleList = GetUploadedList(model.UploadedFile);
            }
            return View(model);
        }

        private List<ScheduleDetailsImportModel> GetUploadedList(HttpPostedFileBase file)
        {
            List<ScheduleDetailsImportModel> scheduleList = null;

            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                string fileName = file.FileName;
                string fileContentType = file.ContentType;
                byte[] fileBytes = new byte[file.ContentLength];
                var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                //Get list of imported schedule items
                IScheduleService scheduleService = new ScheduleService();
                List<ScheduleDetailsDTO> scheduleDTOList = scheduleService.GetImportedList(file.InputStream);
                scheduleList = scheduleDTOList.Select(s => new ScheduleDetailsImportModel
                {
                    ID = s.ID,
                    FlightStateID = s.FlightStateID,
                    FlightID = s.FlightID,
                    DepartureDT = s.DepartureDT,
                    ArrivalDT = s.ArrivalDT,
                    Comment = s.Comment,
                    CityDeparture = s.CityDeparture,
                    CountryDeparture = s.CountryDeparture,
                    CityArrival = s.CityArrival,
                    CountryArrival = s.CountryArrival,
                    Company = s.Company,
                    Check = false
                }).ToList();
            }
            return scheduleList;
        }

        public ActionResult ExportSchedule(bool all, int? page, DateTime? from = null, DateTime? to = null)
        {
            byte[] excelBytes;
            IScheduleService scheduleService = new ScheduleService();
            if (all)
            {
                excelBytes = scheduleService.ExportSchedule(scheduleService.GetAll());
            }
            else
            {
                //pagination
                if (page == null ||
                    page < 1)
                {
                    page = 1;
                }
                int pageNumber = page.Value;
                int pageSize;
                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["pageSize"].ToString(), out pageSize);
                int totalItemsCount = 0;
                excelBytes = scheduleService.ExportSchedule(scheduleService.GetList(pageNumber, pageSize, out totalItemsCount, from, to));
            }
            FileResult fr = new FileContentResult(excelBytes, "application/vnd.ms-excel")
            {
                FileDownloadName = string.Format("Export_{0}_{1}.xlsx", DateTime.Now.ToString("yyMMdd"), "Schedules")
            };

            return fr;
        }

        [HttpGet]
        public ActionResult Edit(Guid id, int? page)
        {
            IScheduleService scheduleService = new ScheduleService();
            ScheduleDTO schdItem = scheduleService.GetAll().FirstOrDefault(s => s.ID == id);
            ScheduleModel scheduleItem = new ScheduleModel
            {
                ID = schdItem.ID,
                FlightID = schdItem.FlightID,
                FlightStateID = schdItem.FlightStateID,
                DepartureDT = schdItem.DepartureDT,
                ArrivalDT = schdItem.ArrivalDT,
                Comment = schdItem.Comment
            };

            IFlightService flightService = new FlightService();
            List<FlightDTO> fliList = flightService.GetAll();
            List<FlightModel> flightList = new List<FlightModel>();
            foreach (FlightDTO fli in fliList)
            {
                FlightModel fliItem = new FlightModel
                {
                    ID = fli.ID,
                    CompanyID = fli.CompanyID,
                    Name = fli.Name,
                    DayOfWeek = fli.DayOfWeek,
                    CityDepartureID = fli.CityDepartureID,
                    CityArrivalID = fli.CityArrivalID,
                    DepartureTime = fli.DepartureTime,
                    ArrivalTime = fli.ArrivalTime
                };
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
                ScheduleDTO schd = new ScheduleDTO
                {
                    ID = schedule.ID,
                    FlightID = schedule.FlightID,
                    FlightStateID = schedule.FlightStateID,
                    DepartureDT = schedule.DepartureDT,
                    ArrivalDT = schedule.ArrivalDT,
                    Comment = schedule.Comment
                };
                scheduleService.Edit(schd);

                return RedirectToAction("List", new { page = page });
            }
            else
            {
                ViewBag.Page = page;

                IFlightService flightService = new FlightService();
                List<FlightDTO> fliList = flightService.GetAll();
                List<FlightModel> flightList = new List<FlightModel>();
                foreach (FlightDTO fli in fliList)
                {
                    FlightModel fliItem = new FlightModel
                    {
                        ID = fli.ID,
                        CompanyID = fli.CompanyID,
                        Name = fli.Name,
                        DayOfWeek = fli.DayOfWeek,
                        CityDepartureID = fli.CityDepartureID,
                        CityArrivalID = fli.CityArrivalID,
                        DepartureTime = fli.DepartureTime,
                        ArrivalTime = fli.ArrivalTime
                    };
                    flightList.Add(fliItem);
                }
                ViewBag.FlightList = flightList;

                IFlightStateService flightStateService = new FlightStateService();
                List<FlightStateDTO> flightStateList = flightStateService.GetAll();
                ViewBag.FlightStateList = flightStateList;

                return View();
            }
        }

    }
}