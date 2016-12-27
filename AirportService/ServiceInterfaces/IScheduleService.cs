using System;
using System.Collections.Generic;
using AirportService.DTO;

namespace AirportService
{
    public interface IScheduleService
    {
        Guid Add(Guid idFlight, DateTime departureDT, DateTime arrivalDT);
        Guid Add(Guid idFlight, DateTime departureDT, DateTime arrivalDT, string comment);
        void Remove(Guid id);
        void Edit(ScheduleDTO scheduleDTO);
        List<ScheduleDTO> GetAll();
        void GenerateSchedule(DateTime startDate, DateTime endDate, Guid? flightId);
    }
}
