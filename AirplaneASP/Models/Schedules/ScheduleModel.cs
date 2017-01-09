using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AirplaneASP.Models.Schedules
{
    public class ScheduleModel
    {
        [Display(Name = "Schedule ID")]
        public Guid ID { get; set; }

        [Display(Name = "Flight ID")]
        public Guid FlightID { get; set; }

        [Display(Name = "Flight state ID")]
        public Guid FlightStateID { get; set; }

        [Display(Name = "Departure")]
        [Required(ErrorMessage = "Please enter a departure date and time")]
        [DataType(DataType.DateTime, ErrorMessage = "Please enter datetime format")]
        public DateTime? DepartureDT { get; set; }

        [Display(Name = "Arrival")]
        [Required(ErrorMessage = "Please enter a arrival date and time")]
        [DataType(DataType.DateTime, ErrorMessage = "Please enter datetime format")]
        public DateTime? ArrivalDT { get; set; }

        [Display(Name = "Comment")]
        [DataType(DataType.Text, ErrorMessage = "Please enter text")]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "Invalid character")]
        [StringLength(50, ErrorMessage = "Comment length must be less than 50")]
        public string Comment { get; set; }
    }
}