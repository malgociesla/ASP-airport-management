using AirplaneASP.Mapping;
using AirplaneASP.Models.Flights;
using AirplaneASP.Models.Schedules;
using AirportService;
using AirportService.DTO;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace AirplaneASP.Controllers
{
    public class SchedulesController : Controller
    {
        private readonly IScheduleService _scheduleService;
        private readonly IFlightStateService _flightStateService;
        private readonly IFlightService _flightService;

        private readonly IMapper<ScheduleDTO, ScheduleModel> _scheduleMaper;
        private readonly IMapper<ScheduleDetailsDTO, ScheduleDetailsImportModel> _scheduleDetailsMaper;
        private readonly IMapper<FlightDTO, FlightModel> _flightMaper;

        //paging
        private static int _pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"].ToString());

        public SchedulesController(IScheduleService scheduleService,
                                   IFlightStateService flightStateService,
                                   IFlightService flightService,

                                   IMapper<ScheduleDTO, ScheduleModel> scheduleMaper,
                                   IMapper<ScheduleDetailsDTO, ScheduleDetailsImportModel> scheduleDetailsMaper,
                                   IMapper<FlightDTO, FlightModel> flightMaper)
        {
            this._scheduleService = scheduleService;
            this._flightStateService = flightStateService;
            this._flightService = flightService;

            this._scheduleMaper = scheduleMaper;
            this._scheduleDetailsMaper = scheduleDetailsMaper;
            this._flightMaper = flightMaper;
        }

        private int ValidatePageNo(int? page)
        {
            if (page == null ||
                page < 1)
            {
                page = 1;
            }
            return page.Value;
        }

        private IPagedList GetPage(int pageNumber, int pageSize, DateTime? from = null, DateTime? to = null)
        {
            int totalItemsCount = 0;
            List<ScheduleDetailsDTO> scheduleDTOPage = _scheduleService.GetList(pageNumber, pageSize, out totalItemsCount, from, to);
            //get subset of IPagedList and translate from ScheduleDetailsDTO to ScheduleDetailsModel
            var subset = _scheduleDetailsMaper.Map(scheduleDTOPage);

            IPagedList schedulePage = new StaticPagedList<ScheduleDetailsModel>(subset, pageNumber, pageSize, totalItemsCount) as IPagedList;
            return schedulePage;
        }
        //[HttpGet]
        public ActionResult List(int? page, DateTime? from, DateTime? to)
        {
            //pagination
            int pageNumber = ValidatePageNo(page);

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
            IPagedList schedulePage = GetPage(pageNumber, _pageSize, from, to);

            return View(schedulePage);
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
                int pageNumber = ValidatePageNo(page);

                ViewBag.FilterModel = filterModel;
                return View(GetPage(pageNumber, _pageSize)); //returns page without filter
            }
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

        [HttpGet]
        public ActionResult Remove(Guid id, int? page)
        {
            _scheduleService.Remove(id);

            return RedirectToAction("List", new { page = page });
        }

        public ActionResult ImportSchedule()
        {
            ImportViewModel model = new ImportViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult ImportSchedule(ImportViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //import based on model.ScheduleList
                    if (model.ScheduleList.Count != 0)
                    {
                        //import parameter -> list of checked items
                        var scheduleDTOList = _scheduleMaper.MapBack(model.ScheduleList.Where(s => s.Check == true)).ToList();
                        _scheduleService.UpdateSchedule(scheduleDTOList);
                    }
                    //upload items from file to view
                    if (model.UploadedFile != null)
                    {
                        model.ScheduleList = GetUploadedList(model.UploadedFile);
                    }
                }
            }
            catch (AirportServiceException ex)
            {
                ModelState.AddModelError("ImportModelException", ex.Message);
            }
            return View(model);
        }

        private List<ScheduleDetailsImportModel> GetUploadedList(HttpPostedFileBase file)
        {
            List<ScheduleDetailsImportModel> scheduleList = new List<ScheduleDetailsImportModel>();
            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                //Get list of imported schedule items
                List<ScheduleDetailsDTO> scheduleDTOList = _scheduleService.Import(file.InputStream);

                scheduleList = _scheduleDetailsMaper.Map(scheduleDTOList).ToList();
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
                int pageNumber = ValidatePageNo(page);

                int totalItemsCount = 0;
                excelBytes = _scheduleService.Export(_scheduleService.GetList(pageNumber, _pageSize, out totalItemsCount, from, to));
            }
            FileResult fr = new FileContentResult(excelBytes, "application/vnd.ms-excel")
            {
                FileDownloadName = string.Format("Export_{0}_{1}.xlsx", DateTime.Now.ToString("yyMMdd"), "Schedules")
            };

            return fr;
        }
    }
}