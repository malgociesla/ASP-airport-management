using System;

namespace AirportService.DTO
{
    public class CityDTO
    {
        public Guid ID { get; set; }
        public Guid CountryID { get; set; }
        public string Name { get; set; }
    }
}
