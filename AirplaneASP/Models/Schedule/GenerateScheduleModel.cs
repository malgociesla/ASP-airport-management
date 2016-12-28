using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirplaneASP.Models.Schedule
{
    public class GenerateScheduleModel
    {
        public Guid? FlightID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}