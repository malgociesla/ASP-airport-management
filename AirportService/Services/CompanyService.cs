using System;
using System.Collections.Generic;
using System.Linq;
using AirportService.DTO;
using AirplaneEF;

namespace AirportService
{
    public class CompanyService : ICompanyService
    {
        private readonly AirportContext _airplaneContext;
        public CompanyService()
        {
            _airplaneContext = new AirportContext();
        }
        public Guid Add(CompanyDTO companyDTO)
        {
            Company company = new Company { Name = companyDTO.Name };
            _airplaneContext.Companies.Add(company);
            _airplaneContext.SaveChanges();
            return company.Id;
        }

        public void Edit(CompanyDTO companyDTO)
        {
            var company = _airplaneContext.Companies.FirstOrDefault(c => c.Id == companyDTO.ID);
            if (company != null)
            {
                company.Name = companyDTO.Name;
                _airplaneContext.SaveChanges();
            }
        }

        public List<CompanyDTO> GetAll()
        {
            var companies = _airplaneContext.Companies.ToList().Select(c => new CompanyDTO
            {
                ID = c.Id,
                Name = c.Name
            });

            return companies.ToList();
        }

        public void Remove(Guid id)
        {
            var company = _airplaneContext.Companies.FirstOrDefault(c => c.Id == id);
            if (company != null)
            { 
                _airplaneContext.Companies.Remove(company);
            _airplaneContext.SaveChanges();
        }
        }
    }
}
