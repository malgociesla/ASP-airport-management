using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AirportService;
using AirportService.DTO;

namespace AirplaneASP.Models.Companies
{
    public class CompaniesModel
    {
        [Display(Name = "Company ID")]
        public Guid ID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}