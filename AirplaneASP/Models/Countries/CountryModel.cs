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
        [Required(ErrorMessage = "Please enter a name")]
        [DataType(DataType.Text, ErrorMessage = "Please enter text")]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "Invalid character")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name length must be between 2 and 50")]
        public string Name { get; set; }
    }
}