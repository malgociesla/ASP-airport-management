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
        void Edit(Guid id, Guid idFlight, DateTime departureDT, DateTime arrivalDT);
        void Edit(Guid id, Guid idFlight, DateTime departureDT, DateTime arrivalDT, string comment);
        void Edit(Guid id, Guid idFlight);
        void Edit(Guid id, string comment);
        void EditDeparture(Guid id, DateTime departureDT);
        void EditArrival(Guid id, DateTime arrivalDT);
        List<ScheduleDTO> GetAll();
        void GenerateSchedule(DateTime startDate, DateTime endDate, Guid? flightId);
    }
}
