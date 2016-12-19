using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace airplaneCA
{
    class SQLConnection: IConnection
    {
        private string connStr;
        private SqlConnection sqlConn;
        private SQLDataManager sqlDataManager;

        public SQLConnection()
        {
            sqlConn = new SqlConnection();
            sqlDataManager = new SQLDataManager(sqlConn);
        }

        public void SetConnection(string connStr)
        {
            this.connStr = connStr;
        }

        public bool Open()
        {
            if (connStr.Length != 0)
            {
                sqlConn.Open();
                return true;
            }
            else return false;
        }
        public bool Close()
        {
            if (sqlConn != null)
            {
                sqlConn.Close();
                return true;
            }
            else return false;
        }
    }
}
