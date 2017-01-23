using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace airplaneCA
{
    class AirportRepository
    {
        private SqlConnection GetSqlConnection()
        {
            SqlConnection sqlConn = null;
            try
            {
                //get connstr from config file
                string connStr = ConfigurationManager.ConnectionStrings["airport"].ConnectionString;
                sqlConn = new SqlConnection(connStr);
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Error message: {0}", sqlEx.Message);
                Console.WriteLine("Error stack trace: {0}", sqlEx.StackTrace);
            }
            return sqlConn;
        }

        public void ExecuteStoredProcedure(string storedProcedureName, Dictionary<string, string> parameters)
        {
            using (var connection = GetSqlConnection())
            {
                connection.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand(storedProcedureName, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var item in parameters.Keys)
                    {
                        cmd.Parameters.Add(new SqlParameter(item, parameters[item]));
                    }
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Error message: {0}", sqlEx.Message);
                    Console.WriteLine("Error stack trace: {0}", sqlEx.StackTrace);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public object ExecuteReader(string query, Func<SqlDataReader, object> processReader)
        {
            object objResult = null;
            SqlDataReader dataReader = null;
            using (var connection = GetSqlConnection())
            {
                connection.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand(query, connection);
                    dataReader = cmd.ExecuteReader();
                    objResult = processReader(dataReader);
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Error message: {0}", sqlEx.Message);
                    Console.WriteLine("Error stack trace: {0}", sqlEx.StackTrace);
                }
                finally
                {
                    if (dataReader != null)
                        dataReader.Close();
                    connection.Close();
                }
            }
            return objResult;
        }
    }
}
