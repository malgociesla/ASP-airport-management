using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AirplaneASP.Models.Schedules
{
    public class ScheduleDetailsModel : ScheduleModel
    {
        [Display(Name = "City")]
        public string CityDeparture { get; set; }
        [Display(Name = "Country")]
        public string CountryDeparture { get; set; }
        [Display(Name = "City")]
        public string CityArrival { get; set; }
        [Display(Name = "Country")]
        public string CountryArrival { get; set; }
        [Display(Name = "Company")]
        public string Company { get; set; }
    }
}