using System;
using System.Collections.Generic;
using System.Linq;
using AirportService.DTO;
using AirplaneEF;

namespace AirportService
{
    public class ScheduleService : IScheduleService
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
                comment = scheduleDTO.Comment
            };
            _airplaneContext.Schedules.Add(schedule);
            _airplaneContext.SaveChanges();
            return schedule.idSchedule;
        }

        public void Edit(ScheduleDTO scheduleDTO)
        {
            if (scheduleDTO != null)
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
            else
            {
                //throw ex
            }
        }

        public void GenerateSchedule(DateTime startDate, DateTime endDate, Guid? flightId)
        {
            _airplaneContext.GenerateSchedule(startDate, endDate, flightId);
            _airplaneContext.SaveChanges();
        }

        public List<ScheduleDTO> GetAll()
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

            return schedules.ToList();
        }

        private IQueryable<ScheduleDTO> GetFilteredByDate(DateTime from, DateTime to)
        {
            var schedules = GetAllDefault()
                .Where(s =>
                        ((s.ArrivalDT >= from && s.ArrivalDT <= to)
                     && (s.DepartureDT >= from && s.DepartureDT <= to)));
            return schedules;
        }

        private IQueryable<ScheduleDTO> GetAllDefault()
        {
            var scheduleQ = _airplaneContext.Schedules
                    .OrderBy(s => s.departureDT)
                    .Select(s => new ScheduleDTO
                    {
                        ID = s.idSchedule,
                        FlightStateID = s.idFlightState,
                        FlightID = s.idFlight,
                        DepartureDT = s.departureDT,
                        ArrivalDT = s.arrivalDT,
                        Comment = s.comment
                    });
            return scheduleQ;
        }

        public List<ScheduleDTO> GetList(int pageNumber,
            int pageSize,
            out int totalItemsCount,
            DateTime? from = null,
            DateTime? to = null)
        {
            IQueryable<ScheduleDTO> scheduleQ = null;
            List<ScheduleDTO> schedules = new List<ScheduleDTO>();

            if (from == null || to == null)
                scheduleQ = GetAllDefault();
            else
                scheduleQ = GetFilteredByDate((DateTime)from, (DateTime)to);

            schedules = scheduleQ.Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize)
                    .ToList();
            totalItemsCount = scheduleQ.ToList().Count;
            return schedules;
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
