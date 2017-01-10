using System;
using System.Collections.Generic;

namespace AirportService.DTO
{
    public class FlightDTO
    {
        public Guid ID { get; set; }
        public Guid CompanyID { get; set; }
        public string Name { get; set; }
        public int? DayOfWeek { get; set; }
        public Guid CityDepartureID { get; set; }
        public Guid CityArrivalID { get; set; }
        public TimeSpan? DepartureTime { get; set; }
        public TimeSpan? ArrivalTime { get; set; }
        public virtual CityDTO CityArrival { get; set; }
        public virtual CityDTO CityDeparture { get; set; }
        public virtual CompanyDTO Company { get; set; }
        public virtual ICollection<ScheduleDTO> Schedules { get; set; }
    }
}
