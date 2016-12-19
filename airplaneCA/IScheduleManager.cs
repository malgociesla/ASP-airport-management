using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airplaneCA
{
    interface IScheduleManager
    {
        void GenerateSchedule(string startDate, string endDate);
        List<Schedule> GetSchedule(string query);
    }
}
