using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirportService;
using AirportService.DTO;
using AirplaneASP.Models.Countries;

namespace AirplaneASP.Controllers
{
    public class CountriesController : Controller
    {
        [HttpGet]
        public ActionResult List()
        {
            ICountryService countryService = new CountryService();
            List<CountryDTO> ctrList = countryService.GetAll();
            List<CountryModel> countryList = new List<CountryModel>();
            foreach (CountryDTO ctr in ctrList)
            {
                CountryModel ctrItem = new CountryModel { ID = ctr.ID, Name = ctr.Name };
                countryList.Add(ctrItem);
            }

            return View("List",countryList);
        }

        [HttpGet]
        public ActionResult Remove(Guid id)
        {
            ICountryService countryService = new CountryService();
            countryService.Remove(id);

            return RedirectToAction("List");
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(CountryModel country)
        {
            ICountryService countryService = new CountryService();
            CountryDTO ctr = new CountryDTO { ID = country.ID, Name = country.Name };
            countryService.Add(ctr);

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            ICountryService countryService = new CountryService();
            CountryDTO ctrItem = countryService.GetAll().FirstOrDefault(c => c.ID == id);
            CountryModel countryItem = new CountryModel { ID = ctrItem.ID, Name = ctrItem.Name };

            return View("Edit", countryItem);
        }

        [HttpPost]
        public ActionResult Edit(CountryModel country)
        {
            ICountryService countryService = new CountryService();
            CountryDTO ctr = new CountryDTO { ID = country.ID, Name = country.Name };
            countryService.Edit(ctr);

            return RedirectToAction("List");
        }
    }
}