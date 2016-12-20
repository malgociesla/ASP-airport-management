using AirportService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportService
{
    public interface ICityService
    {
        Guid AddCity(string name);
        void Remove(Guid id);
        void Edit(Guid id, string name);
        List<CityDTO> GetCities();
    }
}
