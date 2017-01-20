using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AirplaneASP.Models.Schedules
{
    public class ScheduleDetailsImportModel : ScheduleDetailsModel
    {
        [Display(Name = "Import")]
        public bool Check { get; set; }
    }
}