using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirplaneASP.Models.Schedules
{
    public class ImportViewModel
    {
        public ImportViewModel()
        {
            ScheduleList = new List<ScheduleDetailsImportModel>();
        }

        public List<ScheduleDetailsImportModel> ScheduleList { get; set; }
    }
}