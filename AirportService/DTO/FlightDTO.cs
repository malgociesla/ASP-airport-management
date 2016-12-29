using System;

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
    }
}
