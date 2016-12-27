﻿using System;
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
        public Guid Add(string name)
        {
            Country country = new Country { name = name };
            _airplaneContext.Countries.Add(country);
            _airplaneContext.SaveChanges();
            return country.idCountry;
        }

        public void Edit(CountryDTO countryDTO)
        {
            var country = _airplaneContext.Countries.FirstOrDefault(c => c.idCountry == countryDTO.ID);
            if (country != null)
            {
                country.name = countryDTO.Name;
                _airplaneContext.SaveChanges();
            }
        }

        public List<CountryDTO> GetAll()
        {
            var countries = _airplaneContext.Countries.ToList().Select(c => new CountryDTO
            {
                ID = c.idCountry,
                Name = c.name
            });

            return countries.ToList();
        }

        public void Remove(Guid id)
        {
            var country = _airplaneContext.Countries.FirstOrDefault(c => c.idCountry == id);
            if (country != null)
            {
                var city = _airplaneContext.Cities.Where(c => c.idCountry == country.idCountry);
                ICityService cityService = new CityService();
                if (city.Any())
                    foreach (var c in city)                   
                        cityService.Remove(c.idCity);
                _airplaneContext.Countries.Remove(country);
                _airplaneContext.SaveChanges();
            }
        }
    }
}
