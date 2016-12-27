using System;
using System.Collections.Generic;
using AirportService.DTO;

namespace AirportService
{
    public interface ICompanyService
    {
        Guid Add(CompanyDTO companyDTO);
        void Remove(Guid id);
        void Edit(CompanyDTO companyDTO);
        List<CompanyDTO> GetAll();
    }
}
