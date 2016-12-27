using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportService.DTO;
using AirplaneEF;

namespace AirportService
{
    public class CityService : ICityService
    {
        private readonly AirportContext _airplaneContext;
        public CityService()
        {
            _airplaneContext = new AirportContext();
        }

        public Guid Add(string name, Guid idCountry)
        {
            City city = new City { name = name, idCountry=idCountry };
            _airplaneContext.Cities.Add(city);
            _airplaneContext.SaveChanges();
            return city.idCity;
        }

        public void Edit(CityDTO cityDTO)
        {
            var city = _airplaneContext.Cities.FirstOrDefault(c => c.idCity == cityDTO.ID || c.idCountry==cityDTO.CountryID);
            if (city != null)
            {
                city.name = cityDTO.Name;
                _airplaneContext.SaveChanges();
            }
        }

        public List<CityDTO> GetAll()
        {  
            var cities = _airplaneContext.Cities.ToList().Select(c => new CityDTO
            {
                ID = c.idCity,
                CountryID=c.idCountry,
                Name = c.name
            });

            return cities.ToList();
        }

        public List<CityDTO> GetByCountry(Guid idCountry)
        {
            var cities = _airplaneContext.Cities.Where(c => c.idCountry == idCountry).ToList().Select(c => new CityDTO
            {
                ID = c.idCity,
                CountryID=c.idCountry,
                Name = c.name
            });

            return cities.ToList();
        }

        public void Remove(Guid id)
        {
            var city = _airplaneContext.Cities.FirstOrDefault(c=> c.idCity==id);
            if (city != null)
            {
                var flight = _airplaneContext.Flights.Where(f => (f.idCityArrival == id || f.idCityDeparture == id));
                if (flight.Any())
                    _airplaneContext.Flights.RemoveRange(flight);
                _airplaneContext.Cities.Remove(city);
                _airplaneContext.SaveChanges();
            }
        }


    }
}
