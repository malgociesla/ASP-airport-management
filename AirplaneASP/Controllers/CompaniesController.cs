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
        [HttpGet]
        public ActionResult List()
        {
            ICompanyService companyService = new CompanyService();
            List<CompanyDTO> cmpList = companyService.GetAll();
            List<CompaniesModel> companyList = new List<CompaniesModel>();
            foreach (CompanyDTO cmp in cmpList)
            {
                CompaniesModel companyItem = new CompaniesModel { ID = cmp.ID, Name = cmp.Name };
                companyList.Add(companyItem);
            }

            return View("List", companyList);
        }

        [HttpGet]
        public ActionResult Remove(Guid id)
        {
            ICompanyService companyService = new CompanyService();
            companyService.Remove(id);

            return RedirectToAction("List");
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(CompaniesModel company)
        {
            ICompanyService companyService = new CompanyService();
            CompanyDTO cmp = new CompanyDTO { ID=company.ID, Name=company.Name};
            companyService.Add(cmp);
 
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            ICompanyService companyService = new CompanyService();
            CompanyDTO cmpItem = companyService.GetAll().FirstOrDefault(c=>c.ID==id);
            CompaniesModel companyItem = new CompaniesModel { ID=cmpItem.ID, Name=cmpItem.Name };

            return View("Edit",companyItem);
        }

        [HttpPost]
        public ActionResult Edit(CompaniesModel company)
        {
            ICompanyService companyService = new CompanyService();
            CompanyDTO cmp = new CompanyDTO { ID= company.ID, Name=company.Name};
            companyService.Edit(cmp);

            return RedirectToAction("List");
        }
    }
}