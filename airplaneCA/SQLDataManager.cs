﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace airplaneCA
{
    class SQLDataManager: IDataManager
    {
        private SqlDataReader rdr=null;
        private SqlConnection conn=null;

        public void SetDataManager(IConnection conn)
        {
            if (conn is SQLConnection)
            {
                this.conn=((SQLConnection)conn).sqlConn;
            }
        }

        public void GenerateSchedule(string startDate, string endDate)
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
        private void Execute(SqlCommand cmd)
        {
            rdr = cmd.ExecuteReader();
        }
        public void Print()
        {
            if (rdr != null)
            {
                while (rdr.Read())
                {
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        Console.Write(rdr[i] + "\t");
                    }
                    Console.WriteLine();
                }
                
            }
        }
    }
}
