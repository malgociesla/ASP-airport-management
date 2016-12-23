using System;
using System.Collections.Generic;
using AirportService.DTO;

namespace AirportService
{
    public interface ICompanyService
    {
        Guid Add(string name);
        void Remove(Guid id);
        void Edit(Guid id, string name);
        List<CompanyDTO> GetAll();
    }
}
