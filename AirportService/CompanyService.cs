using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportService.DTO;
using AirplaneEF;

namespace AirportService
{
    public class CompanyService : ICompanyService
    {
        private AirportContext _airplaneContext;
        public CompanyService()
        {
            _airplaneContext = new AirportContext();
        }
        public Guid Add(string name)
        {
            Company company = new Company { name = name };
            _airplaneContext.Companies.Add(company);
            _airplaneContext.SaveChanges();
            return company.idCompany;
        }

        public void Edit(Guid id, string name)
        {
            var company = _airplaneContext.Companies.FirstOrDefault(c => c.idCompany == id);
            company.name = name;
            _airplaneContext.SaveChanges();
        }

        public List<CompanyDTO> GetAll()
        {
            var companies = _airplaneContext.Companies.ToList().Select(c => new CompanyDTO
            {
                ID = c.idCompany,
                Name = c.name
            });

            return companies.ToList();
        }

        public void Remove(Guid id)
        {
            var company = _airplaneContext.Companies.FirstOrDefault(c => c.idCompany == id);
            if (company != null)
                _airplaneContext.Companies.Remove(company);

            var flight = _airplaneContext.Flights.Where(f => f.idCompany == id);
            if (flight != null)
                _airplaneContext.Flights.RemoveRange(flight);
            _airplaneContext.SaveChanges();
        }
    }
}
