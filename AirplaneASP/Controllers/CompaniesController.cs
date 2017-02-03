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

        public CompaniesController(ICompanyService companyService, IMapper<CompanyDTO,CompanyModel> companyMaper)
        {
            this._companyService = companyService;
            this._companyMaper = companyMaper;
        }

        [HttpGet]
        public ActionResult List()
        {
            List<CompanyDTO> cmpList = _companyService.GetAll();
            var companyList = this._companyMaper.Map(cmpList);

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
                var cmp = AutoMapper.Mapper.Map<CompanyModel, CompanyDTO>(company);
                _companyService.Add(cmp);

                return RedirectToAction("List");
            }
            else return View();
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            ICompanyService companyService = new CompanyService();
            CompanyDTO cmpItem = companyService.GetAll().FirstOrDefault(c => c.ID == id);
            var companyItem = AutoMapper.Mapper.Map<CompanyDTO, CompanyModel>(cmpItem);

            return View("Edit", companyItem);
        }

        [HttpPost]
        public ActionResult Edit(CompanyModel company)
        {
            if (ModelState.IsValid)
            {
                var cmp = AutoMapper.Mapper.Map<CompanyModel , CompanyDTO>(company);
                _companyService.Edit(cmp);

                return RedirectToAction("List");
            }
            else return View();
        }
    }
}