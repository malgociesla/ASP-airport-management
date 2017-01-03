﻿using System;
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
            List<CompanyModel> companyList = cmpList.Select(cmp => new CompanyModel
            {
                ID = cmp.ID,
                Name = cmp.Name
            }).ToList();

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
        public ActionResult Add(CompanyModel company)
        {
            if (ModelState.IsValid)
            {
                ICompanyService companyService = new CompanyService();
                CompanyDTO cmp = new CompanyDTO { ID = company.ID, Name = company.Name };
                companyService.Add(cmp);

                return RedirectToAction("List");
            }
            else return View();
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            ICompanyService companyService = new CompanyService();
            CompanyDTO cmpItem = companyService.GetAll().FirstOrDefault(c=>c.ID==id);
            CompanyModel companyItem = new CompanyModel { ID=cmpItem.ID, Name=cmpItem.Name };

            return View("Edit",companyItem);
        }

        [HttpPost]
        public ActionResult Edit(CompanyModel company)
        {
            if (ModelState.IsValid)
            {
                ICompanyService companyService = new CompanyService();
                CompanyDTO cmp = new CompanyDTO { ID= company.ID, Name=company.Name};
                companyService.Edit(cmp);

                return RedirectToAction("List");
        }
            else return View();
    }
    }
}