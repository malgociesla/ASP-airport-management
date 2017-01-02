using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AirportService;
using AirportService.DTO;

namespace AirplaneASP.Models.Companies
{
    public class CompanyModel
    {
        [Display(Name = "Company ID")]
        public Guid ID { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter a name")]
        [DataType(DataType.Text, ErrorMessage = "Please enter text")]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "Invalid character")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Name length must be between 2 and 30")]
        public string Name { get; set; }
    }
}