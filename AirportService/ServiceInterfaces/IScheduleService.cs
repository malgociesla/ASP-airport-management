using System;
using System.Collections.Generic;
using AirportService.DTO;

namespace AirportService
{
    public interface IScheduleService
    {
        Guid Add(ScheduleDTO scheduleDTO);
        void Remove(Guid id);
        void Edit(ScheduleDTO scheduleDTO);
        List<ScheduleDTO> GetAll();
        void GenerateSchedule(DateTime startDate, DateTime endDate, Guid? flightId);
    }
}
