using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airplaneCA
{
    class ScheduleManager : IScheduleManager
    {
        private AirportRepository _airportRepository;
        public ScheduleManager()
        {
            _airportRepository = new AirportRepository();
        }
        public void GenerateSchedule(string startDate, string endDate)
        {
            Dictionary<string, string> dates = new Dictionary<string, string>();
            dates.Add("@startDate", startDate);
            dates.Add("@endDate", endDate);
            _airportRepository.ExecuteStoredProcedure("GenerateSchedule",dates);
        }

        public List<Schedule> GetSchedule(string query)
        {
            SqlDataReader dataReader = null;
            List<Schedule> scheduleList = new List<Schedule>();
            try
            {
                dataReader = _airportRepository.ExecuteReader(query);               
                while (dataReader.Read())
                {
                    scheduleList.Add(new Schedule()
                    {
                        Id = dataReader.GetGuid(0),
                        IdFlight = dataReader.GetGuid(1),
                        DepartureDT = dataReader.GetDateTime(2),
                        ArrivalDT = dataReader.GetDateTime(3)
                    });
                }
            }
            catch(SqlException sqlEx)
            {
            }
            finally
            {
                if(dataReader!=null)
                dataReader.Close();
            }

            return scheduleList;
        }
    }
}
