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
        [Required(ErrorMessage = "Please enter a name")]
        [DataType(DataType.Text, ErrorMessage = "Please enter text")]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "Invalid character")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name length must be between 2 and 50")]
        public string Name { get; set; }

        [Display(Name = "Day of week")]
        [Required(ErrorMessage = "Please enter a day of week")]
        [Range(1, 7, ErrorMessage = "Value must be integer between 1 and 7")]
        public int? DayOfWeek { get; set; }

        [Display(Name = "City departure ID")]
        public Guid CityDepartureID { get; set; }

        [Display(Name = "City arrival ID")]
        public Guid CityArrivalID { get; set; }

        [Display(Name = "Departure time")]
        [Required(ErrorMessage = "Please enter a departure time")]
        [DataType(DataType.Time, ErrorMessage = "Please enter time format")]
        public TimeSpan? DepartureTime { get; set; }

        [Display(Name = "Arrival time")]
        [Required(ErrorMessage = "Please enter an arrival time")]
        [DataType(DataType.Time, ErrorMessage = "Please enter time format")]
        public TimeSpan? ArrivalTime { get; set; }
    }
}