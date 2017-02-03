using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirportService;
using AirportService.DTO;
using AirplaneASP.Models.Countries;
using AirplaneASP.Mapping;

namespace AirplaneASP.Controllers
{
    public class CountriesController : Controller
    {
        private readonly ICountryService _countryService;
        private readonly IMapper<CountryDTO, CountryModel> _countryMaper;

        public CountriesController(ICountryService countryService, IMapper<CountryDTO, CountryModel> countryMaper)
        {
            this._countryService = countryService;
            this._countryMaper = countryMaper;
        }

        [HttpGet]
        public ActionResult List()
        {
            List<CountryDTO> ctrList = _countryService.GetAll();
            var countryList = _countryMaper.Map(ctrList);

            return View("List", countryList);
        }

        [HttpGet]
        public ActionResult Remove(Guid id)
        {
            _countryService.Remove(id);

            return RedirectToAction("List");
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(CountryModel country)
        {
            if (ModelState.IsValid)
            {
                var ctr = _countryMaper.MapBack(country);
                _countryService.Add(ctr);

                return RedirectToAction("List");
            }
            else return View();
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            CountryDTO ctrItem = _countryService.GetAll().FirstOrDefault(c => c.ID == id);
            var countryItem = _countryMaper.Map(ctrItem);

            return View("Edit", countryItem);
        }

        [HttpPost]
        public ActionResult Edit(CountryModel country)
        {
            if (ModelState.IsValid)
            {
                var ctr = _countryMaper.MapBack(country);
                _countryService.Edit(ctr);

                return RedirectToAction("List");
            }
            else return View();
        }
    }
}