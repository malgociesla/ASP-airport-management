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

        public void Edit(Guid id, int dayOfWeek)
        {
            var flight = _airplaneContext.Flights.FirstOrDefault(f => f.idFlight == id);
            if (flight != null)
            {
                flight.fDayofWeek = dayOfWeek;
                _airplaneContext.SaveChanges();
            }//else flight doesn't exist
        }

        public void Edit(Guid id, string name)
        {
            var flight = _airplaneContext.Flights.FirstOrDefault(f => f.idFlight == id);
            if (flight != null)
            {
                flight.name = name;
                _airplaneContext.SaveChanges();
            }//else flight doesn't exist
        }

        public void Edit(Guid id, Guid idCompany, Guid idState, string name, int dayOfWeek, Guid idCityDeparture, Guid idCityArrival, TimeSpan departureTime, TimeSpan arrivalTime)
        {
            var flight = _airplaneContext.Flights.FirstOrDefault(f => f.idFlight == id);
            if (flight != null)
            {
                flight.idCompany = idCompany;
                flight.idFlightState = idState;
                flight.name = name;
                flight.fDayofWeek = dayOfWeek;
                flight.idCityDeparture = idCityDeparture;
                flight.idCityArrival = idCityArrival;
                flight.departureTime = departureTime;
                flight.arrivalTime = arrivalTime;

                _airplaneContext.SaveChanges();
            }//else flight doesn't exist
        }

        public void EditCityArrival(Guid id, Guid idCityArrival)
        {
            var flight = _airplaneContext.Flights.FirstOrDefault(f => f.idFlight == id);
            if (flight != null)
            {
                flight.idCityArrival = idCityArrival;
                _airplaneContext.SaveChanges();
            }//else flight doesn't exist
        }

        public void EditCityDeparture(Guid id, Guid idCityDeparture)
        {
            var flight = _airplaneContext.Flights.FirstOrDefault(f => f.idFlight == id);
            if (flight != null)
            {
                flight.idCityDeparture = idCityDeparture;
                _airplaneContext.SaveChanges();
            }//else flight doesn't exist
        }

        public void EditCompany(Guid id, Guid idCompany)
        {
            var flight = _airplaneContext.Flights.FirstOrDefault(f => f.idFlight == id);
            if (flight != null)
            {
                flight.idCompany = idCompany;
                _airplaneContext.SaveChanges();
            }//else flight doesn't exist
        }

        public void EditDepartureTime(Guid id, TimeSpan departureTime)
        {
            var flight = _airplaneContext.Flights.FirstOrDefault(f => f.idFlight == id);
            if (flight != null)
            {
                flight.departureTime = departureTime;
                _airplaneContext.SaveChanges();
            }//else flight doesn't exist
        }

        public void EditArrivalTime(Guid id, TimeSpan arrivalTime)
        {
            var flight = _airplaneContext.Flights.FirstOrDefault(f => f.idFlight == id);
            if (flight != null)
            {
                flight.arrivalTime = arrivalTime;
                _airplaneContext.SaveChanges();
            }//else flight doesn't exist
        }

        public void EditState(Guid id, Guid idState)
        {
            var flight = _airplaneContext.Flights.FirstOrDefault(f => f.idFlight == id);
            if (flight != null)
            {
                flight.idFlightState = idState;
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
