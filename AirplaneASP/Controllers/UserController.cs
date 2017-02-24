using AirplaneASP.Authorization;
using AirplaneASP.Mapping;
using AirplaneASP.Models.Cities;
using AirplaneASP.Models.Countries;
using AirplaneASP.Models.Schedules;
using AirportService;
using AirportService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirplaneASP.Controllers
{
    [AuthorizeAccess(Roles = "USER")]
    public class UserController : Controller
    {
        //private readonly ICityService _cityService;
        //private readonly ICountryService _countryService;
       private readonly IScheduleService _scheduleService;

        //private readonly IMapper<CityDTO, CityModel> _cityMaper;
        //private readonly IMapper<CountryDTO, CountryModel> _countryMaper;
        private readonly IMapper<ScheduleDetailsDTO, ScheduleDetailsImportModel> _scheduleDetailsMaper;

        public UserController(
                                 //ICityService cityService,
                                //ICountryService countryService,
                                IScheduleService scheduleService,

                                //IMapper<CityDTO, CityModel> cityMaper,
                                //IMapper<CountryDTO, CountryModel> countryMaper,
                                IMapper<ScheduleDetailsDTO, ScheduleDetailsImportModel> scheduleDetailsMaper)
        {
            //this._cityService = cityService;
            //this._countryService = countryService;
            this._scheduleService = scheduleService;
            //this._cityMaper = cityMaper;
            //this._countryMaper = countryMaper;
            this._scheduleDetailsMaper = scheduleDetailsMaper;
        }
        public ActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //public JsonResult GetCities()
        //{
        //    var cityDTOList = _cityService.GetAll();
        //    var cityList = _cityMaper.Map(cityDTOList);
        //    return Json(cityList, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public JsonResult GetSchedule()
        {
            int totalItemsCount = 0;
            //List<ScheduleDetailsDTO> scheduleDTOPage = _scheduleService.GetList(pageNumber, pageSize, out totalItemsCount, from, to);
            List<ScheduleDetailsDTO> scheduleDTOPage = _scheduleService.GetList(1, 12, out totalItemsCount);
            //get subset of IPagedList and translate from ScheduleDetailsDTO to ScheduleDetailsModel
            var subset = _scheduleDetailsMaper.Map(scheduleDTOPage);
            return Json(subset, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetListByCity(DateTime from, DateTime to, List<Guid> selectedCityIDs = null)
        {
            List<ScheduleDetailsDTO> scheduleDTO = _scheduleService.GetListByCity(from,to,selectedCityIDs);
            var schedules = _scheduleDetailsMaper.Map(scheduleDTO);
            return Json(schedules, JsonRequestBehavior.AllowGet);
        }
    }
}