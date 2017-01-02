using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AirplaneASP.Models.Flights
{
    public class FlightModel
    {
        [Display(Name = "Flight ID")]
        public Guid ID { get; set; }

        [Display(Name = "Company ID")]
        public Guid CompanyID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Day of week")]
        public int? DayOfWeek { get; set; }

        [Display(Name = "City departure ID")]
        public Guid CityDepartureID { get; set; }

        [Display(Name = "City arrival ID")]
        public Guid CityArrivalID { get; set; }

        [Display(Name = "Departure time")]
        public TimeSpan? DepartureTime { get; set; }

        [Display(Name = "Arrival time")]
        public TimeSpan? ArrivalTime { get; set; }
    }
}