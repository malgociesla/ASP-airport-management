using System;
using System.Collections.Generic;
using System.Linq;
using AirportService.DTO;
using AirplaneEF;
using PagedList;

namespace AirportService
{
    public class ScheduleService:IScheduleService
    {
        private readonly AirportContext _airplaneContext;
        public ScheduleService()
        {
            _airplaneContext = new AirportContext();
        }

        public Guid Add(ScheduleDTO scheduleDTO)
        {
            Schedule schedule = new Schedule
            {
                idFlight = scheduleDTO.FlightID,
                idFlightState = scheduleDTO.FlightStateID,
                departureDT = scheduleDTO.DepartureDT,
                arrivalDT = scheduleDTO.ArrivalDT,
                comment= scheduleDTO.Comment
            };
            _airplaneContext.Schedules.Add(schedule);
            _airplaneContext.SaveChanges();
            return schedule.idSchedule;
        }

        public void Edit(ScheduleDTO scheduleDTO)
        {
            var schedule = _airplaneContext.Schedules.FirstOrDefault(c => c.idSchedule == scheduleDTO.ID);
            if (schedule != null)
            {
                schedule.idFlight = scheduleDTO.FlightID;
                schedule.idFlightState = scheduleDTO.FlightStateID;
                schedule.departureDT = scheduleDTO.DepartureDT;
                schedule.arrivalDT = scheduleDTO.ArrivalDT;
                schedule.comment = scheduleDTO.Comment;

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
                FlightStateID=s.idFlightState,
                FlightID=s.idFlight,
                DepartureDT=s.departureDT,
                ArrivalDT=s.arrivalDT,
                Comment=s.comment
            });

            return schedules.ToList();
        }

        public IPagedList<ScheduleDTO> GetPage(int pageNumber, int pageSize)
        {
            var schedules = _airplaneContext.Schedules.ToList().Select(s => new ScheduleDTO
            {
                ID = s.idSchedule,
                FlightStateID = s.idFlightState,
                FlightID = s.idFlight,
                DepartureDT = s.departureDT,
                ArrivalDT = s.arrivalDT,
                Comment = s.comment
            });

            return schedules.ToPagedList(pageNumber, pageSize);
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
