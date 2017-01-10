using System;
using System.Collections.Generic;
using System.Linq;
using AirportService.DTO;
using AirplaneEF;

namespace AirportService
{
    public class FlightStateService : IFlightStateService
    {
        private readonly AirportContext _airplaneContext;
        public FlightStateService()
        {
            _airplaneContext = new AirportContext();
        }
        public Guid Add(FlightStateDTO flightStateDTO)
        {
            FlightState state = new FlightState { Name = flightStateDTO.Name };
            _airplaneContext.FlightStates.Add(state);
            _airplaneContext.SaveChanges();
            return state.Id;
        }

        public void Edit(FlightStateDTO flightStateDTO)
        {
            var state = _airplaneContext.FlightStates.FirstOrDefault(c => c.Id == flightStateDTO.ID);
            if (state != null)
            {
                state.Name = flightStateDTO.Name;
                _airplaneContext.SaveChanges();
            }
        }

        public List<FlightStateDTO> GetAll()
        {
            var states = _airplaneContext.FlightStates.ToList().Select(c => new FlightStateDTO
            {
                ID = c.Id,
                Name = c.Name
            });

            return states.ToList();
        }

        public void Remove(Guid id)
        {
            var state = _airplaneContext.FlightStates.FirstOrDefault(c => c.Id == id);
            if (state != null)
            {
                _airplaneContext.FlightStates.Remove(state);
                _airplaneContext.SaveChanges();
            }
        }
    }
}
