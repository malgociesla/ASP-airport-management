using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AirplaneASP.Models.Schedules
{
    public class GenerateScheduleModel
    {
        [Display(Name = "Flight ID")]
        public Guid? FlightID { get; set; }

        [Display(Name = "Start date")]
        [DataType(DataType.Date, ErrorMessage = "Please enter date format")]
        [Required(ErrorMessage = "Please enter a start date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End date")]
        [DataType(DataType.Date, ErrorMessage = "Please enter date format")]
        [Required(ErrorMessage = "Please enter an end date")]
        public DateTime EndDate { get; set; }
    }
}