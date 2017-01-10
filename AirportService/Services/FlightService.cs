using System;
using System.Collections.Generic;
using System.Linq;
using AirportService.DTO;
using AirplaneEF;

namespace AirportService
{
    public class FlightService : IFlightService
    {
        private readonly AirportContext _airplaneContext;
        public FlightService()
        {
            _airplaneContext = new AirportContext();
        }

        public Guid Add(FlightDTO flightDTO)
        {
            Flight flight = new Flight
            {
                IdCompany = flightDTO.CompanyID,
                Name = flightDTO.Name,
                FDayofWeek = flightDTO.DayOfWeek,
                IdCityDeparture = flightDTO.CityDepartureID,
                IdCityArrival = flightDTO.CityArrivalID,
                DepartureTime = flightDTO.DepartureTime,
                ArrivalTime = flightDTO.ArrivalTime
            };
            _airplaneContext.Flights.Add(flight);
            _airplaneContext.SaveChanges();
            return flight.Id;
        }

        public void Edit(FlightDTO flightDTO)
        {
            var flight = _airplaneContext.Flights.FirstOrDefault(f => f.Id == flightDTO.ID);
            if (flight != null)
            {
                flight.IdCompany = flightDTO.CompanyID;
                flight.Name = flightDTO.Name;
                flight.FDayofWeek = flightDTO.DayOfWeek;
                flight.IdCityDeparture = flightDTO.CityDepartureID;
                flight.IdCityArrival = flightDTO.CityArrivalID;
                flight.DepartureTime = flightDTO.DepartureTime;
                flight.ArrivalTime = flightDTO.ArrivalTime;

                _airplaneContext.SaveChanges();
            }//else flight doesn't exist
        }

        public List<FlightDTO> GetAll()
        {
            var flights = _airplaneContext.Flights.ToList().Select(f => new FlightDTO
            {
                ID = f.Id,
                CompanyID = f.IdCompany,
                Name = f.Name,
                DayOfWeek = f.FDayofWeek,
                CityDepartureID = f.IdCityDeparture,
                CityArrivalID = f.IdCityArrival,
                DepartureTime = f.DepartureTime,
                ArrivalTime = f.ArrivalTime
            });

            return flights.ToList();
        }

        public void Remove(Guid id)
        {
            var flight = _airplaneContext.Flights.FirstOrDefault(f => f.Id == id);
            if (flight != null)
            {
                _airplaneContext.Flights.Remove(flight);
                _airplaneContext.SaveChanges();
            }
        }
    }
}
