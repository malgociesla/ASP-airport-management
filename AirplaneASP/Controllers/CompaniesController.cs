using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirportService;
using AirportService.DTO;
using AirplaneASP.Models.Companies;

namespace AirplaneASP.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            this._companyService = companyService;
        }

        [HttpGet]
        public ActionResult List()
        {
            List<CompanyDTO> cmpList = _companyService.GetAll();
            var companyList = AutoMapper.Mapper.Map<List<CompanyDTO>, List<CompanyModel>>(cmpList);

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