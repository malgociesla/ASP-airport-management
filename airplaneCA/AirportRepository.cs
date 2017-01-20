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
                Console.WriteLine("Error message: {0}",sqlEx.Message);
                Console.WriteLine("Error stack trace: {0}", sqlEx.StackTrace);
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
                        SqlCommand cmd = new SqlCommand(storedProcedureName,Connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach(var item in parameters.Keys)
                        {
                            cmd.Parameters.Add(new SqlParameter(item,parameters[item]));
                        }
                        cmd.ExecuteNonQuery();
                }
                    catch (Exception ex)
                    { }
                    finally
                    {
                        Connection.Close();
                    }
                
            }

            
        }

        public object ExecuteReader(string query, Func<SqlDataReader,object> processReader)
        {
            object objResult=null;
            SqlDataReader dataReader = null;
            using (var connection = GetSqlConnection())
            {
                connection.Open();         
                try
                {
                    SqlCommand cmd = new SqlCommand(query,connection);
                    dataReader = cmd.ExecuteReader();
                    objResult= processReader(dataReader);
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
            return objResult;
        }

    }
}
