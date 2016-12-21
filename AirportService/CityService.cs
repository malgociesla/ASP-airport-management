using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportService.DTO;
using AirplaneDB;

namespace AirportService
{
    public class CityService : ICityService
    {
        public CityService()
        {
            _airplaneContext = new AirportContext();
        }

        private AirportContext _airplaneContext;

        public Guid AddCity(string name)
        {
            City city = new City { name = name };
            _airplaneContext.Cities.Add(city);
            _airplaneContext.SaveChanges();
            return city.idCity;
        }

        public void Edit(Guid id, string name)
        {
            var city = _airplaneContext.Cities.FirstOrDefault(c=> c.idCity==id);
            city.name = name;
            _airplaneContext.SaveChanges();
        }

        public List<CityDTO> GetCities()
        {  
            var cities = _airplaneContext.Cities.ToList().Select(c => new CityDTO
            {
                ID = c.idCity,
                Name = c.name
            });

            return cities.ToList();
        }

        public void Remove(Guid id)
        {
            var city = _airplaneContext.Cities.FirstOrDefault(c=> c.idCity==id);
            _airplaneContext.Cities.Remove(city);
            _airplaneContext.SaveChanges();
            //_airplaneContext.Flights.Where(c=>c.idFlight)

        }
    }
}
