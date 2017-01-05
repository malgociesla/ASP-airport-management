using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AirplaneASP.ModelValidation;

namespace AirplaneASP.Models.Schedules
{
    [FromToDateAttribute]
    public class FilterViewModel
    {
        [Display(Name = "From date")]
        [DataType(DataType.Date, ErrorMessage = "Please enter date format")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FromDate { get; set; }

        [Display(Name = "To date")]
        [DataType(DataType.Date, ErrorMessage = "Please enter date format")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ToDate { get; set; }
    }
}