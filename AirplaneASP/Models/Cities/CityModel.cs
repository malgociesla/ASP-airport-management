using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AirplaneASP.Models.Cities
{
    public class CityModel
    {
        [Display(Name = "City ID")]
        public Guid ID { get; set; }

        [Display(Name = "Country ID")]
        public Guid CountryID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}