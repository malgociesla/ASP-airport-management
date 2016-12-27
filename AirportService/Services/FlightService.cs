using System;
using System.Collections.Generic;
using System.Linq;
using AirportService.DTO;
using AirplaneEF;

namespace AirportService
{
    public class FlightService: IFlightService
    {
        private readonly AirportContext _airplaneContext;
        public FlightService()
        {
            _airplaneContext = new AirportContext();
        }

        public Guid Add(Guid idCompany, Guid idState, string name, int dayOfWeek, Guid idCityDeparture, Guid idCityArrival, TimeSpan departureTime, TimeSpan arrivalTime)
        {
            Flight flight = new Flight
            {
                idCompany=idCompany,
                idFlightState=idState,
                name=name,
                fDayofWeek=dayOfWeek,
                idCityDeparture=idCityDeparture,
                idCityArrival=idCityArrival,
                departureTime=departureTime,
                arrivalTime=arrivalTime
            };
            _airplaneContext.Flights.Add(flight);
            _airplaneContext.SaveChanges();
            return flight.idFlight;
        }

        public void Edit(FlightDTO flightDTO)
        {
            var flight = _airplaneContext.Flights.FirstOrDefault(f => f.idFlight == flightDTO.ID);
            if (flight != null)
            {
                flight.idCompany = flightDTO.CompanyID;
                flight.idFlightState = flightDTO.FlightStateID;
                flight.name = flightDTO.Name;
                flight.fDayofWeek = flightDTO.DayOfWeek;
                flight.idCityDeparture = flightDTO.CityDepartureID;
                flight.idCityArrival = flightDTO.CityArrivalID;
                flight.departureTime = flightDTO.DepartureTime;
                flight.arrivalTime = flightDTO.ArrivalTime;

                _airplaneContext.SaveChanges();
            }//else flight doesn't exist
        }

        public List<FlightDTO> GetAll()
        {
            var flights = _airplaneContext.Flights.ToList().Select(f => new FlightDTO
            {
                ID = f.idFlight,
                CompanyID=f.idCompany,
                FlightStateID=f.idFlightState,
                Name=f.name,
                DayOfWeek=f.fDayofWeek,
                CityDepartureID=f.idCityDeparture,
                CityArrivalID=f.idCityArrival,
                DepartureTime=f.departureTime,
                ArrivalTime=f.arrivalTime
            });

            return flights.ToList();
        }

        public void Remove(Guid id)
        {
            var flight = _airplaneContext.Flights.FirstOrDefault(f => f.idFlight == id);
            if (flight != null)
            {
                _airplaneContext.Flights.Remove(flight);
                _airplaneContext.SaveChanges();
            }
        }
    }
}
