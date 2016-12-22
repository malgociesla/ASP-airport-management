﻿using System;
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
        private AirportContext _airplaneContext;
        public CityService()
        {
            _airplaneContext = new AirportContext();
        }

        public Guid Add(string name)
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

        public List<CityDTO> GetAll()
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
            if(city!=null)
            _airplaneContext.Cities.Remove(city);

            var flight = _airplaneContext.Flights.Where(f => (f.idCityArrival == id || f.idCityDeparture == id));
            if(flight!=null)
            _airplaneContext.Flights.RemoveRange(flight);
            _airplaneContext.SaveChanges();
        }


    }
}
