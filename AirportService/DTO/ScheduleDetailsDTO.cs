using System;

namespace AirportService.DTO
{
    public class ScheduleDetailsDTO : ScheduleDTO
    {
        public string CityDeparture { get; set; }
        public string CountryDeparture { get; set; }
        public string CityArrival { get; set; }
        public string CountryArrival { get; set; }
        public string Company { get; set; }
    }
}
