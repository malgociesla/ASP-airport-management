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
        static void Main(string[] args)
        {
            //set console
            SetConsole();

            //do work
            MyApp app = new MyApp();
            app.Run();

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
