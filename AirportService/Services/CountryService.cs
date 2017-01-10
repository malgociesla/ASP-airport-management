using System;
using System.Collections.Generic;
using System.Linq;
using AirportService.DTO;
using AirplaneEF;

namespace AirportService
{
    public class CountryService : ICountryService
    {
        private readonly AirportContext _airplaneContext;
        public CountryService()
        {
            _airplaneContext = new AirportContext();
        }
        public Guid Add(CountryDTO countryDTO)
        {
            Country country = new Country { Name = countryDTO.Name };
            _airplaneContext.Countries.Add(country);
            _airplaneContext.SaveChanges();
            return country.Id;
        }

        public void Edit(CountryDTO countryDTO)
        {
            var country = _airplaneContext.Countries.FirstOrDefault(c => c.Id == countryDTO.ID);
            if (country != null)
            {
                country.Name = countryDTO.Name;
                _airplaneContext.SaveChanges();
            }
        }

        public List<CountryDTO> GetAll()
        {
            var countries = _airplaneContext.Countries.ToList().Select(c => new CountryDTO
            {
                ID = c.Id,
                Name = c.Name
            });

            return countries.ToList();
        }

        public void Remove(Guid id)
        {
            var country = _airplaneContext.Countries.FirstOrDefault(c => c.Id == id);
            if (country != null)
            {
                var city = _airplaneContext.Cities.Where(c => c.IdCountry == country.Id);
                ICityService cityService = new CityService();
                if (city.Any())
                    foreach (var c in city)                   
                        cityService.Remove(c.Id);
                _airplaneContext.Countries.Remove(country);
                _airplaneContext.SaveChanges();
            }
        }
    }
}
