using AirportService.DTO;
using System;
using System.Collections.Generic;

namespace AirportService
{
    public interface ICityService
    {
        Guid Add(string name);
        void Remove(Guid id);
        void Edit(Guid id, string name);
        List<CityDTO> GetAll();
    }
}
