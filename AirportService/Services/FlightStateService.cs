using System;
using System.Collections.Generic;
using System.Linq;
using AirportService.DTO;
using AirplaneEF;

namespace AirportService
{
    public class FStatusService : IFlightStateService
    {
        private readonly AirportContext _airplaneContext;
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

        public void Edit(FlightStateDTO flightStateDTO)
        {
            var state = _airplaneContext.FlightStates.FirstOrDefault(c => c.idFlightState == flightStateDTO.ID);
            if (state != null)
            {
                state.name = flightStateDTO.Name;
                _airplaneContext.SaveChanges();
            }
        }

        public List<FlightStateDTO> GetAll()
        {
            var states = _airplaneContext.FlightStates.ToList().Select(c => new FlightStateDTO
            {
                ID = c.idFlightState,
                Name = c.name
            });

            return states.ToList();
        }

        public void Remove(Guid id)
        {
            var state = _airplaneContext.FlightStates.FirstOrDefault(c => c.idFlightState == id);
            if (state != null)
            {
                _airplaneContext.FlightStates.Remove(state);
                _airplaneContext.SaveChanges();
            }
        }
    }
}
