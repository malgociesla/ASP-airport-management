﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace airplaneCA
{
    class SQLDataManager
    {
        private SqlDataReader rdr = null;
        
        public void GenerateSchedule()
        {
            SqlCommand cmd = new SqlCommand("GenerateSchedule", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@startDate", startDate));
            cmd.Parameters.Add(new SqlParameter("@endDate", endDate));
            Execute(cmd);
            Close();
        }
        public void PrintTable(string tableName)
        {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [airport].[dbo].[" + tableName + "]", conn);
                Execute(cmd);
                Print();
        }
        public void Close()
        {
            rdr.Close();
        }
        public void Execute(SqlCommand cmd)
        {
            rdr = cmd.ExecuteReader();
        }
        public void Print()
        {
            if (rdr != null)
            {
                int i = 0;
                while (rdr.Read() && rdr[i]!=null)
                {                         
                    Console.WriteLine(rdr[i] + "\t");
                    i++;
                }
            }
        }
    }
}
