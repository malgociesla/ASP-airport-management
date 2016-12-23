using AirportService.DTO;
using System;
using System.Collections.Generic;

namespace AirportService
{
    public interface ICityService
    {
        Guid Add(string name, Guid idCountry);
        void Remove(Guid id);
        void Edit(Guid id, string name);
        void Edit(Guid idCity, Guid idCountry);
        List<CityDTO> GetAll();
        List<CityDTO> GetByCountry(Guid idCountry);
    }
}
