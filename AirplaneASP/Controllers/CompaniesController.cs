using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirportService;
using AirportService.DTO;

namespace AirplaneASP.Controllers
{
    public class CompaniesController : Controller
    {
        [HttpGet]
        public ActionResult List()
        {
            ICompanyService companyService = new CompanyService();
            List<CompanyDTO> companyList = companyService.GetAll();

            return View("List", companyList);
        }

        [HttpGet]
        public ActionResult Remove(Guid companyID)
        {
            ICompanyService companyService = new CompanyService();
            companyService.Remove(companyID);

            return List();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(CompanyDTO company)
        {
            ICompanyService companyService = new CompanyService();
            companyService.Add(company);
            return List();
        }

        [HttpGet]
        public ActionResult Edit(Guid companyID)
        {
            ICompanyService companyService = new CompanyService();
            CompanyDTO companyItem = companyService.GetAll().FirstOrDefault(c=>c.ID==companyID);
            return View("Edit",companyItem);
        }

        [HttpPost]
        public ActionResult Edit(CompanyDTO company)
        {
            ICompanyService companyService = new CompanyService();
            companyService.Edit(company);
            return List();
        }
    }
}