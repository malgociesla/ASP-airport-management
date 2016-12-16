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
            SqlConnection conn = null;
            SqlDataReader rdr = null;
            string startDate= "2016-12-11";
            string endDate= "2016-12-24";

            try
            {
                conn = new SqlConnection("Data Source=.;Initial Catalog=airport;Integrated Security=True");
                conn.Open();

                Console.WriteLine(conn.Database.ToString());

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand("GenerateSchedule", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // 3. add parameter to command, which
                // will be passed to the stored procedure

                cmd.Parameters.Add(new SqlParameter("@startDate", startDate));
                cmd.Parameters.Add(new SqlParameter("@endDate", endDate));

                // execute the command
                rdr = cmd.ExecuteReader();
                Console.WriteLine(rdr.RecordsAffected);
                
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            //do not close window
            Console.ReadLine();
        }
    }
}
