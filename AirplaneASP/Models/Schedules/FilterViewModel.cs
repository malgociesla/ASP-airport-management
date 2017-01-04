using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AirplaneASP.Models.Schedules
{
    public class FilterViewModel
    {
        [Display(Name = "From date")]
        [DataType(DataType.Date, ErrorMessage = "Please enter date format")]
        [Required(ErrorMessage = "Please enter a from date")]
        public DateTime FromDate { get; set; }

        [Display(Name = "To date")]
        [DataType(DataType.Date, ErrorMessage = "Please enter date format")]
        [Required(ErrorMessage = "Please enter a to date")]
        public DateTime ToDate { get; set; }
    }
}