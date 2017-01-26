using System;
using System.Collections.Generic;
using AirportService.DTO;
using System.IO;

namespace AirportService
{
    public interface IScheduleService
    {
        Guid Add(ScheduleDTO scheduleDTO);
        void Remove(Guid id);
        void Edit(ScheduleDTO scheduleDTO);
        List<ScheduleDetailsDTO> GetAll();
        List<ScheduleDetailsDTO> GetList(int pageNumber, int pageSize, out int totalItemsCount, DateTime? from = null, DateTime? to = null);
        void GenerateSchedule(DateTime startDate, DateTime endDate, Guid? flightId);
        void UpdateSchedule(List<ScheduleDTO> schedulesList);
    }
}
