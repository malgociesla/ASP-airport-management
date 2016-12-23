﻿using System;
using System.Collections.Generic;
using System.Linq;
using AirportService.DTO;
using AirplaneEF;

namespace AirportService
{
    public class FStatusService : IFlightStateService
    {
        private AirportContext _airplaneContext;
        public FStatusService()
        {
            _airplaneContext = new AirportContext();
        }
        public Guid Add(string name)
        {
            FlightState state = new FlightState { name = name };
            _airplaneContext.FlightStates.Add(state);
            _airplaneContext.SaveChanges();
            return state.idFlightState;
        }

        public void Edit(Guid id, string name)
        {
            var state = _airplaneContext.FlightStates.FirstOrDefault(c => c.idFlightState == id);
            state.name = name;
            _airplaneContext.SaveChanges();
        }

        public List<FlightStateDTO> GetAll()
        {
            var statuses = _airplaneContext.FlightStates.ToList().Select(c => new FlightStateDTO
            {
                ID = c.idFlightState,
                Name = c.name
            });

            return statuses.ToList();
        }

        public void Remove(Guid id)
        {
            var state = _airplaneContext.FlightStates.FirstOrDefault(c => c.idFlightState == id);
            if (state != null)
            {
                _airplaneContext.FlightStates.Remove(state);

                //var flight = _airplaneContext.Flights.Where(f => f.idFStatus == id);
                //if (flight != null)
                //    _airplaneContext.Flights.RemoveRange(flight);
                _airplaneContext.SaveChanges();
            }
        }
    }
}
