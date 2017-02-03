using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AirportService;
using AirportService.DTO;
using AirplaneASP.Models.Companies;
using AirplaneASP.Mapping;

namespace AirplaneASP.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper<CompanyDTO, CompanyModel> _companyMaper;

        public CompaniesController(ICompanyService companyService, IMapper<CompanyDTO,CompanyModel> companyModelMaper)
        {
            this._companyService = companyService;
            this._companyMaper = companyModelMaper;
        }

        [HttpGet]
        public ActionResult List()
        {
            List<CompanyDTO> companyDTOList = _companyService.GetAll();
            var companyList = this._companyMaper.Map(companyDTOList);

            return View("List", companyList);
        }

        [HttpGet]
        public ActionResult Remove(Guid id)
        {
            _companyService.Remove(id);

            return RedirectToAction("List");
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(CompanyModel company)
        {
            if (ModelState.IsValid)
            {
                var companyDTO = _companyMaper.MapBack(company);
                _companyService.Add(companyDTO);

                return RedirectToAction("List");
            }
            else return View();
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            ICompanyService companyService = new CompanyService();
            CompanyDTO companyDTO = companyService.GetAll().FirstOrDefault(c => c.ID == id);
            var company = this._companyMaper.Map(companyDTO);

            return View("Edit", company);
        }

        [HttpPost]
        public ActionResult Edit(CompanyModel company)
        {
            if (ModelState.IsValid)
            {
                var companyDTO = _companyMaper.MapBack(company);
                _companyService.Edit(companyDTO);

                return RedirectToAction("List");
            }
            else return View();
        }
    }
}