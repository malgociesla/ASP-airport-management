using System;
using System.Collections.Generic;
using AirportService.DTO;

namespace AirportService
{
    public interface IFlightService
    {
        Guid Add(Guid idCompany, Guid idState, string name, int dayOfWeek, Guid idCityDeparture, Guid idCityArrival, TimeSpan departureTime, TimeSpan arrivalTime);
        void Remove(Guid id);
        void Edit(FlightDTO flightDTO);
        List<FlightDTO> GetAll();
    }
}
