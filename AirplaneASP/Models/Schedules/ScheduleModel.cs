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

        [Display(Name = "Departure date and time")]
        public DateTime? DepartureDT { get; set; }

        [Display(Name = "Arrival date and time")]
        public DateTime? ArrivalDT { get; set; }

        [Display(Name = "Comment")]
        public string Comment { get; set; }
    }
}