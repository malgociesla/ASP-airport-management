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
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CityDeparture { get; set; }
        [Display(Name = "Country")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CountryDeparture { get; set; }
        [Display(Name = "City")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CityArrival { get; set; }
        [Display(Name = "Country")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CountryArrival { get; set; }
        [Display(Name = "Company")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Company { get; set; }
    }
}