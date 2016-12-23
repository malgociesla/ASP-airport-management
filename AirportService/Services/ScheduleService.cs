using System;
using System.Collections.Generic;
using System.Linq;
using AirportService.DTO;
using AirplaneEF;

namespace AirportService
{
    public class ScheduleService:IScheduleService
    {
        private AirportContext _airplaneContext;
        public ScheduleService()
        {
            _airplaneContext = new AirportContext();
        }

        public Guid Add(Guid idFlight, DateTime departureDT, DateTime arrivalDT)
        {
            return Add(idFlight, departureDT, arrivalDT,"");
        }

        public Guid Add(Guid idFlight, DateTime departureDT, DateTime arrivalDT, string comment)
        {
            Schedule schedule = new Schedule
            {
                idFlight = idFlight,
                departureDT = departureDT,
                arrivalDT = arrivalDT,
                comment=comment
            };
            _airplaneContext.Schedules.Add(schedule);
            _airplaneContext.SaveChanges();
            return schedule.idSchedule;
        }

        public void Edit(Guid id, string comment)
        {
            var schedule = _airplaneContext.Schedules.FirstOrDefault(c => c.idSchedule == id);
            if (schedule != null)
            {
                schedule.comment = comment;
                _airplaneContext.SaveChanges();
            }//else schedule doesn't exist
        }

        public void Edit(Guid id, Guid idFlight)
        {
            var flight = _airplaneContext.Flights.FirstOrDefault(f => f.idFlight == idFlight);
            if (flight != null)
            {
                var schedule = _airplaneContext.Schedules.FirstOrDefault(s => s.idSchedule == id);
                if (schedule != null)
                {
                    schedule.idFlight = idFlight;
                    _airplaneContext.SaveChanges();
                }//else schedule doesn't exist
            }
            //else flight doesn't exist
        }

        public void Edit(Guid id, Guid idFlight, DateTime departureDT, DateTime arrivalDT)
        {
            Edit(id,idFlight,departureDT,arrivalDT,"");
        }

        public void Edit(Guid id, Guid idFlight, DateTime departureDT, DateTime arrivalDT, string comment)
        {
            var flight = _airplaneContext.Flights.FirstOrDefault(f => f.idFlight == idFlight);
            if (flight != null)
            {
                var schedule = _airplaneContext.Schedules.FirstOrDefault(s => s.idSchedule == id);
                if (schedule != null)
                {
                    schedule.idFlight = idFlight;
                    schedule.departureDT = departureDT;
                    schedule.arrivalDT = arrivalDT;
                    schedule.comment = comment;
                    _airplaneContext.SaveChanges();
                }//else schedule doesn't exist
        }
        //else flight doesn't exist
    }

    public void EditArrival(Guid id, DateTime arrivalDT)
        {
            var schedule = _airplaneContext.Schedules.FirstOrDefault(s => s.idSchedule == id);
            if (schedule != null)
            {
                schedule.arrivalDT = arrivalDT;
                _airplaneContext.SaveChanges();
            }//else schedule doesn't exist
        }

        public void EditDeparture(Guid id, DateTime departureDT)
        {
            var schedule = _airplaneContext.Schedules.FirstOrDefault(s => s.idSchedule == id);
            if (schedule != null)
            {
                schedule.departureDT = departureDT;
                _airplaneContext.SaveChanges();
            }//else schedule doesn't exist
        }

        public void GenerateSchedule(DateTime startDate, DateTime endDate, Guid? flightId)
        {
            _airplaneContext.GenerateSchedule(startDate, endDate,flightId);
            _airplaneContext.SaveChanges();
        }

        public List<ScheduleDTO> GetAll()
        {
            var schedules = _airplaneContext.Schedules.ToList().Select(s => new ScheduleDTO
            {
                ID = s.idSchedule,
                FlightID=s.idFlight,
                DepartureDT=s.departureDT,
                ArrivalDT=s.arrivalDT,
                Comment=s.comment
            });

            return schedules.ToList();
        }

        public void Remove(Guid id)
        {
            var schedule = _airplaneContext.Schedules.FirstOrDefault(s => s.idSchedule == id);
            if (schedule != null)
            {
                _airplaneContext.Schedules.Remove(schedule);
                _airplaneContext.SaveChanges();
            }
        }
    
    }
}
