using System;
using System.Collections.Generic;
using AirportService.DTO;

namespace AirportService
{
    public interface IFlightService
    {
        Guid Add(FlightDTO flightDTO);
        void Remove(Guid id);
        void Edit(FlightDTO flightDTO);
        List<FlightDTO> GetAll();
    }
}
