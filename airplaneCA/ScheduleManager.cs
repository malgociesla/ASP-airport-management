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

        private object ProcessReader(SqlDataReader dataReader)
        {
            List<Schedule> scheduleList = new List<Schedule>();
            try
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        scheduleList.Add(new Schedule()
                        {
                            Id = dataReader.GetGuid(0),
                            IdFlight = dataReader.GetGuid(1),
                            DepartureDT = dataReader.GetDateTime(3),
                            ArrivalDT = dataReader.GetDateTime(4)
                        });
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Error message: {0}", sqlEx.Message);
                Console.WriteLine("Error stack trace: {0}", sqlEx.StackTrace);
            }
            return scheduleList;
        }

        public void GenerateSchedule(string startDate, string endDate)
        {
            if (startDate != null && endDate != null)
            {
                Dictionary<string, string> dates = new Dictionary<string, string>();
                dates.Add("@startDate", startDate);
                dates.Add("@endDate", endDate);
                _airportRepository.ExecuteStoredProcedure("GenerateSchedule", dates);
            }
            else
            {
                throw new ArgumentNullException("Provided parameters cannot be empty");
            }
        }

        public List<Schedule> GetSchedule(string query)
        {
            List<Schedule> scheduleList = new List<Schedule>();
            try
            {
                return _airportRepository.ExecuteReader(query, ProcessReader) as List<Schedule>;               
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Error message: {0}", sqlEx.Message);
                Console.WriteLine("Error stack trace: {0}", sqlEx.StackTrace);
            }
            return null;
        }
    }
}
