using System;
using System.Collections.Generic;
using AirportService.DTO;

namespace AirportService
{
    public interface IFlightStateService
    {
        Guid Add(string name);
        void Remove(Guid id);
        void Edit(FlightStateDTO flightStateDTO);
        List<FlightStateDTO> GetAll();
    }
}
