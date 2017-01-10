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

        public Guid Add(CityDTO cityDTO)
        {
            City city = new City { Name = cityDTO.Name, IdCountry= cityDTO.CountryID };
            _airplaneContext.Cities.Add(city);
            _airplaneContext.SaveChanges();
            return city.Id;
        }

        public void Edit(CityDTO cityDTO)
        {
            var city = _airplaneContext.Cities.FirstOrDefault(c => c.Id == cityDTO.ID);
            if (city != null)
            {
                city.Name = cityDTO.Name;
                city.IdCountry = cityDTO.CountryID;
                _airplaneContext.SaveChanges();
            }
        }

        public List<CityDTO> GetAll()
        {  
            var cities = _airplaneContext.Cities.ToList().Select(c => new CityDTO
            {
                ID = c.Id,
                CountryID=c.IdCountry,
                Name = c.Name
            });

            return cities.ToList();
        }

        public List<CityDTO> GetByCountry(Guid idCountry)
        {
            var cities = _airplaneContext.Cities.Where(c => c.IdCountry == idCountry).ToList().Select(c => new CityDTO
            {
                ID = c.Id,
                CountryID=c.IdCountry,
                Name = c.Name
            });

            return cities.ToList();
        }

        public void Remove(Guid id)
        {
            var city = _airplaneContext.Cities.FirstOrDefault(c=> c.Id==id);
            if (city != null)
            {
                var flight = _airplaneContext.Flights.Where(f => (f.IdCityArrival == id || f.IdCityDeparture == id));
                if (flight.Any())
                    _airplaneContext.Flights.RemoveRange(flight);
                _airplaneContext.Cities.Remove(city);
                _airplaneContext.SaveChanges();
            }
        }


    }
}
