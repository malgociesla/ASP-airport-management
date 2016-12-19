using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airplaneCA
{
    class MyApp
    {
        private IConnection connection;
        private static string conStr="Data Source=.;Initial Catalog=airport;Integrated Security=True";

        public void Run()
        {
            try
            {
                SetConnection();
                connection.Open();

                //get input
                string startDate;
                string endDate;
                Console.WriteLine("Input @startDate");
                startDate = Console.ReadLine();
                Console.WriteLine("Input @endDate");
                endDate = Console.ReadLine();

                //do work
                GenerateSchedule();
                PrintTable("Schedule");

                connection.Close();
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }


        void SetConnection()
        {
            connection = new SQLConnection();
            connection.SetConnection(conStr);
         }

    }
}
