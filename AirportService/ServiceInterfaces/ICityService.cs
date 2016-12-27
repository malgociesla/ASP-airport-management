using AirportService.DTO;
using System;
using System.Collections.Generic;

namespace AirportService
{
    public interface ICityService
    {
        Guid Add(string name, Guid idCountry);
        void Remove(Guid id);
        void Edit(CityDTO cityDTO);
        List<CityDTO> GetAll();
        List<CityDTO> GetByCountry(Guid idCountry);
    }
}
