using System;

namespace AirportService.DTO
{
    public class ScheduleDTO
    {
        public Guid ID { get; set; }
        public Guid FlightID { get; set; }
        public Guid FlightStateID { get; set; }
        public DateTime? DepartureDT { get; set; }
        public DateTime? ArrivalDT { get; set; }
        public string Comment { get; set; }
    }
}
