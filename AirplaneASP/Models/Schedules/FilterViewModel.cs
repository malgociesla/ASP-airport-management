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
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FromDate { get; set; }

        [Display(Name = "To date")]
        [DataType(DataType.Date, ErrorMessage = "Please enter date format")]
        [Required(ErrorMessage = "Please enter a to date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ToDate { get; set; }
    }
}