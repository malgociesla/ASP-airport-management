using System;
using System.Collections.Generic;
using AirportService.DTO;
using PagedList;

namespace AirportService
{
    public interface IScheduleService
    {
        Guid Add(ScheduleDTO scheduleDTO);
        void Remove(Guid id);
        void Edit(ScheduleDTO scheduleDTO);
        List<ScheduleDTO> GetAll();
        IPagedList<ScheduleDTO> GetPage(int pageNumber, int pageSize);
        void GenerateSchedule(DateTime startDate, DateTime endDate, Guid? flightId);
    }
}
