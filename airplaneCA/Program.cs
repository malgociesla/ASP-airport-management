using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace airplaneCA
{
    class Program
    {
        private static IScheduleManager _scheduleManager;

        static void Main(string[] args)
        {
            //set console
            SetConsole();

            //get input
            string startDate;
            string endDate;
            Console.WriteLine("Input @startDate");
            startDate = Console.ReadLine();
            Console.WriteLine("Input @endDate");
            endDate = Console.ReadLine();
            
            //do work
            _scheduleManager = new ScheduleManager();
            _scheduleManager.GenerateSchedule(startDate, endDate);
            List<Schedule> scheduleList=_scheduleManager.GetSchedule("SELECT * FROM [airport].[dbo].[Schedule]");
            foreach (Schedule s in scheduleList)
            {
                Console.WriteLine(s.ToString());
            }
            //do not close window
            Console.ReadLine();
        }

        static void SetConsole()
        {
            Console.SetWindowSize(239, 63);
            Console.Title="execute [airport].[dbo].[GenerateSchedule] @startDate='2016-12-11', @endDate='2016-12-24'";
        }
    }
}
