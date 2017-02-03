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
using AirplaneASP.Mapping;

namespace AirplaneASP.Controllers
{
    public class SchedulesController : Controller
    {
        private readonly IScheduleService _scheduleService;
        private readonly IFlightStateService _flightStateService;
        private readonly IFlightService _flightService;

        private readonly IMapper<ScheduleDTO, ScheduleModel> _scheduleMaper;
        private readonly IMapper<FlightDTO, FlightModel> _flightMaper;

        public SchedulesController(IScheduleService scheduleService,
                                   IFlightStateService flightStateService,
                                   IFlightService flightService,

                                   IMapper<ScheduleDTO, ScheduleModel> scheduleMaper,
                                   IMapper<FlightDTO, FlightModel> flightMaper)
        {
            this._scheduleService = scheduleService;
            this._flightStateService = flightStateService;
            this._flightService = flightService;

            this._scheduleMaper = scheduleMaper;
            this._flightMaper = flightMaper;
        }

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
            int totalItemsCount = 0;
            List<ScheduleDetailsDTO> scheduleDTOPage = _scheduleService.GetList(pageNumber, pageSize, out totalItemsCount, from, to);
            //get subset of IPagedList and translate from ScheduleDTO to ScheduleModel
            var subset = scheduleDTOPage
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
            _scheduleService.Remove(id);

            return RedirectToAction("List", new { page = page });
        }

        public ActionResult GenerateSchedule()
        {
            List<FlightDTO> flightDTOList = _flightService.GetAll();
            var flightList = _flightMaper.Map(flightDTOList);
            ViewBag.FlightList = flightList;

            return View();
        }

        [HttpPost]
        public ActionResult GenerateSchedule(GenerateScheduleModel generateScheduleModel)
        {
            if (ModelState.IsValid)
            {
                _scheduleService.GenerateSchedule(generateScheduleModel.StartDate, generateScheduleModel.EndDate, generateScheduleModel.FlightID);

                return RedirectToAction("List");
            }
            else
            {
                List<FlightDTO> flightDTOList = _flightService.GetAll();
                var flightList = _flightMaper.Map(flightDTOList);
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
            if (ModelState.IsValid)
            {
                //import based on model.ScheduleList
                if (model.ScheduleList.Count != 0)
                {
                    //import parameter -> list of checked items
                    _scheduleService.UpdateSchedule(model.ScheduleList.Where(s => s.Check == true).Select(s => new ScheduleDTO()
                    {
                        ID = s.ID,
                        FlightStateID = s.FlightStateID,
                        FlightID = s.FlightID,
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
            }
            return View(model);
        }

        private List<ScheduleDetailsImportModel> GetUploadedList(HttpPostedFileBase file)
        {
            List<ScheduleDetailsImportModel> scheduleList = null;

            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                    //Get list of imported schedule items
                    List<ScheduleDetailsDTO> scheduleDTOList = _scheduleService.Import(file.InputStream);

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

                //error: couldn't import file
            }
            return scheduleList;
        }

        public ActionResult ExportSchedule(bool all, int? page, DateTime? from = null, DateTime? to = null)
        {
            byte[] excelBytes;
            if (all)
            {
                excelBytes = _scheduleService.Export(_scheduleService.GetAll());
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
                excelBytes = _scheduleService.Export(_scheduleService.GetList(pageNumber, pageSize, out totalItemsCount, from, to));
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
            ScheduleDTO scheduleDTO = _scheduleService.GetAll().FirstOrDefault(s => s.ID == id);
            var schedule = _scheduleMaper.Map(scheduleDTO);

            List<FlightDTO> flightDTOList = _flightService.GetAll();
            var flightList = _flightMaper.Map(flightDTOList);
            ViewBag.FlightList = flightList;

            List<FlightStateDTO> flightStateDTOList = _flightStateService.GetAll();
            ViewBag.FlightStateList = flightStateDTOList;

            ViewBag.Page = page;

            return View("Edit", schedule);
        }

        [HttpPost]
        public ActionResult Edit(ScheduleModel schedule, int? page)
        {
            if (ModelState.IsValid)
            {
                var scheduleDTO = _scheduleMaper.MapBack(schedule);
                _scheduleService.Edit(scheduleDTO);

                return RedirectToAction("List", new { page = page });
            }
            else
            {
                ViewBag.Page = page;

                List<FlightDTO> flightDTOList = _flightService.GetAll();
                var flightList = _flightMaper.Map(flightDTOList);
                ViewBag.FlightList = flightList;

                List<FlightStateDTO> flightStateDTOList = _flightStateService.GetAll();
                ViewBag.FlightStateList = flightStateDTOList;

                return View();
            }
        }

    }
}