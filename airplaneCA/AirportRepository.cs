using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                string connStr = ConfigurationManager.ConnectionStrings["airport"].ConnectionString;
                sqlConn = new SqlConnection(connStr);//conStr returned configured with connstr config file
            }
            catch(SqlException sqlEx)
            {
            }
            return sqlConn;
        }

        public void ExecuteStoredProcedure(string storedProcedureName, Dictionary<string,string> parameters)
        {
            using (var Connection = GetSqlConnection())
            {
                    Connection.Open();
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach(var item in parameters.Keys)
                        {
                            cmd.Parameters.Add(new SqlParameter(item,parameters[item]));
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    { }
                    finally
                    {
                        Connection.Close();
                    }
                
            }

            
        }

        public SqlDataReader ExecuteReader(string query)
        {
            SqlDataReader dataReader = null;
            using (var connection = GetSqlConnection())
            {
                connection.Open();         
                try
                {
                    SqlCommand cmd = new SqlCommand(query,connection);
                    dataReader = cmd.ExecuteReader();
                    return dataReader;
                }
                catch(Exception ex)
                { }
                finally
                {
                    if (dataReader != null)
                        dataReader.Close();
                    connection.Close();
                }
            }
            return dataReader;
        }

    }
}
