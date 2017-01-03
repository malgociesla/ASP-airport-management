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
            List<CountryModel> countryList = ctrList.Select(ctr => new CountryModel
            {
                ID = ctr.ID,
                Name = ctr.Name
            }).ToList();

            return View("List", countryList);
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
            if (ModelState.IsValid)
            {
                ICountryService countryService = new CountryService();
                CountryDTO ctr = new CountryDTO
                {
                    ID = country.ID,
                    Name = country.Name
                };
                countryService.Add(ctr);

                return RedirectToAction("List");
            }
            else return View();
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            ICountryService countryService = new CountryService();
            CountryDTO ctrItem = countryService.GetAll().FirstOrDefault(c => c.ID == id);
            CountryModel countryItem = new CountryModel
            {
                ID = ctrItem.ID,
                Name = ctrItem.Name
            };

            return View("Edit", countryItem);
        }

        [HttpPost]
        public ActionResult Edit(CountryModel country)
        {
            if (ModelState.IsValid)
            {
                ICountryService countryService = new CountryService();
                CountryDTO ctr = new CountryDTO
                {
                    ID = country.ID,
                    Name = country.Name
                };
                countryService.Edit(ctr);

                return RedirectToAction("List");
            }
            else return View();
        }
    }
}