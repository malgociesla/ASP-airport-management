using System;
using System.Collections.Generic;
using AirportService.DTO;

namespace AirportService
{
    public interface IFlightService
    {
        Guid Add(Guid idCompany, Guid idState, string name, int dayOfWeek, Guid idCityDeparture, Guid idCityArrival, TimeSpan departureTime, TimeSpan arrivalTime);
        void Remove(Guid id);
        void Edit(Guid id, Guid idCompany, Guid idState, string name, int dayOfWeek, Guid idCityDeparture, Guid idCityArrival, TimeSpan departureTime, TimeSpan arrivalTime);
        void Edit(Guid id, string name);
        void Edit(Guid id, int dayOfWeek);
        void EditCompany(Guid id, Guid idCompany);
        void EditState(Guid id, Guid idState);
        void EditCityDeparture(Guid id, Guid idCityDeparture);
        void EditCityArrival(Guid id, Guid idCityArrival);
        void EditDepartureTime(Guid id, TimeSpan departureTime);
        void EditArrivalTime(Guid id, TimeSpan arrivalTime);
        List<FlightDTO> GetAll();
    }
}
