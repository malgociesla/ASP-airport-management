using System.Collections.Generic;

namespace airplaneCA
{
    interface IScheduleManager
    {
        void GenerateSchedule(string startDate, string endDate);
        List<Schedule> GetSchedule(string query);
    }
}
