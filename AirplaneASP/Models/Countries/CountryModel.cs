using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AirplaneASP.Models.Countries
{
    public class CountryModel
    {
        [Display(Name = "Country ID")]
        public Guid? ID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}