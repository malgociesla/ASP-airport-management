using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AirplaneASP.Models.Schedules
{
    public class ScheduleDetailsModel : ScheduleModel
    {
        private string _cityDeparture;
        private string _cityArrival;

        [Display(Name = "From")]
        public string CityDeparture
        {
            get
            {
                if(this._cityDeparture !=null && this.CountryDeparture !=null)
                    return string.Format("{0} ({1})", this._cityDeparture, this.CountryDeparture);
                return this._cityDeparture;
            }
            set
            {
                this._cityDeparture = value;
            }
        }
        [Display(Name = "From country")]
        public string CountryDeparture { get; set; }
        [Display(Name = "To")]
        public string CityArrival
        {
            get
            {   
                if(this._cityArrival != null && this.CountryArrival !=null)
                    return string.Format("{0} ({1})", this._cityArrival, this.CountryArrival);
                return this._cityArrival;
            }
            set
            {
                this._cityArrival = value;
            }
        }
        [Display(Name = "To country")]
        public string CountryArrival { get; set; }
        [Display(Name = "Company")]
        public string Company { get; set; }
    }
}